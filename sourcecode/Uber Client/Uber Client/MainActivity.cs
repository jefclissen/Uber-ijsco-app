using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Plugin.Geolocator;
using System.Collections.Specialized;
//using System.Net;

namespace Uber_Client
{
    [Activity(Label = "Uber_Client", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button mBtnSignUp;
        private Button mBtnSignIn;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);
            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);
            mBtnSignUp.Click += MBtnSignUp_Click;
            mBtnSignIn.Click += MBtnSignIn_Click;

        }

        private void MBtnSignIn_Click(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.Main);
        }

        private void MBtnSignUp_Click(object sender, EventArgs e)
        {
            //pull up dialog
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            SignUpDialog signUpDialog = new SignUpDialog();
            signUpDialog.Show(trans, "dialog fragment");
        }


        /*
        private async void B_Click(object sender, EventArgs e)
        {
            
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            //var location = await locator.GetPositionAsync(timeoutMilliseconds:10000);

            //Toast.MakeText(this, location.Latitude.ToString(), ToastLength.Long).Show();

            Toast.MakeText(this, "trying to send data", ToastLength.Long).Show();
            SendLocation("12.3456","65.4321");
            
            SetContentView(Resource.Layout.Login);
        }

        private void SendLocation(string latitude,string longitude)
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["Latitude"] = latitude;
                values["Longitude"] = longitude;

                var response = client.UploadValues("", values);
            }
        }
    */

    }
}

