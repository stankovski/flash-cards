using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Core.ViewModel
{
    public class MainView : INotifyPropertyChanged
    {
        private IDataStore _dataStore;
        public MainView(IDataStore dataStore)
        {
            if (dataStore == null)
            {
                throw new ArgumentNullException(nameof(dataStore));
            }
            Collections = new ObservableCollection<CollectionView>();
            _dataStore = dataStore;
        }

        private ObservableCollection<CollectionView> collections;
        public ObservableCollection<CollectionView> Collections
        {
            get { return collections; }
            set
            {
                if (value == collections)
                    return;

                collections = value;

                OnPropertyChanged(nameof(Collections));
            }
        }

        public void Load()
        {
            Collections.Clear();
            foreach (var collection in _dataStore.GetCollections())
            {
                var collectionView = new CollectionView(_dataStore, collection);
                Collections.Add(collectionView);
                collectionView.Load();
            }
        }

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
