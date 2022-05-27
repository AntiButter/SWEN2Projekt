 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public bool EditOrAdd { get; set; }

        public LogManager logManager { get; set; }
        private ICommand logCommand;

        public ICommand LogCommand => logCommand ??= new RelayCommand(Log);
        public TourLogsViewModel(bool chooserEditOrAdd, int ID)
        {
            logManager = new LogManager();
            EditOrAdd = chooserEditOrAdd;
            TourID = ID;
        }

        
        public void Log(object commandParameter)
        {
            if (EditOrAdd)
            {
                AddLog();
            }

            else
            {
                EditLog();
            }

        }
        public void AddLog()
        {
            logManager.addNewLog(LogComment, LogDifficulty, LogTime, LogRating, TourID);
        }


        public void EditLog()
        {
            //logManager.changeLog(,LogComment, LogDifficulty, LogTime, LogRating, TourID);
        }

    }
}
