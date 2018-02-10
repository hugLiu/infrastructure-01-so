using log4net.Layout.Pattern;
using System;
using System.Reflection;

namespace Jurassic.So.Infrastructure.Logging.ExtLayouts
{
    public class ExtLoggingLayoutConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
        {
            if (Option != null)
            {
                // Write the value for the specified key
                WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            }
            else
            {
                // Write all the key value pairs
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }
        private object LookupProperty(string property, log4net.Core.LoggingEvent loggingEvent)
        {
            object propertyValue = string.Empty;
            if (loggingEvent.MessageObject != null)
            {
                try
                {
                    PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
                    if (propertyInfo != null)
                    {
                        propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);
                    }
                }
                catch (Exception) { }
            }
            return propertyValue;
        }
    }

}
