

using System;
using System.Collections.Generic;

namespace Chensoft.Externals.Redis
{
    public class CollectionRemovedEventArgs : EventArgs
    {
        #region 成员字段
        private CollectionRemovedReason _reason;
        private object _item;
        #endregion

        #region 构造函数
        public CollectionRemovedEventArgs(CollectionRemovedReason reason, object item)
        {
            _reason = reason;
            _item = item;
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 获取被删除的原因。
        /// </summary>
        public CollectionRemovedReason Reason
        {
            get
            {
                return _reason;
            }
        }

        /// <summary>
        /// 获取被删除的集合元素。
        /// </summary>
        public object Item
        {
            get
            {
                return _item;
            }
        }
        #endregion
    }
}
