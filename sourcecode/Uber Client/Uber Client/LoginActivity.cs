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

namespace Uber_Client
{
    [Activity(Label = "LoginActivity", MainLauncher = true, Icon = "@drawable/icoon")]
    public class LoginActivity : Activity
    {
        //SignUp Menu
        private Button mBtnSignUp;
        private Button mBtnSignIn;
        private ProgressBar mProgressBar;
        NameValueCollection mCredentials;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);
            mCredentials = new NameValueCollection();
            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            mBtnSignUp.Click += MBtnSignUp_Click;
            mBtnSignIn.Click += MBtnSignIn_Click;

        }

        private void MBtnSignIn_Click(object sender, EventArgs e)
        {
            //SetContentView(Resource.Layout.MainApp);
            if (mCredentials.Get("email") != null)
            {
                var intent = new Intent(this, typeof(MainAppActivity));
                intent.PutExtra("username", mCredentials.Get("username"));
                intent.PutExtra("email", mCredentials.Get("email"));
                intent.PutExtra("password", mCredentials.Get("password"));
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this,"Please sign up first?", ToastLength.Long).Show();
            }
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
            mCredentials.Add("username", e.Firstname);
            mCredentials.Add("email", e.Email);
            mCredentials.Add("password", e.Password);
            
        }
    }
}