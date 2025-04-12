using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunPositionWallpaper
{
    class clsUtcOffsetByCoordinates
    {
        public static async Task<double> GetUtcOffsetByCoordinates(double latitude, double longitude)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiKey = "E84DKSI63S8T";
                string url = $"http://api.timezonedb.com/v2.1/get-time-zone?key={apiKey}&format=json&by=position&lat={latitude}&lng={longitude}";

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(json);
                    double? offset = data["gmtOffset"]?.ToObject<double>() / 3600;

                    if (offset != null)
                    {
                        return offset.Value;
                    }
                }

                throw new Exception("Failed to retrieve timezone data.");
            }
        }

    }
}
