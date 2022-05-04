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
        public TransportType TransportType { get; set; } 
        public double? TourDistance { get; set; } //saved as kilometer //from MapQuestAPI
        public string? EstimatedTime { get; set; } //saved as string // from MapQuestAPI
        public int? Popularity { get; set; } // 1 - 5   //calculated afterwards
        public int? ChildFriendliness { get; set; } // 1 - 5   //calculated afterwards
        public int? ID { get; set; } //from Database

        //create without data from MapQuestAPI (will be added later)
        public Tour(string name, string? description, string from, string to, TransportType transportType)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
            TourDistance = null;
            EstimatedTime = null;
            ID = null;
        }        
        //second constructor, constructs complete Tour object
        public Tour(int id, string name, string? description, string from, string to, TransportType transportType, double tourDistance, string estimatedTime)
        {
            ID = id;
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
            TourDistance = tourDistance;
            EstimatedTime = estimatedTime;
        }

        //adds the data from the MapQuestAPI
        public void addMapQuestData( double tourDistance, string estimatedTime)
        {
            TourDistance = tourDistance;
            EstimatedTime = estimatedTime;
        }

        public void setID (int id)
        {
            ID = id;
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
