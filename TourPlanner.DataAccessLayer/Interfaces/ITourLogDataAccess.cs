using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.Interfaces
{
    public interface ITourLogDataAccess
    {
        public int getTourLogAmountTotal();

        public void addLogToDB(TourLogs newTour);

        public void deleteLog(int logID);

        public void changeLog(int logID, TourLogs newLog);
    }
}
