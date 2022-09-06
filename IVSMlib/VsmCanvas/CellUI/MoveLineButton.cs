using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSMlib.VsmCanvas.Units;
using IVSMlib.VsmCanvas.LineUI;
using System.Windows;

namespace IVSMlib.VsmCanvas.CellUI
{
    public class MoveLineButton:VisualButton
    {
        public enum Side {W,N,E,S }
        public struct Bindex
        {
            public Side Side;
            public Int32 Pos;

            public Bindex(Side side, int pos)
            {
                Side = side;
                Pos = pos;
            }
        }

        public Bindex ButtonIndex;

        public enum LineDirect {Start, End}
        public LineDirect StartLineDirect;
        public List<Line> LineList { get; set; }

        public delegate void AddLine(Line line);
        public AddLine AddLineDelegate;

        public delegate Point GetStartPoint();
        public GetStartPoint GetStartPointDelegate;
    }
}
