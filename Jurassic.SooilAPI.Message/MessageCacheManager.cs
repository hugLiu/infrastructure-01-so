using Microsoft.Practices.EnterpriseLibrary.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message
{
    public static class MessageCacheManager
    {
        public static string CacheManageName = "Message Cache Manager";

        private static ICacheManager cacheManager = null;
        private static object objLock = new object();

        static MessageCacheManager()
        {
            cacheManager = CacheFactory.GetCacheManager();
        }

        #region Get Messages collection for the specified Culture
        private static IDictionary<MessageKey, MessageData> GetCachedMessages()
        {
            if (!cacheManager.Contains(CacheManageName))
            {
                lock (objLock)
                {
                    if (!cacheManager.Contains(CacheManageName))
                    {
                        IDictionary<MessageKey, MessageData> messages = LoadMessages();
                        if (messages == null)
                            messages = new Dictionary<MessageKey, MessageData>();

                        cacheManager.Add(CacheManageName, messages);
                    }
                }
            }

            return (IDictionary<MessageKey, MessageData>)cacheManager[CacheManageName];
        }

        private static IDictionary<MessageKey, MessageData> LoadMessages()
        {
            MessageLoader loader = new MessageLoader();
            return loader.LoadMessages();
        }


        #endregion

        #region Load Messages into MessageCacheManager with specified MessageLoader and culture
     

      
        #endregion

        #region Get Message from specified MessageKey

        public static MessageData GetMessage(MessageKey messageKey)
        {
            MessageData msg = null;
            IDictionary<MessageKey, MessageData> messages = GetCachedMessages();
            if (messages != null && messages.ContainsKey(messageKey))
                msg = messages[messageKey];

            if (msg == null)
                msg = new MessageData(messageKey);
            else
                msg = new MessageData(msg);

            return msg;
        }

        public static MessageData GetMessage(MessageKey messageKey, string[] messageVariables)
        {
            MessageData msg = GetMessage(messageKey);

            if (messageVariables != null && messageVariables.Length > 0)
            {
                msg.FormatDescription(messageVariables);
            }
            return msg;
        }
        #endregion
    }
}
