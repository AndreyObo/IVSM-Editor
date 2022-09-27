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
    public abstract class Cell: TableUI
    {
        protected Size size = new Size();
        protected Point Loc = new Point();

        protected Point RelLocation = new Point();
        protected Size RelSize = new Size();

        public struct CellIndex
        {
            public Int32 Row;
            public Int32 Column;

            public CellIndex(int row, int column)
            {
                Row = row;
                Column = column;
            }
        }

        public delegate void MouseResult(Cell sender, Point e, CellMouseResult res);

        public event MouseResult MouseResultEvent;


        public CellIndex TableIndex;

        public struct Margin
        {
            public Int32 Left;
            public Int32 Top;
        }

        protected bool IsSelect;

        public Margin CellMargin = new Margin();

        protected Brush BackgoundBrush;
        protected Brush FocusedBrush;
        protected Brush SelectedBrush;

        protected Brush DrawBrush;

        public virtual void SetCellColor(Color c)
        {
            BackgoundBrush = new SolidColorBrush(c);
            DrawUI();
        }

        public virtual void SetMargin(Int32 left, Int32 top)
        {
            CellMargin.Left = left;
            CellMargin.Top = top;
            UpdateRel();
        }

        protected virtual void UpdateRel()
        {
            RelLocation.X = Loc.X + CellMargin.Left;
            RelLocation.Y = Loc.Y + CellMargin.Top;

            RelSize.Width = size.Width - 2 * CellMargin.Left;
            RelSize.Height = size.Height - 2 * CellMargin.Top;
        }


        public virtual Size GetSize()
        {
            return new Size(size.Width, size.Height);
        }

        public virtual Size GetContentSize()
        {
            return new Size(size.Width, size.Height);
        }

        public virtual Point GetLocation()
        {
            return new Point(Loc.X, Loc.Y);
        }

        public virtual void SetWidth(double _width)
        {
            size.Width = _width;
            DrawUI();
        }

        public virtual void SetHeight(double _height)
        {
            size.Height = _height;
            DrawUI();
        }

        public virtual void SetSize(double _width, double _height)
        {
            size.Width = _width;
            size.Height = _height;
            UpdateRel();
            DrawUI();
        }

        public virtual void SetLocation(double x, double y)
        {
            Loc.X = x;
            Loc.Y = y;
            UpdateRel();
            DrawUI();
        }

        public void Backgound(Color color)
        {
            BackgoundBrush = new SolidColorBrush(color);

        }


        public virtual CellMouseResult GetMouseDownResult(Point e)
        {
            CellMouseResult res = new CellMouseResult();
            res.CellResult = CellMouseResult.Result.CellClick;
            return res;
        }

       
        protected void InvokeEvent(Point e, CellMouseResult res)
        {
            MouseResultEvent.Invoke(this, e, res);
        }

    }
}
