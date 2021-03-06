﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Core.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FlashCards.Core.ViewModel
{
    public class CollectionView : INotifyPropertyChanged
    {
        private IDataStore _dataStore;

        public CollectionView(IDataStore dataStore, CardCollection collection)
        {
            if (dataStore == null)
            {
                throw new ArgumentNullException(nameof(dataStore));
            }

            Cards = new ObservableCollection<CardView>();

            _dataStore = dataStore;
            this.Name = collection.Name;
            this.Description = collection.Description;
            this.CardCollection = collection;
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

        public CardCollection CardCollection { get; private set; }

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

        private bool editMode;
        public bool EditMode
        {
            get { return editMode; }
            set
            {
                if (value == editMode)
                    return;

                editMode = value;

                OnPropertyChanged(nameof(EditMode));
            }
        }

        public void Edit()
        {
            EditMode = true;
        }

        public void Cancel()
        {
            EditMode = false;
        }

        public void Load()
        {
            foreach (var card in CardCollection.Cards)
            {
                var cardView = new CardView();
                this.Cards.Add(cardView);
                cardView.Load(card);
            }
            
            this.Cards.Add(new CardView
            {
                IsNew = true
            });
        }

        public void Save()
        {
            EditMode = false;
            CardCollection.Name = this.Name;
            CardCollection.Description = this.Description;
            CardCollection.Cards.Clear();
            CardCollection.Cards.AddRange(this.Cards.Select(c => c.GetCard()));
            _dataStore.SaveCollection(CardCollection);
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
