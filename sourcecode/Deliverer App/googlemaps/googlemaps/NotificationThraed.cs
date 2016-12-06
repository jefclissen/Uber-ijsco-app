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
    class NotificationThraed
    {
        public static void getNotification()
        {
            //Toast.MakeText(, "klanten zijn toegevoegd", ToastLength.Short).Show();
            while (true)
            {
                //code voor notification
                /*Notification.Builder builder = new Notification.Builder(this)
                .SetContentTitle("Sample Notification")
                .SetContentText("Hello World! This is my first notification!");*/
    //.SetSmallIcon(Resource.Drawable.ic_notification);
                Thread.Sleep(5000);
            }
        }
    }
}