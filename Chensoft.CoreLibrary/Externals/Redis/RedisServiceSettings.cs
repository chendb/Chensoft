

using System;
using System.Net;
using System.Collections.Generic;

namespace Chensoft.Externals.Redis
{
	[Serializable]
	public class RedisServiceSettings
	{
		#region 成员字段
		private IPEndPoint _address;
		private string _password;
		private int _databaseId;
		private TimeSpan _timeout;
		private int _poolSize;
		#endregion

		#region 构造函数
		public RedisServiceSettings()
		{
			_address = new IPEndPoint(IPAddress.Loopback, 6379);
		}

		public RedisServiceSettings(IPEndPoint address, string password, int databaseId = 0)
		{
			if(address == null)
				throw new ArgumentNullException("address");

			_address = address;
			_password = string.IsNullOrEmpty(password) ? null : password;
			_databaseId = Math.Abs(databaseId);

			if(_address.Port == 0)
				_address.Port = 6379;
		}
		#endregion

		#region 公共属性
		public IPEndPoint Address
		{
			get
			{
				return _address;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException();

				if(_address != value)
					_address = value;
			}
		}

		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		public int DatabaseId
		{
			get
			{
				return _databaseId;
			}
			set
			{
				if(_databaseId != value)
					_databaseId = Math.Abs(value);
			}
		}

		public TimeSpan Timeout
		{
			get
			{
				return _timeout;
			}
			set
			{
				_timeout = value;
			}
		}

		public int PoolSize
		{
			get
			{
				return _poolSize;
			}
			set
			{
				_poolSize = Math.Max(value, 16);
			}
		}
		#endregion
	}
}
