

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Chensoft.Collections
{
	[Serializable]
	public class CategoryCollection : HierarchicalNodeCollection<Category>
	{
		#region 构造函数
		public CategoryCollection(Category owner) : base(owner)
		{
		}
		#endregion

		#region 公共方法
		public Category Add(string name, string title, string description)
		{
			var category = new Category(name, title, description);
			this.Add(category);
			return category;
		}
		#endregion
	}
}
