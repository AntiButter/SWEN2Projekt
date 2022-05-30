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
using System.Windows;

namespace TourPlanner_Lercher_Polley.ViewModels
{
    public class TourPlannerViewModel : ViewModelBase
    {
        private IEnumerable<Tour> allTours;
        private TourGetter tourGetter;
        private TourManager tourManager;
        private LogManager logManager;
        private ImportExport importExport;

        private Tour? currentItem;
        private ICommand searchCommand;
        private ICommand clearListCommand;
        private ICommand addTourCommand;
        private ICommand createTourReportCommand;
        private ICommand createSummarizeReportCommand;
        private ICommand addTourLogCommand;
        private ICommand deleteTourLogCommand;
        private ICommand editTourLogCommand;
        private string searchName;
        private string tourPicture;

        public ICommand SearchCommand => searchCommand ??= new RelayCommand(Search);
        public ICommand ClearListCommand => clearListCommand ??= new RelayCommand(ClearList);
        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour); 
        public ICommand CreateTourReportCommand => createTourReportCommand ??= new RelayCommand(CreateTourReport); 
        public ICommand CreateSummarizeReportCommand => createSummarizeReportCommand ??= new RelayCommand(CreateSummarizeReport);
        public ICommand AddTourLogCommand => addTourLogCommand ??= new RelayCommand(addTourLog);
        public ICommand DeleteTourLogCommand => deleteTourLogCommand ??= new RelayCommand(deleteTourLog);
        public ICommand EditTourLogCommand => editTourLogCommand ??= new RelayCommand(editTourLog);



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

                    setPicture();
                }
            }
        }

        public string TourPicture
        {
            get { return tourPicture; }
            set 
            { 
                tourPicture = value;
                RaisePropertyChangedEvent(nameof(TourPicture));
            }
        }
        public TourPlannerViewModel()
        {
            tourGetter = new TourGetter();
            tourManager = new TourManager();
            Items = new ObservableCollection<Tour>();

            //test
            //tourCreator.addNewTour("Test0805", "Wiener Spaziergang", "Eisenstadt", "Gramadneusiedl", TransportType.running);

            //Tour testTour = new Tour("aaa", "Wiener asd", "Eisenstadt", "Gramadneusiedl", TransportType.running);
            //testTour.setID(99);
            //tourManager.changeTour(testTour, "Test0805", "Wiener Spaziergang", "Eisenstadt", "Gramadneusiedl", TransportType.running);

            //logManager = new LogManager();
            //logManager.addNewLog("", 5, 100, 5, 28);
            //TourPlanner.Models.TourLogs testLog = new TourPlanner.Models.TourLogs("", 5, 100, 5, 28);
            //testLog.setID(8);
            //logManager.changeLog(testLog, "change test", 3, 100, 5, 28);


            //importExport = new ImportExport();
            //importExport.export();

            LoadList();
        }

        private void setPicture()
        {
            TourPicture = tourGetter.getPicture((int)currentItem.ID);
        }        

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

        private void AddTour(object commandParameter)
        {
            AddTourWindow addTourWindow = new AddTourWindow();
            addTourWindow.ShowDialog();

            Items.Clear();
            LoadList();
        }
        public void addTourLog(object commandParameter)
        {
            if (currentItem == null)
            {
                //log
                //make it more MVVM friendly
                MessageBox.Show("FEHLER: Bitte wählen Sie zuerst eine Tour aus!");
                return;
            }

            TourLogs tourLogs = new TourLogs(true,(int)CurrentItem.ID);
            tourLogs.ShowDialog();

            Items.Clear();
            LoadList();
        }

        public void deleteTourLog(object commandParameter)
        {

        }

        public void editTourLog(object commandParameter)
        {

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

            currentItem = null;
            RaisePropertyChangedEvent(nameof(CurrentItem));
        }        
        private void CreateTourReport(object commandParameter)
        {
            if(currentItem == null)
            {
                //log
                //make it more MVVM friendly
                MessageBox.Show("FEHLER: Bitte wählen Sie zuerst eine Tour aus!");
                return;
            }

            PDFGenerator.tourReport(currentItem);

            //give feedback that the report was created
        }        
        private void CreateSummarizeReport(object commandParameter)
        {

            PDFGenerator.summarizeReport();

            //give feedback that the report was created
        }
    }
}
