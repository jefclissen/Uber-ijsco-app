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
using Android.Content.PM;

namespace Uber_Client
{
    [Activity(Label = "Uber Ijsco", MainLauncher = true, Icon = "@drawable/icoon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        //SignUp Menu
        private Button mBtnSignUp;
        private Button mBtnSignIn;
        private ProgressBar mProgressBar;
        NameValueCollection mCredentials;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);
            mCredentials = new NameValueCollection();
            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);
            mBtnSignUp.Click += MBtnSignUp_Click;
            mBtnSignIn.Click += MBtnSignIn_Click;
        }

        private void MBtnSignIn_Click(object sender, EventArgs e)
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            //Toast.MakeText(this, mCredentials.Get("email"), ToastLength.Long);
            SignInDialog signInDialog = new SignInDialog(mCredentials.Get("email"),mCredentials.Get("password"));
            signInDialog.Show(trans, "dialog fragment");
            signInDialog.mOnsignInComplete += SignInDialog_mOnsignInComplete;
        }

        private void SignInDialog_mOnsignInComplete(object sender, OnSignInEventArgs e)
        {
            var intent = new Intent(this, typeof(MainAppActivity));
            intent.PutExtra("username", mCredentials.Get("username"));//can be empty
            intent.PutExtra("email", e.Email);
            intent.PutExtra("password", e.Password);
            StartActivity(intent);
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
            Toast.MakeText(this, "Account Created\nTap 'Sign In' to get started!", ToastLength.Long).Show();
            
        }
    }
}