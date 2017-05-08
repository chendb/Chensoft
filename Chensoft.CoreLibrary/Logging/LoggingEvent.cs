using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chensoft.Logging
{
    public class LoggingEvent
    {
        public LoggingEvent()
        {
            Errors = new List<EventMessage>();
        }
        public string EventType
        {
            get;
            set;
        }
        public string ServerTag
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public string AppName
        {
            get;
            set;
        }
        public DateTime ErrorTime
        {
            get;
            set;
        }
        public List<EventMessage> Errors
        {
            get;
            set;
        }

    }
}
