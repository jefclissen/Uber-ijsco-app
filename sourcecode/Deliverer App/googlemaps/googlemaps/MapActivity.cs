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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Deliverer.Core.Service;
using Deliverer.Core.Modle;
using Plugin.Geolocator;
using Android.Graphics;
using static Android.Gms.Maps.GoogleMap;
using Deliverer.Core.Model;
using System.Net;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace googlemaps
{
    [Activity(Label = "googlemaps", Icon = "@drawable/icon")]
    public class MapActivity : Activity , IInfoWindowAdapter, IOnInfoWindowClickListener
    {
        private List<Klant> klanten;
        private List<Klant> unhandeledKlaten;
        private List<Klant> newKlant;
        private List<Route> routes;
        private List<MarkerOptions> markers;
        private List<MarkerOptions> newMarkers;
        private KlantDataService dataService;
        private RoutesDataService routeDataService;
        private LatLng myPosition;
        private MarkerOptions myPositionMarker;
        private GoogleMap map;
        private List<LatLng> punten;
        private Thread myThread;
        private bool threadRunning = false;
        private string geklikteKlant;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            
        }
        protected override void OnResume()
        {
            base.OnResume();
            
            ThreadStart myThreadDelegate = new ThreadStart(zoekNieuweKlanten);  
            myThread = new Thread(myThreadDelegate);

            dataService = new KlantDataService();

            /*
            ThreadStart myThreadDelegate = new ThreadStart(notificationThread);
            myThread = new Thread(myThreadDelegate);
            threadRunning = true;
            myThread.Start();
            */
            getKlanten();
            if (klanten != null)
            {
                // getMyLocation();
                makeMarkers();
                makeMap();
                myThread.Start();
            }
            else
            {//als er geen klanten zijn terug naar mainmenu en toast meegeven
                Toast.MakeText(this, "geen klanten geselecteerd", ToastLength.Long).Show();
                var intent = new Intent(this, typeof(MainMenuActivity));
                StartActivity(intent);
            }
        }
        public void notificationThread()
        {
            int aantalNieuweKlanten = 0;
            int vorigAantalNieuweKlanten = 0;

            List<Klant> serverKlanten = new List<Klant>();
            List<Klant> geweigerdeKlanten = new List<Klant>();
            KlantDataService dataService = new KlantDataService();

            //while (threadRunning == true) //zorg voor een eeuwige lus
            while (true) //zorg voor een eeuwige lus
            {
                if (threadRunning == true)
                {
                    Thread.Sleep(7000);
                    serverKlanten = dataService.GeefAlleKlantenFromServer();
                    geweigerdeKlanten = dataService.getGewijgerdeKlanten();

                    aantalNieuweKlanten = serverKlanten.Count; //maximum aantal nieuwe klanten (gelijk aan server klanten)
                    if (geweigerdeKlanten[0].Username != "XXXXGEENKLANTENXXXX")//wanneer er nog geen gewijgerde klanten zijn
                    {
                        for (int i = 0; i < serverKlanten.Count; i++)
                        {
                            for (int j = 0; j < geweigerdeKlanten.Count; j++)
                            {
                                if (serverKlanten == geweigerdeKlanten)//als bepaalde klant op de server == aan de reeds geweigerde klant
                                    aantalNieuweKlanten--;  //wordt maximum aantal klanten verminderd met 1
                            }
                        }
                    }

                    if (aantalNieuweKlanten != 0 && aantalNieuweKlanten != vorigAantalNieuweKlanten)
                    {
                        vorigAantalNieuweKlanten++;
                        //voor het maken van de klik event
                        // When the user clicks the notification, SecondActivity will start up.
                        Intent resultIntent = new Intent(this, typeof(AccepteerActivity));
                        // Construct a back stack for cross-task navigation:
                        TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
                        stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(AccepteerActivity)));
                        stackBuilder.AddNextIntent(resultIntent);
                        // Create the PendingIntent with the back stack:            
                        PendingIntent resultPendingIntent =
                            stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

                       // Android.Net.Uri alarmSound = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

                        Notification.Builder builder = new Notification.Builder(this)
                            .SetContentTitle("Nieuwe klanten")
                            .SetContentText("Er zijn " + aantalNieuweKlanten + " nieuwe klanten")
                            .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                            .SetContentIntent(resultPendingIntent)  // Start 2nd activity when the intent is clicked.
                            .SetSmallIcon(Android.Resource.Drawable.IcDialogAlert)
                          //  .SetSound(alarmSound)
                        .SetVibrate(new long[] { 500, 500, 500, 500, 500 })
                        .SetPriority(10);

                        // Build the notification:
                        Notification nieuweKlantNotification = builder.Build();



                        // Get the notification manager:
                        NotificationManager notificationManager =
                            GetSystemService(Context.NotificationService) as NotificationManager;

                        // Publish the notification:
                        const int notificationId = 0;
                        notificationManager.Notify(notificationId, nieuweKlantNotification);
                    }
                    
                }
            }
        }
        public void getKlanten()
        {
            /*
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("http://35.165.103.236:80/inprogress");
                klanten = JsonConvert.DeserializeObject<List<Klant>>(json);
            }*/
            klanten = dataService.getGeaccepteerdeKlanten();
        }
        public void makeMarkers()
        {
            markers = new List<MarkerOptions>();

            for (int i = 0; i < klanten.Count; i++)
            {
                MarkerOptions marker = new MarkerOptions();
                marker.SetPosition(new LatLng(klanten[i].Latitude, klanten[i].Longitude));
                marker.SetTitle("behandel");
                //marker.SetTitle("" + klanten[i].Username);
                markers.Add(marker);
            }
            //nieuwe klanten marker
            
        }
        private void zoekNieuweKlanten()
        {
            while (true)
            {
                Thread.Sleep(7000);
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString("http://35.165.103.236:80/unhandledclients");
                    unhandeledKlaten = JsonConvert.DeserializeObject<List<Klant>>(json);
                }/*
                if (newKlant == null)
                {
                    newKlant = new List<Klant>();
                    newKlant = unhandeledKlaten;
                } else if(unhandeledKlaten.Count != newKlant.Count){
                    for (int i = 0; i < unhandeledKlaten.Count; i++)
                    {
                        if (unhandeledKlaten[i].Email !=)
                    }
                }*/
                newMarkers = new List<MarkerOptions>();
                newMarkers.Clear();
                for (int i = 0; i < unhandeledKlaten.Count; i++)
                {
                    MarkerOptions marker = new MarkerOptions();
                    marker.SetPosition(new LatLng(unhandeledKlaten[i].Latitude, unhandeledKlaten[i].Longitude));
                    marker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
                    marker.SetTitle("new");
                    
                        //marker.SetTitle("" + newKlaten[i].Username);

                    newMarkers.Add(marker);
                    unhandeledKlaten[i].markerID = newMarkers.IndexOf(marker);
                    
                }
                
                RunOnUiThread(() => tekenNieuweKlanten());
            }
        }
        private void tekenNieuweKlanten()
        {
           // map.Clear();
            //maakRoute();
            //tekenGeaccepteerdeKlanten();
            for (int i = 0; i < newMarkers.Count; i++)
            {
                map.AddMarker(newMarkers[i]);
                map.SetInfoWindowAdapter(this);
                map.SetOnInfoWindowClickListener(this);
            }
        }
        private void tekenGeaccepteerdeKlanten()
        {
            map.MyLocationEnabled = true;
            for (int i = 0; i < markers.Count; i++)
            {
                map.AddMarker(markers[i]);
                map.SetInfoWindowAdapter(this);
                map.SetOnInfoWindowClickListener(this);
                //map.SetInfoWindowAdapter(new CustomMarkerPopupAdapter(LayoutInflater));
            }
        }
        private void makeMap()
        {
            SetContentView(Resource.Layout.MapView);
            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            map = mapFrag.Map;


            if (map != null)
            {

                zoomToLocation();
                maakRoute();
                tekenGeaccepteerdeKlanten();
            }
        }

        private void maakRoute()
        {
            routeDataService = new RoutesDataService();
            routes = routeDataService.GeefRoutes();
            for (int i = 0; i < routes.Count; i++)
            {
                for (int j = 0; j < routes[i].steps.Length - 1; j++)
                {
                    Polyline line = map.AddPolyline(new PolylineOptions()
     .Add(new LatLng(routes[i].steps[j].latitude, routes[i].steps[j].longitude), new LatLng(routes[i].steps[j + 1].latitude, routes[i].steps[j + 1].longitude)));
                    if (i % 2 == 0)
                        line.Color = Color.Red;
                    else
                        line.Color = Color.Blue;
                }
            }
        }
        private async void zoomToLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(position.Latitude, position.Longitude), 12));
            map.AnimateCamera(CameraUpdateFactory.ZoomTo(12), 2000, null);
        }

        
        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            string s = marker.Id.Substring(1);
            int i = Convert.ToInt16(s);

            View view;
            Button behandelButton;
            if (marker.Title == "behandel")
            {
                
                view = LayoutInflater.Inflate(Resource.Layout.CustomMarkerLayout, null, false);
                //view.FindViewById<Button>(Resource.Id.bediendButton).Click += BehandelButton_Click;
                view.FindViewById<TextView>(Resource.Id.naamTextView).Text = (string)klanten[i].Username;
                //geklikteKlant = klanten[i].Email;
               // behandelButton.Click += BehandelButton_Click;
            }
            else
            {
                view = LayoutInflater.Inflate(Resource.Layout.CustomMarkerLayout, null, false);
                view.FindViewById<TextView>(Resource.Id.naamTextView).Text = (string)unhandeledKlaten[i - klanten.Count].Username;
            }
            //view.FindViewById<Button>(Resource.Id.bedienButton).Click += MapActivity_Click;
            return view;
        }

        private async void BehandelButton_Click(object sender, EventArgs e)
        {
            //data uit geaccepteerd
            List<Klant> klant = new List<Klant>();
            klant = klanten.Where(k => k.Email == geklikteKlant).ToList();
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
        }
        
        public void OnInfoWindowClick(Marker marker)
        {
            string s = marker.Id.Substring(1);
            int i = Convert.ToInt16(s);
            
            
            if (marker.Title == "behandel")
            {
                var intent = new Intent(this, typeof(KlantenDetailActivity));
                intent.PutExtra("email", klanten[i].Email);
                StartActivity(intent);
            }
            else
            {
                /*
                var intent = new Intent(this, typeof(KlantAccepterenActivity));
                intent.PutExtra("email", unhandeledKlaten[i - klanten.Count].Email);*/
                var intent = new Intent(this, typeof(AccepteerActivity));

                StartActivity(intent);
            }
            //var intent = new Intent(this, typeof(KlantenDetailActivity));
            //intent.PutExtra("KlantenId", Convert.ToString(i));
           // intent.PutExtra("email", klanten[i].Email);
            //StartActivity(intent);
            
        }
    }
}