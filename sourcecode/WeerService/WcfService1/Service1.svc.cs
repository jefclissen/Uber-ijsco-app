using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public Weerbericht Today()
        {
            return new Weerbericht() { Description = "mooi weer", Temperature = 25 };
        }
        public List<Weerbericht> Voorspelling(string days)
        {
            /*Simulate database*/
            List<Weerbericht> aList = new List<Weerbericht>();
            aList.Add(new Weerbericht() { Description = "mooi weer", Temperature = 25 });
            aList.Add(new Weerbericht() { Description = "slecht weer", Temperature = 15 });
            aList.Add(new Weerbericht() { Description = "regen", Temperature = 20 });
            aList.Add(new Weerbericht() { Description = "mooi weer", Temperature = 23 });
            aList.Add(new Weerbericht() { Description = "onweer", Temperature = 25 });
            aList.Add(new Weerbericht() { Description = "orkaan", Temperature = 5 });
            aList.Add(new Weerbericht() { Description = "sneeuw", Temperature = 0 });
            aList.Add(new Weerbericht() { Description = "ijzel", Temperature = -5 });
            aList.Add(new Weerbericht() { Description = "hagel", Temperature = 5 });
            aList.Add(new Weerbericht() { Description = "zonnig", Temperature = 35 });
            List<Weerbericht> sendList = new List<Weerbericht>();
            for (int i = 0; i < int.Parse(days); i++)
            {
                sendList.Add(aList[i]);
            }
            return sendList;
        }
        
        public Weerbericht Save(Weerbericht bericht)
        {
            if (WebOperationContext.Current.IncomingRequest.Method == "POST")
            {
                Debug.WriteLine("Got new weerbericht:" + bericht.Description);
            }
            return null;
        }
    }
}
