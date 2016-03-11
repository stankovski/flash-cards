/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 */

using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Microsoft.Azure.Engagement.Overlay
{
  /// <summary>
  /// Engagement grid for overlay insertion of announcement
  /// </summary>
  public partial class EngagementOverlayAnnouncement : EngagementBaseOverlay
  {
    /// <summary>
    /// Unique instance of the overlay.
    /// </summary>
    private static EngagementOverlayAnnouncement instance;

    /// <summary>
    /// Singleton instance.
    /// </summary>
    public static EngagementOverlayAnnouncement Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new EngagementOverlayAnnouncement();
        }
        return instance;
      }
    }

    /// <summary>
    /// Create the overlay.
    /// </summary>
    private EngagementOverlayAnnouncement()
    {
      this.InitializeComponent();
    }

    /// <summary>
    /// Set the webview to the right size.
    /// </summary>
    public override void SetWebView()
    {
      short lastPixel = 0;

      /* Fix the 1px margin on Windows/WP 8.1. */
#if WINDOWS_PHONE_APP || WINDOWS_APP
      lastPixel = 1;
#endif
#if WINDOWS_PHONE_APP || WINDOWS_UWP
      /* Take only the visible region of the App window into account. */
      this.engagement_announcement_content.Width = ApplicationView.GetForCurrentView().VisibleBounds.Width + lastPixel;
      this.engagement_announcement_content.Height = ApplicationView.GetForCurrentView().VisibleBounds.Height + lastPixel;
#else
      /* Windows store app window full width and height. */
      this.engagement_announcement_content.Width =  Window.Current.Bounds.Width + lastPixel;
      this.engagement_announcement_content.Height =  Window.Current.Bounds.Height + lastPixel;
#endif
    }
  }
}
