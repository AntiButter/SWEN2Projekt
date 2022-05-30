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
        private TourDataAccess tourDataAccess = new TourDataAccess();

        public void export()
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

            /*
            JObject exportJSON =
                new JObject(
                    new JProperty("channel",
                        new JObject(
                            new JProperty("title", "James Newton-King"),
                            new JProperty("link", "http://james.newtonking.com"),
                            new JProperty("description", "James Newton-King's blog."),
                            new JProperty("item", "test"))));
                            
                                new JArray(
                                    from p in posts
                                    orderby p.Title
                                    select new JObject(
                                        new JProperty("title", p.Title),
                                        new JProperty("description", p.Description),
                                        new JProperty("link", p.Link),
                                        new JProperty("category",
                                            new JArray(
                                                from c in p.Categories
                                                select new JValue(c)))))))));
                            */
            File.WriteAllText("../../../../ExportFiles/Export"+ DateTime.Now.ToString("_yyyyMMdd_HHmmss") + ".txt", exportJSON.ToString());

            //add current timestamp to export

        }        
        public void import()
        {
            throw new NotImplementedException();    
        }
    }
}
