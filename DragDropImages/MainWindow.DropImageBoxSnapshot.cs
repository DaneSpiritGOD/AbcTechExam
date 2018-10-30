using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropImages
{
    public partial class MainWindow
    {
        private struct DropedImageBoxSnapshot
        {
            public DropedImageBox ClickedDropedImageBox { get; set; }
            public MousePoint ClickedMousePoint { get; set; }

            public DropedImageBoxSnapshot(DropedImageBox dropedImageBox)
            {
                ClickedDropedImageBox = dropedImageBox;
                ClickedMousePoint = dropedImageBox.MousePoint;
            }

            public bool IsValid => ClickedDropedImageBox != default;
        }
    }
}
