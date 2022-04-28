using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
     class FileSystem : IDataAccess
    {
        private string filePath;
        public FileSystem()
        {
            //get filepath from config file maybe, or not idc
            this.filePath = "...";

        }
        public List<MediaItem> GetItems()
        {
        
            //get Items from file system
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
