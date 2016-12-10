using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Plugin.Geolocator;
using System.Collections.Specialized;
using System.Net;

namespace Uber_Client
{
    [Activity(Label = "Uber_Client", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        //SignUp Menu
        private Button mBtnSignUp;
        private Button mBtnSignIn;
        private ProgressBar mProgressBar;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);

            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
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

            signUpDialog.mOnsignUpComplete += SignUpDialog_mOnsignUpComplete;
        }

        private void SignUpDialog_mOnsignUpComplete(object sender, OnSignUpEventArgs e)
        {
            mProgressBar.Visibility = Android.Views.ViewStates.Visible;
            //make invisible after HTTP async request
            //user clicked signup btn/  event broadcasted from signupdialog class
            //Toast.MakeText(this, "trying to upload", ToastLength.Long);


            WebClient client = new WebClient();
            Uri uri = new Uri("http://localhost:3000/createuser");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", e.Firstname);
            parameters.Add("email", e.Email);
            parameters.Add("password", e.Password);

            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            //client.UploadValuesAsync(uri, parameters);

        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            //fires when data is send and we have received an response from the server
            //whether the data is accepted or not proceed to the app or give an error message
            //succeeded intent to other activity.
            //we can use the arguments of this method to indicate if the post to the database was accepted & we can store the accounts id here globally
        }
    }
}

