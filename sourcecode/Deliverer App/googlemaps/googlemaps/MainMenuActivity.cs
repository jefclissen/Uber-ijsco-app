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

namespace googlemaps
{
    [Activity(Label = "MainMenuActivity", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainMenuActivity : Activity
    {
        private Button mapButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainMenuLayout);

            FindViews();
            HandleEvents();
        }

        private void FindViews()
        {
            mapButton = FindViewById<Button>(Resource.Id.mapButton);
        }

        private void HandleEvents()
        {
            mapButton.Click += MapButton_Click;
        }

        private void MapButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MapActivity));
            StartActivity(intent);
        }
    }
}