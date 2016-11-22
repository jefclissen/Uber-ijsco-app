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

namespace googlemaps
{
    [Activity(Label = "AccepteerActivity")]
    public class AccepteerActivity : Activity
    {
        private ListView klantenlijst;
        private List<Klant> klanten;
        private Button accepteerButton;
        private List<Klant> geaccepteerdeKlanten;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AccepteerLayout);

            geaccepteerdeKlanten = new List<Klant>();

            FindViews();
            VulLijst();
            HandleEvents();
        }

        private void HandleEvents()
        {
            //klantenlijst.ItemClick += Klantenlijst_ItemClick;
            accepteerButton.Click += AccepteerButton_Click;
            klantenlijst.ItemSelected += Klantenlijst_ItemSelected;
            
        }
        private void Klantenlijst_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            
        }
        private void AccepteerButton_Click(object sender, EventArgs e)
        {
            /*List<Klant> geaccepteerdeKlanten = new List<Klant>();
            foreach (string s in klantenlijst.ItemSelected)
            {
            
            }*/
            /*
            for (int i = 0; i < klanten.Count; i++)
            {
                if (klantenlijst.Item == true){
                    geaccepteerdeKlanten.Add(klanten[i]);

            }*/
        }
        private void Klantenlijst_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
             
            //klantenlijst.SetItemChecked(1, true);
        }
        private void VulLijst()
        {
            KlantDataService dataService = new KlantDataService();
            klanten = dataService.GeefAlleKlanten();
            klanten = dataService.GeefAlleKlanten();

            List<string> naamKlanten = new List<string>();

            for (int i = 0; i < klanten.Count; i++)
            {
                naamKlanten.Add(klanten[i].Naam);
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