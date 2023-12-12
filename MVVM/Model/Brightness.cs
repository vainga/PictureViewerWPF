using PictureViewer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PictureViewer.MVVM.Model
{
    public class Brightness : IBrightness
    {
        public Image AdjustColor(Image originalImage, double sliderValue)
        {
            Image newImage = new Image();
            sliderValue = Math.Max(0, Math.Min(100, sliderValue));

            if (originalImage.Source is BitmapSource bitmapSource)
            {

                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    if (sliderValue != 50)
                    {
                        System.Windows.Media.Color overlayColor = (sliderValue > 50) ? Colors.White : Colors.Black;
                        int overlayAlpha = (sliderValue > 50) ? (int)(2.55 * (sliderValue - 50)) : (int)(2.55 * (50 - sliderValue));
                        System.Windows.Media.Color overlayColorWithAlpha = System.Windows.Media.Color.FromArgb((byte)overlayAlpha, overlayColor.R, overlayColor.G, overlayColor.B);

                        drawingContext.DrawImage(bitmapSource, new Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight));
                        drawingContext.DrawRectangle(new SolidColorBrush(overlayColorWithAlpha), null, new Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight));
                    }
                    else
                    {
                        drawingContext.DrawImage(bitmapSource, new Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight));
                    }
                }

                RenderTargetBitmap resultImage = new RenderTargetBitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY, PixelFormats.Pbgra32);
                resultImage.Render(drawingVisual);

                newImage.Source = resultImage;

            }
            else
            {

            }

            return newImage;
        }
    }
}
