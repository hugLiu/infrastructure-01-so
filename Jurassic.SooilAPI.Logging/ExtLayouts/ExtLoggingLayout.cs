using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logging.ExtLayouts;

namespace Logging.ExtLayouts
{
    public class ExtLoggingLayout : PatternLayout
    {
        public ExtLoggingLayout()
        {
            this.AddConverter("extProperty", typeof(ExtLoggingLayoutConverter));
        }
    }
}
