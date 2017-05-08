
using log4net.Appender;
using log4net.Core;
using log4net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using System.Net;
using System.Net.Sockets;

namespace Chensoft.Logging
{
    /// <summary>
    /// <code>
    /// <appender name="MemoryQueueAppender" type="Chensoft.Logging.MemoryQueueAppender,Chensoft.CoreLibrary">
    ///  <param name="ServerTag" value="192.168.0.1"/>
    ///  <param name="AppName" value="test"/>
    ///  <param name="MaxRecords" value="1000"/>
    /// </appender>
    /// </code>
    /// </summary>
    public class MemoryQueueAppender : AppenderSkeleton
    {
        #region 私有变量
        private readonly static Type declaringType = typeof(MemoryQueueAppender);
        private static object _lock = new object();
        #endregion

        #region 构造函数
        public MemoryQueueAppender()
        {
            MaxRecords = 300;
        }
        #endregion

        #region 公共属性

        /// <summary>
        /// 服务标签
        /// </summary>
        public string ServerTag
        {
            get;
            set;
        }

        /// <summary>
        /// 应用程序名称
        /// </summary>
        public string AppName
        {
            get;
            set;
        }

        /// <summary>
        /// 是大记录数
        /// </summary>
        public int MaxRecords
        {
            get;
            set;
        }


        #endregion

        #region 重载方法
        protected override void Append(global::log4net.Core.LoggingEvent loggingEvent)
        {
            try
            {

                LoggingEvent le = new LoggingEvent();
                le.ErrorTime = loggingEvent.TimeStamp;
                le.EventType = loggingEvent.Level.ToString();
                le.Message = loggingEvent.RenderedMessage;
                le.AppName = AppName;
                le.ServerTag = GetHostIP();
                AppendError(le, loggingEvent.ExceptionObject);
                Add(le);
            }
            catch (Exception e_)
            {
                LogLog.Error(declaringType, e_.Message);
            }
        }

        #endregion

        #region 静态方法
        public static string GetHostIP()
        {
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    return (ipa.ToString());
            }
            return null;
        }
        #endregion

        #region 辅助方法
        private void AppendError(LoggingEvent e, Exception err)
        {
            if (err != null)
            {
                e.Errors.Add(new EventMessage { Message = err.Message, StackTrace = err.StackTrace });
                err = err.InnerException;
                while (err != null)
                {
                    e.Errors.Add(new EventMessage { Message = err.Message, StackTrace = err.StackTrace });
                    err = err.InnerException;
                }
            }

        }


        private Queue<LoggingEvent> GetQueue()
        {
            return LoggingEventCache.GetInstance(MaxRecords).Queue;
        }


        private void Add(LoggingEvent item)
        {

            lock (_lock)
            {
                GetQueue().Enqueue(item);
                if (GetQueue().Count > MaxRecords)
                    GetQueue().Dequeue();
            }
        }

        #endregion

    }
}
