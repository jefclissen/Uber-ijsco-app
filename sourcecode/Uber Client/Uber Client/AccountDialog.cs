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
    public class OnAccountChangeEventArgs : EventArgs
    {
        //event aanmaken overgeërfd van Event args zodat we deze argumenten kunnen broadcasten naar onze Account Activity
        private string mOldPassword;
        private string mPassword;
        private string mPhone;
        private string mCreditCardNumber;
        private string mCardHolder;
        private string mZipCode;
        public string OldPassword
        {
            get { return mOldPassword; }
            set { mOldPassword = value; }
        }
        public string NewPassword
        {
            get { return mPassword; }
            set { mPassword = value; }
        }
        public string Phone
        {
            get { return mPhone; }
            set { mPhone = value; }
        }
        public string CreditCardNumber
        {
            get { return mCreditCardNumber; }
            set { mCreditCardNumber = value; }
        }
        public string Cardholder
        {
            get { return mCardHolder; }
            set { mCardHolder = value; }
        }
        public string ZipCode
        {
            get { return mZipCode; }
            set { mZipCode = value; }
        }
        

        public OnAccountChangeEventArgs(string _oldpassword,string _newpassword,string _phone, string _creditCardNumber, string _cardHolder, string _zipCode) : base()
        {
            OldPassword = _oldpassword;
            NewPassword = _newpassword;
            Phone = _phone;
            CreditCardNumber = _creditCardNumber;
            Cardholder = _cardHolder;
            ZipCode = _zipCode;
        }
    }
    class AccountDialog : DialogFragment
    {
        private string mEmail, mUsername;
        private string mPrevPassword, mPrevPhone, mPrevCreditCardNumber, mPrevCardHolder, mPrevZipCode;
        EditText mTxtOldPassword, mTxtNewPassword, mTxtNewPassword2,mTxtPhone,mTxtCreditCardNumber, mTxtCardHolder,mTxtZipCode;
        TextView mTxtEmail, mTxtUsername;
        Button mBtnChangeAccount;
        Context Parent;
        public event EventHandler<OnAccountChangeEventArgs> mOnAccountChangeComplete;
        public AccountDialog(Context _c, string _password,string _email,string _username,string _phone, string _creditCardNumber, string _cardHolder, string _zipCode)
        {
            Parent = _c;
            mEmail = _email;
            mUsername = _username;
            mPrevPassword = _password;
            mPrevPhone = _phone;
            mPrevCreditCardNumber = _creditCardNumber;
            mPrevCardHolder = _cardHolder;
            mPrevZipCode = _zipCode;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.ChangeAccount, container, false);
            mTxtOldPassword             = view.FindViewById<EditText>(Resource.Id.txtMyOldPassword);
            mTxtNewPassword             = view.FindViewById<EditText>(Resource.Id.txtMyNewPassword);
            mTxtNewPassword2            = view.FindViewById<EditText>(Resource.Id.txtMyNewPassword2);
            mTxtPhone                   = view.FindViewById<EditText>(Resource.Id.txtMyPhoneNumber);
            mTxtCreditCardNumber        = view.FindViewById<EditText>(Resource.Id.txtMyCreditCardNumber);
            mTxtCardHolder              = view.FindViewById<EditText>(Resource.Id.txtMyCardHolder);
            mTxtZipCode                 = view.FindViewById<EditText>(Resource.Id.txtMyZipCode);

            mTxtEmail                   = view.FindViewById<TextView>(Resource.Id.txtMyEmail);
            mTxtUsername                = view.FindViewById<TextView>(Resource.Id.txtMyUsername);
        
            mTxtEmail.Text              = mEmail;
            mTxtUsername.Text           = mUsername;
            mTxtPhone.Text              = mPrevPhone;
            mTxtCreditCardNumber.Text   = mPrevCreditCardNumber;
            mTxtCardHolder.Text         = mPrevCardHolder;
            mTxtZipCode.Text            = mPrevZipCode;

            mBtnChangeAccount           = view.FindViewById<Button>(Resource.Id.AccountSave);
            mBtnChangeAccount.Click += MBtnChangeAccount_Click;
            return view;
        }

        private void MBtnChangeAccount_Click(object sender, EventArgs e)
        {
            if (mTxtNewPassword.Text == mTxtNewPassword2.Text) {
                if (mTxtNewPassword.Text == "")
                {
                    //PASSWORD NOT CHANGED -> use previous password
                    mOnAccountChangeComplete.Invoke(this, new OnAccountChangeEventArgs(mPrevPassword, mPrevPassword,
                                    mTxtPhone.Text, mTxtCreditCardNumber.Text, mTxtCardHolder.Text, mTxtZipCode.Text));
                    this.Dismiss();
                }
                else if (mTxtOldPassword.Text == "")
                {
                    //PLEASE ENTER OLD PASSWORD 
                    mTxtNewPassword.Text = "";
                    mTxtNewPassword2.Text = "";
                    Toast.MakeText(Parent, "please enter old password", ToastLength.Long).Show();
                } else if(mTxtOldPassword.Text == mPrevPassword)
                { 
                    //PASSWORD CHANGED & OLD PASSWORD FILLED IN -> use new password
                    mOnAccountChangeComplete.Invoke(this, new OnAccountChangeEventArgs(mPrevPassword,mTxtNewPassword.Text,
                                    mTxtPhone.Text, mTxtCreditCardNumber.Text, mTxtCardHolder.Text, mTxtZipCode.Text));
                    this.Dismiss();
                }else
                {
                    //OLD PASSWORD IS WRONG
                    Toast.MakeText(Parent, "Old password invalid", ToastLength.Long).Show();
                    mTxtOldPassword.Text = "";
                    mTxtNewPassword.Text = "";
                    mTxtNewPassword2.Text = "";
                }

            }
            else
            {
                //NEW PASSWORDS DO NOT MATCH EACHOTHER
                mTxtOldPassword.Text = "";
                mTxtNewPassword.Text = "";
                mTxtNewPassword2.Text = "";
                Toast.MakeText(Parent, "passwords don't match", ToastLength.Long).Show();
            }
            
        }
    }
}