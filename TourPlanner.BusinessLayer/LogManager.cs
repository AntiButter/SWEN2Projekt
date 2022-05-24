using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public class LogManager
    {
        private TourLogDataAccess tourLogDataAccess = new TourLogDataAccess();

        public void addNewLog(string? comment, int difficulty, int totalTime, int rating, int TourID)
        {
            TourLogs newLog = new(comment, difficulty, totalTime, rating, TourID);

            tourLogDataAccess.addLogToDB(newLog);
        }
    }
}
