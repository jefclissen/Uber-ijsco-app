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

namespace googlemaps
{
    [Activity(Label = "KlantenDetailActivity")]
    public class KlantenDetailActivity : Activity
    {
        private List<Klant> geaccepteerdeKlanten;
        private Klant klant;
        private TextView naamKlantTextView;
        private Button bediendButton;
        private int klantId;

        private KlantDataService dataService;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.KlantDetailActivity);

            dataService = new KlantDataService();
            geaccepteerdeKlanten = new List<Klant>();
            klant = new Klant();

            string k = Intent.GetStringExtra("KlantenId");
            klantId = Convert.ToInt32(k);

            GetKlant();
            FindViews();
            FillViews();
            // Create your application here
        }

        private void GetKlant()
        {
            geaccepteerdeKlanten = dataService.getGeaccepteerdeKlanten();
            klant = geaccepteerdeKlanten[klantId];
        }

        private void FillViews()
        {
            naamKlantTextView.Text = klant.Naam;
            bediendButton.Click += BediendButton_Click;
        }

        private void BediendButton_Click(object sender, EventArgs e)
        {
            //data uit geaccepteerd
            dataService.klantBediend(geaccepteerdeKlanten[klantId]);
            //data in handeld klanten
            var intent = new Intent(this, typeof(MapActivity));
            StartActivity(intent);
        }

        private void FindViews()
        {
            naamKlantTextView = FindViewById<TextView>(Resource.Id.naamKlantTextView);
            bediendButton = FindViewById<Button>(Resource.Id.bediendButton);
        }
    }
}