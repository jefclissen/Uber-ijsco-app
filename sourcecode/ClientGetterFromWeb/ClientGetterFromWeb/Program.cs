using System;
using System.IO;
using System.Net;
using System.Text;

using System.Web;

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace Examples.System.Net
{
    public class WebRequestGetExample
    {
        public class Client
        {
            public string _id { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Naam { get; set; }
        }
        
        public static void Main()
        {
            Console.WriteLine("POST");
            Console.Write("Username");
            string username = Console.ReadLine();
            Console.Write("Latitude");
            string latitude = Console.ReadLine();
            Console.Write("Longitude");
            string longitude = Console.ReadLine();
            post(username, latitude, longitude);*/
            Console.WriteLine("GET");
            get();
        }

        public static void post(string n, string lat, string longi)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                   { "thing1", "hello" },
                   { "thing2", "world" }
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://localhost:3000/addlocation", content);

                var responseString = await response.Content.ReadAsStringAsync();
            }
        }
        public static void get()
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(
              "http://localhost:3000/userlist");

            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            WebResponse response = request.GetResponse();

            // Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            // Display the content + deserialize json to location object
            //Console.WriteLine(responseFromServer);
            List<Client> clientList = new List<Client>();
            clientList = JsonConvert.DeserializeObject<List<Client>>(responseFromServer);
            //Console.WriteLine(clientList);
            for (int i = 0; i < clientList.Count; i++)
            {
                Console.WriteLine(clientList[i].Naam+" lat: "+clientList[i].Latitude+", long: "+clientList[i].Longitude);

            }

            // Clean up the streams and the response.
            reader.Close();
            response.Close();
            Console.ReadLine();
        }
    }
}