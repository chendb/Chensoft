/*
 * Authors:
 *   钟峰(Popeye Zhong) <Chensoft@gmail.com>
 *
 * Copyright (C) 2014-2015 Chensoft Corporation <http://www.Chensoft.com>
 *
 * This file is part of Chensoft.Externals.Redis.
 *
 * Chensoft.Externals.Redis is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * Chensoft.Externals.Redis is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with Chensoft.Externals.Redis; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chensoft.Externals.Redis
{
	internal static class Utility
	{
		public static T ConvertValue<T>(object value)
		{
			if(value == null)
				return default(T);

			if(typeof(T) == typeof(string) || Chensoft.Common.TypeExtension.IsScalarType(typeof(T)))
				return Chensoft.Common.Convert.ConvertValue<T>(value);
            
			if(value is string)
				return Newtonsoft.Json.JsonConvert.DeserializeObject<T>((string)value);

			//强制转换，可能会导致无效转换异常
			return (T)value;
		}

		public static string GetStoreString(object value)
		{
			string text;

			if(Chensoft.Common.TypeExtension.IsScalarType(value.GetType()))
				text = value.ToString();
			else
				text = Newtonsoft.Json.JsonConvert.SerializeObject(value);

			return text;
		}
	}
}
