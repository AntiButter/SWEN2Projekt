using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
      public class MediaItemDAO
    {
        private IDataAccess dataAccess;
        public MediaItemDAO()
        {
            //check which data source for example (potential if/switch)
            dataAccess = new DBConnection();
        }
        public List<MediaItem> GetItems()
        {
            return dataAccess.GetItems();
        }
            
    }
}
