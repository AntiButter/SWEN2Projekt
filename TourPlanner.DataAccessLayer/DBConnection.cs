using System;
using System.Collections.Generic;
using TourPlanner.Models;
using TourPlanner.Models.Enum;

namespace TourPlanner.DataAccessLayer
{
     class DBConnection
    {
        private string connectionString;
        public DBConnection()
        {
            //get shit from config file
            connectionString = "...";
            //establish db connection, mitm beidl
        }

        //maybe singleton ?
        /*private static IMediaItemFactory instance; 

        public static IMediaItemFactory GetInstance()
        {
            if (instance == null)
            {
                instance = new MediaItemFactoryImpl();
            }
            return instance;
        }
        */

        //delete
        /*
        public List<MediaItem> GetItems()
        {
            //SQL shit 
            return new List<MediaItem>()
            {
                new MediaItem() { Name = "Item1" },
                new MediaItem() { Name = "Item2" },
                new MediaItem() { Name = "FHTW" },
                new MediaItem() { Name = "SWE" },
                new MediaItem() { Name = "Another" }

            };
        }
        */
        
        
        public List<Tour> getTours()
        {
            return new List<Tour>()
            {
                new Tour("TestTour1", "Beschreibung", "Wien", "Salzburg", TransportTypeEnum.running),
                new Tour("TestTour2", "Beschreibung", "Wien", "Salzburg", TransportTypeEnum.running),
                new Tour("TestTour3", "Beschreibung", "Wien", "Salzburg", TransportTypeEnum.running)
            };
        }
        
    }
}
