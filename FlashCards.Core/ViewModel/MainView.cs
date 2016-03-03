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
        public MainView()
        {
            Collections = new ObservableCollection<CollectionView>();
        }

        public ObservableCollection<CollectionView> Collections { get; private set; }

        public void Load(IDataStore dataStore)
        {
            if (dataStore == null)
            {
                throw new ArgumentNullException(nameof(dataStore));
            }

            foreach (var name in dataStore.GetCollections())
            {
                Collections.Add(new CollectionView
                {
                    Name = name
                });
            }
        }
    }
}
