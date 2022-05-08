using System;
using System.Collections.Generic;
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
            
            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }

        public IEnumerable<Tour> GetItems()
        {
            return tourDataAccess.GetItems();
        }

        //public 
    }
}
