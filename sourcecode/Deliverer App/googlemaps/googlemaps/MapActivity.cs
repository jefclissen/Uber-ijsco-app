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

namespace googlemaps
{
    [Activity(Label = "googlemaps", Icon = "@drawable/icon")]
    public class MapActivity : Activity
    {
        private List<Klant> klanten;
        private MarkerOptions[] locaties;
        private KlantDataService dataService;
        private LatLng myPosition;
        private MarkerOptions myPositionMarker;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            

            getKlanten();
            if(klanten != null)
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
            locaties = new MarkerOptions[klanten.Count];
            for (int i = 0; i < klanten.Count; i++)
            {
                
                MarkerOptions marker = new MarkerOptions();
                marker.SetPosition(new LatLng(klanten[i].Longitude, klanten[i].Latitude));
                marker.SetTitle(klanten[i].Naam);

                locaties[i] = marker;
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

            map.MyLocationEnabled = true;
            if (map != null)
            {
                for (int i = 0; i < locaties.Length; i++)
                {
                    map.AddMarker(locaties[i]);
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
                if(locaties == null || locaties.Length ==0)
                {
                    MarkerOptions marker = new MarkerOptions();
                    marker.SetPosition(new LatLng(51.218999, 4.401556));
                    zoomToLocation(marker, map, 20);
                }
                else
                {
                    zoomToLocation(locaties[0], map, 7);
                }*/
            }
        }
        private void zoomToLocation(LatLng locatie, GoogleMap map, int zoom)
        {
            map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(locatie, zoom));
            map.AnimateCamera(CameraUpdateFactory.ZoomTo(zoom), 2000, null);
        }
    }
}