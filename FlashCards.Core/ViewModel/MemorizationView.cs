using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Core.Model;
using Microsoft.Azure.Engagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FlashCards.Core.ViewModel
{
    public class MemorizationView : INotifyPropertyChanged
    {
        public MemorizationView()
        {
            Cards = new ObservableCollection<CardView>();            
        }

        private ObservableCollection<CardView> cards;
        public ObservableCollection<CardView> Cards
        {
            get { return cards; }
            set
            {
                if (value == cards)
                    return;

                cards = value;

                OnPropertyChanged(nameof(Cards));
            }
        }

        private int currentCardIndex;
        public int CurrentCardIndex
        {
            get { return currentCardIndex; }
            set
            {
                if (value == currentCardIndex)
                    return;

                if (value >= 0 && value < Cards.Count)
                {
                    Dictionary<object, object> details = new Dictionary<object, object>();
                    details["SideA"] = Cards[value].SideA.Text;
                    details["SideB"] = Cards[value].SideB.Text;
                    EngagementAgent.Instance.SendEvent("FlipPage", details);
                }

                currentCardIndex = value;

                OnPropertyChanged(nameof(CurrentCardIndex));
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value == name)
                    return;

                name = value;

                OnPropertyChanged(nameof(Name));
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value == description)
                    return;

                description = value;

                OnPropertyChanged(nameof(Description));
            }
        }

        public void Load(CardCollection collection)
        {
            this.Name = collection.Name;
            this.Description = collection.Description;
            foreach (var card in collection.Cards)
            {
                var cardView = new CardView();
                this.Cards.Add(cardView);
                cardView.Load(card);
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
