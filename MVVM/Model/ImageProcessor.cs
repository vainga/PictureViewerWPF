using Microsoft.Win32;
using PictureViewer.MVVM.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace PictureViewer.MVVM.Model
{

    public class ImageProcessor
    {
        public Image GIGAImage { get; set; }
       
        public ImageProcessor()
        {
            GIGAImage = new Image();
        }
        private void updateImage(Image targetImage, Image sourceImage)
        {
            targetImage.Source = sourceImage.Source;
        }

        public Image LoadImage(Image image)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                image.Source = new BitmapImage(new Uri(op.FileName));
            }
        
            return image;
        }

        public Image CropImage(Image image)
        {
            Image cropped = new Image();
            cropped.Width = 0.4 * image.Width;
            cropped.Height = 0.4 * image.Height;

            CroppedBitmap cb = new CroppedBitmap((BitmapSource)image.Source, new Int32Rect((int)(0.2 * image.ActualWidth), (int)(0.2 * image.ActualHeight), (int)(0.4 * image.ActualWidth), (int)(0.4 * image.ActualHeight)));
            cropped.Source = cb;
            updateImage(image,cropped);

            return cropped;
        }

        public Image turnCWImage(Image image)
        {
            Image turned = new Image();
            turned.Height = image.Width;
            turned.Width = image.Height;

            TransformedBitmap tb = new TransformedBitmap();
            tb.BeginInit();
            tb.Source = (BitmapSource)image.Source;
            RotateTransform transform = new RotateTransform(270);
            tb.Transform = transform;
            tb.EndInit();
            turned.Source = tb;
            updateImage(image, turned);
            return turned;
        }

        public Image turnCCWImage(Image image)
        {
            Image turned = new Image();
            turned.Height = image.Width;
            turned.Width = image.Height;

            TransformedBitmap tb = new TransformedBitmap();
            tb.BeginInit();
            tb.Source = (BitmapSource)image.Source;
            RotateTransform transform = new RotateTransform(90);
            tb.Transform = transform;
            tb.EndInit();
            turned.Source = tb;
            updateImage(image, turned);

            return turned;
        }

        public void saveImage(Image image)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Title = "Сохранить изображение (по умолчанию .png)";
            sv.Filter = "Все форматы изображений|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            sv.DefaultExt = ".png";
            if (sv.ShowDialog() == true)
            {
                string ext = sv.FileName.Substring(sv.FileName.IndexOf('.'));
                switch (ext)
                {
                    case ".png":
                        {
                            PngBitmapEncoder encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(image.Source as BitmapSource));
                            using (FileStream fileStream = new FileStream(sv.FileName, FileMode.Create))
                                encoder.Save(fileStream);
                        }
                        break;
                    case ".jpeg":
                    case ".jpg":
                        {
                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(image.Source as BitmapSource));
                            using (FileStream fileStream = new FileStream(sv.FileName, FileMode.Create))
                                encoder.Save(fileStream);
                        }
                        break;
                    default:
                        {
                            MessageBox.Show("Не удалось сохранить изображение, попробуйте еще раз.");
                        }
                        break;
                }
            }
        }

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

                updateImage(image, finalImageWithText);

                return finalImageWithText;
            }

            return image;
        }

        public Image AdjustBrightness(Image originalImage, double sliderValue)
        {
            Image newImage = new Image();
            //originalImage = GIGAImage;
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

                updateImage(originalImage,newImage);
            }
            else
            {

            }

            return newImage;
        }

        public Image AdjustRedColor(Image originalImage, double sliderValue)
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
                        byte redColorValue = (byte)(255 * (sliderValue / 100)); 

                        System.Windows.Media.Color overlayColor = Color.FromArgb((byte)(sliderValue * 2.55), redColorValue, 0, 0);
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

                updateImage(originalImage, newImage);
            }
            else
            {

            }

            return newImage;
        }

        public Image AdjustGreenColor(Image originalImage, double sliderValue)
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

                updateImage(originalImage, newImage);
            }
            else
            {
           
            }

            return newImage;
        }
        public Image AdjustBlueColor(Image originalImage, double sliderValue)
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
                        byte blueColorValue = (byte)(255 * (sliderValue / 100)); 

                        System.Windows.Media.Color overlayColor = Color.FromArgb((byte)(sliderValue * 2.55), 0, 0, blueColorValue);
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

                updateImage(originalImage, newImage);
            }
            else
            {

            }

            return newImage;
        }

        public Image AdjustOpacity(Image originalImage, double sliderValue)
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
                        byte alphaValue = (byte)(255 * (sliderValue / 100));

                        System.Windows.Media.Color overlayColor = Color.FromArgb(alphaValue, 255, 255, 255);
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

                updateImage(originalImage, newImage);
            }
            else
            {

            }

            return newImage;
        }

        public Image DrawOnImage(Image originalImage)
        {
            return null;
        }
    }  
}
