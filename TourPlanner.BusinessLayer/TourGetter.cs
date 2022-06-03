using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TourPlanner.DataAccessLayer;
using TourPlanner.DataAccessLayer.Interfaces;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public class TourGetter
    {
        private ITourDataAccess tourDataAccess;
        private ITourLogDataAccess tourLogDataAccess;

        public TourGetter()
        {
            tourDataAccess = new TourDataAccess();
            tourLogDataAccess = new TourLogDataAccess();
        }
        public TourGetter(ITourDataAccess dataAccessTourMockable, ITourLogDataAccess dataAccessLogMockable)
        {
            tourDataAccess = dataAccessTourMockable;
            tourLogDataAccess = dataAccessLogMockable;
        }

        public IEnumerable<Tour> Search(string itemName)
        {
            IEnumerable<Tour> items = GetItems();
          
            if(items == null)
                return items;
            
            var searchResult = items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));

            if (searchResult.Count() == 0)
                Logger.Warn("No search results for \"" + itemName + "\" could be found");

            return searchResult;
        }

        public IEnumerable<Tour> GetItems()
        {
            var tours = tourDataAccess.GetItems();

            calculateParameters(tours);

            return tours;
        }

        public BitmapImage getPicture(int ID)
        {
            string pictureString;
            string path = "../../../../Pictures/TourID" + ID + ".png";

            if (File.Exists(path))
            {
                pictureString = path;
            }
            else
            {
                Logger.Error("Picture for ID:" + ID + " could not be found, using filler.png as backup");
                pictureString = "../../../../Pictures/filler.png";
            }

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.UriSource = new Uri(Path.GetFullPath(pictureString));
            image.EndInit();

            return image;
        }

        public void calculateParameters(IEnumerable<Tour> tours)
        {
            foreach (Tour tour in tours)
            {
                calculatePopularity(tour);
                calculateChildFriendliness(tour);
            }
        }

        public void calculateChildFriendliness(Tour tour)
        {
            //base Friendliness is 5, any negatives will abduct from that value
            int childFriendliness = 5;

            //if no logs exists, set to 1 and exit
            if (tour.Logs.Count() == 0)
            {
                tour.setChildFriendliness(1); //manually set to 1 because unknown
                return;
            }

            //difficulty
            childFriendliness -= difficultyPenalty(tour);            
            
            //difficulty
            childFriendliness -= timePenalty(tour);

            //distance
            childFriendliness -= distancePenalty(tour);

            //set it to 1, if it is below
            if(childFriendliness < 1)
            {
                childFriendliness = 1;
            }

            tour.setChildFriendliness(childFriendliness);
        }
        public int difficultyPenalty(Tour tour)
        {

            //calculate average difficulty from all logs
            int count = tour.Logs.Count();
            int sum = 0;
            foreach (TourLogs log in tour.Logs)
            {
                sum += log.Difficulty;
            }

            //does not need to be double, because we want in trimmed
            int averageDifficulty = sum / count;

            //for every difficulty higher than 2 (so 3,4,5) we decrease the childfriendliness by 1   //if 
            int subtractorValue = (averageDifficulty - 2);

            if (subtractorValue < 0)
                subtractorValue = 0;

            return subtractorValue;
        }

        public int timePenalty(Tour tour)
        {
            int subtractor = 0;

            //calculate average duration from all logs
            int count = tour.Logs.Count();
            int sum = 0;
            foreach (TourLogs log in tour.Logs)
            {
                sum += log.TotalTime;
            }

            //does not need to be double, because we want in trimmed
            int averageTime = sum / count;

            //for every difficulty higher than 2 (so 3,4,5) we decrease the childfriendliness by 1   //if 

            if (averageTime > 180)
            {
                subtractor = 2;
            }
            else if (averageTime > 90)
            {
                subtractor = 1;
            }

            return subtractor;
        }        
        public int distancePenalty(Tour tour)
        {
            int subtractorValueDistance = 0;

            if (tour.TourDistance > 10)
            {
                subtractorValueDistance = 2;
            }
            else if (tour.TourDistance > 5)
            {
                subtractorValueDistance = 1;
            }

            return subtractorValueDistance;
        }


        public void calculatePopularity(Tour tour)
        {
            int popularity = 1;

            if(tour.Logs.Count() == 0)
            {
                tour.setPopularity(popularity);
                return;
            }

            //get the amount of all tour logs and compare it to the amount of logs in this tour
            double logAmountAll = tourLogDataAccess.getTourLogAmountTotal();
            double logAmountThisTour = tour.Logs.Count();

            double percent = (logAmountThisTour / logAmountAll) * 100;

            if (percent > 40)
            {
                popularity = 5;
            } 
            else if (percent > 30)
            {
                popularity = 4;
            } 
            else if (percent > 20)
            {
                popularity = 3;
            } 
            else if (percent > 10)
            {
                popularity = 2;
            }

            tour.setPopularity(popularity);
        }
    }
}
