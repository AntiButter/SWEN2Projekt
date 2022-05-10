using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.DataAccessLayer
{
    public class TourLogDataAcess
    {
        private DB dataAccess;
        public TourLogDataAcess()
        {
            dataAccess = DB.getInstance();
        }

        /*
        public List<Tour> GetItems()
        {
            //return dataAccess.getToursStatic();
            return dataAccess.getTours();
        }
        */
    }
}
