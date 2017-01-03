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

namespace googlemaps
{
    [Activity(Label = "googlemaps", Icon = "@drawable/icon")]
    public class MapActivity : Activity, IInfoWindowAdapter, IOnInfoWindowClickListener
    {
        private List<Klant> klanten;
        private List<Route> routes;
        private MarkerOptions[] markers;
        private KlantDataService dataService;
        private RoutesDataService routeDataService;
        private LatLng myPosition;
        private MarkerOptions myPositionMarker;
        private GoogleMap map;
        private List<LatLng> punten;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            getKlanten();
            if (klanten != null)
            {
                // getMyLocation();
                makeMarkers();
                makeMap();
            }
            else
            {//als er geen klanten zijn terug naar mainmenu en toast meegeven
                Toast.MakeText(this, "geen klanten geselecteerd", ToastLength.Long).Show();
                var intent = new Intent(this, typeof(MainMenuActivity));
                StartActivity(intent);
            }
        }

        public void getKlanten()
        {
            KlantDataService dataService = new KlantDataService();
            klanten = dataService.getGeaccepteerdeKlanten();
        }
        public void makeMarkers()
        {
            markers = new MarkerOptions[klanten.Count];
            for (int i = 0; i < klanten.Count; i++)
            {

                MarkerOptions marker = new MarkerOptions();
                marker.SetPosition(new LatLng(klanten[i].Latitude, klanten[i].Longitude));
                marker.SetTitle(klanten[i].Username);

                markers[i] = marker;
            }
        }
        private async void makeMap()
        {
            SetContentView(Resource.Layout.MapView);
            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            map = mapFrag.Map;


            punten = new List<LatLng>();
            /*
            punten.Add(new LatLng(51.37223, 4.47566));
            punten.Add(new LatLng(51.37164, 4.47443));
            punten.Add(new LatLng(51.37135, 4.47386));
            punten.Add(new LatLng(51.37098, 4.47303));
            punten.Add(new LatLng(51.37098, 4.47303));
            punten.Add(new LatLng(51.3706, 4.47411));
            punten.Add(new LatLng(51.36993, 4.47607));
            punten.Add(new LatLng(51.36927, 4.47816));
            punten.Add(new LatLng(51.3692, 4.47831));
            punten.Add(new LatLng(51.36915, 4.47838));
            punten.Add(new LatLng(51.3691, 4.47843));
            punten.Add(new LatLng(51.36887, 4.47853));
            punten.Add(new LatLng(51.36887, 4));*/
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


            if (map != null)
            {
                map.MyLocationEnabled = true;
                for (int i = 0; i < markers.Length; i++)
                {
                    map.AddMarker(markers[i]);
                    map.SetInfoWindowAdapter(this);
                    map.SetOnInfoWindowClickListener(this);
                    //map.SetInfoWindowAdapter(new CustomMarkerPopupAdapter(LayoutInflater));
                }
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                zoomToLocation(new LatLng(position.Latitude, position.Longitude), map, 15);
            }
        }

        private void zoomToLocation(LatLng locatie, GoogleMap map, int zoom)
        {
            map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(locatie, zoom));
            map.AnimateCamera(CameraUpdateFactory.ZoomTo(zoom), 2000, null);
        }


        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            string s = marker.Id.Substring(1);
            int i = Convert.ToInt16(s);

            View view = LayoutInflater.Inflate(Resource.Layout.CustomMarkerLayout, null, false);
            view.FindViewById<TextView>(Resource.Id.naamTextView).Text = (string)klanten[i].Username;
            //view.FindViewById<Button>(Resource.Id.bedienButton).Click += MapActivity_Click;
            return view;
        }

        /*private void MapActivity_Click(object sender, EventArgs e)
        {
            //data uit geacpeteerd zetten
            klanten.Remove(klanten[e])
            //data in handeld zetten
        }*/

        public void OnInfoWindowClick(Marker marker)
        {
            string s = marker.Id.Substring(1);
            int i = Convert.ToInt16(s);
            var intent = new Intent(this, typeof(KlantenDetailActivity));
            intent.PutExtra("KlantenId", Convert.ToString(i));
            StartActivity(intent);
        }
    }
    /*
    public class CustomMarkerPopupAdapter : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
    {
        private LayoutInflater _layoutInflater = null;

        public CustomMarkerPopupAdapter(LayoutInflater inflater)
        {
            _layoutInflater = inflater;
        }

        public View GetInfoWindow(Marker marker)
        {
            return null;
        }

        public View GetInfoContents(Marker marker)
        {
            var customPopup = _layoutInflater.Inflate(Resource.Layout.CustomMarkerPopup, null);

            TextView titleTextView = customPopup.FindViewById<TextView>(Resource.Id.custom_marker_popup_title);
            if (titleTextView != null)
            {
                titleTextView.Text = "info";
            }

            TextView snippetTextView = customPopup.FindViewById<TextView>(Resource.Id.custom_marker_popup_snippet);
            if (snippetTextView != null)
            {
                snippetTextView.Text = "snipit";
            }

            return customPopup;
        }
    }*/
}