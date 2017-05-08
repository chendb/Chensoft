
using System;
using System.Collections.Generic;
using System.IO;


namespace Chensoft.Plugins
{
    public class PluginLoader : MarshalByRefObject
    {
        #region 公共属性

        public string PluginRootDir { get; set; }
        public List<PluginFeature> Features
        {
            get; set;
        }

        #endregion

        #region 接口方法
        public void Load(PluginSetup pluginSetup)
        {
            this.Features = new List<PluginFeature>();
            this.PluginRootDir = Path.Combine(pluginSetup.ApplicationDirectory, pluginSetup.PluginsDirectoryName);
            if (!Directory.Exists(this.PluginRootDir)) return;
            if (ExitsPlugin(PluginRootDir))
            {
                var masterFeature = ResolveFeature(pluginSetup.ApplicationDirectory, pluginSetup.PluginsDirectoryName, true);

                this.Features.Add(masterFeature);
            }
            else
            {
                PluginUtils.AppendPrivatePath(pluginSetup.PluginsDirectoryName);
            }

            string[] dirs = Directory.GetDirectories(PluginRootDir);
            foreach (var dir in dirs)
            {
                if (ExitsPlugin(dir))
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    var feature = ResolveFeature(di.Parent.FullName, di.Name);
                    this.Features.Add(feature);
                }
            }
        }

        public void Unload(Plugin plugin)
        {
            foreach (var dir in this.Features)
            {
                dir.Plugins.Remove(plugin);
            }
        }

        #endregion

        #region 辅助方法
        private bool ExitsPlugin(string dir)
        {
            var files = Directory.GetFiles(dir, "*.plugin");
            return files != null && files.Length > 0;
        }

        private PluginFeature ResolveFeature(string fullPath, string directoryName, bool isMaster = false)
        {
            var dir = new PluginFeature()
            {
                FullPath = Path.Combine(fullPath, directoryName),
                Path = directoryName,
                IsMaster = isMaster
            };
            dir.Plugins = new List<Plugin>(ResolvePlugins(dir));
            dir.SetPrivatePath();
            return dir;
        }

        private IEnumerable<Plugin> ResolvePlugins(PluginFeature feature)
        {
            List<Plugin> plugins = new List<Plugin>();
            string[] files = Directory.GetFiles(feature.FullPath, "*.plugin", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var plugin = PluginUtils.Get<Plugin>(file);
                plugin.Initizate(feature);
                yield return plugin;
            }
        }
        #endregion
    }
}