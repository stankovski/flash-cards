using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FlashCards.Core.ViewModel
{
    public class CardView : INotifyPropertyChanged
    {
        private string label;
        public string Label
        {
            get { return label; }
            set
            {
                if (value == label)
                    return;

                label = value;

                OnPropertyChanged(nameof(Label));
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
