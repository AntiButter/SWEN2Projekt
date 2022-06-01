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
using System.Windows;
using System.Windows.Controls;

namespace TourPlanner_Lercher_Polley.ViewModels
{
    public class EditTourViewModel : ViewModelBase
    {
        public string TourName { get; set; }
        public string TourTo { get; set; }
        public string TourFrom { get; set; }
        public string TourDescription { get; set; }
        public string ButtonType { get; set; }
        public int oldID { get; set; }
        public TransportType TransportType { get; set; }
        private TourManager tourManager;
        private ICommand createTourCommand;

        public ICommand CreateTourCommand => createTourCommand ??= new RelayCommand(CreateTour);


        public EditTourViewModel(int ID, string name, string to, string from, string description, TransportType type)
        {
            oldID = ID;
            TourName = name;
            TourTo = to;
            TourFrom = from;
            TourDescription = description;
            TransportType = type;
            ButtonType = "Edit Tour";
        }


        private void CreateTour(object commandParameter)
        {
            tourManager = new TourManager();

            tourManager.changeTour(oldID, TourName, TourDescription, TourFrom, TourTo, TransportType);




                foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this) item.Close();
            }
        }


    }
}
