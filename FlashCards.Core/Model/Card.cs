using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Core.Model
{
    public class Card
    {
        public Guid Id { get; set; }

        public CardSide SideA { get; set; }

        public CardSide SideB { get; set; }
    }
}
