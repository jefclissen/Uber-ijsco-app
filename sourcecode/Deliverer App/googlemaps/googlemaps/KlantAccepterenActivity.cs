using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Deliverer.Core.Modle;
using Deliverer.Core.Service;
using System.Net;
using Newtonsoft.Json;
using Plugin.Geolocator;
using System.IO;

namespace googlemaps
{
    [Activity(Label = "KlantAccepterenActivity")]
    public class KlantAccepterenActivity : Activity
    {
        private List<Klant> nieuweKlanten;
        private List<Klant> klant;
        private TextView naamNieuweKlant;
        private Button accepteerButton;
        //private int klantId;
        private string email;

        private KlantDataService dataService;
        private RoutesDataService routeDataService;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.KlantDetailActivity);

            dataService = new KlantDataService();
            routeDataService = new RoutesDataService();

            nieuweKlanten = new List<Klant>();
            klant = new List<Klant>();

            email = Intent.GetStringExtra("email");
            //klantId = Convert.ToInt32(k);

            GetKlant();
            FindViews();
            FillViews();
            // Create your application here
        }

        private void FillViews()
        {
            naamNieuweKlant.Text = klant[0].Username;
            accepteerButton.Click += AccepteerButton_Click;
        }

        private async void AccepteerButton_Click(object sender, EventArgs e)
        {
            dataService.pushGeaccepteerdeKlanten(klant);

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://35.165.103.236:80/helpclient");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string userString = "[";
            for (int i = 0; i < klant.Count; i++)
            {
                userString += "{\"useremail\":\"" + klant[0].Email + "\"," +
                              "\"userLat\":\"" + klant[0].Latitude.ToString().Replace(',', '.') + "\"," +
                              "\"userLong\":\"" + klant[0].Longitude.ToString().Replace(',', '.') + "\"}";
                if (i != klant.Count - 1)
                    userString += ",";
            }
            userString += "]";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"driveremail\":\"robbe@driver.com\"," +
                              "\"driverLat\":\"" + position.Latitude.ToString().Replace(',', '.') + "\"," +
                              "\"driverLong\":\"" + position.Longitude.ToString().Replace(',', '.') + "\"," +
                              "\"users\":" + userString + "}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                routeDataService.pushRoute(result);
            }


            //dataService.pushGeaccepteerdeKlanten(geaccepteerdeKlanten);
            Toast.MakeText(this, "klant is toegevoegd", ToastLength.Long).Show();
            var intent = new Intent(this, typeof(MapActivity));
            StartActivity(intent);
        }

        private void FindViews()
        {
            naamNieuweKlant = FindViewById<TextView>(Resource.Id.naamNieuweKlant);
            accepteerButton = FindViewById<Button>(Resource.Id.bediendButton);
        }

        private void GetKlant()
        { 
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("http://35.165.103.236:80/unhandledclients");
                nieuweKlanten = JsonConvert.DeserializeObject<List<Klant>>(json);
            }
            klant = nieuweKlanten.Where(k => k.Email == email).ToList();
        }
    }
}