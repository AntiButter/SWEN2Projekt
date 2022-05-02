using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models.Enum;

namespace TourPlanner.Models
{
    public class Tour
    {

        public string Name { get; set; }
        public string? Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public TransportTypeEnum TransportType { get; set; } 
        public double? TourDistance { get; set; } //saved as kilometer //from MapQuestAPI
        public int? EstimatedTime { get; set; } //saved as minute, later potentially converted to hours ? // from MapQuestAPI
        //public xxxx? TourMap { get; set; } //the picture //where do we save it ? // from MapQuestAPI
        public int? Popularity { get; set; } // 1 - 5   //calculated afterwards
        public int? ChildFriendliness { get; set; } // 1 - 5   //calculated afterwards

        //create without data from MapQuestAPI (will be added later)
        public Tour(string name, string? description, string from, string to, TransportTypeEnum transportType)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
            TourDistance = null;
            EstimatedTime = null;
            //TourMap = null;
        }

        //adds the data from the MapQuestAPI
        public void addMapQuestData( /*mapquestdata*/)
        {
            throw new NotImplementedException();
            //TourDistance = null;
            //EstimatedTime = null;
            //TourMap = null;
        }

        public void setPopularity ()
        {
            throw new NotImplementedException();
        }        
        public void setChildFriendliness()
        {
            throw new NotImplementedException();
        }

    }
}
