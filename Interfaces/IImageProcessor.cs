using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PictureViewer.Interfaces
{
    internal interface IImageProcessor
    {
        Image GIGAImage { get; set; }
        Image CropImage(Image image);
        Image turnCWImage(Image image);
        Image turnCCWImage(Image image);
        Image AdjustBrightness(Image originalImage, double sliderValue);

        Image AdjustRedColor(Image originalImage, double sliderValue);
        Image AdjustGreenColor(Image originalImage, double sliderValue);
        Image AdjustBlueColor(Image originalImage, double sliderValue);
        Image AdjustOpacity(Image originalImage, double sliderValue);

    }
}
