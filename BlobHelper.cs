using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AttachmentsManagement
{
    public class BlobHelper : RestHelper
    {
        public CloudBlobContainer BlobContainer
        {
            get;
            set;
        }
        public BlobHelper(string storageAccount, string sasKey) : base("https://" + storageAccount + ".blob.core.windows.net/", storageAccount, sasKey)
        {

        }
        public byte[] GetBlob(string container, string blob)
        {
            return Retry(delegate
            {
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    HttpWebResponse httpWebResponse = CreateRestRequest("GET", container + "/" + blob).GetResponse() as HttpWebResponse;
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    byte[] array = new byte[16384];
                    using (memoryStream)
                    {
                        int count;
                        while ((count = responseStream.Read(array, 0, array.Length)) > 0)
                        {
                            memoryStream.Write(array, 0, count);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (WebResponse webResponse = ex.Response)
                    {
                        HttpWebResponse httpWebResponse2 = (HttpWebResponse)webResponse;
                        using (Stream stream = webResponse.GetResponseStream())
                        {
                            string text = new StreamReader(stream).ReadToEnd();
                        }
                    }
                }
                return memoryStream.ToArray();
            });
        }
        public bool PutBlob(string container, string blob, string content, SortedList<string, string> metadataList = null)
        {
            return Retry(delegate
            {
                try
                {
                    SortedList<string, string> sortedList = new SortedList<string, string>
                    {
                        {
                            "x-ms-blob-type",
                            "BlockBlob"
                        }
                    };
                    if (metadataList != null)
                    {
                        foreach (KeyValuePair<string, string> metadata in metadataList)
                        {
                            sortedList.Add("x-ms-meta-" + metadata.Key, metadata.Value);
                        }
                    }
                    (CreateRestRequest("PUT", container + "/" + blob, content, sortedList).GetResponse() as HttpWebResponse)?.Close();
                    return true;
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            });
        }
        public bool DeleteBlob(string container, string blob)
        {
            return RestHelper.Retry(delegate
            {
                try
                {
                    (CreateRestRequest("DELETE", container + "/" + blob).GetResponse() as HttpWebResponse)?.Close();
                    return true;
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            });
        }
        public SortedList<string, string> GetBlobProperties(string container, string blob)
        {
            return Retry(delegate
            {
                SortedList<string, string> sortedList = new SortedList<string, string>();
                try
                {
                    HttpWebResponse httpWebResponse = CreateRestRequest("HEAD", container + "/" + blob).GetResponse() as HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        httpWebResponse.Close();
                        if (httpWebResponse.StatusCode == HttpStatusCode.OK && httpWebResponse.Headers != null)
                        {
                            for (int i = 0; i < httpWebResponse.Headers.Count; i++)
                            {
                                sortedList.Add(httpWebResponse.Headers.Keys[i], httpWebResponse.Headers[i]);
                            }
                        }
                    }
                    return sortedList;
                }
                catch (WebException)
                {
                    throw;
                }
            });
        }
    }
}
