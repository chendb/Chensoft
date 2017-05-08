

using System;
using System.Collections;
using System.Collections.Generic;

namespace Chensoft.Collections
{
	public class BinaryComparer : IEqualityComparer, IEqualityComparer<byte[]>
	{
		public static readonly BinaryComparer Default = new BinaryComparer();

		public bool Equals(byte[] x, byte[] y)
		{
			if(x == null && y == null)
				return true;

			if(x == null || y == null || x.Length != y.Length)
				return false;

			for(int i = 0; i < x.Length; i++)
			{
				if(x[i] != y[i])
					return false;
			}

			return true;
		}

		public int GetHashCode(byte[] obj)
		{
			if(obj == null || obj.Length == 0)
				return 0;

			if(obj.Length == 4)
				return BitConverter.ToInt32(obj, 0);

			int result = 0;

			for(int i = 0; i < obj.Length; i++)
			{
				result ^= obj[i] << (i % 4) * 8;
			}

			return result;
		}

		public bool Equals(object x, object y)
		{
			if(x == null && y == null)
				return true;

			if(x == null || y == null || x.GetType() != typeof(byte[]) || y.GetType() != typeof(byte[]))
				return false;

			return this.Equals((byte[])x, (byte[])y);
		}

		public int GetHashCode(object obj)
		{
			if(obj == null || obj.GetType() != typeof(byte[]))
				return 0;

			return this.GetHashCode((byte[])obj);
		}
	}
}
