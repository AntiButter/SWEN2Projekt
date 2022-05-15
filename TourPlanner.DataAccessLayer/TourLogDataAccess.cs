using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class TourLogDataAccess
    {
        private DB dataAccess;
        public TourLogDataAccess()
        {
            dataAccess = DB.getInstance();
        }

        
        public int getTourLogAmountTotal()
        {
            return dataAccess.getTourLogAmountTotal();
        }
        
    }
}
