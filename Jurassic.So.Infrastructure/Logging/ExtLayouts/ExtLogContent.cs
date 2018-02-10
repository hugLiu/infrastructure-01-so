

using Jurassic.So.Infrastructure.Logging.Message;

namespace Jurassic.So.Infrastructure.Logging.ExtLayouts
{
    public class ExtLogContent
    {
        public ExtLogContent(MessageData message)
        {
            this.MessageClass = message;
        }
        public ExtLogContent()
        {

        }
        public MessageData MessageClass { get; set; }
        public string MessageText { get; set; }
        public string Message { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        //public string DataKey { get; set; }
        public string LogonMethod { get; set; }
        public string LogonUser { get; set; }

        public override string ToString()
        {
            string str = string.Empty;

            if (Message != null)
            {
                str = Message.ToString();
            }

            return str;
        }
    }
}
