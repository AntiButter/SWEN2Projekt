using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Logging;

namespace TourPlanner.DataAccessLayer
{
    internal static class PictureManager
    {
        internal static void savePicture(Image image, int ID)
        {
            string path = "../../../../Pictures/TourID"+ID+".png";

            Logger.Info("TourID" + ID + ".png was saved");

            image.Save(path);
        }
    }
}
