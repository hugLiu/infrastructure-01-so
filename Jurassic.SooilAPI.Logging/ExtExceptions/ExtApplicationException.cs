﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Message;

namespace Logging.ExtExceptions
{
    [Serializable]
    public class ExtApplicationException: ExtBaseException
    {
          #region Constructors

        /// <summary>
        /// Protected constructor to de-serialize data
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ExtApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        internal ExtApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public ExtApplicationException(
            MessageKey messageKey,
            Exception innerException,
            string[] messageVariables = null,
            object request = null,
            object response = null,
            string logonMethod = null,
            string logonUser = null)
            : base(
                messageKey,
                innerException,
                messageVariables,
                request,
                response,
                logonMethod,
                logonUser)
        {
        }

        public ExtApplicationException(MessageKey messageKey, string[] messageVariables = null, object request = null, object response = null)
            : this(messageKey, null, messageVariables, request, response)
        {

        }

        #endregion
    }
}
