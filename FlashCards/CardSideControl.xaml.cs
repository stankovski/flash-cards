using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using FlashCards.Core;
using FlashCards.Core.Model;
using FlashCards.Core.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FlashCards
{
    public sealed partial class CardSideControl : UserControl
    {
        public CardSideControl()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
            typeof(string), typeof(CardSideControl), new PropertyMetadata(null));

        public CardSideView CardSide
        {
            get { return (CardSideView)GetValue(CardSideProperty); }
            set { SetValue(CardSideProperty, value); }
        }
        public static readonly DependencyProperty CardSideProperty = DependencyProperty.Register(nameof(CardSide),
            typeof(CardSideView), typeof(CardSideControl), new PropertyMetadata(new CardSideView(), ChangeCardSide));

        private static void ChangeCardSide(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var thisControl = source as CardSideControl;
            if (thisControl != null)
            {
                var cardSide = e.NewValue as CardSideView;
                if (cardSide != null)
                {
                    thisControl.Text = cardSide.Text;
                    if (cardSide.Strokes.Count > 0)
                    {
                        thisControl.DrawingCanvas.InkPresenter.StrokeContainer.Clear();
                        thisControl.DrawingCanvas.InkPresenter.StrokeContainer.AddStrokes(cardSide.Strokes.Select(s => s.ToInkStroke()));
                        thisControl.DrawingCanvas.InkPresenter.IsInputEnabled = false;
                        thisControl.TextOnlyCard.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        thisControl.TextOnlyCard.Visibility = Visibility.Visible;
                    }
                }                
            }
        }
    }
}
