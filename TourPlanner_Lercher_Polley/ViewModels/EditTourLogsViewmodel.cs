
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
    public class EditTourLogsViewModel : ViewModelBase
    {
        public int LogDifficulty { get; set; }
        public int LogTime { get; set; }
        public int LogRating { get; set; }
        public string LogComment { get; set; }
        public int TourID { get; set; }

        public int ID { get; set; }
        public string ButtonContent { get; set; }

        public LogManager logManager { get; set; }
        private ICommand logCommand;

        public ICommand LogCommand => logCommand ??= new RelayCommand(EditLog);

        public EditTourLogsViewModel(int id, string? comment, int difficulty, int totalTime, int rating, int IDTour)
        {
            logManager = new LogManager();
            ID = id;
            LogTime = totalTime;
            LogDifficulty = difficulty;
            LogRating = rating;
            LogComment = comment;
            TourID = IDTour;
            ButtonContent = "Edit";

        }

        public void EditLog(object commandParameter)
        {
            logManager.changeLog(ID,LogComment, LogDifficulty, LogTime, LogRating, TourID);

            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this) item.Close();
            }
        }

    }
}
