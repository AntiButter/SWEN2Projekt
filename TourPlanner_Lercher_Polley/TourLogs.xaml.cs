using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TourPlanner_Lercher_Polley.ViewModels;

namespace TourPlanner_Lercher_Polley
{
    /// <summary>
    /// Interaction logic for TourLogs.xaml
    /// </summary>
    public partial class TourLogs : Window
    {
        public TourLogs(int IDTour)
        {
            InitializeComponent();
            this.DataContext = new TourLogsViewModel(IDTour);
        }

        public TourLogs(int id, string? comment, int difficulty, int totalTime, int rating, int TourID)
        {
            InitializeComponent();
            this.DataContext = new EditTourLogsViewModel(id,comment,difficulty,totalTime,rating,TourID);
        }
    }
}
