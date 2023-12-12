using Microsoft.Win32;
using PictureViewer.Interfaces;
using PictureViewer.MVVM.Model;
using PictureViewer.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using static System.Net.Mime.MediaTypeNames;

namespace PictureViewer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        IImageData imageData = new ImageData();
        private IImageProcessor imageProcessor = new ImageProcessor();
        private IPaintProcessor paintProcessor = new PaintProcessor();
        private ITextProcessor textProcessor = new TextProcessor();
        private IImageCanvas canvasProcessor = new ImageCanvas();
        private bool isPainting = false;
        private bool isCanvas = false;

        private void loadImageButton_Click(object sender, RoutedEventArgs e)
        {
            if(imageProcessor.GIGAImage != null)
            {
                imageProcessor.GIGAImage.Source = imageData.LoadImage(workingImage).Source;
            }
            else
            {
                throw new ArgumentException("Ошибка открытия изображения!");
            }
        }

        private void addTextButton_Click(Object sender, RoutedEventArgs e)
        {
            textProcessor.AddTextToImage(workingImage);
        }
        private void preCropImageButton_Click(object sender, RoutedEventArgs e)
        {
            cropImageButton.Visibility = Visibility.Visible;
            cancelCropImageButton.Visibility = Visibility.Visible;
            cropRectOut.Rect = new Rect(0,0, (int)workingImage.ActualWidth, (int)workingImage.ActualHeight);
            cropRectIn.Rect = new Rect((int)(0.2 * workingImage.ActualWidth), (int)(0.2 * workingImage.ActualHeight), (int)(0.4 * workingImage.ActualWidth), (int)(0.4 * workingImage.ActualHeight));

        }
        

        private void cropImageButton_Click(object sender, RoutedEventArgs e)
        {   if (isCanvas)
            {
                imageProcessor.GIGAImage.Source = imageProcessor.CropImage(workingImage).Source;
                cropRectOut.Rect = new Rect();
                cropRectIn.Rect = new Rect();
                cropImageButton.Visibility = Visibility.Hidden;
                cancelCropImageButton.Visibility = Visibility.Hidden;
            }
            else
            {
                cropImageButton.Visibility = Visibility.Hidden;
                cancelCropImageButton.Visibility = Visibility.Hidden;

                Image cropped = new Image();
                cropped.Width = cropRectIn.Rect.Width;
                cropped.Height = cropRectIn.Rect.Height;

                CroppedBitmap cb = new CroppedBitmap(canvasProcessor.canvasToBitmap(canvas), new Int32Rect((int)cropRectIn.Rect.Left, (int)cropRectIn.Rect.Top, (int)cropped.Width, (int)cropped.Height));
                cropped.Source = cb;
                canvas.Children.Clear();
                canvas.Children.Add(cropped);
                canvas.Width = cropped.Width;
                canvas.Height = cropped.Height;

                cropRectOut.Rect = new Rect();
                cropRectIn.Rect = new Rect();
            }
        }

        private void cancelCropImageButton_Click(object sender, RoutedEventArgs e)
        {
            cropRectOut.Rect = new Rect();
            cropRectIn.Rect = new Rect();

            cropImageButton.Visibility = Visibility.Hidden;
            cancelCropImageButton.Visibility = Visibility.Hidden;
        }

        private void turnCWImageButton_Click(object sender, RoutedEventArgs e)
        {
            imageProcessor.GIGAImage.Source = imageProcessor.turnCWImage(workingImage).Source;
        }

        private void turnCCWImageButton_Click(object sender, RoutedEventArgs e)
        {
            imageProcessor.GIGAImage.Source = imageProcessor.turnCCWImage(workingImage).Source;
        }


        private void saveImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (isCanvas)
                canvasProcessor.saveCanvas(canvas);
            else
                imageData.saveImage(workingImage);
        }

        private void brightness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(imageProcessor.GIGAImage != null)
            {
                workingImage.Source = imageProcessor.GIGAImage.Source;
            }
           imageProcessor.AdjustBrightness(workingImage, sliderBrightness.Value);
        }

        private void R_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (imageProcessor.GIGAImage != null)
            {
                workingImage.Source = imageProcessor.GIGAImage.Source;
            }
            imageProcessor.AdjustRedColor(workingImage, sliderR.Value);
        }

        private void G_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (imageProcessor.GIGAImage != null)
            {
                workingImage.Source = imageProcessor.GIGAImage.Source;
            }
            imageProcessor.AdjustGreenColor(workingImage, sliderG.Value);
        }

        private void B_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (imageProcessor.GIGAImage != null)
            {
                workingImage.Source = imageProcessor.GIGAImage.Source;
            }
            imageProcessor.AdjustBlueColor(workingImage, sliderB.Value);
        }

        private void A_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (imageProcessor.GIGAImage != null)
            {
                workingImage.Source = imageProcessor.GIGAImage.Source;
            }
            imageProcessor.AdjustOpacity(workingImage, sliderA.Value);
        }

        private void mouseButton_Click(object sender, RoutedEventArgs e)
        {
            isPainting = false;
        }

        private void paintButton_Click(object sender, RoutedEventArgs e)
        {
            isPainting = true;
            PaintWindow paintWindow = new PaintWindow();
            bool? result = paintWindow.ShowDialog();
            if (result == true)
            {
                paintProcessor = paintWindow.GetPaint();
            }
        }

        private void upscaleButton_Click(object sender, RoutedEventArgs e)
        {
            scaleTransform.ScaleX *= 1.5;
            scaleTransform.ScaleY *= 1.5;
        }

        private void downscaleButton_Click(object sender, RoutedEventArgs e)
        {
            scaleTransform.ScaleX /= 1.5;
            scaleTransform.ScaleY /= 1.5;
        }

        private void collageOnButton_Checked(object sender, RoutedEventArgs e)
        {
            canvas.EditingMode = InkCanvasEditingMode.Select;
            CollageMode.Visibility = Visibility.Visible;
            isCanvas = true;
        }

        private void collageOnButton_Unchecked(object sender, RoutedEventArgs e)
        {
            canvas.EditingMode = InkCanvasEditingMode.None;
            CollageMode.Visibility = Visibility.Hidden;
            isCanvas = false;
        }

        private void addImageButton_Click(object sender, RoutedEventArgs e)
        {
            Image newImage = new Image();
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Выбор изображения";
            op.Filter = "Все файлы изображений|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                newImage.Source = new BitmapImage(new Uri(op.FileName));
            }
            canvas.Children.Add(newImage);
        }

        private void ResultImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPainting && e.LeftButton == MouseButtonState.Pressed)
            { 
                Image resultImage = (Image)sender;
                Point currentPosition = e.GetPosition(resultImage);
                paintProcessor.DrawOnImage(workingImage, currentPosition);
            }
        }


    }
}
