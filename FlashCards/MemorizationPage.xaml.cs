using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using FlashCards.Core;
using FlashCards.Core.ViewModel;
using FlashCards.NavigationModels;
using Microsoft.Azure.Engagement;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FlashCards
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MemorizationPage : Page
    {
        public MemorizationView ViewModel { get; set; }
        public ObservableCollection<NavigationLink> NavigationLinks { get; private set; }

        public MemorizationPage()
        {
            this.ViewModel = new MemorizationView();
            this.NavigationLinks = new ObservableCollection<NavigationLink>();
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            EngagementAgent.Instance.StartActivity("MemorizationPage");

            base.OnNavigatedTo(e);
            var collection = e.Parameter as MemorizationNavigationModel;
            if (collection == null)
            {
                throw new InvalidOperationException("Memorization View is missing MemorizationNavigationModel as parameter");
            }
            this.ViewModel.Load(collection.CardCollection);
            BuildLocalizedMenu();
        }

        private void BuildLocalizedMenu()
        {
            NavigationLinks.Add(new NavigationLink
            {
                Label = "Home",
                Symbol = Symbol.Home,
                Action = () => { Navigation.Service.Navigate<MainPage>(); }
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
    }
}
