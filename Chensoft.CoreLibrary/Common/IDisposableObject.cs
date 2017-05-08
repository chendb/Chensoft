using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chensoft.Common
{
    public interface IDisposableObject : System.IDisposable
    {
        event EventHandler<DisposedEventArgs> Disposed;

        bool IsDisposed
        {
            get;
        }
    }

    public class DisposedEventArgs : EventArgs
    {
        #region 成员字段
        private bool _disposing;
        #endregion

        #region 构造函数
        public DisposedEventArgs(bool disposing)
        {
            _disposing = disposing;
        }
        #endregion

        #region 公共属性
        public bool Disposing
        {
            get
            {
                return _disposing;
            }
        }
        #endregion
    }
}
