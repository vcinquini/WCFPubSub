using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PubSubService
{
       [ServiceContract(Namespace = "http://ListPublishSubscribe.Service",
                    SessionMode = SessionMode.Required)]
    public interface IPubSubContract
    {
        [OperationContract(IsOneWay = true)]
        void NameChange(string Name);
    }
}
