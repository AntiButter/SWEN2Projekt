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
        private ICommand tourChangerCommand;

        public ICommand TourChangerCommand => tourChangerCommand ??= new RelayCommand(EditTour);


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


        private void EditTour(object commandParameter)
        {
            tourManager = new TourManager();
            if (String.IsNullOrWhiteSpace(TourName) || String.IsNullOrWhiteSpace(TourTo) || String.IsNullOrWhiteSpace(TourFrom))
            {
                MessageBox.Show("Fehler: " + "Bitte alle Felder außer Description ausfüllen.");
                return;
            }
            tourManager.changeTour(oldID, TourName, TourDescription, TourFrom, TourTo, TransportType);




                foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this) item.Close();
            }
        }


    }
}
