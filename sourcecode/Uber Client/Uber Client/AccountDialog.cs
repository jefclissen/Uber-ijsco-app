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
        private string mPassword;
        private string mPhone;
        private string mCreditCardNumber;
        private string mCardHolder;
        private string mZipCode;
        public string Password
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
        

        public OnAccountChangeEventArgs(string _password,string _phone, string _creditCardNumber, string _cardHolder, string _zipCode) : base()
        {
            Password = _password;
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
        EditText mTxtPassword, mTxtPassword2,mTxtPhone,mTxtCreditCardNumber, mTxtCardHolder,mTxtZipCode;
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
            mTxtPassword = view.FindViewById<EditText>(Resource.Id.txtMyPassword);
            mTxtPassword2 = view.FindViewById<EditText>(Resource.Id.txtMyPassword2);
            mTxtPhone = view.FindViewById<EditText>(Resource.Id.txtMyPhoneNumber);
            mTxtCreditCardNumber = view.FindViewById<EditText>(Resource.Id.txtMyCreditCardNumber);
            mTxtCardHolder = view.FindViewById<EditText>(Resource.Id.txtMyCardHolder);
            mTxtZipCode = view.FindViewById<EditText>(Resource.Id.txtMyZipCode);

            mTxtEmail = view.FindViewById<TextView>(Resource.Id.txtMyEmail);
            mTxtUsername = view.FindViewById<TextView>(Resource.Id.txtMyEmail);
        
            mTxtEmail.Text = mEmail;
            mTxtUsername.Text = mUsername;
            mTxtPhone.Text = mPrevPhone;
            mTxtCreditCardNumber.Text = mPrevCreditCardNumber;
            mTxtCardHolder.Text = mPrevCardHolder;
            mTxtZipCode.Text = mPrevZipCode;

            mBtnChangeAccount = view.FindViewById<Button>(Resource.Id.AccountSave);
            mBtnChangeAccount.Click += MBtnChangeAccount_Click;
            return view;
        }

        private void MBtnChangeAccount_Click(object sender, EventArgs e)
        {
            if (mTxtPassword.Text == mTxtPassword2.Text) {
                if(mTxtPassword.Text == "")
                {
                    //PASSWORD NOT CHANGED -> use previous password
                    mOnAccountChangeComplete.Invoke(this, new OnAccountChangeEventArgs(mPrevPassword,
                                    mTxtPhone.Text,mTxtCreditCardNumber.Text,mTxtCardHolder.Text,mTxtZipCode.Text));
                    this.Dismiss();
                }else
                {
                    //PASSWORD CHANGED -> use new password
                    mOnAccountChangeComplete.Invoke(this, new OnAccountChangeEventArgs(mTxtPassword.Text,
                                    mTxtPhone.Text, mTxtCreditCardNumber.Text, mTxtCardHolder.Text, mTxtZipCode.Text));
                    this.Dismiss();
                }

            }
            else
            {
                mTxtPassword.Text = "";
                mTxtPassword2.Text = "";
                Toast.MakeText(Parent, "passwords don't match", ToastLength.Long).Show();
            }
            
        }
    }
}