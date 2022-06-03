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
using TourPlanner.Logging;

namespace TourPlanner_Lercher_Polley.ViewModels
{
    public class TourPlannerViewModel : ViewModelBase
    {
        private IEnumerable<Tour> allTours;
        private TourGetter tourGetter;
        private TourManager tourManager;
        private LogManager logManager;
        private ImportExport importExport;
        public  TourPlanner.Models.TourLogs CurrentLog { get; set; }

        private Tour? currentItem;
        private ICommand searchCommand;
        private ICommand clearListCommand;
        private ICommand addTourCommand;
        private ICommand createTourReportCommand;
        private ICommand createSummarizeReportCommand;
        private ICommand addTourLogCommand;
        private ICommand deleteTourLogCommand;
        private ICommand deleteTourCommand;
        private ICommand editTourLogCommand;
        private ICommand editTourCommand;
        private ICommand exportCommand;
        private ICommand uniqueCommand;
        private string searchName = "";
        private BitmapImage tourPicture;

        public ICommand SearchCommand => searchCommand ??= new RelayCommand(Search);
        public ICommand ClearListCommand => clearListCommand ??= new RelayCommand(ClearList);
        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour); 
        public ICommand CreateTourReportCommand => createTourReportCommand ??= new RelayCommand(CreateTourReport); 
        public ICommand CreateSummarizeReportCommand => createSummarizeReportCommand ??= new RelayCommand(CreateSummarizeReport);
        public ICommand AddTourLogCommand => addTourLogCommand ??= new RelayCommand(addTourLog);
        public ICommand DeleteTourLogCommand => deleteTourLogCommand ??= new RelayCommand(deleteTourLog);
        public ICommand EditTourLogCommand => editTourLogCommand ??= new RelayCommand(editTourLog);
        public ICommand DeleteTourCommand => deleteTourCommand ??= new RelayCommand(deleteTour);
        public ICommand EditTourCommand => editTourCommand ??= new RelayCommand(editTour);
        public ICommand ExportCommand => exportCommand ??= new RelayCommand(Export);
        public ICommand UniqueCommand => uniqueCommand ??= new RelayCommand(Unique);



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

        public BitmapImage TourPicture
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
            logManager = new LogManager();
            tourGetter = new TourGetter();
            tourManager = new TourManager();
            Items = new ObservableCollection<Tour>();

            LoadList();

            Logger.Info("Programm started successfully");
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

        private void Unique(object commandParameter)
        {
            UniqueFeature uniqueFeature = new UniqueFeature();
            uniqueFeature.ShowDialog();

            Items.Clear();
            LoadList();
        }
        private void deleteTour(object commandParameter)
        {
            if (currentItem == null)
            {
                //log
                //make it more MVVM friendly
                MessageBox.Show("FEHLER: Bitte wählen Sie zuerst eine Tour aus!");
                return;
            }
            tourManager.deleteTour(currentItem);
            clearAll();
        }

        private void editTour(object commandParameter)
        {
            if (currentItem == null)
            {
                //log
                //make it more MVVM friendly
                MessageBox.Show("FEHLER: Bitte wählen Sie zuerst eine Tour aus!");
                return;
            }
            AddTourWindow editTourWindow = new AddTourWindow((int)currentItem.ID,currentItem.Name, currentItem.To, currentItem.From,
                currentItem.Description, currentItem.TransportType);            
            editTourWindow.ShowDialog();

            int oldID = (int)CurrentItem.ID;

            clearAll();

            getOldSelection(oldID);
        }
        public void addTourLog(object commandParameter)
        {
            if (currentItem == null)
            {
                MessageBox.Show("FEHLER: Bitte wählen Sie zuerst eine Tour aus!");

                Logger.Error("Es wurde keine Tour ausgewählt, welcher ein Log hinzugefügt werden könnte");

                return;
            }

            TourLogs tourLogs = new TourLogs((int)CurrentItem.ID);
            tourLogs.ShowDialog();

            Items.Clear();
            LoadList();
        }

        public void deleteTourLog(object commandParameter)
        {
            if (currentItem == null)
            {
                //log
                //make it more MVVM friendly
                MessageBox.Show("FEHLER: Bitte wählen Sie zuerst eine Tour aus!");
                return;
            }
            logManager.deleteLog(CurrentLog);

            int oldID = (int)CurrentItem.ID;

            clearAll();

            getOldSelection(oldID);
        }

        public void editTourLog(object commandParameter)
        {

            if (currentItem == null)
            {
                //log
                //make it more MVVM friendly
                MessageBox.Show("FEHLER: Bitte wählen Sie zuerst eine Tour aus!");
                return;
            }

            TourLogs tourLogs = new TourLogs((int)CurrentLog.LogID,CurrentLog.Comment,CurrentLog.Difficulty,CurrentLog.TotalTime,CurrentLog.Rating,(int)CurrentItem.ID);
            tourLogs.ShowDialog();

            int oldID = (int)CurrentItem.ID;

            clearAll();

            getOldSelection(oldID);
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
            clearAll();
        }        

        private void clearAll()
        {
            Items.Clear();

            SearchName = "";
            LoadList();

            currentItem = null;
            TourPicture = null;
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

        private void Export(object commandParameter)
        {
            importExport.save(importExport.export());

        }

        private void getOldSelection(int id)
        {
            foreach(var item in Items)
            {
                if(item.ID == id)
                {
                    CurrentItem = item;
                    return;
                }
            }
        }
    }
}
