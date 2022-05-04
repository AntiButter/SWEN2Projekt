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
    }
}
