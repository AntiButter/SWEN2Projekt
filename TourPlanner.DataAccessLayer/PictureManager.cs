using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.DataAccessLayer
{
    internal static class PictureManager
    {
        internal static void savePicture(Image image, int ID)
        {
            string path = "../../../../Pictures/TourID"+ID+".png";

            image.Save(path);
        }
    }
}
