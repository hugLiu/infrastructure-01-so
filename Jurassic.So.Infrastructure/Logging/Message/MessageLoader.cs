using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jurassic.So.Infrastructure.Logging.Message
{
    public class MessageLoader
    {
        #region MessageLoader Members

        public IDictionary<MessageKey, MessageData> LoadMessages()
        {
            string content = MessageResources.Messages;
            return LoadMessagesFromString(content);
        }


        #endregion

        private IDictionary<MessageKey, MessageData> LoadMessagesFromString(string content)
        {
            StringReader strReader = new StringReader(content);
            XmlTextReader xmlReader = new XmlTextReader(strReader);
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlReader);
                return LoadMessagesFromXmlDocument(xmlDoc);
            }
            finally
            {
                xmlReader.Close();
                strReader.Close();
            }
        }

        private IDictionary<MessageKey, MessageData> LoadMessagesFromXmlDocument(XmlDocument xmlDoc)
        {
            Dictionary<MessageKey, MessageData> messages = new Dictionary<MessageKey, MessageData>();

            MessageType[] values = (MessageType[])Enum.GetValues(typeof(MessageType));
            XmlNodeList messagesByTypeNodes = xmlDoc.DocumentElement.SelectNodes("MessagesByType");
            foreach (XmlNode messagesByTypeNode in messagesByTypeNodes)
            {
                MessageType msgKeyType = MessageType.UNASSIGNED;
                string str = messagesByTypeNode.Attributes.GetNamedItem("typeCode").Value;
                foreach (MessageType type2 in values)
                {
                    if (type2.ToString("D").Equals(str))
                    {
                        msgKeyType = type2;
                        break;
                    }
                }

                XmlNodeList messageNodes = messagesByTypeNode.SelectNodes("Message");
                foreach (XmlNode messageNode in messageNodes)
                {
                    XmlNode node = messageNode.Attributes.GetNamedItem("id");
                    if (node != null)
                    {
                        MessageData msg = new MessageData(new MessageKey(msgKeyType, node.Value));
                      
                        XmlNode descriptionChild = messageNode.SelectSingleNode("Description");
                        if (descriptionChild != null)
                        {
                            msg.Description = descriptionChild.FirstChild.Value.Trim();
                        }
                        else
                        {
                            msg.Description = "UNDEFINED DESCRIPTION";
                        }

                        if (!messages.ContainsKey(msg.Key))
                            messages.Add(msg.Key, msg);
                    }
                }
            }

            return messages;
        }
    }
}
