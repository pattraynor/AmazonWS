using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AmazonWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "Keywords={keywords}&SearchIndex={search_index}")]
        List<string> getItemsFromStore(string keywords, string search_index);
        //http://webservices.amazon.com/onca/xml?
        //Your unique Associate ID is asutest20-20
        //Access Key ID: AKIAJCGNMCLKEFFNXMAA
        //Secret Key: 6nbBPxu82irfFREUb0bd4X4i+Z/eM3I3Xrf1AYo2


        // TODO: Add your service operations here
    }

}
