 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
 using TourPlanner.BusinessLayer;
 using TourPlanner.DataAccessLayer;
 using TourPlanner.Models;
namespace TourPlanner_Lercher_Polley.ViewModels
{
    public class TourLogsViewModel : ViewModelBase
    {
        public int LogDifficulty { get; set; }
        public int LogTime { get; set; }
        public int LogRating { get; set; }
        public string LogComment { get; set; }
        public int TourID { get; set; }
        public string ButtonContent { get; set; }

        public LogManager logManager { get; set; }
        private ICommand logCommand;
        private ICommand cancelCommand;

        public ICommand LogCommand => logCommand ??= new RelayCommand(AddLog);
        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(Cancel);

        public TourLogsViewModel(int IDTour)
        {
            TourID = IDTour;
            ButtonContent = "Add Log";
            logManager = new LogManager();
        }
        
        public void AddLog(object commandParameter)
        {
            logManager.addNewLog(LogComment, LogDifficulty, LogTime, LogRating, TourID);

            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this) item.Close();
            }
        }
        private void Cancel(object commandParameter)
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this) item.Close();
            }
        }
    }
}
