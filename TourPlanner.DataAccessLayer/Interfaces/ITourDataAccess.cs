using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface ITourDataAccess
    {
        public List<Tour> GetItems();
        public int getNextValTour();

        public void addTourToDB(Tour tour);
        public void deleteTour(int ID);
        public void changeTour(Tour changedTour);
    }
}
