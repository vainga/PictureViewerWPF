using PictureViewer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;

namespace PictureViewer.MVVM.Model
{
    internal abstract class ImageColors : IColors
    {
        public abstract Image AdjustColor(Image originalImage, double sliderValue);
    }
}
