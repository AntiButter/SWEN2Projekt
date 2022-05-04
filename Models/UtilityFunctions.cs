using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class UtilityFunctions
    {
        //check if certain data is null
        public static string? checkNull(string isNull)
        {
            if (isNull == "")
                return null;

            return isNull;
        }

        public static int getTimeFromString(string time)
        {
            int newTime = 0;

            string[] subStrings = time.Split(':');

            newTime += Int32.Parse(subStrings[0]) * 60 * 60;
            newTime += Int32.Parse(subStrings[1]) * 60;
            newTime += Int32.Parse(subStrings[2]);

            return newTime;
        }
    }
}
