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
using Windows.UI.Input.Inking;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
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
                    ViewModel.Load(parameter.Card);
                    if (ViewModel.SideA.Strokes.Count > 0)
                    {
                        UpdateDrawing(SideAImage, ViewModel.SideA.Strokes);
                        SideAImageButton.Visibility = Visibility.Collapsed;
                    }
                    if (ViewModel.SideB.Strokes.Count > 0)
                    {
                        UpdateDrawing(SideBImage, ViewModel.SideB.Strokes);
                        SideBImageButton.Visibility = Visibility.Collapsed;
                    }
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
                ViewModel.SideA.Strokes = this.SideAImage.InkPresenter.StrokeContainer.GetStrokes().Select(s => s.Clone()).ToList();
                ViewModel.SideB.Strokes = this.SideBImage.InkPresenter.StrokeContainer.GetStrokes().Select(s => s.Clone()).ToList();

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
            if (await Utils.ShowQuestionMessageBox("Are you sure you want to delete the card?", "Delete Card"))
            {
                _cancelSave = true;
                _cardCollection.Cards.RemoveAll(c => c.Id == ViewModel.Id);
                App.DataStore.SaveCollection(_cardCollection);
                Navigation.Service.GoBack();
            }
        }

        private async void SideAImageTapped(object sender, RoutedEventArgs e)
        {
            if (await GetDrawing(SideAImage))
            {
                this.SideAImageButton.Visibility = Visibility.Collapsed;
            }
        }


        private async void SideBImageTapped(object sender, RoutedEventArgs e)
        {
            if (await GetDrawing(SideBImage))
            {
                this.SideBImageButton.Visibility = Visibility.Collapsed;
            }
        }

        private async Task<bool> GetDrawing(InkCanvas canvas)
        {
            using (var dialog = new DrawingDialog())
            {
                if (canvas.InkPresenter.StrokeContainer.BoundingRect.Width > 0)
                {
                    dialog.Strokes.AddRange(
                        canvas.InkPresenter.StrokeContainer.GetStrokes().Select(s => s.Clone()));
                }
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    UpdateDrawing(canvas, dialog.Strokes);
                    return true;
                }
                return false;
            }
        }

        private static void UpdateDrawing(InkCanvas canvas, List<InkStroke> strokes)
        {
            canvas.InkPresenter.StrokeContainer.Clear();
            canvas.InkPresenter.StrokeContainer.AddStrokes(strokes);
            canvas.Width = canvas.InkPresenter.StrokeContainer.BoundingRect.Right;
            canvas.Height = canvas.InkPresenter.StrokeContainer.BoundingRect.Bottom;
            canvas.InkPresenter.IsInputEnabled = false;
        }
    }
}
