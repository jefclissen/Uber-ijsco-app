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
using Deliverer.Core.Modle;
using Deliverer.Core.Service;

namespace googlemaps
{
    public class NotificationRunning
    {
        private bool threadRunning = false;

        public void setRunning()
        {
            threadRunning = true;
        }
        public void setStop()
        {
            threadRunning = false;
        }
        public bool getStatus()
        {
            return threadRunning;
        }
    }
}