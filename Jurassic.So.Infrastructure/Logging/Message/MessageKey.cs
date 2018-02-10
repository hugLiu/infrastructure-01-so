using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.Infrastructure.Logging.Message
{
    public class MessageKey
    {
        private MessageType type = MessageType.UNASSIGNED;
        private string code = string.Empty;


        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageKey()
        {
        }

        /// <summary>
        /// Two-parm constructor
        /// </summary>
        /// <param name="type">Message type</param>
        /// <param name="code">Message code</param>
        public MessageKey(MessageType type, string code)
        {
            this.type = type;
            this.code = code;
        }

        public MessageType Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = (value == null ? string.Empty : value); }
        }

        #region Overrides

        public override string ToString()
        {
            return (type.ToString() + " " + code);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;
            MessageKey compare = obj as MessageKey;
            if (compare != null)
            {
                equals = (type.Equals(compare.type) && code.Equals(compare.code));
            }
            return equals;
        }

        /// <summary>
        /// GetHashCode() override.
        /// </summary>
        /// <returns>Hash code based on object attributes</returns>
        public override int GetHashCode()
        {
            return (type.GetHashCode() ^ code.GetHashCode());
        }

        #endregion Overrides

    }
}
