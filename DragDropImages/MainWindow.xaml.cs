using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragDropImages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataObjectFormats.ImageSource))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Handled == false)
            {
                var canvas = (Canvas)sender;
                var img = (ImageSource)e.Data.GetData(DataObjectFormats.ImageSource);
                var sourceBox = (ImageBox)e.Data.GetData(DataObjectFormats.ImageBox);
                var sourceParent = (Panel)VisualTreeHelper.GetParent(sourceBox);

                if (canvas != null && img != null && sourceParent != null)
                {
                    var box = new ImageBox(img)
                    {
                        Height = sourceBox.ActualHeight,
                        Width = sourceBox.ActualWidth
                    };
                    var pt = e.GetPosition(canvas);
                    Canvas.SetTop(box, pt.Y - sourceBox.ActualHeight / 2.0);
                    Canvas.SetLeft(box, pt.X - sourceBox.ActualWidth / 2.0);
                    Panel.SetZIndex(box, _topZIndex);
                    canvas.Children.Add(box);

                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
            }
        }

        private int _topZIndex = 1;
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (Canvas)sender;
            var box = e.Source as ImageBox;
            if (canvas != default && box != default)
            {
                maxImageBoxZIndex(canvas, box);


            }
        }

        private void maxImageBoxZIndex(Canvas canvas, ImageBox box)
        {
            foreach (var child in canvas.Children)
            {
                Panel.SetZIndex((UIElement)child, 0);
            }
            Panel.SetZIndex(box, _topZIndex);
        }


    }
}
