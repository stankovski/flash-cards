using System.Collections.Generic;
using FlashCards.Core.Model;

namespace FlashCards.Core
{
    public interface IDataStore
    {
        CardCollection GetCollection(string name);
        IEnumerable<CardCollection> GetCollections();
        void SaveCollection(CardCollection collection);
        void RemoveCollection(CardCollection collection);
    }
}