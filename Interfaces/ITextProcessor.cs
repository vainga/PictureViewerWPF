using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PictureViewer.Interfaces
{
    internal interface ITextProcessor
    {
        string content { get; set; }
        double size { get; set; }
        double posX { get; set; }
        double posY { get; set; }
        public Image AddTextToImage(Image image);
    }
}
