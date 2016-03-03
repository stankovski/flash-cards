﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Core.Model
{
    public class CardCollection
    {
        public CardCollection()
        {
            Cards = new List<Card>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public CardFormat Format { get; set; }

        public List<Card> Cards { get; private set; }
    }
}
