
using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Chensoft.Collections
{
	[Serializable]
	public class CategoryBase : HierarchicalNode
	{
		#region 成员字段
		private string _title;
		private string _description;
		private bool _visible;
		#endregion

		#region 构造函数
		protected CategoryBase()
		{
		}

		protected CategoryBase(string name)
			: this(name, name, string.Empty, true)
		{
		}

		protected CategoryBase(string name, string title, string description)
			: this(name, title, description, true)
		{
		}

		protected CategoryBase(string name, string title, string description, bool visible)
			: base(name)
		{
			_title = string.IsNullOrEmpty(title) ? name : title;
			_description = description;
			_visible = visible;
		}
		#endregion

		#region 公共属性
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}

		[DefaultValue(true)]
		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				_visible = value;
			}
		}
		#endregion
	}
}
