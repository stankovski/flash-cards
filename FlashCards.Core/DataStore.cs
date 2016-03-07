using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Core.Model;
using Newtonsoft.Json;

namespace FlashCards.Core
{
    public class DataStore : IDataStore
    {
        Dictionary<string, string> _dataStore = new Dictionary<string, string>();

        public DataStore()
        {
            var collection = new CardCollection
            {
                Name = "foo",
                Description = "my description",
                Format = CardFormat.Mixed
            };
            collection.Cards.Add(new Card
            {
                Id = Guid.NewGuid(),
                SideA = new CardSide { Text = "What is A?" },
                SideB = new CardSide { Text = "A is a letter" }
            });
            collection.Cards.Add(new Card
            {
                Id = Guid.NewGuid(),
                SideA = new CardSide { Text = "What is B?" },
                SideB = new CardSide { Text = "B is a letter" }
            });
            collection.Cards.Add(new Card
            {
                Id = Guid.NewGuid(),
                SideA = new CardSide { Text = "What is *?" },
                SideB = new CardSide { Text = "* is a symbol" }
            });
            _dataStore["foo.memcards"] = JsonConvert.SerializeObject(collection);

            collection = new CardCollection
            {
                Name = "bar",
                Description = "my description",
                Format = CardFormat.Text
            };
            collection.Cards.Add(new Card
            {
                Id = Guid.NewGuid(),
                SideA = new CardSide { Text = "What is x?" },
                SideB = new CardSide { Text = "x is a letter" }
            });
            collection.Cards.Add(new Card
            {
                Id = Guid.NewGuid(),
                SideA = new CardSide { Text = "What is *?" },
                SideB = new CardSide { Text = "* is a symbol" }
            });
            _dataStore["bar.memcards"] = JsonConvert.SerializeObject(collection);
        }

        public IEnumerable<string> GetCollections()
        {
            return _dataStore.Keys.Where(k => k.EndsWith(".memcards")).Select(k => Path.GetFileNameWithoutExtension(k));
        }

        public CardCollection GetCollection(string name)
        {
            if (_dataStore.ContainsKey(name + ".memcards"))
            {
                return JsonConvert.DeserializeObject<CardCollection>(_dataStore[name + ".memcards"]);
            }
            return null;
        }

        public void SaveCollection(CardCollection collection)
        {
            _dataStore[collection.Name + ".memcards"] = JsonConvert.SerializeObject(collection);
        }
    }
}
