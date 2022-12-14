using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BuildHappiness.Core.Helpers
{
    public static class LocationHelper
    {
        private static string BaseURL = "https://app.zipcodebase.com/api/v1/";
        private static string APIKey = "00b131e0-0d29-11eb-8219-39db93e62462";

        private static HttpClient client;

        public static async Task<string> GetNearbyLocation(string postCode, string radius, string country)
        {
            try
            {
                if (client == null)
                {
                    client = new HttpClient();
                }
                var response = await client.GetAsync($"{BaseURL}radius?code={postCode}&radius={radius}&country={country}&unit=miles&apikey={APIKey}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {

            }
            return "";
        }

        public static async Task<List<Location>> GetLocationByPostAsync(string code, string country)
        {
            try
            {
                if (client == null)
                {
                    client = new HttpClient();
                }
                var response = await client.GetAsync($"{BaseURL}search?codes={code}&apikey={APIKey}&country={country}");

                var resObject = await ParseResponseFromJSONAsync<dynamic>(response, null);

                //var results = resObject["results"];

                //var items = results["642206"];

                List<Location> locations = new List<Location>();
                //foreach (var item in items)
                //{
                //    Location location = new Location();

                //    location.PostalCode = item["postal_code"];
                //    location.CountryCode = item["country_code"];
                //    location.Latitude = item["latitude"];
                //    location.Longitude = item["longitude"];
                //    location.City = item["city"];
                //    location.State = item["state"];
                //    location.StateCode = item["state_code"];
                //    location.Province = item["province"];
                //    location.ProvinceCode = item["province_code"];

                //    locations.Add(location);
                //}

                return locations;

            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public static async Task<T> ParseResponseFromJSONAsync<T>(HttpResponseMessage msg, JsonSerializerSettings jsonSettings)
            where T : class
        {
            if (msg == null)
                throw new Exception("Failed to retrieve any data");
            if (msg.IsSuccessStatusCode)
            {
                var textContent = await msg.Content.ReadAsStringAsync();
                if (jsonSettings == null)
                    return JsonConvert.DeserializeObject<T>(textContent);
                else
                    return JsonConvert.DeserializeObject<T>(textContent, jsonSettings);
            }
            else
            {
                throw new Exception($"Failed, {msg.ReasonPhrase} ({(int)msg.StatusCode})");
            }
        }
    }

    public class Location
    {
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string Province { get; set; }
        public string ProvinceCode { get; set; }
    }
}