using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using FlashCards.Core;
using FlashCards.Core.Model;
using FlashCards.Core.ViewModel;
using FlashCards.NavigationModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FlashCards
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CardEditPage : Page
    {
        private bool _cancelSave = false;
        private CardCollection _cardCollection = null;

        public CardView ViewModel { get; set; }
        public ObservableCollection<NavigationLink> NavigationLinks { get; private set; }

        public CardEditPage()
        {
            this.ViewModel = new CardView();
            this.NavigationLinks = new ObservableCollection<NavigationLink>();
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                CardNavigationModel parameter = e.Parameter as CardNavigationModel;
                if (parameter.CardCollection == null)
                {
                    throw new InvalidOperationException("Card Collection can not be null.");
                }

                if (parameter.Card != null)
                {
                    await ViewModel.Load(parameter.Card);
                }
                _cardCollection = parameter.CardCollection;
            }
            else
            {
                throw new ArgumentNullException(nameof(e.Parameter));
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
   
            // Save changes
            if (!_cancelSave)
            {
                // Update ViewModel
                ViewModel.SideA.Text = this.SideAText.Text;
                ViewModel.SideB.Text = this.SideBText.Text;

                var card = ViewModel.GetCard();
                var existingCard = _cardCollection.Cards.FirstOrDefault(c => c.Id == card.Id);
                if (existingCard == null)
                {
                    _cardCollection.Cards.Add(card);
                }
                else
                {
                    existingCard.SideA = card.SideA;
                    existingCard.SideB = card.SideB;
                }
                App.DataStore.SaveCollection(_cardCollection);
            }
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

        private void CancelCardClick(object sender, RoutedEventArgs e)
        {
            _cancelSave = true;
            Navigation.Service.GoBack();
        }

        private async void DeleteCardClick(object sender, RoutedEventArgs e)
        {
            if (await ShowQuestionMessageBox("Are you sure you want to delete the card?", "Delete Card"))
            {
                _cancelSave = true;
                _cardCollection.Cards.RemoveAll(c => c.Id == ViewModel.Id);
                App.DataStore.SaveCollection(_cardCollection);
                Navigation.Service.GoBack();
            }
        }

        private async Task<bool> ShowQuestionMessageBox(string text, string title)
        {
            var dialog = new MessageDialog(text);
            dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            IUICommand result = await dialog.ShowAsync();
            return (int)result.Id == 0;
        }

        private async void SideAImageTapped(object sender, RoutedEventArgs e)
        {
            using (var dialog = new DrawingDialog())
            {
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    this.SideAImageButton.Visibility = Visibility.Collapsed;
                    BitmapImage image = new BitmapImage();
                    image.SetSource(dialog.Drawing);
                    SideAImage.Source = image;
                }
            }
        }

        private async void SideBImageTapped(object sender, RoutedEventArgs e)
        {
            using (var dialog = new DrawingDialog())
            {
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    this.SideBImageButton.Visibility = Visibility.Collapsed;
                    BitmapImage image = new BitmapImage();
                    image.SetSource(dialog.Drawing);
                    SideBImage.Source = image;
                }
            }
        }
    }
}
