using Deliverer.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Deliverer.Core.Repository
{
    public class RouteRepository
    {
        private List<Route> routes = new List<Route>();

        public void pushRoute(string response)
        {
            routes = JsonConvert.DeserializeObject<List<Route>>(response);
        }

        public List<Route> GeefAlleRoutes()
        {
            return routes;
        }
    }
}
