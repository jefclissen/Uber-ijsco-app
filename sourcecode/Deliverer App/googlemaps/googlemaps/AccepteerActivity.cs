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
using System.Threading;

namespace googlemaps
{
    [Activity(Label = "AccepteerActivity")]
    public class AccepteerActivity : Activity
    {
        private ListView klantenlijst;
        private List<Klant> serverKlanten;
        private Button accepteerButton;
        private List<Klant> geaccepteerdeKlanten;
        private bool[] klantenHelper;
        private List<string> naamKlanten; //lijst als hulpmiddel
        private List<string> selectedKlanten = new List<string>();

        private KlantDataService dataService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AccepteerLayout);
            dataService = new KlantDataService();

            FindViews();         
            VulLijst();
            HandleEvents();
        }

        private void HandleEvents()
        {
            klantenlijst.ItemClick += Klantenlijst_ItemClick;
            accepteerButton.Click += AccepteerButton_Click;
        }
        private void AccepteerButton_Click(object sender, EventArgs e)
        {
            geaccepteerdeKlanten = new List<Klant>();
            for (int i = 0; i < serverKlanten.Count; i++)
            {
                if (klantenHelper[i] == true)
                    geaccepteerdeKlanten.Add(serverKlanten[i]);
            }
            dataService.pushGeaccepteerdeKlanten(geaccepteerdeKlanten);
            Toast.MakeText(this, "klanten zijn toegevoegd", ToastLength.Long).Show();
            var intent = new Intent(this, typeof(MapActivity));
            StartActivity(intent);
        }


        private void Klantenlijst_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            klantenHelper[e.Position] = !klantenHelper[e.Position];
        }
        private void VulLijst()
        {

            serverKlanten = dataService.GeefAlleKlantenFromServer();

            naamKlanten = new List<string>();
            klantenHelper = new bool[serverKlanten.Count];

            for (int i = 0; i < serverKlanten.Count; i++)
            {
                naamKlanten.Add(serverKlanten[i].Naam);
                klantenHelper[i] = false;
            }
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemMultipleChoice, naamKlanten);
            klantenlijst.Adapter = adapter;
            klantenlijst.ChoiceMode = ChoiceMode.Multiple;
        }
        private void FindViews()
        {
            klantenlijst = FindViewById<ListView>(Resource.Id.klantenListView);
            accepteerButton = FindViewById<Button>(Resource.Id.accepteerButton);
        }
    }
}