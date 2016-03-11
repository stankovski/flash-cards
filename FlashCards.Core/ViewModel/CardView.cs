using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Core.Model;
using Newtonsoft.Json;
using Windows.Storage.Streams;
using Windows.UI.Input.Inking;
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

        private bool flipped;
        public bool Flipped
        {
            get { return flipped; }
            set
            {
                if (value == flipped)
                    return;

                flipped = value;

                OnPropertyChanged(nameof(Flipped));
            }
        }
        
        public void Load(Card card)
        {
            this.Id = card.Id;
            this.SideA.Text = card.SideA.Text;
            if (card.SideA.InkStrokes != null)
            {
                this.SideA.Strokes = card.SideA.InkStrokes;
            }
            this.SideB.Text = card.SideB.Text;
            if (card.SideB.InkStrokes != null)
            {
                this.SideB.Strokes = card.SideB.InkStrokes;
            }
        }

        public Card GetCard()
        {
            Card card = new Card();
            if (this.Id == default(Guid))
            {
                card.Id = Guid.NewGuid();
            }
            else
            {
                card.Id = this.Id;
            }

            card.SideA = new CardSide
            {
                Text = this.SideA.Text,
                InkStrokes = this.SideA.Strokes
            };
            card.SideB = new CardSide
            {
                Text = this.SideB.Text,
                InkStrokes = this.SideB.Strokes
            };
            return card;
        }

        public void Flip()
        {
            Flipped = !Flipped;
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
