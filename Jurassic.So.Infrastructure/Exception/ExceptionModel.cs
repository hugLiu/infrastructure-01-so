using System;
using System.Runtime.Serialization;

namespace Jurassic.So.Infrastructure
{
    /// <summary>�쳣����</summary>
    [Serializable]
    [DataContract]
    public class ExceptionModel
    {
        /// <summary>�쳣����</summary>
        [DataMember(Name = "exceptioncode")]
        public string Code { get; set; }
        /// <summary>�쳣��Ϣ(�����û�)</summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
        /// <summary>�쳣��ϸ(���������)</summary>
        [DataMember(Name = "details")]
        public string Details { get; set; }
    }
}