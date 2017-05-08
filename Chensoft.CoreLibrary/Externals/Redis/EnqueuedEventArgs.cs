using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chensoft.Externals.Redis
{
    [Serializable]
    public class EnqueuedEventArgs : EventArgs
    {
        #region 成员变量
        private object _value;
        private bool _isMultiple;
        #endregion

        #region 构造函数
        public EnqueuedEventArgs(object value, bool isMultiple)
        {
            _value = value;
            _isMultiple = isMultiple;
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 获取入队的内容值。
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
        }

        /// <summary>
        /// 获取一个指示本次入队是否为批量出队操作，如果为批量入队则<see cref="Value"/>属性返回的则是多值。
        /// </summary>
        public bool IsMultiple
        {
            get
            {
                return _isMultiple;
            }
        }
        #endregion
    }
}
