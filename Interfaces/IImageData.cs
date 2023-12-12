using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;

namespace PictureViewer.Interfaces
{
    internal interface IImageData
    {
        Image LoadImage(Image image);
        void saveImage(Image image);
    }
}
