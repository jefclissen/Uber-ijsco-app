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
            GetDataFromDB();
        }

        private void GetDataFromDB()
        {
            string result;
            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri("http://35.165.103.236:80/clientcredentials");
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("email", mCredentials.Get("email"));
                parameters.Add("password", mCredentials.Get("password"));

                byte[] response = client.UploadValues(uri, parameters);
                result = System.Text.Encoding.UTF8.GetString(response);

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