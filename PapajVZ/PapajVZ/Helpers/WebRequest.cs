using System;
using System.IO;
using System.Net;
using System.Net.Http;
using ModernHttpClient;
using Newtonsoft.Json;

namespace PapajVZ.Helpers
{
    public static class WebApi
    {
        public static TObject GetRequest<TObject>(string uri)
        {
            var client = new HttpClient(new NativeMessageHandler());


            using (var response = client.GetAsync(new Uri(uri)).Result)
            using (var content = response.Content)
            {
                
                return JsonConvert.DeserializeObject<TObject>(content.ReadAsStringAsync().Result);
            }
        }

        //todo USE HTTPCLIENT
        public static object PostRequest<T>(this T obj, string url)
        {
            string result;
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(obj);
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return result;
        }
    }
}