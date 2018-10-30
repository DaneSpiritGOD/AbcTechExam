using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _dot = new FlowDot(_canvas);
        }

        private void _canvas_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataObjectFormats.ImageSource))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void _canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Handled == false)
            {
                var canvas = (Canvas)sender;
                var sourceBox = (ImageBox)e.Data.GetData(DataObjectFormats.ImageBox);

                if (canvas != default && sourceBox != default)
                {
                    var box = new DropedImageBox(sourceBox);
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

        private const int _topZIndex = 1;
        private static readonly Cursor RotateCursor = new Cursor(new MemoryStream(Properties.Resources.RotateCursor));
        private readonly FlowDot _dot;
        private DropedImageBoxSnapshot _snapshot;

        private void _canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ensureValidation(sender, e, out DropedImageBox box))
            {
                _dot.Initial();
                _snapshot = new DropedImageBoxSnapshot(box);
                maxImageBoxZIndex(box);
            }
        }

        private void maxImageBoxZIndex(DropedImageBox box)
        {
            foreach (var child in _canvas.Children)
            {
                Panel.SetZIndex((UIElement)child, 0);
            }
            Panel.SetZIndex(box, _topZIndex);
        }

        private void _canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _snapshot.IsValid)
            {
                _dot.MoveTo();
                var (deltaX, deltaY) = _dot.Displacement();
                var snapedBox = _snapshot.ClickedDropedImageBox;
                switch (_snapshot.ClickedMousePoint)
                {
                    case MousePoint.TopLeft:
                        canvasChildTopChange(snapedBox, deltaY);
                        canvasChildLeftChanged(snapedBox, deltaX);
                        break;
                    case MousePoint.TopRight:
                        snapedBox.RightRight(deltaX);
                        canvasChildTopChange(snapedBox, deltaY);
                        break;
                    case MousePoint.BottomLeft:
                        snapedBox.BottomDown(deltaY);
                        canvasChildLeftChanged(snapedBox, deltaX);
                        break;
                    case MousePoint.BottomRight:
                        snapedBox.BottomDown(deltaY);
                        snapedBox.RightRight(deltaX);
                        break;
                    case MousePoint.TopSideMiddle:
                    case MousePoint.LeftSideMiddle:
                    case MousePoint.BottomSideMiddle:
                    case MousePoint.RightSideMiddle:
                        snapedBox.Rotate(_dot.RelativeCenteredRotateAngle(calcBoxCenter(snapedBox)));
                        break;
                    case MousePoint.Center:
                        canvasChildTopChange(snapedBox, deltaY, false);
                        canvasChildLeftChanged(snapedBox, deltaX, false);
                        break;
                    case MousePoint.Other:
                        break;
                    default:
                        break;
                }
                _dot.Reset();
            }
            if (ensureValidation(sender, e, out DropedImageBox box))
            {
                switch (box.MousePoint)
                {
                    case MousePoint.TopLeft:
                    case MousePoint.BottomRight:
                        Mouse.SetCursor(Cursors.SizeNWSE);
                        break;
                    case MousePoint.TopRight:
                    case MousePoint.BottomLeft:
                        Mouse.SetCursor(Cursors.SizeNESW);
                        break;
                    case MousePoint.TopSideMiddle:
                    case MousePoint.RightSideMiddle:
                    case MousePoint.BottomSideMiddle:
                    case MousePoint.LeftSideMiddle:
                        Mouse.SetCursor(RotateCursor);
                        break;
                    case MousePoint.Center:
                        Mouse.SetCursor(Cursors.SizeAll);
                        break;
                    default:
                        break;
                }
            }
        }

        private static Point calcBoxCenter(DropedImageBox box)
        {
            return new Point(Canvas.GetLeft(box) + box.Width / 2, Canvas.GetTop(box) + box.Height / 2);
        }

        /// <summary>
        /// box上边受到移动时作出的反应
        /// </summary>
        /// <param name="box"></param>
        /// <param name="delta"></param>
        /// <param name="resize">是否改变box的大小</param>
        private static void canvasChildTopChange(DropedImageBox box, double delta, bool resize = true)
        {
            if (resize)
            {
                box.TopUp(-delta);
            }

            //只有box的高度大于0时，才会改变box的Canvas.Top
            if (box.Height > 0)
            {
                Canvas.SetTop(box, Canvas.GetTop(box) + delta);
            }
        }

        /// <summary>
        /// DropedImageBox左边受到移动时作出的反应
        /// </summary>
        /// <param name="box"></param>
        /// <param name="delta"></param>
        /// <param name="resize">决定DropedImageBox的尺寸是否改变</param>
        private static void canvasChildLeftChanged(DropedImageBox box, double delta, bool resize = true)
        {
            if (resize)
            {
                box.LeftLeft(-delta);
            }

            //判断box的宽度是否大于0，只有大于0时，box的Canvas.Left会进行扩大或缩小
            if (box.Width > 0)
            {
                Canvas.SetLeft(box, Canvas.GetLeft(box) + delta);
            }
        }

        private bool ensureValidation(object sender, RoutedEventArgs routedEventArgs, out DropedImageBox box)
        {
            var canvas = sender as Canvas;
            box = routedEventArgs.Source as DropedImageBox;
            return canvas != default && box != default;
        }

        private void _canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //使_snapshot失效
            _snapshot = default;
        }
    }
}
