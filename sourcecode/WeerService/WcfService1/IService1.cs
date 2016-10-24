﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        [WebGet(UriTemplate = "today", BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Xml)]
        Weerbericht Today();

        [OperationContract]
        [WebGet(UriTemplate = "voorspelling/{days}", BodyStyle =
        WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat
        = WebMessageFormat.Json)]
        List<Weerbericht> Voorspelling(string days);
        [OperationContract]
        [WebInvoke(Method = "*", UriTemplate = "save", BodyStyle =
        WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat =
        WebMessageFormat.Json)]
        Weerbericht Save(Weerbericht bericht);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}