

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Chensoft.Collections
{
    /// <summary>
    /// 提供通用对象池的相关功能。
    /// </summary>
    public class ObjectPool<T> : MarshalByRefObject, IDisposable
    {
        #region 私有变量
        private ConcurrentBag<T> _idles;
        private Func<T> _creator;
        private Action<T> _remover;
		private int _maximumLimit;
        private SemaphoreSlim _semaphore;
        #endregion

        #region 构造函数
		protected ObjectPool() : this(0)
		{
		}

		protected ObjectPool(int maximumLimit)
		{
			_maximumLimit = Math.Max(maximumLimit, 0);
			_idles = new ConcurrentBag<T>();

			if(_maximumLimit > 0)
				_semaphore = new SemaphoreSlim(_maximumLimit, _maximumLimit);
		}

        /// <summary>
        /// 创建一个新的对象管理池。
        /// </summary>
        /// <param name="creator">对象的创建方法。</param>
        /// <param name="remover">对象移除时的回调，该参数值可以为空(null)。</param>
        public ObjectPool(Func<T> creator, Action<T> remover) : this(creator, remover, 0)
        {
        }

		/// <summary>
		/// 创建一个新的对象管理池。
		/// </summary>
		/// <param name="creator">对象的创建方法。</param>
		/// <param name="remover">对象移除时的回调，该参数值可以为空(null)。</param>
		/// <param name="maximumLimit">对象池的最大容量。</param>
		public ObjectPool(Func<T> creator, Action<T> remover, int maximumLimit)
        {
            if (creator == null)
                throw new ArgumentNullException("creator");

            _creator = creator;
            _remover = remover;
			_maximumLimit = Math.Max(maximumLimit, 0);
            _idles = new ConcurrentBag<T>();

			if(_maximumLimit > 0)
				_semaphore = new SemaphoreSlim(_maximumLimit, _maximumLimit);
        }
        #endregion

		#region 公共属性
		/// <summary>
		/// 获取对象池的可用空闲元素数量，负数表示未限定。
		/// </summary>
		public int Count
		{
			get
			{
				if(_semaphore == null || _maximumLimit < 1)
					return -1;

				return _maximumLimit - _idles.Count;
			}
		}

        /// <summary>
        /// 获取对象池的最大容量，零表示不限制。
        /// </summary>
		public int MaximumLimit
        {
			get
			{
				return _maximumLimit;
			}
        }
        #endregion

		#region 公共方法
		/// <summary>
        /// 从对象池中获取一个可用对象。
        /// </summary>
		public T GetObject()
		{
			//检查当前对象池是否被Disposed，如果是则抛出异常
			TryThrowObjectDisposedException();

			T item;

			if(_semaphore != null)
				_semaphore.Wait();

			if(!_idles.TryTake(out item))
			{
				item = this.OnCreate();
			}

			this.OnTakeout(item);

			return item;
		}

        /// <summary>
        /// 将一个对象释放到池中。
        /// </summary>
		public void Release(T item)
		{
			//检查当前对象池是否被Disposed，如果是则抛出异常
			TryThrowObjectDisposedException();

			this.OnTakein(item);

			_idles.Add(item);

			if(_semaphore != null)
				_semaphore.Release();
		}

        /// <summary>
        /// 清空对象池，该方法会依次调用池中空闲对象的<see cref="OnRemove"/>方法。
        /// </summary>
		public void Clear()
		{
			if(_idles == null)
				return;

			T item;

			while(_idles.TryTake(out item))
			{
				this.OnRemove(item);
			}
		}
        #endregion

		#region 虚拟方法
		protected virtual T OnCreate()
		{
			if(_creator == null)
				throw new InvalidOperationException();

			return _creator();
		}

		protected virtual void OnTakein(T value)
		{
		}

		protected virtual void OnTakeout(T value)
		{
		}

		protected virtual void OnRemove(T value)
		{
			if(_remover != null)
				_remover(value);

			IDisposable disposable = value as IDisposable;

			if(disposable != null)
				disposable.Dispose();
		}
		#endregion

        #region 私有方法
		private void TryThrowObjectDisposedException()
		{
			if(_idles == null)
				throw new ObjectDisposedException(null);
		}
		#endregion

		#region 释放资源
		public void Dispose()
        {
            this.Dispose(true);
        }

		protected virtual void Dispose(bool disposing)
		{
			if(!disposing)
				return;

			Clear();

			_idles = null;
			_creator = null;
			_remover = null;

			if(_semaphore != null)
			{
				_semaphore.Dispose();
				_semaphore = null;
			}
		}
        #endregion
    }
}