using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Core.ViewModel
{
    public class MainView
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

        public ObservableCollection<CollectionView> Collections { get; private set; }

        public void Load()
        {
            foreach (var name in _dataStore.GetCollections())
            {
                Collections.Add(new CollectionView(_dataStore)
                {
                    Name = name
                });
            }
        }
    }
}
