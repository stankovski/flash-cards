/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 */

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Azure.Engagement.Overlay
{
  /// <summary>
  /// Helper class used to replace Windows's Page class.
  /// Automatically introduce the Engagement overlay for Reach in-app campaigns UI and starts a new Engagement activity when displayed.
  /// </summary>
  public class EngagementPageOverlay : EngagementPage
  {
    /* Overlay element containing the webview for in-app banners. */
    EngagementOverlayNotification mNotification;

    /* Overlay element containing the webview for announcements' full screen content. */
    EngagementOverlayAnnouncement mAnnouncement;

    /* Engagement grid element used to insert webviews into current page. */
    Grid mEngagementGrid;

    /// <summary>
    /// This object is used to lock some parts of the Engagement page overlay between threads and references.
    /// </summary>
    private static object thisLock = new object();

    /// <summary>
    /// EngagementPage constructor.
    /// </summary>
    public EngagementPageOverlay()
    {
      mNotification = EngagementOverlayNotification.Instance;
      mAnnouncement = EngagementOverlayAnnouncement.Instance;
      mEngagementGrid = new Grid();
    }

    /// <summary>
    /// Used to find item on current displayed screen.
    /// </summary>
    /// <typeparam name="T">Any control type.</typeparam>
    /// <param name="control">Object to be inspected.</param>
    /// <param name="ctrlName">Name of the control we need.</param>
    /// <returns>The requested control or null if it can't be found.</returns>
    private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
    {
      int childNumber = VisualTreeHelper.GetChildrenCount(control);
      for (int i = 0; i < childNumber; i++)
      {
        DependencyObject child = VisualTreeHelper.GetChild(control, i);
        FrameworkElement fe = child as FrameworkElement;

        /* Not a framework element or is null. */
        if (fe == null) return null;

        if (child is T && fe.Name == ctrlName)
        {
          /* Found the control, return it. */
          return child;
        }
        else
        {
          /* Control not found, search on children. */
          DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
          if (nextLevel != null)
            return nextLevel;
        }
      }
      return null;
    }

    /// <summary>
    /// Initialize the overlay and send an activity to the engagement backend when the page is displayed (not loaded).
    /// </summary>
    /// <param name="e">Navigation event argument.</param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      InitOverlay();
      base.OnNavigatedTo(e);
    }

    /// <summary>
    /// Initialize overlay on the current page.
    /// </summary>
    public void InitOverlay()
    {
      /* We have to use RunOnUI to ensure to be in the same thread pool than UI */
#pragma warning disable 4014
      RunOnUI(() =>
      {
        /* EngagementGrid is locked to ensure fast switching between page doesn't crash the app. */
        lock (thisLock)
      {
        mEngagementGrid = new Grid();

        /* Check if customer have tagged a grid with "engagementGrid". */
        mEngagementGrid = FindChildControl<Grid>(Window.Current.Content, "engagementGrid") as Grid;

        /* Pick the first element found on the grid. */
        if (mEngagementGrid == null)
        {
          mEngagementGrid = FindChildControl<Grid>(Window.Current.Content, "") as Grid;
        }

        /* Take unique instance of Engagement UI elements to add them on the Engagement grid. */
        mNotification = EngagementOverlayNotification.Instance;
        mAnnouncement = EngagementOverlayAnnouncement.Instance;

        /* If we have a grid then insert the overlay in it. */
        if (mEngagementGrid != null)
        {
          /* Check that notification is not already set. */
          if (!mEngagementGrid.Children.Contains(mNotification))
          {
            /* Add it. */
            try
            {
              mEngagementGrid.Children.Add(mNotification);
            }
            catch (Exception)
            {
              /* Because of threading context we can face an unexpected error due to webview insertion.
               * But we have to ensure overlay creation. */
              InitOverlay();
              return;
            }


            /* Set the display_orientation_change event to resize the notification. */
            mNotification.SetHandler();

            /* Make the first resize to be sure to match the current application size. */
            mNotification.SetWebView();
          }

          /* Check an announcement is already set. */
          if (!mEngagementGrid.Children.Contains(mAnnouncement))
          {
            /* Add it. */
            try
            {
              mEngagementGrid.Children.Add(mAnnouncement);
            }
            catch (Exception)
            {
              /* Because of threading context we can face an unexpected error due to webview insertion.
               * But we have to ensure overlay creation. */
              InitOverlay();
              return;
            }

            /* Set the display_orientation_change event to resize the notification. */
            mAnnouncement.SetHandler();

            /* Make the first resize to be sure to match the current application size. */
            mAnnouncement.SetWebView();
          }
        }
        else
        {
          /* Even if we have locked we ensure that event on overlay are detached. */
          mNotification.UnsetHandler();
          mAnnouncement.UnsetHandler();
          mEngagementGrid = new Grid();

          /* Run again the init because sometime graphical context isn't set fine. */
          InitOverlay();
        }
      }
      });
    }

    /// <summary>
    /// Run an action on UI.
    /// </summary>
    /// <param name="action">The action.</param>
    public static async Task RunOnUI(Action action)
    {
      await Task.Run(() =>
      {
        return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        {
          action();
        }).AsTask();
      });
    }

    /// <summary>
    /// Detach webview elements when we navigate off the page.
    /// </summary>
    /// <param name="e">Argument of the navigation event.</param>
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      RunOnUI(() =>
      {
        /* Remove notification from grid. */
        if (mEngagementGrid.Children.Contains(mNotification))
        {
          mNotification.UnsetHandler();
          mEngagementGrid.Children.Remove(mNotification);
        }

        /* Remove notification from grid. */
        if (mEngagementGrid.Children.Contains(mAnnouncement))
        {
          mAnnouncement.UnsetHandler();
          mEngagementGrid.Children.Remove(mAnnouncement);
        }

        /* Forward navigation event. */
        base.OnNavigatedFrom(e);
      });
    }
  }
}
