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
using System.Collections.Specialized;
using System.Net;

namespace Uber_Client
{
    public class RequestEventArgs : EventArgs
    {
        private string mResponse;

        public string Response
        {
            get { return mResponse; }
            set { mResponse = value; }
        }

        public RequestEventArgs(string _response) : base()
        {
            Response = _response;
        }
    }
    class HttpRequest
    {
        string mHostname;
        string mUri;
        string mResponse;
        NameValueCollection mParameters;
        public event EventHandler<RequestEventArgs> mRequestCompleted; //public so others can subscribe to this event

        ThreadStart TS;
        Thread mThread;
        public HttpRequest(string _hostname)
        {
            mHostname = _hostname;
            TS = new ThreadStart(PostRequest);
            mThread = new Thread(TS);
        }
        
        public void PostRequestAsync(string _uri,NameValueCollection _parameters)
        {
            mUri = _uri;
            mParameters = _parameters;

            mThread.Start();
            
            
        }
        private void PostRequest()
        {
            using (WebClient client = new WebClient())
            {
                Uri completeUri = new Uri(mHostname + mUri);               
                byte[] response = client.UploadValues(completeUri, mParameters);
                mResponse = System.Text.Encoding.UTF8.GetString(response);
            }
            mRequestCompleted.Invoke(this, new RequestEventArgs(mResponse));
            mThread.Abort();
        }
    }
}