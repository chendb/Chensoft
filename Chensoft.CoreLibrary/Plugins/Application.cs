using Chensoft.ComponentModel;
using Chensoft.Services;
using Chensoft.Terminals;
using log4net;
using Microsoft.Practices.ServiceLocation;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Chensoft.Plugins
{
    public class Application
    {
        #region 私有变量
        private static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(Application));

        private static Application _default;

        #endregion

        #region 公共属性

        public DateTime SetupTime { get; set; }
        public ApplicationContext Context { get; set; }

        public static ILog Log
        {
            get
            {
                return _log;
            }
        }

        public static Application Default
        {
            get
            {
                if (_default == null)
                    System.Threading.Interlocked.CompareExchange(ref _default, new Application(), null);
                return _default;
            }
        }

        #endregion

        #region 构造函数

        public Application()
        {
            this.Context = new ApplicationContext();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        /// <summary>  
        /// 对外解析dll失败时调用  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="args"></param>  
        /// <returns></returns>  
        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string name = args.Name.Split(',')[0];

            var assembly = PluginUtils.GetAssemblyFormPath(name, this.Context.PluginRootDir);
            if (assembly != null) return assembly;
            foreach (var feature in this.Context.Features)
            {
                assembly = PluginUtils.GetAssemblyFormFeature(name, feature);
                if (assembly != null) return assembly;
            }
            return null;
        }



        #endregion

        #region 入口方法

        public void Start(ApplicationContext context)
        {
            this.Context = context;
            this.Context.Initizate();
            try
            {
                var modules = ServiceLocator.Current.GetAllInstances<IModule>();
                foreach (var module in modules)
                {
                    module.Initizate(this.Context);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            this.SetupTime = DateTime.Now;

            var workers = ServiceLocator.Current.GetAllInstances<IWorker>();
            foreach (var worker in workers)
            {
                _log.Info(String.Format("{0}发现Worker   -->{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), worker.Name));
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        worker.Start();
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex.Message);
                    }
                });
            }

        }
        public void Start()
        {
            Start(new ApplicationContext());
        }
        #endregion
    }
}