using Microsoft.Win32;
using PictureViewer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PictureViewer.MVVM.Model
{
    public class ImageCanvas : IImageCanvas
    {
        public void saveCanvas(InkCanvas canvas)
        {
            //encoder.Frames.Add(BitmapFrame.Create(workingImage.Source as BitmapSource));
            //using (FileStream fileStream = new FileStream(sv.FileName, FileMode.Create))
            //      encoder.Save(fileStream);

            SaveFileDialog sv = new SaveFileDialog();
            sv.Title = "Сохранить изображение (по умолчанию .png)";
            sv.Filter = "Все форматы изображений|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            sv.DefaultExt = ".png";

            BitmapSource rtb = canvasToBitmap(canvas);

            if (sv.ShowDialog() == true)
            {
                string ext = sv.FileName.Substring(sv.FileName.LastIndexOf('.'));
                switch (ext)
                {
                    case ".png":
                        {
                            PngBitmapEncoder encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(rtb));
                            using (FileStream fileStream = new FileStream(sv.FileName, FileMode.Create))
                                encoder.Save(fileStream);
                        }
                        break;
                    case ".jpeg":
                    case ".jpg":
                        {
                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(rtb));
                            using (FileStream fileStream = new FileStream(sv.FileName, FileMode.Create))
                                encoder.Save(fileStream);
                        }
                        break;
                    default:
                        {
                            throw new ArgumentException("Ошибка канваса");
                        }
                }
            }
        }

        private BitmapSource canvasToBitmap(InkCanvas canv)
        {
            int resWidth = (int)(canv.DesiredSize.Width);
            int resHeight = (int)(canv.DesiredSize.Height);

            //int resWidth = (int)(canv.RenderSize.Width);
            //int resHeight = (int)(canv.RenderSize.Height);

            RenderTargetBitmap rtb = new RenderTargetBitmap(resWidth, resHeight, 96, 96, PixelFormats.Default);
            rtb.Render(canv);
            return rtb;
        }
    }
}
