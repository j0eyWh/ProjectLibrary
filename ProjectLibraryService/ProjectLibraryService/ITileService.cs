using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace ProjectLibraryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITileService" in both code and config file together.
    [ServiceContract]
    public interface ITileService
    {
      

        [OperationContract]
        [WebInvoke(Method ="GET", UriTemplate = "/UpdateTile/{userId}", ResponseFormat = WebMessageFormat.Xml)]
        XmlElement UpdateTile(string userId);

        [OperationContract]
        [WebGet(UriTemplate = "GetImage")]
        
        Stream GetImage();


        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/GetNotification/{userId}", ResponseFormat = WebMessageFormat.Xml)]
        XmlElement GetNotification(string userId);
    }
}
