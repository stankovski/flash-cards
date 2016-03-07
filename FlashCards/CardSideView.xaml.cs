using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using FlashCards.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
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


        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image),
            typeof(BitmapImage), typeof(CardSideView), new PropertyMetadata(null));

        public CardFormat Format
        {
            get { return (CardFormat)GetValue(CardFormatProperty); }
            set { SetValue(CardFormatProperty, value); }
        }
        public static readonly DependencyProperty CardFormatProperty = DependencyProperty.Register(nameof(Format),
            typeof(CardFormat), typeof(CardSideView), new PropertyMetadata(CardFormat.Text));
    }
}
