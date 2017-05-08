

using System;
using System.ComponentModel;

namespace Chensoft.Externals.Redis
{
	public enum RedisEntryType
	{
		None = 0,
		String = 1,
		List = 2,
		Set = 3,
		SortedSet = 4,
		Dictionary = 5,
	}
}
