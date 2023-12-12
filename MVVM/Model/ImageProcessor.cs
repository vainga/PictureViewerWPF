using PictureViewer.Interfaces;
using PictureViewer.MVVM.View;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


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

        public Image AdjustBrightness(Image originalImage, double sliderValue)
        {
            IBrightness brightness = new Brightness();
            Image newImage = brightness.AdjustColor(originalImage, sliderValue);
            updateImage(originalImage, newImage);
            return newImage;
        }

        public Image AdjustRedColor(Image originalImage, double sliderValue)
        {
            IRedColor redColor = new RedColor();
            Image newImage = redColor.AdjustColor(originalImage, sliderValue);
            updateImage(originalImage, newImage);
            return newImage;
        }

        public Image AdjustGreenColor(Image originalImage, double sliderValue)
        {
            IGreenColor greenColor = new GreenColor();
            Image newImage = greenColor.AdjustColor(originalImage, sliderValue);
            updateImage(originalImage, newImage);
            return newImage;
        }

        public Image AdjustBlueColor(Image originalImage, double sliderValue)
        {
            IBlueColor blueColor = new BlueColor();
            Image newImage = blueColor.AdjustColor(originalImage, sliderValue);
            updateImage(originalImage, newImage);
            return newImage;
        }

        public Image AdjustOpacity(Image originalImage, double sliderValue)
        {
            IAlphaChanelColor alphaChanelColor = new AlphaChanelColor();
            Image newImage = alphaChanelColor.AdjustColor(originalImage, sliderValue);
            updateImage(originalImage, newImage);
            return newImage;
        }
    }  
}
