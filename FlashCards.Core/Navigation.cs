using FlashCards.Helpers;

namespace FlashCards.Core
{
    public static class Navigation
    {
        /// <summary>
        /// Navigation service, provides a decoupled way to trigger the UI Frame
        /// to transition between views.
        /// </summary>
        public static INavigationService Service { get; set; }
    }
}
