using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Core.Model;

namespace FlashCards.Core
{
    public class DataStore : IDataStore
    {
        public IEnumerable<string> GetCollections()
        {
            return new[] { "foo", "bar" };
        }

        public CardCollection GetCollection(string name)
        {
            var collection = new CardCollection
            {
                Name = "foo",
                Description = "my description",
                Format = CardFormat.Mixed
            };
            var card = new Card
            {
                Id = Guid.NewGuid()
            };
            card.SideA = new CardSide { Text = "What is A?" };
            card.SideB = new CardSide { Text = "A is a letter" };
            collection.Cards.Add(card);
            return collection;
        }

        public void SaveCollection(string name, CardCollection collection)
        {
            // Save
        }
    }
}
