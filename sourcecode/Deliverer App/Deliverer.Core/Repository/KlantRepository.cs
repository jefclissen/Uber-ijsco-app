using Deliverer.Core.Modle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliverer.Core.Repository
{
    public class KlantRepository
    {
        private static List<Klant> klanten = new List<Klant>()
        {
            #region hardcode
            new Klant()
            {
                Naam="Robbe",
                Longitude = 51.228643,
                Latitude = 4.415705
            },
            new Klant()
            {
                Naam="Jef",
                Longitude = 51.229772,
                Latitude = 4.413956
            },
            new Klant()
            {
                Naam="Pim",
                Longitude = 51.229137,
                Latitude = 4.413205
            }
            #endregion
            #region from server

            #endregion

        };

        public List<Klant> GeefAlleKlaten()
        {
            return klanten;

        }
    }
}
