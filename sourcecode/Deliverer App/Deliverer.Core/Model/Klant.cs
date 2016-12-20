using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliverer.Core.Modle
{
    public class Klant
    {
        public string _id
        {
            get;set;
        }
        public string Username
        {
            get;set;
        }
        public string Email
        {
            get; set;
        }
        public double Longitude
        {
            get;set;
        }

        public double Latitude { 
            get;set;
        }
    }
}
