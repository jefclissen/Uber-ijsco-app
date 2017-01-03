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
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Uber_Client
{
    [Activity(Label = "OptionsActivity")]
    public class OptionsActivity : Activity
    {
        NameValueCollection mCredentials;
        TextView txtMyEmail, txtMyUsername, txtMyPhoneNumber, txtMyCreditcardNumber, txtMyCardHolder, txtMyZipcode;
        Button btnChangeAccount;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mCredentials = new NameValueCollection();
            mCredentials.Add("username", Intent.GetStringExtra("username"));//Get the data passed by previous Activity
            mCredentials.Add("email", Intent.GetStringExtra("email"));
            mCredentials.Add("password", Intent.GetStringExtra("password"));

            SetContentView(Resource.Layout.Options);
            txtMyEmail = FindViewById<TextView>(Resource.Id.txtMyEmail);
            txtMyUsername = FindViewById<TextView>(Resource.Id.txtMyUsername);
            txtMyPhoneNumber = FindViewById<TextView>(Resource.Id.txtMyPhoneNumber);
            txtMyCreditcardNumber = FindViewById<TextView>(Resource.Id.txtMyCreditcardNumberAndCVC);
            txtMyCardHolder = FindViewById<TextView>(Resource.Id.txtMyCardHolder);
            txtMyZipcode = FindViewById<TextView>(Resource.Id.txtMyZipCode);
            btnChangeAccount = FindViewById<Button>(Resource.Id.AccountSettingsBtn);
            btnChangeAccount.Click += BtnChangeAccount_Click;
            GetDataFromDB();
        }

        private void BtnChangeAccount_Click(object sender, EventArgs e)
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            AccountDialog accountDialog = new AccountDialog(this,mCredentials.Get("Password"), mCredentials.Get("Email"), mCredentials.Get("username"),
                txtMyPhoneNumber.Text, txtMyCreditcardNumber.Text, txtMyCardHolder.Text, txtMyZipcode.Text);

            accountDialog.Show(trans, "dialog fragment");
            accountDialog.mOnAccountChangeComplete += AccountDialog_mOnAccountChangeComplete;
        }

        private void AccountDialog_mOnAccountChangeComplete(object sender, OnAccountChangeEventArgs e)
        {
            string result;
            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri("http://35.165.103.236:80/updatecredentials");
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("useremail", mCredentials.Get("email"));
                parameters.Add("password", e.Password);
                parameters.Add("phone", e.Phone);
                parameters.Add("cardnr", e.CreditCardNumber);
                //parameters.Add("cvc", e.Phone);
                //parameters.Add("experationdate", e.ExperationDate);
                parameters.Add("zipcode", e.ZipCode);

                byte[] response = client.UploadValues(uri, parameters);
                result = System.Text.Encoding.UTF8.GetString(response);
                result = result.Substring(1, result.Length - 2);
                Toast.MakeText(this, result, ToastLength.Long).Show();
                GetDataFromDB();
            }
        }

        private void GetDataFromDB()
        {
            string result;
            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri("http://35.165.103.236:80/getcredentials");
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("useremail", mCredentials.Get("email"));
                parameters.Add("password", mCredentials.Get("password"));
                //parameters.Add("useremail","jef@jef.be");
                //parameters.Add("password", "jef");

                byte[] response = client.UploadValues(uri, parameters);
                result = System.Text.Encoding.UTF8.GetString(response);
                result = result.Substring(1,result.Length-2);
                
                //get data out of json
                JToken token = JObject.Parse(result);
                string phoneNumber =        (string)token.SelectToken("phoneNumber");
                string creditcardNumber =   (string)token.SelectToken("creditcardnumber");//should only receive last 4 chars
                string cardHolder   =       (string)token.SelectToken("cardholder");
                string zipCode =            (string)token.SelectToken("zipcode");

                //update views
                txtMyPhoneNumber.Text = phoneNumber;
                txtMyCreditcardNumber.Text = creditcardNumber;
                txtMyCardHolder.Text = cardHolder;
                txtMyZipcode.Text = zipCode;
            }

        }
    }
}