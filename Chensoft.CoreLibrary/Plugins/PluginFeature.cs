using System;
using System.Collections.Generic;


namespace Chensoft.Plugins
{
    public class PluginFeature
    {
        public bool IsMaster { get; set; }

        public string FullPath
        {
            get; set;
        }

        public string Path
        {
            get; set;
        }

        public List<Plugin> Plugins
        {
            get; set;
        }

        public List<Builtin> GetBuiltins()
        {
            List<Builtin> builtins = new List<Builtin>();
            foreach (var plugin in this.Plugins)
            {
                if (plugin.Builtins != null)
                    builtins.AddRange(plugin.Builtins);
            }
            return builtins;
        }



        public void SetPrivatePath()
        {
            string path = this.FullPath.Substring(AppDomain.CurrentDomain.BaseDirectory.Length);
            PluginUtils.AppendPrivatePath(path);
        }
    }
}