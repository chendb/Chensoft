using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Chensoft.Plugins
{
    public class Builtin
    {
        #region 公共属性
        public string Contract
        {
            get; set;
        }

        public string Type
        {
            get; set;
        }

        public Plugin Plugin { get; set; }
        #endregion

        public Type GetBuiltinType()
        {
            string name = this.Type.Split(',')[1].Trim();
            string typeName = this.Type.Split(',')[0].Trim();
            var fileName = Directory.GetFiles(this.Plugin.Feature.FullPath, name + ".dll", SearchOption.TopDirectoryOnly).FirstOrDefault();
            Assembly assembly = Assembly.LoadFile(fileName);
            return assembly.GetType(typeName, false, true);
        }
    }
}