

using System;
using System.Collections.Generic;

namespace Chensoft.Externals.Redis
{
	[Serializable]
	public class RedisChannelMessageEventArgs : RedisChannelEventArgs
	{
		#region 成员字段
		private string _message;
		#endregion

		#region 构造函数
		public RedisChannelMessageEventArgs(string channel, string message) : base(channel)
		{
			_message = message;
		}
		#endregion

		#region 公共属性
		/// <summary>
		/// 获取收到的通知消息文本。
		/// </summary>
		public string Message
		{
			get
			{
				return _message;
			}
		}
		#endregion
	}
}
