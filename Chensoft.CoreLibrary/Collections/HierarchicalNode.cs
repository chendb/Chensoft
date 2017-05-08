

using System;
using System.Collections.Generic;

namespace Chensoft.Collections
{
	/// <summary>
	/// 表示层次结构的节点类。
	/// </summary>
	public class HierarchicalNode
	{
		#region 成员变量
		private string _name;
		private string _path;
		private HierarchicalNode _parent;
		#endregion

		#region 构造函数
		protected HierarchicalNode()
		{
			_name = "/";
		}

		protected HierarchicalNode(string name) : this(name, null)
		{
		}

		protected HierarchicalNode(string name, HierarchicalNode parent)
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			if(Chensoft.Common.StringExtension.ContainsCharacters(name, @"./\*?!@#$%^&"))
				throw new ArgumentException("The name contains invalid character(s).");

			_name = name.Trim();
			_parent = parent;
		}
		#endregion

		#region 公共属性
		/// <summary>
		/// 获取层次结构节点的名称，名称不可为空或空字符串，根节点的名称固定为斜杠(即“/”)。
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}

		/// <summary>
		/// 获取层次结构节点的路径。
		/// </summary>
		/// <remarks>
		///		<para>如果为根节点则返回空字符串("")，否则即为父节点的全路径。</para>
		/// </remarks>
		public string Path
		{
			get
			{
				if(_path == null)
				{
					if(_parent == null)
						_path = string.Empty;
					else
						_path = _parent.FullPath;
				}

				return _path;
			}
		}

		/// <summary>
		/// 获取层次结构节点的完整路径，即节点路径与名称的组合。
		/// </summary>
		public string FullPath
		{
			get
			{
				var path = this.Path;

				switch(path)
				{
					case "":
						return _name;
					case "/":
						return path + _name;
					default:
						return path + "/" + _name;
				}
			}
		}

		/// <summary>
		/// 获取或设置层次结构节点的父节点，根节点的父节点为空(null)。
		/// </summary>
		internal protected HierarchicalNode InnerParent
		{
			get
			{
				return _parent;
			}
			set
			{
				if(object.ReferenceEquals(_parent, value))
					return;

				//设置父节点
				_parent = value;

				//更新层次结构节点的路径
				_path = null;
			}
		}
		#endregion
	}
}
