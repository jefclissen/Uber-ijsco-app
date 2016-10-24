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
using Deliverer.Core.Modle;
using Deliverer.Core.Service;

namespace Deliverer_app
{
    [Activity(Label = "View on Map", MainLauncher = true)]
    public class ViewOnMapActivity : Activity
    {
        private Button mapButton;

        private List<Klant> klanten;
        private KlantDataService dataService;
        private Klant activeKlant;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.KaartView);

            dataService = new KlantDataService();
            klanten = dataService.GeefAlleKlanten();

            activeKlant = klanten;
            //mapButton = FindViewById<Button>(Resource.Id.viewOnMapButton);
            mapButton = FindViewById<Button>(Resource.Id.viewOnMapButton);
            mapButton.Click += MapButton_Click;
        }

        private void MapButton_Click(object sender, EventArgs e)
        {
            string geo = "geo:" + activeKlant.Longitude + "," + activeKlant.Latitude;
            Android.Net.Uri klantUri = Android.Net.Uri.Parse(geo);

            Intent mapIntent = new Intent(Intent.ActionView, klantUri);
            StartActivity(mapIntent);
        }
    }
}