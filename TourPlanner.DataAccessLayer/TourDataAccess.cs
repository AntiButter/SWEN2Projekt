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
        private DBConnection dataAccess;
        public TourDataAccess()
        {
            dataAccess = new DBConnection();
        }
        
        public List<Tour> GetItems()
        {
            return dataAccess.getTours();
        }
        
    }
}
