using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using IVSMlib.IVSMModel;

namespace IVSMlib.VsmCanvas.LineUI
{
    public class MoveLine : Line
    {
        private Point StartPoint;
        private Point EndPoint;

        private Pen LinePen;

        private Brush DrawBrush;
        private Brush SelectBrush;

        public override void SetStartPoint(double x, double y)
        {
            StartPoint.X = x;
            StartPoint.Y = y;
        }

        public override void SetEndPoint(double x, double y)
        {
            EndPoint.X = x;
            EndPoint.Y = y;
        }

        public MoveLine()
        {
            StartPoint = new Point(0, 0);
            EndPoint = new Point(0, 0);
            LinePen = new Pen(Brushes.Black, 3);

            LinePen.DashStyle = DashStyles.Dash;

            DrawBrush = Brushes.Black;
            SelectBrush = Brushes.Red;
        }

        public override void DrawUI()
        {
            DrawingContext dc = this.RenderOpen();

            dc.DrawLine(LinePen, StartPoint, EndPoint);

            dc.Close();
        }

        public override void Select()
        {
            LinePen.Brush = SelectBrush;
        }

        public override void Unselect()
        {
            LinePen.Brush = DrawBrush;
        }

        public override void MouseEnter()
        {
            //--
        }

        public override void MouseMove(Point e)
        {

        }

        public override void MouseLeave()
        {

        }

        public override void MouseDown(Point e)
        {

        }

        public override void MouseUp(Point e)
        {

        }

        public override MoveModel GetModel()
        {
            throw new NotImplementedException();
        }
    }
}
