using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class TourLogs
    {
        public string LogTime { get; set; }  
        public string? Comment { get; set; }  
        public int Difficulty { get; set; }  // 1 to 5
        public int TotalTime { get; set; } //saved as minute, later potentially converted to hours ?
        public int Rating { get; set; } // 1 to 5
        public int? LogID { get; set; } //from Database
        public int TourID { get; set; } 

        public TourLogs(string logTime, string? comment, int difficulty, int totalTime, int rating, int? id, int TourID)
        {
            LogTime = logTime;
            Comment = comment;
            Difficulty = difficulty;
            TotalTime = totalTime;
            Rating = rating;
            LogID = id;
            this.TourID = TourID;
        }
    }
}
