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

namespace Uber_Client
{
    public class OnSignUpEventArgs : EventArgs
    {
        //event aanmaken overgeërfd van Event args zodat we deze argumenten kunnen broadcasten naar onze main activity
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

        public event EventHandler<OnSignUpEventArgs> mOnsignUpComplete; //public so others can subscribe to this event

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.SignUpDialog, container, false);

            mTxtFirstname = view.FindViewById<EditText>(Resource.Id.txtFirstname);
            mTxtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            mTxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            mBtnSignUp = view.FindViewById<Button>(Resource.Id.btnDialogEmail);
            mBtnSignUp.Click += MBtnSignUp_Click;
            return view;
        }

        private void MBtnSignUp_Click(object sender, EventArgs e)
        {
            //user clicked signUp btn
            //event broadcast that button has been pressed with custom event argument class (made above)
            mOnsignUpComplete.Invoke(this, new OnSignUpEventArgs(mTxtFirstname.Text, mTxtEmail.Text, mTxtPassword.Text));
            this.Dismiss();//close dialog
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }
    }
}