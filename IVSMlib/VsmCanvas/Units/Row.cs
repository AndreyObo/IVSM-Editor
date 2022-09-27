using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IVSMlib.VsmCanvas.Units
{
    class Row : TableUI
    {

        protected Brush DrawBrush;
        protected Boolean isActive;

        public Row PrevRow { get; set; }
        public Row NextRow { get; set; }

        public bool ShowHeader;

        private static Int32 box_width = 16;
        private static Int32 box_height = 10;

        public static Int32 Y_Offset=0;

        public static Int32 BoxHeigth() => box_height;

        public Int32 Index { get; set; }
        private double Lenght;

        public void SetRowWidht(double lenght)
        {
            Lenght = lenght;
        }

        public Point Rowlocation = new Point(0,0);

        public Row()
        {
            isActive = true;
            ShowHeader = true;
            DrawBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#565656");
        }
        public override void DrawUI()
        {
            DrawingContext dc = this.RenderOpen();
            if (ShowHeader)
            {
                dc.DrawRectangle(DrawBrush, new Pen(Brushes.Transparent, 1), new Rect(Rowlocation.X, Rowlocation.Y + Y_Offset, box_width, box_height));
            }
            dc.DrawLine(new Pen(Brushes.DarkGray, 1), new Point(Rowlocation.X + box_width, Rowlocation.Y + Y_Offset + box_height/2), new Point(Lenght+box_width, Rowlocation.Y + Y_Offset + box_height / 2));

            dc.Close();
        }

        public void SetLocation(double x, double y)
        {
            Rowlocation.Y = y - box_height / 2;
            DrawUI();
        }

        public void SetActiveMode(bool mode)
        {
            if (mode == false)
            {
                DrawBrush = Brushes.LightGray;
                isActive = false;
            }
            else
            {
                DrawBrush = Brushes.DarkGray;
                isActive = true;
            }
        }

        public Point GetPosition()
        {
            return new Point(0, Rowlocation.Y+ box_height / 2);
        }

        public override void MouseEnter()
        {
            DrawBrush = Brushes.Gray;
            DrawUI();
        }

        public override void MouseMove(Point e)
        {

        }

        public override void MouseLeave()
        {
            DrawBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#565656");
            DrawUI();
        }

        public override void MouseDown(Point e)
        {

        }

        public override void MouseUp(Point e)
        {

        }

        public override void Select()
        {

        }

        public override void Unselect()
        {

        }

       
    }
}
