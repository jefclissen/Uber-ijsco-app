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
using System.Threading;

namespace googlemaps
{
    [Activity(Label = "MainMenuActivity", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainMenuActivity : Activity
    {
        private Button mapButton;
        private Button aanvraagButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainMenuLayout);

            ThreadStart myThreadDelegate = new ThreadStart(getNotification);
            Thread myThread = new Thread(myThreadDelegate);
            myThread.Start();

            FindViews();
            HandleEvents();
        }

        public void getNotification()
        {
            int coutn = 0;
            //Toast.MakeText(, "klanten zijn toegevoegd", ToastLength.Short).Show();
            while (true)
            {
                //code voor notification
                Notification.Builder builder = new Notification.Builder(this)
                .SetContentTitle("Nieuwe klanten")
                .SetContentText("Er zijn " + coutn+" nieuwe klanten")
                .SetSmallIcon(Android.Resource.Drawable.IcDialogAlert);

                // Build the notification:
                Notification notification = builder.Build();

                // Get the notification manager:
                NotificationManager notificationManager =
                    GetSystemService(Context.NotificationService) as NotificationManager;

                // Publish the notification:
                const int notificationId = 0;
                notificationManager.Notify(notificationId, notification);

                coutn++;
                Thread.Sleep(7000);
            }
        }
        private void FindViews()
        {
            mapButton = FindViewById<Button>(Resource.Id.mapButton);
            aanvraagButton = FindViewById<Button>(Resource.Id.aanvragenButton);
        }

        private void HandleEvents()
        {
            mapButton.Click += MapButton_Click;
            aanvraagButton.Click += AanvraagButton_Click;
        }

        private void AanvraagButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(AccepteerActivity));
            StartActivity(intent);
        }

        private void MapButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MapActivity));
            StartActivity(intent);
        }
    }
}