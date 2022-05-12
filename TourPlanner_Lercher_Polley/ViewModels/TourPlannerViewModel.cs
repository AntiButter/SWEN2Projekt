using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.Models.Enum;
using TourPlanner_Lercher_Polley.ViewModels;
using System.Windows.Controls;
using System.Threading;

namespace TourPlanner_Lercher_Polley.ViewModels
{
    public class TourPlannerViewModel : ViewModelBase
    {
        private IEnumerable<Tour> allTours;
        private TourGetter tourGetter;
        private TourCreator tourCreator;

        private Tour? currentItem;
        private ICommand searchCommand;
        private ICommand clearListCommand;
        private string searchName;
        private string tourName;
        private string tourFrom;
        private string tourTo;
        private string tourTime;
        //private string tourFriend;
        //private string tourPop;
        private string tourType;
        private string tourDescription;
        private string tourPicture;

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

        public string TourFrom
        {
            get { return tourFrom; }
        }

        public string TourTo
        {
            get { return tourTo; }
        }

        public string TourTime
        {
            get { return tourTime; }
        }
        //private string tourFriend;
        //private string tourPop;
        
        public string TourType
        {
            get { return tourType; }
        }
        public string TourName
        {
            get { return tourName; }
        }
        public string TourDescription
        {
            get { return tourDescription; }
        }
        public string TourPicture
        {
            get { return tourPicture; }
        }
        public TourPlannerViewModel()
        {
            tourGetter = new TourGetter();
            tourCreator = new TourCreator();
            Items = new ObservableCollection<Tour>();

            //test
            //tourCreator.addNewTour("Test0805", "Wiener Spaziergang", "Wien", "Gramadneusiedl", TransportType.running);

            LoadList();

            //Select first Tour as base
            //CurrentItem = Items.First();
        }

        private void UpdateTourDetails()
        {
            
            tourName = currentItem.Name;
            tourDescription = currentItem.Description;
            tourType = currentItem.TransportType.ToString();
            tourTo = currentItem.To;
            tourFrom = currentItem.From;
            tourTime = currentItem.EstimatedTime;
            //tourPicture = "../../../../Pictures/TourID" + currentItem.ID + ".png";
            //tourPicture.UriSource = resourceUri;
            //tourPicture = "..\\..\\..\\..\\Pictures\\TourID38.png";
            //tourPicture = "pack://TourPlanner_Lercher_Polley:,,,,/Pictures/TourID38.png";
            
            //tourPicture = new Bitmap("../../../../Pictures/TourID" + currentItem.ID + ".png");   

            //var tourPicture = new Bitmap(Image.FromFile("../../../../Pictures/TourID" + currentItem.ID + ".png"));
            //var tourPicture = new Bitmap(Image.FromFile("Pictures/TourID44.png"));
            //Uri resourceUri = new Uri("../../../../Pictures/TourID" + currentItem.ID + ".png", UriKind.Relative);
            //Uri resourceUri = new Uri("../../../../Pictures/TourID" + currentItem.ID + ".png", UriKind.Relative);

            tourPicture = Path.GetFullPath("../../../../Pictures/TourID" + currentItem.ID + ".png");

            
            RaisePropertyChangedEvent(nameof(TourName));
            RaisePropertyChangedEvent(nameof(TourTime));
            RaisePropertyChangedEvent(nameof(TourTo));
            RaisePropertyChangedEvent(nameof(TourType));
            RaisePropertyChangedEvent(nameof(TourFrom));
            RaisePropertyChangedEvent(nameof(TourDescription));
            RaisePropertyChangedEvent(nameof(TourPicture));
            
            //tourPicture = null;
            //RaisePropertyChangedEvent(nameof(TourPicture));
        }        
        /*
        private void ClearTourDetails()
        {
            tourName = "";
            tourDescription ="" ;
            tourType = "";
            tourTo = "";
            tourFrom = "";
            tourTime = "";
            tourPicture = "";
            RaisePropertyChangedEvent(nameof(TourName));
            RaisePropertyChangedEvent(nameof(TourTime));
            RaisePropertyChangedEvent(nameof(TourTo));
            RaisePropertyChangedEvent(nameof(TourType));
            RaisePropertyChangedEvent(nameof(TourFrom));
            RaisePropertyChangedEvent(nameof(TourDescription));
            RaisePropertyChangedEvent(nameof(TourPicture));
        }
        */
        private void LoadList()
        {
            allTours = tourGetter.GetItems();
            if(allTours != null)
            {
                foreach (Tour item in allTours)
                {
                    Items.Add(item);
                }
            }
        }

        private void Search(object commandParameter)
        {
            IEnumerable foundItems = tourGetter.Search(SearchName);
            Items.Clear();
            if (allTours != null)
            {
                foreach (Tour item in foundItems)
                {
                    Items.Add(item);
                }
            }
        }

        private void ClearList(object commandParameter)
        {
            Items.Clear();

            SearchName = "";
            LoadList();

            //ClearTourDetails();

            currentItem = null;
            RaisePropertyChangedEvent(nameof(CurrentItem));
        }

    }
}
