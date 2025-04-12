using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunPositionWallpaper
{
    public class clsWallpaperBasedOnSunPosition
    {
        public static async Task<string> GetWallpaperBasedOnSunPosition(double latitude, double longitude)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.sunrise-sunset.org/json?lat={latitude}&lng={longitude}&formatted=0";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        return clsImagePath.GetImagePath("night.png");
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(json);

                    string? sunriseValue = data["results"]?["sunrise"]?.ToString();
                    string? sunsetValue = data["results"]?["sunset"]?.ToString();

                    if (string.IsNullOrEmpty(sunriseValue) || string.IsNullOrEmpty(sunsetValue))
                    {
                        return clsImagePath.GetImagePath("night.png");
                    }

                    double offset = await clsUtcOffsetByCoordinates.GetUtcOffsetByCoordinates(latitude, longitude);

                    DateTimeOffset sunrise = DateTimeOffset.Parse(sunriseValue).ToUniversalTime().ToOffset(TimeSpan.FromHours(offset));
                    DateTimeOffset sunset = DateTimeOffset.Parse(sunsetValue).ToUniversalTime().ToOffset(TimeSpan.FromHours(offset));
                    DateTimeOffset now = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(offset));

                    TimeSpan nowTime = now.TimeOfDay;
                    TimeSpan sunriseTime = sunrise.TimeOfDay;
                    TimeSpan sunsetTime = sunset.TimeOfDay;

                    if (nowTime < sunriseTime) return clsImagePath.GetImagePath("night.png");
                    if (nowTime >= sunriseTime && nowTime <= sunriseTime.Add(TimeSpan.FromMinutes(30))) return clsImagePath.GetImagePath("sunrise.png");
                    if (nowTime > sunriseTime.Add(TimeSpan.FromMinutes(30)) && nowTime < sunsetTime.Subtract(TimeSpan.FromHours(4))) return clsImagePath.GetImagePath("morning.png");
                    if (nowTime >= sunsetTime.Subtract(TimeSpan.FromHours(4)) && nowTime < sunsetTime.Subtract(TimeSpan.FromHours(2))) return clsImagePath.GetImagePath("noon.png");
                    if (nowTime >= sunsetTime.Subtract(TimeSpan.FromHours(2)) && nowTime < sunsetTime) return clsImagePath.GetImagePath("evening.png");
                    if (nowTime >= sunsetTime) return clsImagePath.GetImagePath("night.png");

                    return clsImagePath.GetImagePath("night.png");
                }
                catch (Exception)
                {
                    return clsImagePath.GetImagePath("night.png");
                }
            }
        }

    }
}
