using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Message;

namespace Logging.ExtExceptions
{
    [Serializable]
    public class ExtBaseException: ApplicationException
    {
         #region Declare Member Variables
        // Member variable declarations

        private MessageData richMessage;

        // Collection provided to store any extra information associated with the exception.
        private NameValueCollection additionalInformation = new NameValueCollection();

        #endregion

        #region Constructors
        protected ExtBaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
        protected ExtBaseException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

         protected ExtBaseException(
            MessageKey messageKey,
            Exception innerException,
            string[] messageVariables = null,
            object request = null,
            object response = null,
            string logonMethod = null, 
            string logonUser = null)
            : this(MessageCacheManager.GetMessage(messageKey, messageVariables), innerException, messageVariables, request, response, logonMethod, logonUser)
        {
        }

        protected ExtBaseException(
            MessageData message,
            Exception innerException,
            string[] messageVariables = null,
            object request = null,
            object response = null,
            string logonMethod = null,
            string logonUser = null):base(message.Description,innerException)
        {
            richMessage = message;

            this.Request = request;
            this.Response = response;
            this.LogonMethod = logonMethod;
            this.LogonUser = logonUser;
        }

        #endregion

        #region Public Properties
        public object Request { get; set; }
        public object Response { get; set; }

        public string LogonMethod { get; set; }
        public string LogonUser { get; set; }

        private string GetMessageCode()
        {
            string messageCode = string.Empty;

            if (richMessage != null && richMessage.Key != null)
            {
                messageCode = richMessage.Key.Code;
            }

            return messageCode;
        }
        /// <summary>
        /// Rich message
        /// </summary>
        public MessageData RichMessage
        {
            get { return richMessage; }
        }

        /// <summary>
        /// attached innerException stacktrace information 
        /// </summary>
        public override string StackTrace
        {
            get
            {
                if (InnerException == null)
                {
                    return base.StackTrace;
                }
                else
                {
                    string messageContent = InnerException is ExtBaseException ?
                        (InnerException as ExtBaseException).RichMessage.Description : InnerException.Message;
                    return string.Format("{0}" + Environment.NewLine + "InnerException({1}):{2}" + Environment.NewLine + "{3}",
                        base.StackTrace, InnerException.GetType().Name, messageContent, InnerException.StackTrace);
                }
            }
        }
        #endregion
    }
}
