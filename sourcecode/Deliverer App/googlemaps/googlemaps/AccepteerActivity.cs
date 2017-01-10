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
using Deliverer.Core.Service;
using Deliverer.Core.Modle;
using System.Threading;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using Plugin.Geolocator;
using Newtonsoft.Json;

namespace googlemaps
{
    [Activity(Label = "AccepteerActivity")]
    public class AccepteerActivity : Activity
    {
        private ListView klantenlijst;
        private List<Klant> serverKlanten;
        private Button accepteerButton;
        private List<Klant> geaccepteerdeKlanten;
        private bool[] klantenHelper;
        private List<string> naamKlanten; //lijst als hulpmiddel
        private List<string> selectedKlanten = new List<string>();

        private KlantDataService dataService;
        private RoutesDataService routeDataService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        protected override void OnResume()
        {
            base.OnResume();
            SetContentView(Resource.Layout.AccepteerLayout);
            dataService = new KlantDataService();
            routeDataService = new RoutesDataService();

            FindViews();
            VulLijst();
            HandleEvents();
        }

        private void HandleEvents()
        {
            klantenlijst.ItemClick += Klantenlijst_ItemClick;
            accepteerButton.Click += AccepteerButton_Click;
        }
        private async void AccepteerButton_Click(object sender, EventArgs e)
        {
            geaccepteerdeKlanten = new List<Klant>();
            for (int i = 0; i < serverKlanten.Count; i++)
            {
                if (klantenHelper[i] == true)
                    geaccepteerdeKlanten.Add(serverKlanten[i]);
            }


            /*
            voorbeeld hoe het naar de server moet
            {
               "driveremail": "driver@2jp.be",
               "driverLong": "4.0",
               "driverLat": "51.0",
               "users":[
               {
                   "useremail":"jef@hotmail.be",
                   "userLat":"51.11111",
                   "userLong":"4.11111"
               },
               {
                   "useremail":"pim@hotmail.be",
                   "userLat":"51.22222",
                   "userLong":"4.22222"
               },
               {
                   "useremail":"lobb@hotmail.be",
                   "userLat":"51.333333",
                   "userLong":"4.33333"
               }]
           }
           */
            if (geaccepteerdeKlanten.Count != 0)
            {
                dataService.pushGeaccepteerdeKlanten(geaccepteerdeKlanten);

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var position = await locator.GetPositionAsync(timeoutMilliseconds: 20000);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://35.165.103.236:80/helpclient");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                string userString = "[";
                for (int i = 0; i < geaccepteerdeKlanten.Count; i++)
                {
                    userString += "{\"useremail\":\"" + geaccepteerdeKlanten[i].Email + "\"," +
                                  "\"userLat\":\"" + geaccepteerdeKlanten[i].Latitude.ToString().Replace(',', '.') + "\"," +
                                  "\"userLong\":\"" + geaccepteerdeKlanten[i].Longitude.ToString().Replace(',', '.') + "\"}";
                    if (i != geaccepteerdeKlanten.Count - 1)
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
                Toast.MakeText(this, "klanten zijn toegevoegd", ToastLength.Long).Show();
                var intent = new Intent(this, typeof(MapActivity));
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "gelieve klanten te selecteren", ToastLength.Long).Show();
            }
        }


        private void Klantenlijst_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            klantenHelper[e.Position] = !klantenHelper[e.Position];
        }
        private async void VulLijst()
        {

            //serverKlanten = dataService.GeefAlleKlantenFromServer();
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("http://35.165.103.236:80/unhandledclients");
                serverKlanten = JsonConvert.DeserializeObject<List<Klant>>(json);
            }
            naamKlanten = new List<string>();
            klantenHelper = new bool[serverKlanten.Count];

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

            for (int i = 0; i < serverKlanten.Count; i++)
            {
                naamKlanten.Add(serverKlanten[i].Username +" "+ Math.Round(afstand(serverKlanten[i].Latitude,serverKlanten[i].Longitude, position.Latitude, position.Longitude),2) + "km");
                klantenHelper[i] = false;
            }
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemMultipleChoice, naamKlanten);
            klantenlijst.Adapter = adapter;
            klantenlijst.ChoiceMode = ChoiceMode.Multiple;
        }
        private double afstand(double lat1, double lon1, double lat2, double lon2)
        {

            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
        private void FindViews()
        {
            klantenlijst = FindViewById<ListView>(Resource.Id.klantenListView);
            accepteerButton = FindViewById<Button>(Resource.Id.accepteerButton);
        }
    }
}