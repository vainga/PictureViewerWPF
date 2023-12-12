using PictureViewer.MVVM.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using PictureViewer.Interfaces;

namespace PictureViewer.MVVM.Model
{
    public class TextProcessor : ITextProcessor
    {
        public string content { get;  set; }
        public double size {  get;  set; }
        public double posX {  get;  set; }
        public double posY { get;  set; }

        public Image AddTextToImage(Image image)
        {
            TextWindow textWindow = new TextWindow();
            bool? result = textWindow.ShowDialog();

            if (result == true)
            {
                TextProcessor textProcessor = textWindow.GetText();

                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                    (int)image.ActualWidth, (int)image.ActualHeight, 96, 96, PixelFormats.Pbgra32);

                TextBlock textBlock = new TextBlock
                {
                    Text = textProcessor.content,
                    FontSize = textProcessor.size,
                    Foreground = new SolidColorBrush(Colors.Black)
                };
                textBlock.Measure(new Size(image.ActualWidth, image.ActualHeight));
                textBlock.Arrange(new Rect(0, 0, image.ActualWidth, image.ActualHeight));

                renderTargetBitmap.Render(image);

                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    context.DrawImage(renderTargetBitmap, new Rect(0, 0, image.ActualWidth, image.ActualHeight));
                    context.DrawText(new FormattedText(textProcessor.content, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), textProcessor.size, Brushes.Black), new Point(textProcessor.posX, textProcessor.posY));
                }

                RenderTargetBitmap finalRenderTargetBitmap = new RenderTargetBitmap(
                    (int)image.ActualWidth, (int)image.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                finalRenderTargetBitmap.Render(visual);

                BitmapSource resizedImage = new FormatConvertedBitmap(finalRenderTargetBitmap, PixelFormats.Pbgra32, null, 0);

                Image finalImageWithText = new Image();
                finalImageWithText.Source = resizedImage;

                image.Source = finalImageWithText.Source;

                return finalImageWithText;
            }

            return image;
        }
    }
}
