﻿using System;
using System.Runtime.Serialization;
using Jurassic.So.Infrastructure.Logging.Message;

namespace Jurassic.So.Infrastructure.Logging.ExtExceptions
{
    [Serializable]
    public class ExtSystemException: ExtBaseException
    {
          #region Constructors
        
        /// <summary>
        /// Protected constructor to de-serialize data
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ExtSystemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public ExtSystemException(
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

        public ExtSystemException(MessageKey messageKey, string[] messageVariables = null, object request = null, object response = null)
            : this(messageKey, null, messageVariables, request, response)
        {

        }

        #endregion
    }
}
