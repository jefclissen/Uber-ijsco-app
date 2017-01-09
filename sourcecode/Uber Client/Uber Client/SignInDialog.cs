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
using Android.Graphics;
using System.Net;
using System.Collections.Specialized;

namespace Uber_Client
{
    public class OnSignInEventArgs : EventArgs
    {
        //event aanmaken overgeërfd van Event args zodat we deze argumenten kunnen broadcasten naar onze main activity
        private string mEmail;
        private string mPassword;

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
        public OnSignInEventArgs(string _email, string _password) : base()
        {
            Email = _email;
            Password = _password;
        }
    }
    class SignInDialog : DialogFragment
    {
        private EditText mTxtEmail;
        private string mEmailFromSignUp;
        private string mPasswordFromSignUp;
        private EditText mTxtPassword;
        private Button mBtnSignIn;
        private TextView mtxtInfo;
        public event EventHandler<OnSignInEventArgs> mOnsignInComplete; //public so others can subscribe to this event

        public SignInDialog(string _email,string _password)
        {
            if (_email != null) {
                mEmailFromSignUp = _email;
                mPasswordFromSignUp = _password;
            }
            
            
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.SignInDialog, container, false);
            
            mTxtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            mTxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            mTxtEmail.Text = mEmailFromSignUp;
            mTxtPassword.Text = mPasswordFromSignUp;

            mtxtInfo = view.FindViewById<TextView>(Resource.Id.txtInfo);
            mtxtInfo.SetTextColor(Color.Red);

            mBtnSignIn = view.FindViewById<Button>(Resource.Id.btnSignIn);
            mBtnSignIn.Click += MBtnSignIn_Click;
            return view;
        }

        private void MBtnSignIn_Click(object sender, EventArgs e)
        {
            if (mTxtEmail.Text != null && mTxtPassword.Text != null)
            {
                string mResult;
                using (WebClient client = new WebClient())
                {
                    Uri uri = new Uri("http://35.165.103.236:80/checkpassword");
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("email", mTxtEmail.Text);
                    parameters.Add("password", mTxtPassword.Text);
                    byte[] response = client.UploadValues(uri, parameters);
                    mResult = System.Text.Encoding.UTF8.GetString(response);
                }

                if (mResult.Substring(0, 1) == "1")//SUCCES
                {
                    mtxtInfo.Text = mResult.Substring(1);
                    mOnsignInComplete.Invoke(this, new OnSignInEventArgs(mTxtEmail.Text, mTxtPassword.Text));
                    this.Dismiss();
                }
                else if (mResult.Substring(0, 1) == "0")//Failed to log in
                {
                    mtxtInfo.Text = mResult.Substring(1);
                    mTxtEmail.Text = "";
                    mTxtPassword.Text = "";
                }
            }else
            {
                mtxtInfo.Text = "Gelieve alle velden in te vullen";
            }
        }
        
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }
    }
}