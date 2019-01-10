using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;

namespace AttachmentsManagement
{
    public class RestHelper
    {
        public delegate T RetryDelegate<out T>();
        public delegate void RetryDelegate();
        private const int RetryCount = 3;
        private const int RetryIntervalMs = 200;
        protected bool IsTableStorage
        {
            get;
            set;
        }
        public string EndPoint
        {
            get;
            internal set;
        }
        public string StorageAccount
        {
            get;
            internal set;
        }
        public string SasKey
        {
            get;
            internal set;
        }

        public RestHelper(string endpoint, string storageAccount, string sasKey)
        {
            EndPoint = endpoint;
            StorageAccount = storageAccount;
            SasKey = sasKey;
        }

        public HttpWebRequest CreateRestRequest(string method, string resource, string requestBody = null, SortedList<string, string> headers = null, string ifMatch = "", string md5 = "")
        {
            byte[] requestBodyArray = null;
            DateTime utcNow = DateTime.UtcNow;
            string requestUriString = EndPoint + resource + SasKey;
            HttpWebRequest httpWebRequest = WebRequest.Create(requestUriString) as HttpWebRequest;
            if (httpWebRequest == null)
            {
                return null;
            }
            httpWebRequest.Method = method;
            httpWebRequest.ContentLength = 0L;
            httpWebRequest.Headers.Add("x-ms-date", utcNow.ToString("R", CultureInfo.InvariantCulture));
            httpWebRequest.Headers.Add("x-ms-version", "2015-12-11");
            if (IsTableStorage)
            {
                httpWebRequest.ContentType = "application/atom+xml";
                httpWebRequest.Headers.Add("DataServiceVersion", "1.0;NetFx");
                httpWebRequest.Headers.Add("MaxDataServiceVersion", "1.0;NetFx");
            }
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    httpWebRequest.Headers.Add(header.Key, header.Value);
                }
            }
            if (!string.IsNullOrEmpty(requestBody))
            {
                httpWebRequest.Headers.Add("Accept-Charset", "UTF-8");
                requestBodyArray = Convert.FromBase64String(requestBody);
                httpWebRequest.ContentLength = requestBodyArray.Length;
            }
            if (!string.IsNullOrEmpty(requestBody) && requestBodyArray != null)
            {
                httpWebRequest.GetRequestStream().Write(requestBodyArray, 0, requestBodyArray.Length);
            }
            return httpWebRequest;
        }
        public static T Retry<T>(RetryDelegate<T> del)
        {
            return Retry(del, 3, 200);
        }
        public static T Retry<T>(RetryDelegate<T> del, int numberOfRetries, int msPause)
        {
            int num = 0;
            while (true)
            {
                try
                {
                    num++;
                    return del();
                }
                catch (Exception)
                {
                    if (num > numberOfRetries)
                    {
                        throw;
                    }
                    if (msPause > 0)
                    {
                        Thread.Sleep(msPause);
                    }
                }
            }
        }
        public static bool Retry(RetryDelegate del)
        {
            return Retry(del, 3, 200);
        }
        public static bool Retry(RetryDelegate del, int numberOfRetries, int msPause)
        {
            int num = 0;
            while (true)
            {
                try
                {
                    num++;
                    del();
                    return true;
                }
                catch (Exception)
                {
                    if (num > numberOfRetries)
                    {
                        throw;
                    }
                    if (msPause > 0)
                    {
                        Thread.Sleep(msPause);
                    }
                }
            }
        }
    }
}
