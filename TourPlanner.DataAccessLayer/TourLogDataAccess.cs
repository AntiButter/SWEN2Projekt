using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class TourLogDataAccess : ITourLogDataAccess
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

        public void addLogToDB(TourLogs newTour)
        {
            dataAccess.addLogToDB(newTour);
        }

        public void deleteLog(int logID)
        {
            dataAccess.deleteTourLog(logID);
        }

        public void changeLog(int logID, TourLogs newLog)
        {
            dataAccess.changeLogDB(logID, newLog);
        }
    }
}
