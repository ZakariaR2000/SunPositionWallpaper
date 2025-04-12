using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SunPositionWallpaper
{
    class SunPositionWallpaper
    {
        

        static async Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: dotnet run <latitude> <longitude>");
                return;
            }

            if (!double.TryParse(args[0], out double latitude) || !double.TryParse(args[1], out double longitude))
            {
                Console.WriteLine("Invalid latitude or longitude format.");
                return;
            }

            string result = await clsWallpaperBasedOnSunPosition.GetWallpaperBasedOnSunPosition(latitude, longitude);
            Console.WriteLine(result);
        }

        
    }
}
