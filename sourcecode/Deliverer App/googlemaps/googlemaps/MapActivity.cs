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

namespace googlemaps
{
    [Activity(Label = "googlemaps", MainLauncher = true, Icon = "@drawable/icon")]
    public class MapActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LatLng location = new LatLng(50.897778, 3.013333);

            SetContentView(Resource.Layout.MapView);
            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            GoogleMap map = mapFrag.Map;
            if (map != null)
            {
                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(location);
                markerOpt1.SetTitle("Locatie");
                map.AddMarker(markerOpt1);
            }
        }
    }
}