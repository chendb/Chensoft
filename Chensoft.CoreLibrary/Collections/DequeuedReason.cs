
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chensoft.Collections
{
	/// <summary>
	/// 表示出队的原因。
	/// </summary>
	public enum DequeuedReason
	{
		/// <summary>调用出队方法。</summary>
		Calling,

		/// <summary>因为队列溢出而激发的自动出队。</summary>
		Overflow,
	}
}
