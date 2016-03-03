using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Core.Model
{
    public class Card
    {
        public Card()
        {
            Sides = new List<CardSide>();
        }

        public Guid Id { get; set; }

        public List<CardSide> Sides { get; private set; }
    }
}
