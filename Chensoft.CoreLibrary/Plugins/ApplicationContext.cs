using Microsoft.Mef.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Threading;

namespace Chensoft.Plugins
{
    /// <summary>
    /// 应用程序上下文
    /// </summary>
    public class ApplicationContext
    {
        #region 私有变量
        private PluginLoader _pluginLoader;
        #endregion

        #region 公共属性

        public AggregateCatalog AggregateCatalog { get; set; }

        public CompositionContainer Container { get; set; }

        public IServiceLocator Locator { get; set; }

        public PluginLoader Loader
        {
            get
            {
                if (_pluginLoader == null)
                    Interlocked.CompareExchange(ref _pluginLoader, new PluginLoader(), null);

                return _pluginLoader;
            }
        }

        public string PluginRootDir
        {
            get
            {
                return this.Loader.PluginRootDir;
            }
        }

        public List<PluginFeature> Features
        {
            get
            {
                return this.Loader.Features;
            }
        }

        #endregion


        #region 初始化操作
        public void Initizate()
        {
            Loader.Load(this.CreatePluginSetup());

            CreateContainer();

        }

        /// <summary>
        /// 在指定的容器中组合部件
        /// </summary>
        /// <param name="attributedParts"></param>
        public void ComposeParts(params object[] attributedParts)
        {
            this.Container.ComposeParts(attributedParts);
        }

        #endregion


        #region 内部方法

        private void CreateContainer()
        {
            this.AggregateCatalog = new AggregateCatalog();

            var rootCatalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);
            this.AggregateCatalog.Catalogs.Add(rootCatalog);
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(this.GetType().Assembly));
            foreach (var dir in this.Features)
            {
                InstallCatalog(dir);
            }

            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(this.GetType().Assembly));

            Container = new CompositionContainer(this.AggregateCatalog, new PluginExportProvider(this));
            Container.ComposeParts(this);
            this.Locator = new MefServiceLocator(this.Container);
            ServiceLocator.SetLocatorProvider(() => this.Locator);
        }

        private void InstallCatalog(PluginFeature dir)
        {
            foreach (var plugin in dir.Plugins)
            {
                foreach (var assembly in plugin.GetAssemblies())
                {
                    this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(assembly));
                }
            }
        }

        /// <summary>
        /// 创建插件启动配置对象。
        /// </summary>
        /// <returns>返回创建成功的插件启动配置对象。</returns>
        /// <remarks></remarks>
        protected virtual PluginSetup CreatePluginSetup()
        {
            return new PluginSetup(AppDomain.CurrentDomain.BaseDirectory);
        }
        #endregion
    }
}