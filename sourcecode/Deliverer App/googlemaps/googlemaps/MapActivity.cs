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

namespace googlemaps
{
    [Activity(Label = "googlemaps", MainLauncher = true, Icon = "@drawable/icon")]
    public class MapActivity : Activity
    {
        private List<Klant> klanten;
        private MarkerOptions[] locaties;
        private KlantDataService dataService;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            

            getKlanten();
            makeMarkers();
            makeMap();            
        }

        public void getKlanten()
        {
            KlantDataService dataService = new KlantDataService();
            klanten = dataService.GeefAlleKlanten();
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
        private void makeMap()
        {
            SetContentView(Resource.Layout.MapView);
            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            GoogleMap map = mapFrag.Map;
            if (map != null)
            {
                for (int i = 0; i < klanten.Count; i++)
                {
                    map.AddMarker(locaties[i]);
                }
                map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(locaties[0].Position.Latitude, locaties[0].Position.Longitude), 15));
                map.AnimateCamera(CameraUpdateFactory.ZoomTo(15), 2000, null);
            }
        }
    }
}