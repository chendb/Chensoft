using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chensoft.Common
{
    public interface ISequence
    {
        /// <summary>
        /// 获取指定序列号的下一个数值，递增步长以指定名称的序列号设置为准。
        /// </summary>
        /// <param name="name">指定要获取的序列号名称。</param>
        /// <returns>返回的序列号数值。</returns>
        long GetSequenceNumber(string name);

        /// <summary>
        /// 获取指定序列号的数值。
        /// </summary>
        /// <param name="name">指定要获取的序列号名称。</param>
        /// <param name="interval">指定的序列号递增的步长数，如果为零则表示获取当前值，而不会引发递增。</param>
        /// <param name="seed">初始化的种子数，如果指定名称的序列号不存在，则创建指定名称的序列号，并使用该种子数作为该序列号的初始值。</param>
        /// <returns>返回的序列号数值。</returns>
        long GetSequenceNumber(string name, int interval, int seed = 0);

        /// <summary>
        /// 获取指定序列号的下一个格式化文本值，递增步长以指定名称的序列号设置为准。
        ///		<para>
        ///			有关格式化文本的用法请参考<see cref="SequenceInfo"/>类的<see cref="SequenceInfo.FormatString"/>属性说明。
        ///		</para>
        /// </summary>
        /// <param name="name">指定要获取的序列号名称。</param>
        /// <returns>返回的序列号文本值。</returns>
        string GetSequenceString(string name);

        /// <summary>
        /// 获取指定序列号的格式化文本值，有关格式化文本的用法请参考<see cref="SequenceInfo"/>类的<see cref="SequenceInfo.FormatString"/>属性说明。
        /// </summary>
        /// <param name="name">指定要获取的序列号名称。</param>
        /// <param name="interval">指定的序列号递增的步长数，如果为零则表示获取当前值，而不会引发递增。</param>
        /// <param name="seed">初始化的种子数，如果指定名称的序列号不存在，则创建指定名称的序列号，并使用该种子数作为该序列号的初始值。</param>
        /// <param name="formatString">初始化的格式化文本，如果指定名称的序列号不存在，则创建指定名称的序列号，并使用该格式化文本作为该序列号的格式化文本。</param>
        /// <returns>返回的序列号文本值。</returns>
        string GetSequenceString(string name, int interval, int seed = 0, string formatString = null);

        /// <summary>
        /// 获取指定名称的序列号信息。
        /// </summary>
        /// <param name="name">指定要获取的序列号名称。</param>
        /// <returns>返回成功的<see cref="SequenceInfo"/>序列号信息，如果指定名称的序列号不存在则返回空(null)。</returns>
        SequenceInfo GetSequenceInfo(string name);

        /// <summary>
        /// 重置指定名称的序列号设置，如果指定名称的序列号不存在则创建它。
        /// </summary>
        /// <param name="name">指定要重置的序列号名称。</param>
        /// <param name="value">指定要重置的序列号当前值，默认为零。</param>
        /// <param name="interval">指定要重置的序列号递增步长值，默认为1。</param>
        /// <param name="formatString">指定要重置的序列号格式化文本，默认为空(null)。</param>
        void Reset(string name, int value = 0, int interval = 1, string formatString = null);
    }
}
