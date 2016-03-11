/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 */

using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Azure.Engagement.Overlay
{
  /// <summary>
  /// Represents an Engagement overlay as a Grid.
  /// It resizes the underlying webview depending on the display's events.
  /// </summary>
  public abstract partial class EngagementBaseOverlay : Grid
  {
    /// <summary>
    /// Attach event handlers.
    /// </summary>
    public void SetHandler()
    {
      /* Update the webview when the app window is resized. */
      Window.Current.SizeChanged += DisplayProperties_SizeChanged;

      /* Update the webview when the app/status bar is resized. */
#if WINDOWS_PHONE_APP || WINDOWS_UWP
      ApplicationView.GetForCurrentView().VisibleBoundsChanged += DisplayProperties_SizeChanged;
#endif
    }

    /// <summary>
    /// Detach event handlers.
    /// </summary>
    public void UnsetHandler()
    {
      Window.Current.SizeChanged -= DisplayProperties_SizeChanged;
#if WINDOWS_PHONE_APP || WINDOWS_UWP
      ApplicationView.GetForCurrentView().VisibleBoundsChanged -= DisplayProperties_SizeChanged;
#endif
    }

    /// <summary>
    /// Set the webview to the right size.
    /// </summary>
    public abstract void SetWebView();

    /// <summary>
    /// Occur when a size change on the App window is detected.
    /// The webview size is updated accordingly.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">Event data for the event.</param>
    private void DisplayProperties_SizeChanged(object sender, object e)
    {
      SetWebView();
    }
  }
}
