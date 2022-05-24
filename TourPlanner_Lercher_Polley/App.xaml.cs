using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Logging;

namespace TourPlanner_Lercher_Polley
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            /*
           // var loggerFactory = new LoggerFactory();

            //ConsoleLoggerExtensions.AddConsole();

            var loggerFactory = LoggerFactory.Create(builder =>{
                builder.AddConsole();
                builder.AddFilter(c => c == LogLevel.Debug);
            });

            //im programm dann
            var logger = loggerFactory.CreateLogger("test");
            //logger.Log();

            //Einen Logger erstellen, und den dann immer im ViewModel weitergeben
            //Oder einfach on Startup einen Logger machen, und dann static methoden)

            MainViewModel mainviewmodel = new MainViewModel(logger)

            base.OnStartup(e);
            */
        }
    }
}
