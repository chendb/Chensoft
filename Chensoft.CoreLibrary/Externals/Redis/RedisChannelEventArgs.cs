
using System;
using System.Collections.Generic;

namespace Chensoft.Externals.Redis
{
	[Serializable]
	public class RedisChannelEventArgs : EventArgs
	{
		#region 成员字段
		private string _channel;
		#endregion

		#region 构造函数
		public RedisChannelEventArgs(string channel)
		{
			if(string.IsNullOrWhiteSpace(channel))
				throw new ArgumentNullException("channel");

			_channel = channel;
		}
		#endregion

		#region 公共属性
		/// <summary>
		/// 获取通知通道的名称。
		/// </summary>
		public string Channel
		{
			get
			{
				return _channel;
			}
		}
		#endregion
	}
}
