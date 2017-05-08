using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chensoft.Services
{
    /// <summary>
    /// 关于工作器的接口。
    /// </summary>
    public interface IWorker
    {
        /// <summary>表示已经启动。</summary>
        event EventHandler Started;
        /// <summary>表示准备启动。</summary>
        event CancelEventHandler Starting;

        /// <summary>表示已经停止。</summary>
        event EventHandler Stopped;
        /// <summary>表示准备停止。</summary>
        event CancelEventHandler Stopping;

        /// <summary>
        /// 获取当前工作器的名称。
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取当前工作器的状态。
        /// </summary>
        WorkerState State
        {
            get;
        }

        /// <summary>
        /// 获取或设置是否禁用工作器。
        /// </summary>
        bool Disabled
        {
            get;
            set;
        }

        /// <summary>
        /// 启动工作器。
        /// </summary>
        /// <param name="args">启动的参数。</param>
        void Start(string[] args);

        /// <summary>
        /// 启动工作器。
        /// </summary>
        void Start();

        /// <summary>
        /// 停止工作器。
        /// </summary>
        void Stop();
    }
}
