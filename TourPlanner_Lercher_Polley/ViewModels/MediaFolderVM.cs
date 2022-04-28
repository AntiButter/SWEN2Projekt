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
using TourPlanner_Lercher_Polley.ViewModels;

namespace TourPlanner_Lercher_Polley.ViewModels
{
    public class MediaFolderVM : ViewModelBase
    {
        private IMediaItemFactory mediaItemFactory;
        private MediaItem currentItem;
        private RelayCommand searchCommand;
        private RelayCommand clearCommand;
        private string searchName;
        public ICommand SearchCommand => searchCommand ??= new RelayCommand(Search);
        public ICommand ClearCommand => clearCommand ??= new RelayCommand(Clear);
        public ObservableCollection<MediaItem> Items { get; set; }
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
        public MediaItem CurrentItem
        { 
            get 
            { 
                return currentItem; 
            }
            set
            {
                if ((currentItem != value) && (value != null))
                {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                }
            }
        }
        public MediaFolderVM()
        {
            this.mediaItemFactory = MediaItemFactory.GetInstance();
            InitListBox();
        }

        private void InitListBox()
        {
            Items = new ObservableCollection<MediaItem>();
            FillListBox();
        }

        private void FillListBox()
        {
            foreach (MediaItem item in this.mediaItemFactory.GetItems())
            {
                Items.Add(item);
            }
        }

        private void Search(object commandParameter)
        {
            IEnumerable foundItems = this.mediaItemFactory.Search(SearchName);
            Items.Clear();  
            foreach (MediaItem item in foundItems)
            {
                Items.Add(item);
            }
        }

        private void Clear(object commandParameter)
        {
            Items.Clear();
            SearchName = "";
            FillListBox();
        }

    }
}
