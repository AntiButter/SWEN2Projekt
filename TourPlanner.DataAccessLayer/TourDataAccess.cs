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
            return dataAccess.getTours();
        }

        public int getNextValTour()
        { 
            return dataAccess.getNextValTour();
        } 

        public void addTourToDB(Tour tour)
        {
            dataAccess.addTourToDB(tour);
        }

        public void deleteTour(int ID)
        {
            dataAccess.deleteTour(ID);
        }

        public void changeTour(Tour changedTour)
        {
            dataAccess.changeTourDB(changedTour);
        }
    }
}
