using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DragDropImages
{
    internal class HitRectDetail
    {
        private readonly double _otherRoiLength;
        private readonly Size _otherRoi;
        private readonly Rect _rect;
        private readonly double _centerRoiLength;

        public HitRectDetail(Rect rect, double centerRoiLength = 30, double otherRoiLength = 10)
        {
            _rect = rect;
            _centerRoiLength = centerRoiLength;
            _otherRoiLength = otherRoiLength;
            _otherRoi = new Size(_otherRoiLength, _otherRoiLength);
        }

        public Rect TopLeftRect => new Rect(_rect.TopLeft, _otherRoi);
        public Rect TopRightRect => new Rect(new Point(_rect.TopRight.X - _otherRoiLength, _rect.TopRight.Y), _otherRoi);
        public Rect BottomLeftRect => new Rect(new Point(_rect.BottomLeft.X, _rect.BottomLeft.Y - _otherRoiLength), _otherRoi);
        public Rect BottomRightRect => new Rect(new Point(_rect.BottomRight.X - _otherRoiLength, _rect.BottomRight.Y - _otherRoiLength), _otherRoi);

        public Rect TopSideMiddleRect => centerPointRect(middle(_rect.TopLeft, _rect.TopRight), _otherRoiLength);
        public Rect RightSideMiddleRect => centerPointRect(middle(_rect.TopRight, _rect.BottomRight), _otherRoiLength);
        public Rect BottomSideMiddleRect => centerPointRect(middle(_rect.BottomLeft, _rect.BottomRight), _otherRoiLength);
        public Rect LeftSideMiddleRect => centerPointRect(middle(_rect.BottomLeft, _rect.TopLeft), _otherRoiLength);
        public Rect CenterRect => centerPointRect(middle(_rect.TopLeft, _rect.BottomRight), _centerRoiLength);

        private Rect centerPointRect(Point middle, double roiLength)
        {
            return new Rect(new Point(middle.X - roiLength / 2, middle.Y - roiLength / 2), new Size(roiLength, roiLength));
        }

        private static Point middle(Point pt1, Point pt2)
        {
            return new Point((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
        }
    }
}
