using System;
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
            BuildLocalizedMenu();
        }

        private void BuildLocalizedMenu()
        {
            NavigationLinks.Add(new NavigationLink
            {
                Label = "Start",
                Symbol = Symbol.Play,
                Action = () => 
                {
                    var cardCollection = (CollectionPivot.SelectedItem as CollectionView).CardCollection;

                    if (cardCollection != null)
                    {
                        Navigation.Service.Navigate<MemorizationPage>(new MemorizationNavigationModel { CardCollection = cardCollection });
                    }
                }
            });
        }

        private void NavLinkItemClick(object sender, ItemClickEventArgs e)
        {
            var navLink = e.ClickedItem as NavigationLink;
            if (navLink != null)
            {
                navLink.Action();
            }
        }

        private void GoToSettings(object sender, TappedRoutedEventArgs e)
        {

        }

        private void HamburgerButtonClick(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
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

        private async void DeleteCollection(object sender, RoutedEventArgs e)
        {
            var cardCollection = (CollectionPivot.SelectedItem as CollectionView).CardCollection;
            if (cardCollection == null)
            {
                return;
            }

            if (await Utils.ShowQuestionMessageBox("Are you sure you want to delete the entire collection?", "Delete Collection"))
            {
                App.DataStore.RemoveCollection(cardCollection);
                ViewModel.Load();
                Bindings.Update();
            }
        }

        private void AddCollection(object sender, RoutedEventArgs e)
        {
            var newCollection = new CardCollection
            {
                Id = Guid.NewGuid(),
                Name = "new",
                Description = "New collection"
            };
            App.DataStore.SaveCollection(newCollection);
            ViewModel.Load();
            Bindings.Update();
        }
    }
}
