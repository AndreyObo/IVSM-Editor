using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IVSMlib.VsmCanvas.CellUI
{
    public class EmptyCell : Cell
    {

        public delegate void MouseDownType(Cell sender);
        public event MouseDownType MouseDownEvent;

        private bool IsDisable;

        public EmptyCell()
        {
            Loc.X = 0;
            Loc.Y = 0;
            size.Width = 50;
            size.Height = 50;
            BackgoundBrush = new SolidColorBrush(Colors.White);
            BackgoundBrush.Opacity = 0.5;

            FocusedBrush = new SolidColorBrush(Colors.LightGray);
            FocusedBrush.Opacity = 0.8;

            DrawBrush = BackgoundBrush;
            // DrawUI();
            IsDisable = true;
        }

        public override void DrawUI()
        {
            DrawingContext dc = this.RenderOpen();

            dc.DrawRectangle(DrawBrush, new Pen(Brushes.Transparent, 1), new Rect(Loc.X, Loc.Y, this.size.Width, this.size.Height));

            dc.Close();
        }

        public override void Select()
        {
            if (!IsDisable)
            {
                DrawBrush = SelectedBrush;
                base.IsSelect = true;
            }
        }

        public override void Unselect()
        {
            if (!IsDisable)
            {
                DrawBrush = BackgoundBrush;
                base.IsSelect = false;
            }
        }

        public override void MouseEnter()
        {
            if (!IsDisable)
            {
                DrawBrush = FocusedBrush;
                DrawUI();
            }
        }

        public override void MouseMove(Point e)
        {
        }

        public override void MouseLeave()
        {
            if (!IsDisable)
            {
                DrawBrush = BackgoundBrush;
                DrawUI();
            }
        }

        public override void MouseDown(Point e)
        {
            if (!IsDisable)
            {
                MouseDownEvent.Invoke(this);
            }
        }

        public override void MouseUp(Point e)
        {
           
        }

        public void Disable()
        {
            IsDisable = true;
        }

        public void Enable()
        {
            IsDisable = false;
        }

        public override void SetLocation(double x, double y)
        {
            Loc.X = x;
            Loc.Y = y;
            UpdateRel();
            DrawUI();
        }
    }
}
