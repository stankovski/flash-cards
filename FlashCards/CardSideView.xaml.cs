using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using FlashCards.Core;
using FlashCards.Core.Model;
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
    public sealed partial class CardSideView : UserControl
    {
        public CardSideView()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
            typeof(string), typeof(CardSideView), new PropertyMetadata(null));


        public List<InkStroke> Strokes
        {
            get { return (List<InkStroke>)GetValue(StrokesProperty); }
            set { SetValue(StrokesProperty, value); }
        }
        public static readonly DependencyProperty StrokesProperty = DependencyProperty.Register(nameof(Strokes),
            typeof(List<InkStroke>), typeof(CardSideView), new PropertyMetadata(new List<InkStroke>(), ChangeInkStrokes));

        private static void ChangeInkStrokes(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var thisControl = source as CardSideView;
            if (thisControl != null)
            {
                var strokes = e.NewValue as List<InkStroke>;
                if (strokes != null && strokes.Count > 0)
                {
                    thisControl.DrawingCanvas.InkPresenter.StrokeContainer.Clear();
                    thisControl.DrawingCanvas.InkPresenter.StrokeContainer.AddStrokes(e.NewValue as List<InkStroke>);
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
