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
    /// Interaction logic for ImageBox.xaml
    /// </summary>
    public partial class ImageBox : UserControl
    {
        public static readonly DependencyProperty SourceProperty;
        static ImageBox()
        {
            SourceProperty = Image.SourceProperty.AddOwner(typeof(ImageBox));
        }

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        private bool _droped = false;
        public ImageBox()
        {
            InitializeComponent();
        }

        public ImageBox(ImageSource imageSource) : this()
        {
            _droped = true;
            Source = imageSource ?? throw new ArgumentNullException(nameof(imageSource));
        }

        private const int CornerWidth = 10;
        private const int CornerHeight = 10;
        private static readonly Size CornerSize = new Size(CornerWidth, CornerHeight);
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_droped)
            {
                var pt = Mouse.GetPosition(this);
                var rect = new Rect(new Size(ActualWidth, ActualHeight));
                var topLeftRect = new Rect(rect.TopLeft, CornerSize);
                var topRightRect = new Rect(new Point(rect.TopRight.X - CornerWidth, rect.TopRight.Y), CornerSize);
                var bottomLeftRect = new Rect(new Point(rect.BottomLeft.X, rect.BottomLeft.Y - CornerHeight), CornerSize);
                var bottomRightRect = new Rect(new Point(rect.BottomRight.X - CornerWidth, rect.BottomRight.Y - CornerHeight), CornerSize);
                bool cornerSizemark = false;
                if (topLeftRect.Contains(pt) || bottomRightRect.Contains(pt))
                {
                    Mouse.SetCursor(Cursors.SizeNWSE);
                    cornerSizemark = true;
                }
                else if (topRightRect.Contains(pt) || bottomLeftRect.Contains(pt))
                {
                    Mouse.SetCursor(Cursors.SizeNESW);
                    cornerSizemark = true;
                }
                if (cornerSizemark && e.LeftButton == MouseButtonState.Pressed)
                {
                }
                return;
            }
            else
            {
                Mouse.SetCursor(Cursors.SizeAll);
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DataObject data = new DataObject();
                    data.SetData(DataObjectFormats.ImageBox, this);
                    data.SetData(DataObjectFormats.ImageSource, Source);

                    DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
                }
            }
        }
    }
}
