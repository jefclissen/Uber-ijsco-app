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
using Deliverer.Core.Service;
using Deliverer.Core.Modle;
using Deliverer_app.Adapters;

namespace Deliverer_app
{
    [Activity(Label = "Lijst van klanten", MainLauncher = false)]
    public class LijstActivity : Activity
    {
        private ListView klantenLijst;

        private List<Klant> klanten;
        private KlantDataService dataService;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.LijstView);
            klantenLijst = FindViewById<ListView>(Resource.Id.klantenListView);
            KlantDataService dataService = new KlantDataService();
            klanten = dataService.GeefAlleKlanten();
            klantenLijst.Adapter = new KlantenLijstAdapter(this, klanten);
            klantenLijst.FastScrollEnabled = true;
        }

        
    }
}