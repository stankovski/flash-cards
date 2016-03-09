﻿using System.Collections.Generic;
using FlashCards.Core.Model;

namespace FlashCards.Core
{
    public interface IDataStore
    {
        CardCollection GetCollection(string name);
        CardCollection RenameCollection(CardCollection collection, string newName);
        IEnumerable<string> GetCollections();
        void SaveCollection(CardCollection collection);
    }
}