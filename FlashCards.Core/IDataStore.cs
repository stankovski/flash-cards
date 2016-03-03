using System.Collections.Generic;
using FlashCards.Core.Model;

namespace FlashCards.Core
{
    public interface IDataStore
    {
        CardCollection GetCollection(string name);
        IEnumerable<string> GetCollections();
        void SaveCollection(string name, CardCollection collection);
    }
}