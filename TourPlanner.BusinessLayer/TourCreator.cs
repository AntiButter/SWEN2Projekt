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
    public class TourCreator
    {
        private TourDataAccess tourDataAccess = new TourDataAccess();

        public void addNewTour (string name, string? description, string from, string to, TransportType transportType)
        {
            Tour newTour = new Tour(name, description, from, to, transportType);

            newTour.ID = tourDataAccess.getNextValTour();

            //geht noch nicht, aus irgendeinem Grund gibts nen endlos loop im API call
            //mapQuestAPIRequest(newTour);
            
        }

        private void mapQuestAPIRequest (Tour tour)
        {
            _ = new MapQuestAPICall(tour);  

            //throw new NotImplementedException("ende");
        }

        private void completeTourInDatabase()
        {
            throw new NotImplementedException ();   
        }
    }
}
