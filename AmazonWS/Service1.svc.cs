using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AmazonWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        public static byte[] HmacSHA256(String data, byte[] key)
        {
            String algorithm = "HmacSHA256";
            KeyedHashAlgorithm kha = KeyedHashAlgorithm.Create(algorithm);
            kha.Key = key;

            return kha.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        public static byte[] getSignatureKey(String key, String dateStamp, String regionName, String serviceName)
        {
            byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + key).ToCharArray());
            byte[] kDate = HmacSHA256(dateStamp, kSecret);
            byte[] kRegion = HmacSHA256(regionName, kDate);
            byte[] kService = HmacSHA256(serviceName, kRegion);
            byte[] kSigning = HmacSHA256("aws4_request", kService);

            return kSigning;
        }

        public static string ToUrlSafeBase64String(byte[] bytes)
        {
            return System.Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('=', '_')
                .Replace('/', '~');
        }


        public List<string> getItemsFromStore(string keywords, string search_index)
        {
            List<string> test = null;
            string accessKeyId = "AKIAJCGNMCLKEFFNXMAA";
            string associateID = "asutest20-20";
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            /*http://webservices.amazon.com/onca/xml?
            Service = AWSECommerceService &
            AWSAccessKeyId =[AWS Access Key ID] & AKIAJCGNMCLKEFFNXMAA
            AssociateTag =[Associate ID]  asutest20-20 &
            Operation = ItemSearch &
            Keywords = the % 20hunger % 20games &
                  SearchIndex = Books
                  & Timestamp =[YYYY - MM - DDThh:mm: ssZ]
                  & Signature =[Request Signature]
                  */
            byte[] signature = getSignatureKey(accessKeyId, timestamp, "us-west-1", "ItemSearch");
            string signatureStr = System.Text.Encoding.UTF8.GetString(signature);

            System.Text.StringBuilder amazonItemAccessString = new System.Text.StringBuilder();

            amazonItemAccessString.Append("http://webservices.amazon.com/onca/xml?Service=AWSECommerceService&AWSAccessKeyId=").Append(accessKeyId).Append("&AssociateTag=").Append(associateID).Append("&Operation=ItemSearch&").Append("&Keywords=").Append(keywords).Append("&Timestamp=").Append(timestamp).Append("&Signature=").Append(signatureStr);

            var amazonRequest = (HttpWebRequest)System.Net.WebRequest.Create(amazonItemAccessString.ToString());

            using (var amazonResponse = amazonRequest.GetResponse())
            using (var amazonReader = new StreamReader(amazonResponse.GetResponseStream()))
            {
                var amazonResults = amazonReader.ReadToEnd();

            
                string lat = "";

            }

            return test;
        }
    }
}
