using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chensoft.Logging
{
    public class LoggingEventCache
    {
        private static LoggingEventCache _default;

        public Queue<LoggingEvent> Queue { get; set; }

        public LoggingEventCache(int capacity)
        {
            this.Queue = new Queue<LoggingEvent>(100);
        }

        public static LoggingEventCache Instance
        {
            get
            {
                return _default;
            }
        }


        public static LoggingEventCache GetInstance(int capacity)
        {
            Interlocked.CompareExchange(ref _default, new LoggingEventCache(capacity), null);
            return _default;
        }


    }
}
