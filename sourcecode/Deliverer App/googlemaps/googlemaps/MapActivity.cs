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

namespace googlemaps
{
    [Activity(Label = "googlemaps", Icon = "@drawable/icon")]
    public class MapActivity : Activity, IInfoWindowAdapter, IOnInfoWindowClickListener
    {
        private List<Klant> klanten;
        private MarkerOptions[] markers;
        private KlantDataService dataService;
        private LatLng myPosition;
        private MarkerOptions myPositionMarker;
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
                Toast.MakeText(this, "klanten zijn toegevoegd", ToastLength.Long).Show();
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
                marker.SetPosition(new LatLng(klanten[i].Longitude, klanten[i].Latitude));
                marker.SetTitle(klanten[i]._id);

                markers[i] = marker;
            }
        }
        private async void getMyLocation()
        {
            myPosition = new LatLng(0, 0);
            /*var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(10000);

            myPositionMarker.SetPosition(new LatLng(position.Latitude, position.Longitude));
            myPositionMarker.SetTitle("mijn positie");*/


        }
        private void makeMap()
        {
            SetContentView(Resource.Layout.MapView);
            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            GoogleMap map = mapFrag.Map;

            Polyline line = map.AddPolyline(new PolylineOptions()
     .Add(new LatLng(51.5, -0.1), new LatLng(40.7, -74.0)));
            // .Width(5)
            // .Color(Color.Red));

            map.MyLocationEnabled = true;
            if (map != null)
            {

                for (int i = 0; i < markers.Length; i++)
                {
                    map.AddMarker(markers[i]);
                    map.SetInfoWindowAdapter(this);
                    map.SetOnInfoWindowClickListener(this);
                    //map.SetInfoWindowAdapter(new CustomMarkerPopupAdapter(LayoutInflater));
                }
                zoomToLocation(new LatLng(51.218999, 4.401556), map, 30);

                /*if(myPositionMarker != null)
                {
                    map.AddMarker(myPositionMarker); //zet huidige locatie op kaart
                    zoomToLocation(myPositionMarker, map, 7);

                }
                else
                {
                    MarkerOptions marker = new MarkerOptions();
                    marker.SetPosition(new LatLng(51.218999, 4.401556));
                    zoomToLocation(marker, map, 20);
                }
                /*
                if(markers == null || markers.Length ==0)
                {
                    MarkerOptions marker = new MarkerOptions();
                    marker.SetPosition(new LatLng(51.218999, 4.401556));
                    zoomToLocation(marker, map, 20);
                }
                else
                {
                    zoomToLocation(locmarkersaties[0], map, 7);
                }*/
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
            view.FindViewById<TextView>(Resource.Id.naamTextView).Text = (string)klanten[i].Naam;
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

    class KlantMareker {
        public KlantMareker(Klant klant)
        {

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