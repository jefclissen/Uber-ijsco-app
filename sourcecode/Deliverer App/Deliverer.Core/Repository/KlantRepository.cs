using Deliverer.Core.Modle;
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
    public class KlantRepository
    {
        #region from server
        private static List<Klant> klanten = new List<Klant>();
        public KlantRepository()
        {
            getKlanten();
        }
        private async void getKlanten()
        {
            WebRequest request = WebRequest.Create("http://192.168.1.57:3000/userlist"); //ip is ip van de pc waar node/mongo op draaid

            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            WebResponse response = await request.GetResponseAsync();

            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            // Display the content + deserialize json to location object
            //Console.WriteLine(responseFromServer);
            // List<Client> clientList = new List<Client>();
            klanten = JsonConvert.DeserializeObject<List<Klant>>(responseFromServer);
        }
        #endregion
        #region hardcode
        /*
         
        private static List<Klant> klanten = new List<Klant>()
        {
            
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
            
            

            

        };
        
            */
        #endregion

        public List<Klant> GeefAlleKlaten()
        {
            return klanten;

        }
    }
}
