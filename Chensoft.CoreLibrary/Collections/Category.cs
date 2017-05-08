
using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

namespace Chensoft.Collections
{
	[Serializable]
	public class Category : CategoryBase
	{
		#region ��Ա�ֶ�
		private CategoryCollection _children;
		#endregion

		#region ���캯��
		public Category()
		{
		}

		public Category(string name) : this(name, name, string.Empty, true)
		{
		}

		public Category(string name, string title, string description) : this(name, title, description, true)
		{
		}

		public Category(string name, string title, string description, bool visible) : base(name, title, description, visible)
		{
		}
		#endregion

		#region ��������
		public Category Parent
		{
			get
			{
				return (Category)base.InnerParent;
			}
		}

		public CategoryCollection Children
		{
			get
			{
				if(_children == null)
					System.Threading.Interlocked.CompareExchange(ref _children, new CategoryCollection(this), null);

				return _children;
			}
		}
		#endregion

		#region ��������
		public Category[] GetVisibleChildren()
		{
			var children = _children;

			if(children == null || children.Count <= 0)
				return new Category[0];

			var visibleCategories = new List<Category>(children.Count);

			foreach(Category category in children)
			{
				if(category.Visible)
					visibleCategories.Add(category);
			}

			return visibleCategories.ToArray();
		}
		#endregion
	}
}
