using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunPositionWallpaper
{
    public  class clsImagePath
    {
        public static string GetImagePath(string fileName)
        {
            string path = Path.Combine("Images", fileName);
            if (File.Exists(path))
            {
                return fileName;
            }
            else
            {
                return "File Not Found!";
            }
        }
    }
}
