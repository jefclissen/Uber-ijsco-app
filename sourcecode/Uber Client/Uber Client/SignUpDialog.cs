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
using System.Net;
using System.Collections.Specialized;
using Android.Graphics;

namespace Uber_Client
{
    public class OnSignUpEventArgs : EventArgs
    {
        //event aanmaken overge�rfd van Event args zodat we deze argumenten kunnen broadcasten naar onze main activity
        private string mFirstname;
        private string mEmail;
        private string mPassword;
        public string Firstname {
            get { return mFirstname; }
            set { mFirstname = value; }
        }
        public string Email
        {
            get { return mEmail; }
            set { mEmail = value; }
        }
        public string Password
        {
            get { return mPassword; }
            set { mPassword = value; }
        }
        public OnSignUpEventArgs(string _firstname,string _email,string _password) :base()
        {
            Firstname = _firstname;
            Email = _email;
            Password = _password;
        }
    }
    class SignUpDialog : DialogFragment
    {
        private EditText mTxtFirstname;
        private EditText mTxtEmail;
        private EditText mTxtPassword;
        private Button mBtnSignUp;
        private TextView mtxtInfo;

        public event EventHandler<OnSignUpEventArgs> mOnsignUpComplete; //public so others can subscribe to this event

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.SignUpDialog, container, false);

            mTxtFirstname = view.FindViewById<EditText>(Resource.Id.txtFirstname);
            mTxtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            mTxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            mtxtInfo = view.FindViewById<TextView>(Resource.Id.txtInfo);
            mtxtInfo.SetTextColor(Color.Red);
            mBtnSignUp = view.FindViewById<Button>(Resource.Id.btnDialogEmail);
            mBtnSignUp.Click += MBtnSignUp_Click;
            return view;
        }

        private void MBtnSignUp_Click(object sender, EventArgs e)
        {
            //user clicked signUp btn
            //event broadcast that button has been pressed with custom event argument class (made above)
            //POST HERE ?
            //mProgressBar.Visibility = Android.Views.ViewStates.Visible;//make invisible after HTTP async request
            string mResult;
            //Toast.MakeText(this, "trying to upload", ToastLength.Long).Show();

            using (WebClient client = new WebClient())
            {

                Uri uri = new Uri("http://35.165.103.236:80/addclient");
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("username", mTxtFirstname.Text);
                parameters.Add("email", mTxtEmail.Text);
                parameters.Add("password", mTxtPassword.Text);
                byte[] response = client.UploadValues(uri, parameters);
                mResult = System.Text.Encoding.UTF8.GetString(response);
            }
            if (mResult.Substring(0, 1) == "1")//SUCCES
            {
                //Toast.MakeText(this, mResult.Substring(1), ToastLength.Long).Show();
                mtxtInfo.Text = mResult.Substring(1);
                mOnsignUpComplete.Invoke(this, new OnSignUpEventArgs(mTxtFirstname.Text, mTxtEmail.Text, mTxtPassword.Text));
                this.Dismiss();
            }
            else if(mResult.Substring(0, 1) == "0")//Failed to make account 
            {
                //Toast.MakeText(this, mResult.Substring(1), ToastLength.Long).Show();
                mtxtInfo.Text = mResult.Substring(1);
                
                //mTxtEmail.Text = mResult.Substring(1);
                //substring for removing first character of string 
            }
            //POST

            //this.Dismiss();//close dialog

        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }
    }
}