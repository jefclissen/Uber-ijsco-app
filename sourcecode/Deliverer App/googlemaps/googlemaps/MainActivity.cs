using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;

namespace googlemaps
{
    [Activity(Label = "googlemaps", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            GoogleMap map = mapFrag.Map;
            if (map != null)
            {
                // The GoogleMap object is ready to go.
            }
        }
    }
}

