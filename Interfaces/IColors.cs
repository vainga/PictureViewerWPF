using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace PictureViewer.Interfaces
{
    internal interface IColors
    {
        Image AdjustColor(Image originalImage, double sliderValue);
    }
}
