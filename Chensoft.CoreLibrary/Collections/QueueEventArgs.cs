
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Chensoft.Collections
{
	[Serializable]
	public class QueueEventArgs : EventArgs
	{
		#region 成员变量
		private object _value;
		#endregion

		#region 构造函数
		public QueueEventArgs(object value)
		{
			_value = value;
		}
		#endregion

		#region 公共属性
		public object Value
		{
			get
			{
				return _value;
			}
		}
		#endregion
	}
}
