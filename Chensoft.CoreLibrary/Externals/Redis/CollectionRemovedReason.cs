using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chensoft.Externals.Redis
{
    /// <summary>
    /// 表示集合元素被删除的原因。
    /// </summary>
    public enum CollectionRemovedReason
    {
        /// <summary>通过删除方法。</summary>
        [Description("${Text.CollectionRemovedReason.Remove}")]
        Remove,

        /// <summary>因为集合溢出而激发的自动删除。</summary>
        [Description("${Text.CollectionRemovedReason.Overflow}")]
        Overflow,

        /// <summary>因为缓存项过期而被删除。</summary>
        [Description("${Text.CollectionRemovedReason.Expired}")]
        Expired,

        /// <summary>其他原因。</summary>
        [Description("${Text.CollectionRemovedReason.Other}")]
        Other,
    }
}
