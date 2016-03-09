﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using FlashCards.Core;
using FlashCards.Core.Model;
using FlashCards.Core.ViewModel;
using FlashCards.NavigationModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FlashCards
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainView ViewModel { get; set; }
        public ObservableCollection<NavigationLink> NavigationLinks { get; private set; }

        public MainPage()
        {
            this.ViewModel = new MainView(App.DataStore);
            this.NavigationLinks = new ObservableCollection<NavigationLink>();
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Load();
        }

        private void NavLinkItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void GoToSettings(object sender, TappedRoutedEventArgs e)
        {

        }

        private void HamburgerButtonClick(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void PivotItemLoading(Pivot sender, PivotItemEventArgs args)
        {
            var collectionView = args.Item.DataContext as CollectionView;
            if (collectionView != null)
            {
                collectionView.Load();
            }
        }

        private void AddCardClick(object sender, RoutedEventArgs e)
        {
            var parameter = new CardNavigationModel
            {
                 CardCollection = (CollectionPivot.SelectedItem as CollectionView).CardCollection
            };
            Navigation.Service.Navigate<CardEditPage>(parameter);
        }

        private void SelectCardClick(object sender, RoutedEventArgs e)
        {

        }
        
        private void CardClicked(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            var cardView = (e.OriginalSource as FrameworkElement).DataContext as CardView;
            var cardCollection = (CollectionPivot.SelectedItem as CollectionView).CardCollection;

            if (cardView != null && cardCollection != null)
            {
                var parameter = new CardNavigationModel
                {
                    Card = cardCollection.Cards.FirstOrDefault(c => c.Id == cardView.Id),
                    CardCollection = (CollectionPivot.SelectedItem as CollectionView).CardCollection
                };
                Navigation.Service.Navigate<CardEditPage>(parameter);
            }
        }
    }
}
