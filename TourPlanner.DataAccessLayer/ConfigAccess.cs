using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.DataAccessLayer
{
    public static class ConfigAccess
    {
        public static string getDatabaseString ()
        {

            string configPath = "../../../../config.txt";

            string[] config = File.ReadAllLines (configPath);

            //following this format => Host=localhost;Username=postgres;Password=tour;Database=postgres
            string databaseConnection = config[0]+";"+config[1]+";"+config[2]+";"+config[3];

            return databaseConnection;
        }
    }
}
