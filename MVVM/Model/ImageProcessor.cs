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
    public class ImageProcessor
    {
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


    }
}
