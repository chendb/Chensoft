

using System;
using System.Collections.Generic;

namespace Chensoft.Collections
{
	public class HierarchicalNodeCollection<T> : NamedCollectionBase<T> where T : HierarchicalNode
	{
		#region 成员变量
		private T _owner;
		#endregion

		#region 构造函数
		protected HierarchicalNodeCollection(T owner)
		{
			if(owner == null)
				throw new ArgumentNullException("owner");

			_owner = owner;
		}
		#endregion

		#region 重写方法
		protected override string GetKeyForItem(T item)
		{
			return item.Name;
		}

		protected override void InsertItem(int index, T item)
		{
			item.InnerParent = _owner;
			base.InsertItem(index, item);
		}

		protected override void SetItem(int index, T item)
		{
			item.InnerParent = _owner;
			base.SetItem(index, item);
		}
		#endregion
	}
}
