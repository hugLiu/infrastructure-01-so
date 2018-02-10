using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Jurassic.So.Infrastructure.Logging.Message
{
    public class UcpaassMessage
    {
       
        public static async Task<respModel> SendSMS(string MessService, string Sid, string AppId, string TokenId, string mobile, string TemplateId_Code, string code, string Time)
        {
            if (string.IsNullOrWhiteSpace(MessService) || string.IsNullOrWhiteSpace(Sid)
                || string.IsNullOrWhiteSpace(AppId) || string.IsNullOrWhiteSpace(TokenId) || string.IsNullOrWhiteSpace(TemplateId_Code) || string.IsNullOrWhiteSpace(Time))
            {
                throw new Exception("参数错误");
            }

            string date = DateTime.Now.ToString("yyyyMMddHHmmss") + "000";
            string sigstr = UcpaassMessage.MD5Encrypt(Sid + date + TokenId).ToLower();

            string strUrl = MessService
                            + string.Format("?sid={0}&appId={1}&time={2}&sign={3}&to={4}&templateId={5}&param={6}", Sid, AppId, date, sigstr, mobile, TemplateId_Code, code + "," + Time);

            HttpClient client = new HttpClient();
            var io = await client.GetStringAsync(strUrl);
            
            return JsonConvert.DeserializeObject<respModel>(io);
        }


        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source">原内容</param>
        /// <returns>加密后内容</returns>
        public static string MD5Encrypt(string source)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(source));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }



    public class respModel
    {
        public resp resp { get; set; }
    }

    public class resp
    {
        public string respCode { get; set; }
        public string respMsg { get; set; }
        public templateSMS templateSMS { get; set; }
    }

    public class templateSMS
    {
        public string createDate { get; set; }
        public string smsId { get; set; }
    }

}
