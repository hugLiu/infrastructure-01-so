using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Message
{
    public enum MessageType
    {
        [Description("Type Code: 0")]
        UNASSIGNED = 0,
        [Description("Type Code: 1")]
        DEBUG = 1,
        [Description("Type Code: 2")]
        INFO = 2,
        [Description("Type Code: 3")]
        WARN = 3,
        [Description("Type Code: 4")]
        VALIDATION_ERROR = 4,
        [Description("Type Code: 5")]
        APPLICATION_ERROR = 5,
        [Description("Type Code: 6")]
        SYSTEM_ERROR = 6,
    }
}
