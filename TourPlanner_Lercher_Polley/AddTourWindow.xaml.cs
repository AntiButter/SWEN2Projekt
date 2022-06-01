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
using TourPlanner.Models.Enum;
using TourPlanner_Lercher_Polley.ViewModels;

namespace TourPlanner_Lercher_Polley
{
    /// <summary>
    /// Interaction logic for AddTourWindow.xaml
    /// </summary>
    public partial class AddTourWindow : Window
    {
        public AddTourWindow()
        {
            InitializeComponent();
            this.DataContext = new AddTourViewModel();
        }

        public AddTourWindow(int ID,string name, string to, string from, string description, TransportType type)
        {
            InitializeComponent();
            this.DataContext = new EditTourViewModel(ID,name, to, from, description, type);
        }
    }
}
