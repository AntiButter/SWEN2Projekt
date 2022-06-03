using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Logging;

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

            Logger.Info("Config file was accessed - Database string returned");

            return databaseConnection;
        }
    }
}
