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
using Deliverer.Core.Model;
using System.Net;
using Newtonsoft.Json;

namespace googlemaps
{
    [Activity(Label = "Uber IJsco Verdeler", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        private Button loginButton;
        private EditText emailInput;
        private EditText paswoordInput;
        private List<Gebruiker> gebruikers;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.LoginLayout);

            loginButton = FindViewById<Button>(Resource.Id.inlogButton);
            emailInput = FindViewById<EditText>(Resource.Id.emailInput);
            paswoordInput = FindViewById<EditText>(Resource.Id.paswoordInput);

            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("http://35.165.103.236:80/driverlist");
                gebruikers = JsonConvert.DeserializeObject<List<Gebruiker>>(json);
            }

            loginButton.Click += LoginButton_Click;
            
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gebruikers.Count; i++)
            {
                if(gebruikers[i].DriverEmail == emailInput.Text && gebruikers[i].DriverPassword == paswoordInput.Text)
                {
                    var intent = new Intent(this, typeof(MainMenuActivity));
                    StartActivity(intent);
                }
                else
                    Toast.MakeText(this, "e-mail en paswoord niet correct", ToastLength.Long).Show();
            }
        }
    }
}