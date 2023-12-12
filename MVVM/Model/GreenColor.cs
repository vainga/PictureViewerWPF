using PictureViewer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace PictureViewer.MVVM.Model
{
    public class GreenColor : IGreenColor
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
                    if (sliderValue != 0)
                    {
                        byte greenColorValue = (byte)(255 * (sliderValue / 100));

                        System.Windows.Media.Color overlayColor = Color.FromArgb((byte)(sliderValue * 2.55), 0, greenColorValue, 0);
                        drawingContext.DrawImage(bitmapSource, new Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight));
                        drawingContext.DrawRectangle(new SolidColorBrush(overlayColor), null, new Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight));
                    }
                    else
                    {
                        drawingContext.DrawImage(bitmapSource, new Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight));
                    }
                }

                RenderTargetBitmap resultImage = new RenderTargetBitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, bitmapSource.DpiX, bitmapSource.DpiY, PixelFormats.Pbgra32);
                resultImage.Render(drawingVisual);

                newImage.Source = resultImage;

                //updateImage(originalImage, newImage);
            }
            else
            {

            }

            return newImage;
        }
    }
}
