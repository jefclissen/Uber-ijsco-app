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
            new Klant()
            {
                Naam="Robbe",
                Locatie="51.228643, 4.415705"
            },
            new Klant()
            {
                Naam="Jef",
                Locatie="51.229772, 4.413956"
            },
            new Klant()
            {
                Naam="Pim",
                Locatie="51.229137, 4.413205"
            }
        };
    }
}
