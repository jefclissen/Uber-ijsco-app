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

        public List<Klant> GeefAlleKlantenFromServer()
        {
            return klantRepository.GeefAlleKlatenFromServer();
        }
        public void pushGeaccepteerdeKlanten(List<Klant> klanten)
        {
            klantRepository.pushGeaccepteerdeKlanten(klanten);
        }
        public List<Klant> getGeaccepteerdeKlanten()
        {
            return klantRepository.getGeaccepteerdeKlanten();
        }

        public void pushGewijgerdeKlanten(List<Klant> klanten)
        {
            klantRepository.pushGewijgerdeKlanten(klanten);
        }
        public List<Klant> getGewijgerdeKlanten()
        {
            return klantRepository.getGewijgerdeKlanten();
        }

        public void klantBediend(Klant klant)
        {
            klantRepository.klantBediend(klant);
        }
    }
}
