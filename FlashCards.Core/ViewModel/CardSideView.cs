using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace FlashCards.Core.ViewModel
{
    public class CardSideView : INotifyPropertyChanged
    {
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

        private BitmapImage image;
        public BitmapImage Image
        {
            get { return image; }
            set
            {
                if (value == image)
                    return;

                image = value;

                OnPropertyChanged(nameof(Image));
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
