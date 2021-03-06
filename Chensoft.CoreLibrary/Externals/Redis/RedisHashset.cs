﻿

using System;
using System.Collections;
using System.Collections.Generic;

namespace Chensoft.Externals.Redis
{
	public class RedisHashset : RedisObjectBase, IRedisHashset, ICollection, IList, ICollection<string>
	{
		#region 私有变量
		private readonly object _syncRoot;
		#endregion

		#region 构造函数
		public RedisHashset(string name, Chensoft.Collections.ObjectPool<ServiceStack.Redis.IRedisClient> redisPool) : base(name, redisPool)
		{
			_syncRoot = new object();
		}
		#endregion

		#region 公共属性
		public int Count
		{
			get
			{
				var redis = this.Redis;

				try
				{
					return (int)redis.GetSetCount(this.Name);
				}
				finally
				{
					this.RedisPool.Release(redis);
				}
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}
		#endregion

		#region 公共方法
		public HashSet<string> GetExcept(params string[] other)
		{
			var redis = this.Redis;

			try
			{
				return redis.GetDifferencesFromSet(this.Name, other);
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}

		public void SetExcept(string destination, params string[] other)
		{
			var redis = this.Redis;

			try
			{
				redis.StoreDifferencesFromSet(destination, this.Name, other);
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}

		public HashSet<string> GetIntersect(params string[] other)
		{
			var sets = new string[other.Length + 1];
			sets[0] = this.Name;
			Array.Copy(other, 0, sets, 1, other.Length);

			var redis = this.Redis;

			try
			{
				return redis.GetIntersectFromSets(sets);
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}

		public void SetIntersect(string destination, params string[] other)
		{
			var sets = new string[other.Length + 1];
			sets[0] = this.Name;
			Array.Copy(other, 0, sets, 1, other.Length);

			var redis = this.Redis;

			try
			{
				redis.StoreIntersectFromSets(destination, sets);
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}

		public HashSet<string> GetUnion(params string[] other)
		{
			var sets = new string[other.Length + 1];
			sets[0] = this.Name;
			Array.Copy(other, 0, sets, 1, other.Length);

			var redis = this.Redis;

			try
			{
				return redis.GetUnionFromSets(sets);
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}

		public void SetUnion(string destination, params string[] other)
		{
			var sets = new string[other.Length + 1];
			sets[0] = this.Name;
			Array.Copy(other, 0, sets, 1, other.Length);

			var redis = this.Redis;

			try
			{
				redis.StoreUnionFromSets(destination, sets);
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}

		public HashSet<string> GetRandomValues(int count)
		{
			var result = new HashSet<string>();
			var redis = this.Redis;

			try
			{
				for(int i = 0; i < Math.Max(1, count); i++)
				{
					result.Add(redis.GetRandomItemFromSet(this.Name));
				}
			}
			finally
			{
				this.RedisPool.Release(redis);
			}

			return result;
		}

		public bool Move(string destination, string item)
		{
			var redis = this.Redis;

			try
			{
				redis.MoveBetweenSets(this.Name, destination, item);
				return true;
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}

		public void RemoveRange(params string[] items)
		{
			var redis = this.Redis;
			var transaction = redis.CreateTransaction();

			try
			{
				foreach(var item in items)
				{
					transaction.QueueCommand(proxy => proxy.RemoveEntryFromHash(this.Name, item));
				}

				transaction.Commit();
			}
			finally
			{
				if(transaction != null)
					transaction.Dispose();

				this.RedisPool.Release(redis);
			}
		}

		public bool Remove(string item)
		{
			var redis = this.Redis;

			try
			{
				redis.RemoveItemFromSet(this.Name, item);
				return true;
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}

		public void Add(string item)
		{
			var redis = this.Redis;

			try
			{
				redis.AddItemToSet(this.Name, item);
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}

		public void AddRange(IEnumerable<string> items)
		{
			if(items == null)
				return;

			var redis = this.Redis;

			try
			{
				redis.AddRangeToSet(this.Name, System.Linq.Enumerable.ToList(items));
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}

		public void AddRange(params string[] items)
		{
			this.AddRange((IEnumerable<string>)items);
		}

		public void Clear()
		{
			var redis = this.Redis;

			try
			{
				redis.Remove(this.Name);
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}

		public bool Contains(string item)
		{
			var redis = this.Redis;

			try
			{
				return redis.SetContainsItem(this.Name, item);
			}
			finally
			{
				this.RedisPool.Release(redis);
			}
		}
		#endregion

		#region 显式实现
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return _syncRoot;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		void ICollection<string>.CopyTo(string[] array, int arrayIndex)
		{
			throw new NotSupportedException();
		}

		int IList.Add(object value)
		{
			if(value == null)
				throw new ArgumentNullException("value");

			this.Add(value.ToString());

			return -1;
		}

		bool IList.Contains(object value)
		{
			if(value == null)
				return false;

			return this.Contains(value.ToString());
		}

		int IList.IndexOf(object value)
		{
			throw new NotSupportedException();
		}

		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		void IList.Remove(object value)
		{
			if(value == null)
				return;

			this.Remove(value.ToString());
		}

		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		object IList.this[int index]
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}
		#endregion

		#region 遍历枚举
		public IEnumerator<string> GetEnumerator()
		{
			HashSet<string> items;
			var redis = this.Redis;

			try
			{
				items = redis.GetAllItemsFromSet(this.Name);
			}
			finally
			{
				this.RedisPool.Release(redis);
			}

			foreach(var item in items)
			{
				yield return item;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}
