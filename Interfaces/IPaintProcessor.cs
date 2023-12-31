﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace PictureViewer.Interfaces
{
    internal interface IPaintProcessor
    {
        double size { get; set; }
        Color color { get; set; }
        void DrawOnImage(Image image, Point currentPosition);
    }
}
