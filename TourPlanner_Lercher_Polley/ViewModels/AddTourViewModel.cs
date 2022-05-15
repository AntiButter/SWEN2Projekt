using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models.Enum;

namespace TourPlanner_Lercher_Polley.ViewModels
{
    internal class AddTourViewModel : ViewModelBase
    {
        public string TourName { get; set; }
        public string TourTo { get; set; }
        public string TourFrom { get; set; }
        public string TourDescription { get; set; }
        public TransportType TransportType { get; set; }

        public void selectOption_changed()
        {

        }
        

    }
}
