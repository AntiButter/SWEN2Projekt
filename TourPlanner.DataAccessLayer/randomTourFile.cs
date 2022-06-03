using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.DataAccessLayer
{
    public class randomTourFile
    {
        public static string getRandomLocation()
        {
            string filePath = "../../../../CuratedLocationsForUniqueFeature.txt";

            string[] locations = File.ReadAllLines(filePath);

            string to = locations[randomNumber(0, locations.Length)];

            return to;
        }

        public static int randomNumber(int min, int max)
        {
            Random _random = new Random();

            return _random.Next(min, max);
        }
    }
}
