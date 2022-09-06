using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IVSMlib.VsmCanvas.Units
{
    public class Column : TableUI
    {
        protected Brush DrawBrush;
        protected Boolean isActive;

        public Column PrevColumn { get; set; }

        public Column NextColumn { get; set; }

        public bool ShowHeader;

        private static Int32 box_width=10;
        private static Int32 box_height=16;

        public static Int32 X_Offset = 0;

        public static Int32 Y_offset = 0;

        public static Int32 BoxWidth() => box_width;

        private double lenght;

        public void SetColumnWidth(double width)
        {
            lenght = width;
        }

        public Int32 GetXPos() => (int)Columnlocation.X + box_width / 2;

        public Int32 Index { get; set; }

        public Point Columnlocation = new Point(0,0);

        public Column()
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
                dc.DrawRectangle(DrawBrush, new Pen(Brushes.Transparent, 1), new Rect(Columnlocation.X + X_Offset, Columnlocation.Y + Y_offset, box_width, box_height));
            }

            dc.DrawLine(new Pen(Brushes.DarkGray, 1), new Point(Columnlocation.X + X_Offset + box_width/2, Columnlocation.Y+Y_offset + box_height), new Point(Columnlocation.X + X_Offset + box_width/2, lenght+Y_offset+box_height));

            dc.Close();
        }

        public void SetLocation(double x, double y)
        {
            Columnlocation.X = x - box_width / 2;
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
            return new Point(Columnlocation.X+ box_width / 2, 0);
        }

        public override void MouseEnter()
        {
            DrawBrush = Brushes.Gray;
            DrawUI();
        }

        public override void MouseMove(Point e)
        {
           // throw new NotImplementedException();
        }

        public override void MouseLeave()
        {
            DrawBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#565656");
            DrawUI();
        }

        public override void MouseDown(Point e)
        {
        //    throw new NotImplementedException();
        }

        public override void MouseUp(Point e)
        {
         //   throw new NotImplementedException();
        }

        public override void Select()
        {
        //    throw new NotImplementedException();
        }

        public override void Unselect()
        {
           // throw new NotImplementedException();
        }

    }
}
