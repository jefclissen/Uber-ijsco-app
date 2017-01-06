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
using System.Collections.Specialized;
using System.IO;
using Plugin.Geolocator;

namespace googlemaps
{
    [Activity(Label = "KlantenDetailActivity")]
    public class KlantenDetailActivity : Activity
    {
        private List<Klant> geaccepteerdeKlanten;
        private List<Klant> klant;
        private TextView naamKlantTextView;
        private Button bediendButton;
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

            geaccepteerdeKlanten = new List<Klant>();
            klant = new List<Klant>();

            email = Intent.GetStringExtra("email");
            //klantId = Convert.ToInt32(k);

            GetKlant();
            FindViews();
            FillViews();
            // Create your application here
        }
        

        private void GetKlant()
        {
            geaccepteerdeKlanten = dataService.getGeaccepteerdeKlanten();
            //klant = geaccepteerdeKlanten[klantId];
            /*var kkkk = from k in geaccepteerdeKlanten
                    where k.Email == email
                    select k;*/
            //klant = (Object) kkkk;
            //klant = ee;

             klant = geaccepteerdeKlanten.Where(k => k.Email == email).ToList();
        }

        private void FillViews()
        {
            naamKlantTextView.Text = klant[0].Username;
            bediendButton.Click += BediendButton_Click;
        }

        private async void BediendButton_Click(object sender, EventArgs e)
        {
            //data uit geaccepteerd
            dataService.klantBediend(klant[0]);

            /*
            string mResult;
            using (WebClient client = new WebClient())
            {

                //Uri uri = new Uri("http://35.165.103.236:80/clientlogin");
                string uri = "http://35.165.103.236:80/doneclient";
                NameValueCollection parameters = new NameValueCollection();
               // parameters.Add("username", geaccepteerdeKlanten[klantId].Username);
               // parameters.Add("userLong", Convert.ToString(geaccepteerdeKlanten[klantId].Longitude));
               // parameters.Add("userLat", Convert.ToString(geaccepteerdeKlanten[klantId].Latitude));
                parameters.Add("email", klant[0].Email);
                byte[] response = client.UploadValues(uri, parameters);
                mResult = System.Text.Encoding.UTF8.GetString(response);
            }*/

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://35.165.103.236:80/doneclient");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"driveremail\":\"robbe@driver.com\"," +
                    "\"driverLat\":\"" + position.Latitude.ToString().Replace(',', '.') + "\"," +
                                  "\"driverLong\":\"" + position.Longitude.ToString().Replace(',', '.') + "\"," +
                                 "\"useremail\":\"" + klant[0].Email + "\"}";
                              

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
            
            /*
            dataService.pushGeaccepteerdeKlanten(geaccepteerdeKlanten);
            Toast.MakeText(this, "klanten zijn toegevoegd", ToastLength.Long).Show();
            var intent = new Intent(this, typeof(MapActivity));
            StartActivity(intent);*/

            //data in handeld klanten
            Toast.MakeText(this, klant[0].Email, ToastLength.Long).Show();
            var intent = new Intent(this, typeof(MapActivity));
            StartActivity(intent);

        }

        private void FindViews()
        {
            naamKlantTextView = FindViewById<TextView>(Resource.Id.naamKlantTextView);
            bediendButton = FindViewById<Button>(Resource.Id.bediendButton);
        }
    }
}