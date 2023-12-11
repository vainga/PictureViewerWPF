using PictureViewer.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace PictureViewer.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для PaintWindow.xaml
    /// </summary>
    public partial class PaintWindow : Window
    {
        private Color selectedColor;
        public PaintWindow()
        {
            InitializeComponent();
        }
        public PaintProcessor GetPaint()
        {
            PaintProcessor processor = new PaintProcessor();
            processor.size = Convert.ToDouble(paintSize.Text);
            processor.color = selectedColor;
            return processor;
        }
        private void ColorCanvas_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            if (e.NewValue.HasValue)
            {

                selectedColor = e.NewValue.Value;
            }
        }
        private void Done_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

    }
}
