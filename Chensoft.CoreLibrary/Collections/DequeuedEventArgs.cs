
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Chensoft.Collections
{
	[Serializable]
	public class DequeuedEventArgs : QueueEventArgs
	{
		#region 成员变量
		private DequeuedReason _reason;
		#endregion

		#region 构造函数
		public DequeuedEventArgs(object value, DequeuedReason reason) : base(value)
		{
			_reason = reason;
		}
		#endregion

		#region 公共属性
		public DequeuedReason Reason
		{
			get
			{
				return _reason;
			}
		}
		#endregion
	}
}
