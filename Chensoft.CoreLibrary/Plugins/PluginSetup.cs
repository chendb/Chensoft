using System;
using System.IO;


namespace Chensoft.Plugins
{
    public class PluginSetup
    {
        #region 公共属性
        public string PluginsDirectoryName
        {
            get; set;
        }

        public string ApplicationDirectory
        {
            get; set;
        }
        #endregion

        #region 构造插件
        /// <summary>
        /// 构造插件设置对象。
        /// </summary>
        public PluginSetup()
            : this(null, null)
        {
        }

        /// <summary>
        /// 构造插件设置对象。
        /// </summary>
        /// <param name="applicationDirectory">应用程序目录完整限定路径。</param>
        public PluginSetup(string applicationDirectory)
            : this(applicationDirectory, null)
        {
        }

        public PluginSetup(string applicationDirectory, string pluginsDirectoryName)
        {

            if (string.IsNullOrEmpty(applicationDirectory))
            {
                this.ApplicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                this.ApplicationDirectory = applicationDirectory.Trim();

                if (!Path.IsPathRooted(this.ApplicationDirectory))
                    throw new ArgumentException("This value of 'applicationDirectory' parameter is invalid.");
            }

            if (string.IsNullOrEmpty(pluginsDirectoryName))
                this.PluginsDirectoryName = "plugins";
            else
            {
                this.PluginsDirectoryName = pluginsDirectoryName.Trim();

                if (Path.IsPathRooted(this.PluginsDirectoryName))
                {
                    if (this.ApplicationDirectory.StartsWith(PluginsDirectoryName))
                        PluginsDirectoryName = PluginsDirectoryName.Substring(ApplicationDirectory.Length);
                    else
                        throw new ArgumentException("This value of 'pluginsDirectoryName' parameter is invalid.");
                }
            }
        }

        #endregion
    }
}