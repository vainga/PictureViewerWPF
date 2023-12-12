using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using PictureViewer.Interfaces;

namespace PictureViewer.MVVM.Model
{
    public class ImageData : IImageData
    {
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
                            throw new ArgumentException("Ошибка сохранения!");
                        }
                }
            }
        }
    }
}
