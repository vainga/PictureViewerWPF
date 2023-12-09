using Microsoft.Win32;
using PictureViewer.MVVM.Model;
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

        private ObservableCollection<TextLabel> textLabels = new ObservableCollection<TextLabel>();
        private TextLabel selectedTextLabel;
        private bool isDragging = false;
        private Point startPosition;
        private ImageProcessor imageProcessor = new ImageProcessor();

        //public Image Crop(int x, int y, int width, int height) {}

        private void loadImageButton_Click(object sender, RoutedEventArgs e)
        {
            imageProcessor.LoadImage(workingImage);
        }

        private void preCropImageButton_Click(object sender, RoutedEventArgs e)
        {
            cropImageButton.Visibility = Visibility.Visible;
            cancelCropImageButton.Visibility = Visibility.Visible;
            cropRectOut.Rect = new Rect(0,0, (int)workingImage.ActualWidth, (int)workingImage.ActualHeight);
            cropRectIn.Rect = new Rect((int)(0.2 * workingImage.ActualWidth), (int)(0.2 * workingImage.ActualHeight), (int)(0.4 * workingImage.ActualWidth), (int)(0.4 * workingImage.ActualHeight));

        }

        private void cropImageButton_Click(object sender, RoutedEventArgs e)
        {
            imageProcessor.CropImage(workingImage);

            cropRectOut.Rect = new Rect();
            cropRectIn.Rect = new Rect();
            cropImageButton.Visibility = Visibility.Hidden;
            cancelCropImageButton.Visibility = Visibility.Hidden;
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
            imageProcessor.turnCWImage(workingImage);
        }

        private void turnCCWImageButton_Click(object sender, RoutedEventArgs e)
        {
            imageProcessor.turnCCWImage(workingImage);
        }


        private void saveImageButton_Click(object sender, RoutedEventArgs e)
        {
            imageProcessor.saveImage(workingImage);
        }

        private void upscaleButton_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void downscaleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void collageOnButton_Checked(object sender, RoutedEventArgs e)
        {
            canvas.EditingMode = InkCanvasEditingMode.Select;
            CollageMode.Visibility = Visibility.Visible;
        }

        private void collageOnButton_Unchecked(object sender, RoutedEventArgs e)
        {
            canvas.EditingMode = InkCanvasEditingMode.None;
            CollageMode.Visibility = Visibility.Hidden;
        }

        private void addImageButton_Click(object sender, RoutedEventArgs e)
        {
            /*Здесь будет функция, добавляющая на canvas другие изображения*/
        }


    }
}
