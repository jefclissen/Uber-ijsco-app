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
using System.Collections.Specialized;
using System.Net;
using Plugin.Geolocator;
using System.Threading;

namespace Uber_Client
{
    [Activity(Label = "MainAppActivity")]
    public class MainAppActivity : Activity
    {
        private Button mBtnIceCream;
        private Button mBtnAccount;
        NameValueCollection mCredentials;
        private TextView txtInfo;
        public event EventHandler<EventArgs> Poll;

        public int ETA = 0;
        public int currentETA = 0;
        Thread mThread;
        public bool threadRunning = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainApp);
            mCredentials = new NameValueCollection();

            mCredentials.Add("username", Intent.GetStringExtra("username"));//Get the data passed by previous Activity
            mCredentials.Add("email", Intent.GetStringExtra("email"));
            mCredentials.Add("password", Intent.GetStringExtra("password"));

            mBtnIceCream = FindViewById<Button>(Resource.Id.IceCreamBtn);
            mBtnAccount = FindViewById<Button>(Resource.Id.AccountBtn);
            mBtnIceCream.Click += MBtnIceCream_Click;
            mBtnAccount.Click += MBtnAccount_Click;
            txtInfo = FindViewById<TextView>(Resource.Id.txtInfo);
            Poll += MainAppActivity_Poll;

            ThreadStart TS = new ThreadStart(Polling);
            mThread = new Thread(TS);
        }



        private void MBtnIceCream_Click(object sender, EventArgs e)
        {
            if (!threadRunning) { 
                
                mThread.Start();
            }
            #region
            /*
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("email", credentials.Get("email"));
            parameters.Add("username", credentials.Get("username"));
            parameters.Add("userLong", position.Longitude.ToString().Replace(',', '.'));
            parameters.Add("userLat", position.Latitude.ToString().Replace(',', '.'));

            HttpRequest httpReq = new HttpRequest("http://35.165.103.236");
            httpReq.PostRequestAsync("/icecreamrequest", parameters);
            httpReq.mRequestCompleted += HttpReq_mRequestCompleted;
            */
            ///*
            #endregion

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            //var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            //string Long = position.Longitude.ToString().Replace(',', '.');

            string mResult;
            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri("http://35.165.103.236:80/icecreamrequest");
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("email", mCredentials.Get("email"));
                parameters.Add("username", mCredentials.Get("username"));
                //parameters.Add("userLong", position.Longitude.ToString().Replace(',', '.'));
                //parameters.Add("userLat", position.Latitude.ToString().Replace(',', '.'));
                parameters.Add("userLong", "4.4164027");
                parameters.Add("userLat", "51.2298936");
                byte[] response = client.UploadValues(uri, parameters);
                mResult = System.Text.Encoding.UTF8.GetString(response);
            }
             
            Toast.MakeText(this, mResult, ToastLength.Long).Show();
            //*/
        }

        private void Polling()
        {
            
            //Toast.MakeText(this, "Thread running...", ToastLength.Long).Show();
            while (true) {
                Poll.Invoke(this, new EventArgs());      
                Thread.Sleep(1000);
            }
        }
        /*
private void HttpReq_mRequestCompleted(object sender, RequestEventArgs e)
{
   Toast.MakeText(this, e.Response, ToastLength.Long).Show();
}
*/
        private void MainAppActivity_Poll(object sender, EventArgs e)
        {  
            string result;
            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri("http://35.165.103.236:80/geteta");
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("useremail", mCredentials.Get("email"));

                byte[] response = client.UploadValues(uri, parameters);
                result = System.Text.Encoding.UTF8.GetString(response);
                if(result.Substring(0,1) == "0")
                {
                    RunOnUiThread(() => {
                        txtInfo.Text = result.Substring(1);
                    });
                }else
                {
                    if(Convert.ToInt16(result) != ETA)
                    {
                        ETA = Convert.ToInt16(result); //set ETA
                        RunOnUiThread(() => {
                            txtInfo.Text = result.ToString();
                        });
                        currentETA = ETA;
                    }else if(currentETA > 0)
                    {
                        currentETA--;
                        
                        RunOnUiThread(() => {
                            txtInfo.Text = (currentETA/60).ToString() + "min";
                        });
                    }else
                    {
                        RunOnUiThread(() => {
                            txtInfo.Text = "The deliverer will arrive soon";
                        });
                    }

                }
            }
            
        }

        private void MBtnAccount_Click(object sender, EventArgs e)
        {
           // mThread.Abort();
            
            var intent = new Intent(this, typeof(OptionsActivity));
            intent.PutExtra("username", mCredentials.Get("username"));
            intent.PutExtra("email", mCredentials.Get("email"));
            intent.PutExtra("password", mCredentials.Get("password"));
            StartActivity(intent);

        }
    } 
}