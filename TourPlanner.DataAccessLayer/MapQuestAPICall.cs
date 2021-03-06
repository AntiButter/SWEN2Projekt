using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class MapQuestAPICall
    {
        Tour currentTour;

        public bool callAPI(Tour tour)
        {

            currentTour = tour;

            try
            {
                var test = GetContent();
                Task.WaitAll(test);

                if(test.Result == null)
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("FEHLER: Unbekannter Fehler \n\n" +
                    "Bitte überprüfen Sie ihre Eingabedaten und versuchen Sie es erneut!");

                Logger.Error("Unbekannter Fehler beim Aufruf der MapQuestAPI");

                return false;
            }

            return true;
        }

        public async Task<JObject> GetContent()
        {
            //DirectionsAPI
            using var client = new HttpClient();

            string key = "ZYfMAsTV3NSge6ewOfWQZDpzqYXYK3W9";

            //will be decided with the switch
            string routeType;

            if(currentTour.TransportType == Models.Enum.TransportType.bike)
            {
                routeType = "bicycle";
            }
            else
            {
                routeType = "pedestrian";
            }


            string directionURL = "http://www.mapquestapi.com/directions/v2/route?key=" + key + "&from=" + currentTour.From + "&to=" + currentTour.To + "&routeType=" + routeType;
;
            //throw new Exception(directionURL);

            var response = await client.GetAsync(directionURL).ConfigureAwait(false);

            JObject pageContent = JObject.Parse(await response.Content.ReadAsStringAsync());


            try
            {
                //results
                double distanceMiles = (int)pageContent.SelectToken("route.distance");
                //convert miles to km
                double distance = distanceMiles * 1.60934;
                distance = Math.Round(distance,2); 

                //time
                string time = (string)pageContent.SelectToken("route.formattedTime");

                //save in Tour object
                currentTour.addMapQuestData(distance, time);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FEHLER: Die Tour ist zu lang, oder einer der Orte konnte nicht gefunden werden!\n\n" +
                    "Bitte überprüfen Sie ihre Eingabedaten und versuchen Sie es erneut!");

                Logger.Error("FEHLER: Die Tour ist zu lang, oder einer der Orte konnte nicht gefunden werden!\n\n" +
                    "Bitte überprüfen Sie ihre Eingabedaten und versuchen Sie es erneut!");

                return null;
            }


            //Static API
            string session = (string)pageContent.SelectToken("route.sessionId");
            string lr_lng = (string)pageContent.SelectToken("route.boundingBox.lr.lng"); ;
            string lr_lat = (string)pageContent.SelectToken("route.boundingBox.lr.lat"); ;
            string ul_lng = (string)pageContent.SelectToken("route.boundingBox.ul.lng"); ;
            string ul_lat = (string)pageContent.SelectToken("route.boundingBox.ul.lat"); ;

            string boundingBox = ul_lat + "," + ul_lng + "," + lr_lat + "," + lr_lng;

            string staticURL = "https://www.mapquestapi.com/staticmap/v5/map?key=ZYfMAsTV3NSge6ewOfWQZDpzqYXYK3W9&size=640,480&defaultMarker=none&session=" + session + "&boundingBox=" + boundingBox;

            //return pageContent;

            Image tourImage = (Image)new ImageConverter().ConvertFrom(client.GetByteArrayAsync(staticURL).Result);

            PictureManager.savePicture(tourImage, (int)currentTour.ID);

            return pageContent;
        }
    }
}

