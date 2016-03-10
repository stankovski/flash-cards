using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Core.Model;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace FlashCards.Core.ViewModel
{
    public class CardSideView : INotifyPropertyChanged
    {
        public CardSideView()
        {
            Strokes = new List<StrokeData>();
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (value == text)
                    return;

                text = value;

                OnPropertyChanged(nameof(Text));
            }
        }

        private List<StrokeData> strokes;
        public List<StrokeData> Strokes
        {
            get { return strokes; }
            set
            {
                if (value == strokes)
                    return;

                strokes = value;

                OnPropertyChanged(nameof(Strokes));
            }
        }

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
