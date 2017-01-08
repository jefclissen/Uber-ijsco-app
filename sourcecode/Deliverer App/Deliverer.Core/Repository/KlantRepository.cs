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
        #region get klanten from server
        #region from server
        
        private static List<Klant> klanten = new List<Klant>();
        private async void getKlantenFromServer()
        {
            WebRequest request = WebRequest.Create("http://35.165.103.236:80/unhandledclients"); 
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = await request.GetResponseAsync();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
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
        
        public List<Klant> GeefAlleKlatenFromServer()
        {
            getKlantenFromServer(); //enkel gebruiken bij niet hardcode deel
            /*using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("url");
            }*/
            return klanten;
        }
        #endregion




        #region geaccepteerde klanten
        private List<Klant> geaccepteerdeKlanten = new List<Klant>();
        private List<Klant> gewijgerdeKlanten;
        
        public void pushGeaccepteerdeKlanten(List<Klant> klanten) //push klanten naar server
        {
           // geaccepteerdeKlanten = new List<Klant>();
            for (int i = 0; i < klanten.Count; i++)
            {
                geaccepteerdeKlanten.Add(klanten[i]);
            } 



            //hier code om klanten naar server te poushen
            /*
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://35.165.103.236:80/addclient");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"username\":\"test\"," +
                              "\"email\":\"test@test.be\"," +
                              "\"password\":\"bla\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }*/
        }
        public void pushGewijgerdeKlanten(List<Klant> klanten) //push klanten naar server
        {
            gewijgerdeKlanten = new List<Klant>();
            gewijgerdeKlanten = klanten;

            //klanten moeten niet naar server (later misschien een optie voor extra veiligheid)
        }
        public void klantBediend(Klant klant)
        {
            //data uit geaccepteerd
            geaccepteerdeKlanten.Remove(klant);
            //data nog uit handel van server
            //data in handeld klanten
        }
        public List<Klant> getGeaccepteerdeKlanten()
        {
            //getGeaccepteerdeKlantenFromServer();
            //deze moeten niet van server gehaald worden
            //eventueel later een optie (om veiligheid in te bouwen)
            return geaccepteerdeKlanten;
        }

        private async void getGeaccepteerdeKlantenFromServer()
        {
            geaccepteerdeKlanten = new List<Klant>();

            WebRequest request = WebRequest.Create("http://35.165.103.236:80/inprogress");
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = await request.GetResponseAsync();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            geaccepteerdeKlanten = JsonConvert.DeserializeObject<List<Klant>>(responseFromServer);
        }
        public List<Klant> getGewijgerdeKlanten()
        {
            
            if (gewijgerdeKlanten == null)
            {
                gewijgerdeKlanten = new List<Klant>();
                gewijgerdeKlanten.Add(new Klant()
                {
                    Username = "XXXXGEENKLANTENXXXX",
                    Longitude = 0,
                    Latitude = 0
                });
            }
            //deze moeten niet van server worden gehaald
            return gewijgerdeKlanten;
        }
        #endregion
    }
}
