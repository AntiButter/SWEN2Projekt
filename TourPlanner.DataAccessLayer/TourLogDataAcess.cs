using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class TourLogDataAcess
    {
        private DB dataAccess;
        public TourLogDataAcess()
        {
            dataAccess = DB.getInstance();
        }

        
        public List<TourLogs> GetAllTourLogs()
        {
            return dataAccess.getAllTourLogs();
        }
        
    }
}
