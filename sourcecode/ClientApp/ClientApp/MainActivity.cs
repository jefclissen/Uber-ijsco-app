using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Net;
using System.Collections.Specialized;
using Plugin.Geolocator;
using Android;
using Android.Media;

namespace ClientApp
{
    [Activity(Label = "ClientApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button iceCreamButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            getViews();
            setActions();
        }
        private void setActions()
        {
            iceCreamButton.Click += IceCreamButton_Click;
        }

        private async void IceCreamButton_Click(object sender, EventArgs e)
        {
            var locator = CrossGeolocator.Current;

            locator.DesiredAccuracy = 10;
            var position = await locator.GetPositionAsync(10000);
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["Latitude"] = position.Latitude.ToString();
                values["Longitude"] = position.Longitude.ToString();

                var response = client.UploadValues("INSERT URL HERE!!", values);
                //http://localhost/clientWantsIceCream
                //var responseString = Encoding.Default.GetString(response);
            }
        }

        private void getViews()
        {
            iceCreamButton = FindViewById<Button>(Resource.Id.iceCreamButton);
        }
    }
}

