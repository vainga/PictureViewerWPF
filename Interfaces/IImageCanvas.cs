using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PictureViewer.Interfaces
{
    internal interface IImageCanvas
    {
        void saveCanvas(InkCanvas canvas);
        BitmapSource canvasToBitmap(InkCanvas canv);
    }
}
