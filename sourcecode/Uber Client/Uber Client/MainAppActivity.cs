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
using System.Collections.Specialized;
using System.Net;
using Plugin.Geolocator;

namespace Uber_Client
{
    [Activity(Label = "MainAppActivity")]
    public class MainAppActivity : Activity
    {
        private Button mBtnIceCream;
        NameValueCollection credentials;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainApp);
            credentials = new NameValueCollection();

            credentials.Add("username", Intent.GetStringExtra("username"));//Get the data passed by previous Activity
            credentials.Add("email", Intent.GetStringExtra("email"));
            credentials.Add("password", Intent.GetStringExtra("password"));

            mBtnIceCream = FindViewById<Button>(Resource.Id.iceCreamButton);
            mBtnIceCream.Click += MBtnIceCream_Click;
        }

        private async void MBtnIceCream_Click(object sender, EventArgs e)
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            

            string mResult;
            using (WebClient client = new WebClient())
            {

                Uri uri = new Uri("http://35.165.103.236:80/icecreamrequest");
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("email", credentials.Get("email"));
                parameters.Add("username", credentials.Get("username"));
                parameters.Add("userLong", position.Longitude.ToString() );
                parameters.Add("userLat", position.Latitude.ToString());
                byte[] response = client.UploadValues(uri, parameters);
                mResult = System.Text.Encoding.UTF8.GetString(response);
            }

            Toast.MakeText(this, mResult, ToastLength.Long).Show();
        }
    }
}