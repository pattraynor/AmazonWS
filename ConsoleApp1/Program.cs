using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {

        public static byte[] HmacSHA256(String data, byte[] key)
        {
            String algorithm = "HmacSHA256";
            KeyedHashAlgorithm kha = new HMACSHA256(key);
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

        static void Main(string[] args)
        {

            string accessKeyId = "AKIAJCGNMCLKEFFNXMAA";
            string associateID = "asutest20-20";
            string timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ssZ");
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
            string timeStampFix = timestamp.Replace(":", "%");
            byte[] signature = getSignatureKey(accessKeyId, timeStampFix, "us-west-1", "ItemSearch");
            string signatureStr = System.Text.Encoding.UTF8.GetString(signature);

            System.Text.StringBuilder amazonItemAccessString = new System.Text.StringBuilder();

            amazonItemAccessString.Append("http://webservices.amazon.com/onca/xml?Service=AWSECommerceService&AWSAccessKeyId=").Append(accessKeyId).Append("&AssociateTag=").Append(associateID).Append("&Operation=ItemSearch&").Append("&Keywords=").Append("Biohazard").Append("&Timestamp=").Append(timeStampFix).Append("&Signature=").Append(signatureStr);

            var amazonRequest = (HttpWebRequest)System.Net.WebRequest.Create(amazonItemAccessString.ToString());

            using (var amazonResponse = amazonRequest.GetResponse())
            using (var amazonReader = new StreamReader(amazonResponse.GetResponseStream()))
            {
                var amazonResults = amazonReader.ReadToEnd();


                string lat = "";

            }
            /*
            Console.WriteLine("Hello World!");
            //http://localhost:51464/Keywords={keywords}&SearchIndex={search_index}

            System.Text.StringBuilder amazonWebString = new System.Text.StringBuilder();
            string keyword = "megadeth";
            string searchIndex = "Book";

            amazonWebString.Append("http://localhost:51464/Keywords=").Append(keyword).Append("&SearchIndex=").Append(searchIndex);
            var amazonReq = (HttpWebRequest)System.Net.WebRequest.Create(amazonWebString.ToString());

            using (var amazonResponse = amazonReq.GetResponse())
            using (var amazonReader = new StreamReader(amazonResponse.GetResponseStream()))
            {
                var amazonResults = amazonReader.ReadToEnd();
                string test = "";
            }
            */



        }
    }
}
