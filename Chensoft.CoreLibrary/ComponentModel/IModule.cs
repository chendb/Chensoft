using System;

namespace Chensoft.ComponentModel
{
    /// <summary>
    /// 模块接口
    /// </summary>
    public interface IModule : IDisposable
    {
        void Initizate(object content);
    }
}