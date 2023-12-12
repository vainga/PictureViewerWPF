using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
namespace PictureViewer.MVVM.Model
{
    public class PaintProcessor
    {
        public double size {  get; set; }
        public Color color { get; set; }
        
        public void DrawOnImage(Image image, Point currentPosition)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(((BitmapSource)image.Source), new Rect(0, 0, image.ActualWidth, image.ActualHeight));
                drawingContext.DrawEllipse(new SolidColorBrush(this.color), null, currentPosition, this.size, this.size);
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                (int)image.ActualWidth, (int)image.ActualHeight, 96, 96, PixelFormats.Pbgra32);

            renderTargetBitmap.Render(drawingVisual);

            image.Source = renderTargetBitmap;
        }
    }
}
