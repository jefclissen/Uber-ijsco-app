using System;
using System.IO;
using System.Net;
using System.Text;

using System.Web;

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Examples.System.Net
{
    public class WebRequestGetExample
    {
        public class Location
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }

        public class Client
        {
            public Location currentLocation;
            public string name { get; set; }
        }
        
        public static void Main()
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(
              "http://localhost:3000/data");

            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            WebResponse response = request.GetResponse();

            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

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
                Console.WriteLine("ik ben " + clientList[i].name + " mijn longitude is: " + clientList[i].currentLocation.Longitude + " mijn latitude: " + clientList[i].currentLocation.Latitude);

            }

            // Clean up the streams and the response.
            reader.Close();
            response.Close();
            Console.ReadLine();
        }
    }
}