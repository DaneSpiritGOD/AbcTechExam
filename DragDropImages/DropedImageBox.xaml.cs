using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for DropedImageBox.xaml
    /// </summary>
    public partial class DropedImageBox : UserControl
    {
        public static readonly DependencyProperty SourceProperty;
        private double _totalAngle;

        static DropedImageBox()
        {
            SourceProperty = Image.SourceProperty.AddOwner(typeof(DropedImageBox));
        }

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        private DropedImageBox()
        {
            InitializeComponent();
        }

        internal DropedImageBox(ImageBox imageBox) : this()
        {
            Source = imageBox?.Source;
            Height = imageBox.ActualHeight;
            Width = imageBox.ActualWidth;
            _totalAngle = 0;
            reInitialTransform();
        }

        private void reInitialTransform()
        {
            RenderTransform = new RotateTransform(_totalAngle, Width / 2, Height / 2);
        }

        internal void TopUp(double height)
        {
            //Height的值不能小于0
            Height = Math.Max(0, Height + height);
            reInitialTransform();
        }

        internal void BottomDown(double height)
        {
            TopUp(height);
        }

        internal void LeftLeft(double width)
        {
            //Width的值不能小于0
            Width = Math.Max(0, Width + width);
            reInitialTransform();
        }

        internal void RightRight(double width)
        {
            LeftLeft(width);
        }

        internal void Rotate(double angle)
        {
            _totalAngle += angle;
            ((RotateTransform)RenderTransform).Angle = _totalAngle;
        }

        internal MousePoint MousePoint
        {
            get
            {
                var pt = Mouse.GetPosition(this);
                var rect = new HitRectDetail(new Rect(new Size(ActualWidth, ActualHeight)));

                if (rect.TopLeftRect.Contains(pt))
                {
                    return MousePoint.TopLeft;
                }
                if (rect.BottomRightRect.Contains(pt))
                {
                    return MousePoint.BottomRight;
                }
                if (rect.TopRightRect.Contains(pt))
                {
                    return MousePoint.TopRight;
                }
                if (rect.BottomLeftRect.Contains(pt))
                {
                    return MousePoint.BottomLeft;
                }
                if (rect.TopSideMiddleRect.Contains(pt))
                {
                    return MousePoint.TopSideMiddle;
                }
                if (rect.RightSideMiddleRect.Contains(pt))
                {
                    return MousePoint.RightSideMiddle;
                }
                if (rect.BottomSideMiddleRect.Contains(pt))
                {
                    return MousePoint.BottomSideMiddle;
                }
                if (rect.LeftSideMiddleRect.Contains(pt))
                {
                    return MousePoint.LeftSideMiddle;
                }

                if (rect.CenterRect.Contains(pt))
                {
                    return MousePoint.Center;
                }
                return MousePoint.Other;
            }
        }
    }
}
