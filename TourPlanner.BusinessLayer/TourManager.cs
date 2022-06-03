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
        private ITourDataAccess tourDataAccess;

        public TourManager()
        {
            tourDataAccess = new TourDataAccess();
        }        
        public TourManager(ITourDataAccess dataAccessMockable)
        {
            tourDataAccess = dataAccessMockable;
        }

        public bool addNewTour (string name, string? description, string from, string to, TransportType transportType)
        {
            Tour newTour = new Tour(name, description, from, to, transportType);

            //get (and reserve) the next value from the DB and set it to the object (for picture file naming)
            newTour.setID(tourDataAccess.getNextValTour());

            if(mapQuestAPIRequest(newTour))
            {
                addTourToDB(newTour);
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool mapQuestAPIRequest (Tour tour)
        {
            MapQuestAPICall mapQuestAPICall = new MapQuestAPICall();
            

            return mapQuestAPICall.callAPI(tour); ;
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
