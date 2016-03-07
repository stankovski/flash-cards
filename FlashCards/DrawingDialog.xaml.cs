using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FlashCards
{
    public sealed partial class DrawingDialog : ContentDialog, IDisposable
    {
        private InkPresenter _inkPresenter;
        private double _penSize = 2;
        private Stack<IReadOnlyList<InkStroke>> _lastStrokes = new Stack<IReadOnlyList<InkStroke>>();
        private InMemoryRandomAccessStream _randomAccessStream = null;

        public DrawingDialog()
        {
            this.InitializeComponent();

            _inkPresenter = InkCanvasControl.InkPresenter;
            _inkPresenter.InputDeviceTypes =
                CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch;
            _inkPresenter.StrokesCollected += _inkPresenter_StrokesCollected;

            UpdatePen();
        }

        public InMemoryRandomAccessStream Drawing
        {
            get
            {
                return _randomAccessStream;
            }
        }

        private void _inkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            _lastStrokes.Push(args.Strokes);
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _randomAccessStream = new InMemoryRandomAccessStream();
            var strokesJson = JsonConvert.SerializeObject(_inkPresenter.StrokeContainer.GetStrokes().ToList());
            await _inkPresenter.StrokeContainer.SaveAsync(_randomAccessStream);
            _randomAccessStream.Seek(0);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ColorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePen();
        }

        private void SizeSmallClick(object sender, RoutedEventArgs e)
        {
            _penSize = 2;
            UpdatePen();
        }

        private void SizeMedClick(object sender, RoutedEventArgs e)
        {
            _penSize = 6;
            UpdatePen();
        }

        private void SizeMaxClick(object sender, RoutedEventArgs e)
        {
            _penSize = 10;
            UpdatePen();
        }

        private void UndoClick(object sender, RoutedEventArgs e)
        {
            if (_lastStrokes != null)
            {
                if (_lastStrokes.Count > 0)
                {
                    foreach (var stroke in _lastStrokes.Pop())
                    {
                        stroke.Selected = true;
                    }
                }
            }
            _inkPresenter.StrokeContainer.DeleteSelected();
        }

        private void ClearAllClick(object sender, RoutedEventArgs e)
        {
            _inkPresenter.StrokeContainer.Clear();
        }

        private void UpdatePen()
        {
            if (_inkPresenter != null)
            {
                var defaultAttributes = _inkPresenter.CopyDefaultDrawingAttributes();

                switch (penColor.SelectedValue.ToString())
                {
                    case "Black":
                        defaultAttributes.Color = Colors.Black;
                        break;
                    case "Red":
                        defaultAttributes.Color = Colors.Red;
                        break;
                    case "Blue":
                        defaultAttributes.Color = Colors.Blue;
                        break;
                    case "Green":
                        defaultAttributes.Color = Colors.Green;
                        break;
                }

                defaultAttributes.Size = new Size(_penSize, _penSize);
                defaultAttributes.PenTipTransform =
                     System.Numerics.Matrix3x2.CreateRotation((float)Math.PI / 4);
                defaultAttributes.Size = new Size(_penSize, _penSize * 2);

                _inkPresenter.UpdateDefaultDrawingAttributes(defaultAttributes);
            }
        }

        public async void Dispose()
        {
            _inkPresenter.StrokesCollected -= _inkPresenter_StrokesCollected;
            if (_randomAccessStream != null)
            {
                await _randomAccessStream.FlushAsync();
                _randomAccessStream.Dispose();
                _randomAccessStream = null;
            }
        }
    }
}
