using log4net.Layout;

namespace Jurassic.So.Infrastructure.Logging.ExtLayouts
{
    public class ExtLoggingLayout : PatternLayout
    {
        public ExtLoggingLayout()
        {
            this.AddConverter("extProperty", typeof(ExtLoggingLayoutConverter));
        }
    }
}
