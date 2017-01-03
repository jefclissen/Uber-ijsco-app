using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Deliverer.Core.Model
{
    public class Route
    {

        public string distance { get; set; }
        public string duration { get; set; }
        public string end_address { get; set; }
        public double end_address_lat { get; set; }
        public string start_address { get; set; }
        public double start_address_lat { get; set; }
        public MyLatLng[] steps;
    }
}
