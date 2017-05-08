
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace Chensoft.Plugins
{
    public class Plugin
    {
        #region 私有变量
        public string Name
        {
            get; set;
        }

        public PluginFeature Feature { get; set; }

        public List<Builtin> Builtins { get; set; }



        public PluginManifest Manifest
        {
            get; set;
        }

        #endregion

        #region 外部方法

        public void Initizate(PluginFeature feature)
        {
            this.Feature = feature;
            if (this.Builtins == null) return;
            foreach (var builtin in this.Builtins)
            {
                builtin.Plugin = this;
            }
        }
        public IEnumerable<Assembly> GetAssemblies()
        {
            foreach (var assemly in this.Manifest.Assemblies)
            {
                var fileName = Path.Combine(this.Feature.FullPath, assemly);
                yield return Assembly.LoadFile(fileName);
            }
        }
        #endregion
    }
}