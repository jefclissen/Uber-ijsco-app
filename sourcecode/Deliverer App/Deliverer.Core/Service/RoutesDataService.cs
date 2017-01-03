using Deliverer.Core.Model;
using Deliverer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliverer.Core.Service
{
    public class RoutesDataService
    {
        private static RouteRepository routeRepository = new RouteRepository();

        public void pushRoute(string response)
        {
            routeRepository.pushRoute(response);
        }
        public List<Route> GeefRoutes()
        {
            return routeRepository.GeefAlleRoutes();
        }
    }
}
