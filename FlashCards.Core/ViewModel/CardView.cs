using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Core.Model;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace FlashCards.Core.ViewModel
{
    public class CardView : INotifyPropertyChanged
    {
        public CardView()
        {
            SideA = new CardSideView();
            SideB = new CardSideView();
        }

        public Guid Id { get; private set; }

        private bool isNew;
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                if (value == isNew)
                    return;

                isNew = value;

                OnPropertyChanged(nameof(IsNew));
            }
        }

        private CardSideView sideA;
        public CardSideView SideA
        {
            get { return sideA; }
            set
            {
                if (value == sideA)
                    return;

                sideA = value;

                OnPropertyChanged(nameof(SideA));
            }
        }

        private CardSideView sideB;
        public CardSideView SideB
        {
            get { return sideB; }
            set
            {
                if (value == sideB)
                    return;

                sideB = value;

                OnPropertyChanged(nameof(SideB));
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

        public async Task Load(Card card)
        {
            this.Id = card.Id;
            this.SideA.Text = card.SideA.Text;
            this.SideA.Image = await GetImage(card.SideA.Data);
            this.SideB.Text = card.SideB.Text;
            this.SideB.Image = await GetImage(card.SideB.Data);
        }

        public Card GetCard()
        {
            Card card = new Card();
            card.Id = this.Id;
            card.SideA = new CardSide
            {
                Text = this.SideA.Text
            };
            card.SideB = new CardSide
            {
                Text = this.SideB.Text
            };
            return card;
        }

        private static async Task<BitmapImage> GetImage(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }
            BitmapImage image = new BitmapImage();
            using (InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream())
            {
                await randomAccessStream.WriteAsync(data.AsBuffer());
                randomAccessStream.Seek(0);
                image.SetSource(randomAccessStream);
            }
            return image;
        }
    }
}
