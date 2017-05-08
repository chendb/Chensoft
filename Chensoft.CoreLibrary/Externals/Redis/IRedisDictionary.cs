

using System;
using System.Collections.Generic;

namespace Chensoft.Externals.Redis
{
	public interface IRedisDictionary : IDictionary<string, string>
	{
		/// <summary>
		/// 获取当前<seealso cref="IRedisDictionary"/>字典的名称。
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// 批量设置指定的键值对集合到字典中，如果指定键值对的键已存在则覆盖保存。
		/// </summary>
		/// <param name="items">指定的要批量设置的键值对集。</param>
		void SetRange(IEnumerable<KeyValuePair<string, string>> items);

		/// <summary>
		/// 尝试新增一个指定的键值对，如果指定的键已存在则不执行任何操作并返回假(false)。
		/// </summary>
		/// <param name="key">要新增的键。</param>
		/// <param name="value">要新增的值。</param>
		/// <returns>如果新增成功则返回真(true)，否则返回假(false)。</returns>
		bool TryAdd(string key, string value);

		IReadOnlyList<string> GetValues(params string[] keys);

		IReadOnlyDictionary<string, string> GetAllEntries();

		long Increment(string key, int interval = 1);
		long Decrement(string key, int interval = 1);
	}
}
