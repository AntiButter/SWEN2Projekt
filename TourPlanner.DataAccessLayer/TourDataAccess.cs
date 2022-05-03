using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class TourDataAccess
    {
        private DB dataAccess;
        public TourDataAccess()
        {
            dataAccess = DB.getInstance();
        }
        
        public List<Tour> GetItems()
        {
            //return dataAccess.getToursStatic();
            return dataAccess.getTours();
        }
        
    }
}
