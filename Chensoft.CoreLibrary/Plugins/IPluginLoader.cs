using System;
using System.Collections.Generic;


namespace Chensoft.Plugins
{
    public interface IPluginLoader
    {
        string PluginRootDir { get; set; }

        List<PluginFeature> Features { get; }

        void Install(PluginSetup pluginSetup);

        void Uninstall(Plugin plugin);
    }
}