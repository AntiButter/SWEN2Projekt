
//log4net.config with the help of the Moodle course
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "../../../../Tourplanner.Logging/log4net.config")]

namespace TourPlanner.Logging
{

    public class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Info(string String)
        {
            log.Info(String);
        }        
        public static void Error(string String)
        {
            log.Error(String);
        }        
        public static void Warn(string String)
        {
            log.Warn(String);
        }        
        public static void Fatal(string String)
        {
            log.Fatal(String);
        }
    }
}