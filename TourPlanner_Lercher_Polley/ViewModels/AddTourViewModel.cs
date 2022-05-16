using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Models.Enum;
using TourPlanner.Models;
using TourPlanner.BusinessLayer;

namespace TourPlanner_Lercher_Polley.ViewModels
{
    public class AddTourViewModel : ViewModelBase
    {
        public string TourName { get; set; }
        public string TourTo { get; set; }
        public string TourFrom { get; set; }
        public string TourDescription { get; set; }
        public TransportType TransportType { get; set; }
        private TourCreator tourCreator;
        private ICommand createTourCommand;

        public ICommand CreateTourCommand => createTourCommand ??= new RelayCommand(CreateTour); 


        public void selectOption_changed()
        {

        }

        private void CreateTour(object commandParameter)
        {
            throw new Exception();
            tourCreator = new TourCreator();
            tourCreator.addNewTour(TourName, TourDescription, TourFrom, TourTo, TransportType.bike);


        }


    }
}
