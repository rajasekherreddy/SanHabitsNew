using BuildHappiness.Core.Common;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HappinessIndex.Common
{
    public class HttpCall
    {
        public static async Task<HttpResponseMessage> Get(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalClass.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    response = await client.GetAsync(client.BaseAddress + url);
                }
                catch
                {
                    response.StatusCode = HttpStatusCode.ExpectationFailed;
                }
                return response;
            }
        }
        public static async Task<HttpResponseMessage> Post<T>(T obj, string url)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(GlobalClass.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    if (obj != null)
                    {
                        var json = JsonConvert.SerializeObject(obj);
                        var sendContent = new StringContent(json, Encoding.UTF8, "application/json");
                        response = await client.PostAsync(client.BaseAddress + url, sendContent);
                    }
                    else
                    {
                        response = await client.PostAsync(client.BaseAddress + url, null);
                    }
                }
                catch
                {
                    response.StatusCode = HttpStatusCode.ExpectationFailed;
                }

                return response;
            }
        }
    }
}
