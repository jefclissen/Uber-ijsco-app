using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Net;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using Deliverer.Core.Modle;
using Deliverer.Core.Service;
using Android.Media;

namespace googlemaps
{
    [Activity(Label = "MainMenuActivity", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainMenuActivity : Activity
    {
        private Button mapButton;
        private Button aanvraagButton;
        private Thread myThread ;
        private bool threadRunning = false;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainMenuLayout);

            
            ThreadStart myThreadDelegate = new ThreadStart(notificationThread);
            myThread = new Thread(myThreadDelegate);
            threadRunning = true;
          //  myThread.Start();
            
            FindViews();
            HandleEvents();
        }
        
        // * Beetje rommerl om thread te stoppen
        protected override void OnResume()
        {
            base.OnResume();
            threadRunning = true;
        }
        protected override void OnPause()
        {
            base.OnPause();
            threadRunning = false;
        }
        
    
        public void notificationThread()
        {
            int aantalNieuweKlanten = 0;
            int vorigAantalNieuweKlanten = 0;

            List<Klant> serverKlanten = new List<Klant>();
            List<Klant> geweigerdeKlanten = new List<Klant>();
            KlantDataService dataService = new KlantDataService();

            //while (threadRunning == true) //zorg voor een eeuwige lus
            while (true) //zorg voor een eeuwige lus
                {
                if (threadRunning == true)
                {
                    serverKlanten = dataService.GeefAlleKlantenFromServer();
                    geweigerdeKlanten = dataService.getGewijgerdeKlanten();

                    aantalNieuweKlanten = serverKlanten.Count; //maximum aantal nieuwe klanten (gelijk aan server klanten)
                    if (geweigerdeKlanten[0].Username != "XXXXGEENKLANTENXXXX")//wanneer er nog geen gewijgerde klanten zijn
                    {
                        for (int i = 0; i < serverKlanten.Count; i++)
                        {
                            for (int j = 0; j < geweigerdeKlanten.Count; j++)
                            {
                                if (serverKlanten == geweigerdeKlanten)//als bepaalde klant op de server == aan de reeds geweigerde klant
                                    aantalNieuweKlanten--;  //wordt maximum aantal klanten verminderd met 1
                            }
                        }
                    }

                    if (aantalNieuweKlanten != 0 && aantalNieuweKlanten != vorigAantalNieuweKlanten)
                    {
                        vorigAantalNieuweKlanten++;
                        //voor het maken van de klik event
                        // When the user clicks the notification, SecondActivity will start up.
                        Intent resultIntent = new Intent(this, typeof(AccepteerActivity));
                        // Construct a back stack for cross-task navigation:
                        TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
                        stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(AccepteerActivity)));
                        stackBuilder.AddNextIntent(resultIntent);
                        // Create the PendingIntent with the back stack:            
                        PendingIntent resultPendingIntent =
                            stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

                        Android.Net.Uri alarmSound = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

                        Notification.Builder builder = new Notification.Builder(this)
                            .SetContentTitle("Nieuwe klanten")
                            .SetContentText("Er zijn " + aantalNieuweKlanten + " nieuwe klanten")
                            .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                            .SetContentIntent(resultPendingIntent)  // Start 2nd activity when the intent is clicked.
                            .SetSmallIcon(Android.Resource.Drawable.IcDialogAlert)
                            .SetSound(alarmSound)
                        .SetVibrate(new long[] { 500, 500, 500, 500, 500 })
                        .SetPriority(10);

                        // Build the notification:
                        Notification nieuweKlantNotification = builder.Build();



                        // Get the notification manager:
                        NotificationManager notificationManager =
                            GetSystemService(Context.NotificationService) as NotificationManager;

                        // Publish the notification:
                        const int notificationId = 0;
                        notificationManager.Notify(notificationId, nieuweKlantNotification);
                    }
                    Thread.Sleep(7000);
                } 
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