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
    [Activity(Label = "Uber Ijsco Client", MainLauncher = true, Icon = "@drawable/icoon")]
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

        }
    }
}

