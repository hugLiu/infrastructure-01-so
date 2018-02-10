using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.Infrastructure.Logging.Message
{
    [Serializable]
    public class MessageData
    {
        private MessageKey key;
        private string description = string.Empty;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageData(MessageKey key)
        {
            this.key = key;
        }

        public MessageData(MessageData msg)
            : this(msg.key)
        {
            this.description = msg.description;
        }

        public MessageKey Key
        {
            get { return key; }
            set { key = (value ?? new MessageKey()); }
        }

        public string Description
        {
            get { return description; }
            set { description = (value ?? string.Empty); }
        }

        public override string ToString()
        {
            return ("[" + key.ToString() + "] " + description).Trim();
        }

        internal void FormatDescription(string[] variables)
        {
            if (variables != null && variables.Length > 0)
            {
                Description = string.Format(Description, variables);
            }
        }

        /// <summary>
        /// Get Message Code
        /// </summary>
        /// <returns></returns>
        public string GetMessageCode()
        {
            return key.Code;
        }

        /// <summary>
        /// Get abbreviate of Message Type 
        /// </summary>
        /// <returns></returns>
        public string GetMessageType()
        {
            string type = string.Empty;
            switch (Key.Type)
            {
                case MessageType.VALIDATION_ERROR:
                    type = "VAL";
                    break;
                case MessageType.APPLICATION_ERROR:
                    type = "APP";
                    break;
                case MessageType.SYSTEM_ERROR:
                    type = "SYS";
                    break;
                case MessageType.INFO:
                    type = "INF";
                    break;
                case MessageType.WARN:
                    type = "WRN";
                    break;
                case MessageType.DEBUG:
                    type = "DEB";
                    break;
            }
            return type;
        }

    }
}
