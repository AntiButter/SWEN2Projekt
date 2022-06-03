using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models.Enum;

namespace TourPlanner_Lercher_Polley.ViewModels
{
    internal class UniqueFeatureViewModel : ViewModelBase
    {
        public string TourName { get; set; }
        public string TourTo { get; set; }
        public string TourFrom { get; set; }
        public string ButtonType { get; set; }
        public bool Edit { get; set; }
        public int oldID { get; set; }
        public TransportType TransportType { get; set; }
        private TourManager tourManager;
        private ICommand tourChangerCommand;

        public ICommand TourChangerCommand => tourChangerCommand ??= new RelayCommand(CreateTour);



        public UniqueFeatureViewModel()
        {
            ButtonType = "RANDOMIZE";
        }

        private void CreateTour(object commandParameter)
        {
            if (String.IsNullOrWhiteSpace(TourName) || String.IsNullOrWhiteSpace(TourTo) || String.IsNullOrWhiteSpace(TourFrom))
            {
                MessageBox.Show("Fehler: " + "Bitte alle Felder außer Description ausfüllen.");
                return;
            }
            tourManager = new TourManager();


            if (tourManager.randomTour(TourName, TourFrom,TransportType))
            {
                //success, close the window
                foreach (Window item in Application.Current.Windows)
                {
                    if (item.DataContext == this) item.Close();
                }
            }
            else
            {
                //failure, dont close the window, user should be able to change the Input and try again
                return;
            }

        }

    }
}
