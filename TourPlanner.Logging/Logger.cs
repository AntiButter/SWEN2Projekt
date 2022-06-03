
//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "../../../../Tourplanner.Logging/log4net.config")]

namespace TourPlanner.Logging
{

    public class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Log(string String)
        {
            log.Info(String);
            log.Error("Error");

            Console.WriteLine("cock");
        }
    }
}