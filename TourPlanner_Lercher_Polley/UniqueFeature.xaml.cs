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
    /// Interaction logic for UniqueFeature.xaml
    /// </summary>
    public partial class UniqueFeature : Window
    {
        public UniqueFeature()
        {
            InitializeComponent();
            this.DataContext = new UniqueFeatureViewModel();
        }
    }
}
