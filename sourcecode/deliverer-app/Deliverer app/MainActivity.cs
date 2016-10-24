using Android.App;
using Android.Widget;
using Android.OS;

namespace Deliverer_app
{
    [Activity(Label = "Deliverer_app", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}

