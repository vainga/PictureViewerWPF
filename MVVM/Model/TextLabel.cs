using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PictureViewer.MVVM.Model
{
    public class TextLabel
    {
        public string Text { get; set; }
        public Point Position { get; set; }
        public Size Size { get; set; }

        public void AddTextToImage(Image image)
        {
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                var formattedText = new FormattedText(
                    Text,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    12, // Размер шрифта
                    Brushes.Black);

                drawingContext.DrawText(formattedText, Position);
            }

            var renderTargetBitmap = new RenderTargetBitmap(
                (int)image.ActualWidth,
                (int)image.ActualHeight,
                96, // DPI
                96, // DPI
                PixelFormats.Default);

            renderTargetBitmap.Render(drawingVisual);

            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (var stream = new System.IO.MemoryStream())
            {
                bitmapEncoder.Save(stream);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }

            image.Source = bitmapImage;
        }
    }
}
