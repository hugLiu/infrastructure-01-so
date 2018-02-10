using log4net;
using System;
using Logging.ExtExceptions;
using Message;
using Logging.ExtLayouts;
using System.IO;

namespace Logging
{
    public static class Logger
    {
        private static readonly ILog logger;
        private const string UNKNOW_EXCEPTION = "UNKNOW_EXCEPTION";

        static Logger()
        {
            logger = LogManager.GetLogger(typeof(Logger));
            log4net.Config.XmlConfigurator.Configure();
        }

        private static ExtLogContent GetExtLogContent(ExtBaseException extException)
        {
            return new ExtLogContent(extException.RichMessage)
            {
                Request = extException.Request != null ? extException.Request.ToString() : string.Empty,
                Response = extException.Response != null ? extException.Response.ToString() : string.Empty,
                LogonMethod = extException.LogonMethod,
                LogonUser = extException.LogonUser,
            };
        }

        private static MessageData GetMessageData(MessageKey messageKey, string[] messageVariables)
        {
            return MessageCacheManager.GetMessage(messageKey, messageVariables);
        }

        private static string GetString(object obj)
        {
            string str = string.Empty;

            if (obj is Stream)
            {
                var stream = obj as Stream;
                stream.Seek(0, SeekOrigin.Begin);
                str = new StreamReader(stream).ReadToEnd();
            }
            else if (obj is string)
            {
                str = obj as string;
            }

            return str;
        }

        [Obsolete]
        public static void LogRequest(object request)
        {
            var extContent = new ExtLogContent()
            {
                Request = GetString(request)
            };

            logger.Info(extContent);
        }
        [Obsolete]
        public static void LogResponse(object response)
        {
            var extContent = new ExtLogContent()
            {
                Response = GetString(response)
            };

            logger.Info(extContent);
        }
        /// <summary>
        /// 同时记录请求和返回结果
        /// addby:shiyaru
        /// date:20151112
        /// </summary>
        /// <param name="response"></param>
        /// <param name="request"></param>
        public static void LogRpRq(object response, object request,string name)
        {
            var extContent = new ExtLogContent()
            {
                Request = GetString(request),
                Response = GetString(response),
                LogonUser = name
            };
            logger.Info(extContent);
        }

        public static void ErrorRp(Exception exception, string url,string name)
        {
            var model = new ExtLogContent
            {
                Request = url,
                LogonUser = name,
                MessageText = exception.Message,
                Message = exception.StackTrace
            };
            logger.Error(model);
        }

        public static void DataRp(object response, object request, string name)
        {
            var extContent = new ExtLogContent()
            {
                Request = GetString(request),
                Response = GetString(response),
                LogonUser = name
            };
            logger.Warn(extContent);
        }

        public static void Error(Exception exception)
        {
            ExtBaseException extException = null;

            if (exception is ExtBaseException)
            {
                var extContent = GetExtLogContent(exception as ExtBaseException);
                logger.Error(extContent, null);
            }
            else
            {
                logger.Error(UNKNOW_EXCEPTION, exception);
            }
        }

        public static void Error(string message)
        {
            logger.Error(message);
        }

        public static void Error(MessageKey messageKey, string[] messageVariables = null)
        {
            logger.Error(GetMessageData(messageKey, messageVariables).ToString());
        }

        public static void Debug(Exception exception)
        {
            ExtLogContent extContent = null;
            ExtBaseException extException = null;

            if (exception is ExtBaseException)
            {
                extContent = GetExtLogContent(exception as ExtBaseException);
                logger.Debug(extContent, extException);
            }
            else
            {
                logger.Debug(UNKNOW_EXCEPTION, exception);
            }

        }

        public static void Debug(string message)
        {
            logger.Error(message);
        }

        public static void Debug(MessageKey messageKey, string[] messageVariables = null)
        {
            logger.Debug(GetMessageData(messageKey, messageVariables).ToString());
        }

        public static void Info(Exception exception)
        {
            ExtLogContent extContent = null;
            ExtBaseException extException = null;

            if (exception is ExtBaseException)
            {
                extContent = GetExtLogContent(exception as ExtBaseException);
                logger.Info(extContent, extException);
            }
            else
            {
                logger.Info(UNKNOW_EXCEPTION, exception);
            }

        }

        public static void Info(string message)
        {
            logger.Info(message);
        }

        public static void Info(MessageKey messageKey, string[] messageVariables = null)
        {
            logger.Info(GetMessageData(messageKey, messageVariables).ToString());
        }

        public static void Warn(Exception exception)
        {
            ExtLogContent extContent = null;
            ExtBaseException extException = null;

            if (exception is ExtBaseException)
            {
                extContent = GetExtLogContent(exception as ExtBaseException);
                logger.Warn(extContent, extException);
            }
            else
            {
                logger.Warn(UNKNOW_EXCEPTION, exception);
            }
        }

        public static void Warn(string message)
        {
            logger.Warn(message);
        }

        public static void Warn(MessageKey messageKey, string[] messageVariables = null)
        {
            logger.Warn(GetMessageData(messageKey, messageVariables).ToString());
        }

        public static void Fatal(Exception exception)
        {
            ExtLogContent extContent = null;
            ExtBaseException extException = null;

            if (exception is ExtBaseException)
            {
                extContent = GetExtLogContent(exception as ExtBaseException);
                logger.Fatal(extContent, extException);
            }
            else
            {
                logger.Fatal(UNKNOW_EXCEPTION, exception);
            }
        }

        public static void Fatal(string message)
        {
            logger.Fatal(message);
        }

        public static void Fatal(MessageKey messageKey, string[] messageVariables = null)
        {
            MessageData message = MessageCacheManager.GetMessage(messageKey, messageVariables);

            logger.Fatal(message.GetMessageCode() + " " + message.Description);
        }
    }
}
