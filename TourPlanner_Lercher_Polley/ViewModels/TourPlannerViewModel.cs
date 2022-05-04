using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.Models.Enum;
using TourPlanner_Lercher_Polley.ViewModels;

namespace TourPlanner_Lercher_Polley.ViewModels
{
    public class TourPlannerViewModel : ViewModelBase
    {
        private IEnumerable<Tour> allTours;
        private TourGetter tourGetter;
        private TourCreator tourCreator;

        private Tour currentItem;
        private ICommand searchCommand;
        private ICommand clearListCommand;
        private string searchName;
        private string tourName;
        public ICommand SearchCommand => searchCommand ??= new RelayCommand(Search);
        public ICommand ClearListCommand => clearListCommand ??= new RelayCommand(ClearList);

        public ObservableCollection<Tour> Items { get; set; }
        public string SearchName
        {
            get { return searchName; }
            set
            {
                if(searchName != value)
                {
                    searchName = value;
                    RaisePropertyChangedEvent(nameof(SearchName));
                }
            }
        }

        public Tour CurrentItem
        {
            get { return currentItem; }
            set
            {
                if ((currentItem != value) && (value != null))
                {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));

                    UpdateTourDetails();
                }
            }
        }

        public string TourName
        {
            get { return tourName; }
        }

        public TourPlannerViewModel()
        {
            tourGetter = new TourGetter();
            tourCreator = new TourCreator();
            Items = new ObservableCollection<Tour>();
            LoadList();

            //test
            tourCreator.addNewTour("testProgramm", "yo hallo", "Wien", "Leobersdorf", TransportType.running);
        }

        private void UpdateTourDetails()
        {
            tourName = currentItem.Name;
            RaisePropertyChangedEvent(nameof(TourName));
        }        
        private void ClearTourDetails()
        {
            tourName = "";
            RaisePropertyChangedEvent(nameof(TourName));
        }
        private void LoadList()
        {
            allTours = tourGetter.GetItems();
            foreach (Tour item in allTours)
            {
                Items.Add(item);
            }
        }

        private void Search(object commandParameter)
        {
            IEnumerable foundItems = tourGetter.Search(SearchName);
            Items.Clear();
            foreach (Tour item in foundItems)
            {
                Items.Add(item);
            }
        }

        private void ClearList(object commandParameter)
        {
            Items.Clear();
            SearchName = "";
            LoadList();

            ClearTourDetails();
        }

    }
}
