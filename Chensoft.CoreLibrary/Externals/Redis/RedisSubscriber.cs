﻿/*
 * Authors:
 *   钟峰(Popeye Zhong) <Chensoft@gmail.com>
 *
 * Copyright (C) 2014-2015 Chensoft Corporation <http://www.Chensoft.com>
 *
 * This file is part of Chensoft.Externals.Redis.
 *
 * Chensoft.Externals.Redis is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * Chensoft.Externals.Redis is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with Chensoft.Externals.Redis; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServiceStack.Redis;

namespace Chensoft.Externals.Redis
{
	public class RedisSubscriber : MarshalByRefObject, Chensoft.Common.IDisposableObject
	{
		#region 事件定义
		public event EventHandler<Chensoft.Common.DisposedEventArgs> Disposed;
		public event EventHandler<RedisChannelEventArgs> Subscribed;
		public event EventHandler<RedisChannelEventArgs> Unsubscribed;
		public event EventHandler<RedisChannelMessageEventArgs> Received;
		#endregion

		#region 成员字段
		private RedisClient _redis;
		private IRedisSubscription _redisSubscription;
		#endregion

		#region 构造函数
		public RedisSubscriber()
		{
		}

		public RedisSubscriber(RedisClient redis)
		{
			if(redis == null)
				throw new ArgumentNullException("redis");

			_redis = redis;
			_redisSubscription = new ServiceStack.Redis.RedisSubscription(_redis);
			_redisSubscription.OnMessage = this.OnReceived;
			_redisSubscription.OnSubscribe = this.OnSubscribed;
			_redisSubscription.OnUnSubscribe = this.OnUnsubscribed;
		}
		#endregion

		#region 公共属性
		public RedisClient Proxy
		{
			get
			{
				return _redis;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException();

				if(object.ReferenceEquals(_redis, value))
					return;

				_redis = value;

				var subscription = System.Threading.Interlocked.Exchange(ref _redisSubscription, new ServiceStack.Redis.RedisSubscription(_redis)
				{
					OnMessage = this.OnReceived,
					OnSubscribe = this.OnSubscribed,
					OnUnSubscribe = this.OnUnsubscribed,
				});

				if(subscription != null)
				{
					subscription.OnMessage = null;
					subscription.OnSubscribe = null;
					subscription.OnUnSubscribe = null;
				}
			}
		}

		public bool IsDisposed
		{
			get
			{
				return _redis == null;
			}
		}
		#endregion

		#region 公共方法
		public void Subscribe(params string[] channels)
		{
			var redis = this.Proxy;

			if(redis == null)
				throw new ObjectDisposedException(this.GetType().FullName);

			_redisSubscription.SubscribeToChannels(channels);
		}

		public void Unsubscribe(params string[] channels)
		{
			var redis = this.Proxy;

			if(redis == null)
				throw new ObjectDisposedException(this.GetType().FullName);

			_redisSubscription.UnSubscribeFromChannels(channels);
		}

		public void UnsubscribeAll()
		{
			_redisSubscription.UnSubscribeFromAllChannels();
		}

		public long Publish(string channel, string message)
		{
			var redis = this.Proxy;

			if(redis == null)
				throw new ObjectDisposedException(this.GetType().FullName);

			if(string.IsNullOrWhiteSpace(channel))
				throw new ArgumentNullException("channel");

			if(string.IsNullOrEmpty(message))
				return 0;

			return redis.PublishMessage(channel, message);
		}
		#endregion

		#region 事件处理
		protected virtual void OnReceived(string channel, string message)
		{
			var handler = this.Received;

			if(handler != null)
				handler(this, new RedisChannelMessageEventArgs(channel, message));
		}

		protected virtual void OnSubscribed(string channel)
		{
			var handler = this.Subscribed;

			if(handler != null)
				handler(this, new RedisChannelEventArgs(channel));
		}

		protected virtual void OnUnsubscribed(string channel)
		{
			var handler = this.Unsubscribed;

			if(handler != null)
				handler(this, new RedisChannelEventArgs(channel));
		}
		#endregion

		#region 处置方法
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);

			var disposed = this.Disposed;

			if(disposed != null)
				disposed(this, new Common.DisposedEventArgs(true));
		}

		protected virtual void Dispose(bool disposing)
		{
			var redis = System.Threading.Interlocked.Exchange(ref _redis, null);

			if(redis != null)
				redis.Dispose();

			var redisSubscription = System.Threading.Interlocked.Exchange(ref _redisSubscription, null);

			if(redisSubscription != null)
			{
				redisSubscription.OnMessage = null;
				redisSubscription.OnSubscribe = null;
				redisSubscription.OnUnSubscribe = null;

				redisSubscription.Dispose();
			}
		}
		#endregion
	}
}
