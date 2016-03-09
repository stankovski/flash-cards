﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Core.Model
{
    public struct CardSide
    {
        public string Text { get; set; }
        public List<StrokeData> InkStrokes { get; set; }
    }
}
