using System;
using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
     class DBConnection : IDataAccess
    {
        private string connectionString;
        public DBConnection()
        {
            //get shit from config file
            connectionString = "...";
            //establish db connection, mitm beidl
        }
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
    }
}
