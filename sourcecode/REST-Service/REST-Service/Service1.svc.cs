 using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace REST_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public List<Location> locations(string id)
        {
            /*Simulate database*/
            List<Location> aList = new List<Location>();
            aList.Add(new Location() { Longitude = 5.111111, Latitude = 43.111111});
            aList.Add(new Location() { Longitude = 5.222222, Latitude = 43.222222 });

            List<Location> sendList = new List<Location>();           
            sendList.Add(aList[int.Parse(id)]); 
            return sendList;
        }
    }
}
