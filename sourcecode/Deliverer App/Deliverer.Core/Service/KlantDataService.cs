using Deliverer.Core.Modle;
using Deliverer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliverer.Core.Service
{
    public class KlantDataService
    {
        private static KlantRepository klantRepository = new KlantRepository();

        public List<Klant> GeefAlleKlanten()
        {
            return klantRepository.GeefAlleKlaten();
        }
    }
}
