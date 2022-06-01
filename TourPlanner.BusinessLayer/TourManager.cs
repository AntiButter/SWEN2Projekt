using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;
using TourPlanner.Models.Enum;

namespace TourPlanner.BusinessLayer
{
    public class TourManager
    {
        private TourDataAccess tourDataAccess = new TourDataAccess();

        public void addNewTour (string name, string? description, string from, string to, TransportType transportType)
        {
            Tour newTour = new Tour(name, description, from, to, transportType);

            //get (and reserve) the next value from the DB and set it to the object (for picture file naming)
            newTour.setID(tourDataAccess.getNextValTour());

            mapQuestAPIRequest(newTour);

            addTourToDB(newTour);
        }

        private void mapQuestAPIRequest (Tour tour)
        {
            _ = new MapQuestAPICall(tour);  

            //throw new NotImplementedException("ende");
        }

        private void addTourToDB(Tour tour)
        {
            tourDataAccess.addTourToDB(tour);   
        }

        public void deleteTour(Tour tour)
        {
            tourDataAccess.deleteTour((int)tour.ID);
        }        
     
        public void changeTour(int oldID, string name, string? description, string from, string to, TransportType transportType)
        {
            Tour changedTour = new Tour(name, description, from, to, transportType);

            changedTour.setID(oldID); 

            mapQuestAPIRequest(changedTour);

            tourDataAccess.changeTour(changedTour);
        }
    }
}
