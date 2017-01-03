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

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://35.165.103.236:80/helpclient");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string userString = "[";
            for (int i = 0; i < geaccepteerdeKlanten.Count; i++)
            {
                userString += "{\"useremail\":\"" + geaccepteerdeKlanten[i].Email + "\"," +
                              "\"userLat\":\"" + geaccepteerdeKlanten[i].Latitude.ToString().Replace(',','.') + "\"," +
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


            dataService.pushGeaccepteerdeKlanten(geaccepteerdeKlanten);
            Toast.MakeText(this, "klanten zijn toegevoegd", ToastLength.Long).Show();
            var intent = new Intent(this, typeof(MapActivity));
            StartActivity(intent);
        }


        private void Klantenlijst_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            klantenHelper[e.Position] = !klantenHelper[e.Position];
        }
        private void VulLijst()
        {

            serverKlanten = dataService.GeefAlleKlantenFromServer();

            naamKlanten = new List<string>();
            klantenHelper = new bool[serverKlanten.Count];

            for (int i = 0; i < serverKlanten.Count; i++)
            {
                naamKlanten.Add(serverKlanten[i].Username);
                klantenHelper[i] = false;
            }
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemMultipleChoice, naamKlanten);
            klantenlijst.Adapter = adapter;
            klantenlijst.ChoiceMode = ChoiceMode.Multiple;
        }
        private void FindViews()
        {
            klantenlijst = FindViewById<ListView>(Resource.Id.klantenListView);
            accepteerButton = FindViewById<Button>(Resource.Id.accepteerButton);
        }
    }
}