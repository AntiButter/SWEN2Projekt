using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public class ImportExport
    {
        private ITourDataAccess tourDataAccess;

        public ImportExport()
        {
            tourDataAccess = new TourDataAccess();
        }        
        public ImportExport(ITourDataAccess dataAccessMockable)
        {
            tourDataAccess = dataAccessMockable;
        }

        public string export()
        {
            IEnumerable<Tour> allTours = tourDataAccess.GetItems();

            JObject exportJSON =
                new JObject(
                    new JProperty("tour",
                        new JArray(
                            from t in allTours
                            select new JObject(
                                new JProperty("tourname", t.Name),
                                new JProperty("description", t.Description),
                                new JProperty("from", t.From),
                                new JProperty("to", t.To),
                                new JProperty("transporttype", t.TransportType.ToString()),
                                new JProperty("logs",
                                    new JArray(
                                        from l in t.Logs
                                        select new JObject(
                                            new JProperty("timestamp", l.LogTime),
                                            new JProperty("comment", l.Comment),
                                            new JProperty("difficulty", l.Difficulty),
                                            new JProperty("rating", l.Rating),
                                            new JProperty("totaltime", l.TotalTime))))))));

            return exportJSON.ToString();
        }  
        
        public void save(string exportJSON)
        {
            //add current timestamp to export
            File.WriteAllText("../../../../ExportFiles/Export" + DateTime.Now.ToString("_yyyyMMdd_HHmmss") + ".txt", exportJSON.ToString());
        }
        public void import()
        {
            throw new NotImplementedException();    
        }


    }
}
