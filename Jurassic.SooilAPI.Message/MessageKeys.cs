using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message
{
    public class MessageKeys
    {
        public static class DebugMessages
        {
            public static readonly MessageKey DebugMessage = new MessageKey(MessageType.DEBUG, "000");
        }
        public static class InfoMessages
        {
            public static readonly MessageKey ActionCompleted = new MessageKey(MessageType.INFO, "001");
        }
        public static class WarnMessages
        {
            public static readonly MessageKey CompletedWithWarn = new MessageKey(MessageType.WARN, "100");
        }
        public static class ValidationErrorMessages
        {
            public static readonly MessageKey MissingRequiredData = new MessageKey(MessageType.VALIDATION_ERROR, "200");
            public static readonly MessageKey InvalidDateRange = new MessageKey(MessageType.VALIDATION_ERROR, "201");
        }

        public static class ApplicationErrorMessages
        {
            public static readonly MessageKey RequiredDataNotFound = new MessageKey(MessageType.APPLICATION_ERROR, "400");
        }

        public static class SystemErrorMessages
        {
            public static readonly MessageKey UndefinedSystemException = new MessageKey(MessageType.SYSTEM_ERROR, "999");
        }
      
    }
}
