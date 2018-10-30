using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DragDropImages
{
    public partial class MainWindow
    {
        private class FlowDot
        {
            private readonly Canvas _canvas;
            private Point _lastClickedPosition;
            private Point _currentClickPosition;

            public FlowDot(Canvas canvas)
            {
                _canvas = canvas;
            }

            public void Initial()
            {
                _lastClickedPosition = Mouse.GetPosition(_canvas);
            }

            public void MoveTo()
            {
                _currentClickPosition = Mouse.GetPosition(_canvas);
            }

            public (double DeltaX, double DeltaY) Displacement()
            {
                return (_currentClickPosition.X - _lastClickedPosition.X, _currentClickPosition.Y - _lastClickedPosition.Y);
            }

            public void Reset()
            {
                _lastClickedPosition = _currentClickPosition;
            }

            public double RelativeCenteredRotateAngle(Point absoluteCenterPoint)
            {
                var vec1 = point2Vector(_lastClickedPosition, absoluteCenterPoint);
                var vec2 = point2Vector(_currentClickPosition, absoluteCenterPoint);
                return Vector.AngleBetween(vec1, vec2);
            }

            private static Vector point2Vector(Point from, Point to)
            {
                return new Vector(to.X - from.X, to.Y - from.Y);
            }
        }
    }
}
