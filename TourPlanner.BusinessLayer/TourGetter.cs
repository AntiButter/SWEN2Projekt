using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public class TourGetter
    {
        private TourDataAccess tourDataAccess = new TourDataAccess();

        public IEnumerable<Tour> Search(string itemName)
        {
            IEnumerable<Tour> items = GetItems();

          
            if(items == null)
                return items;
            
            //call getAllLogs and compare it too 

            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }

        public IEnumerable<Tour> GetItems()
        {
            return tourDataAccess.GetItems();
        }

        public string getPicture(int ID)
        {
            string pictureString = Path.GetFullPath("../../../../Pictures/TourID" + ID + ".png");

            return pictureString;
        }
    }
}
