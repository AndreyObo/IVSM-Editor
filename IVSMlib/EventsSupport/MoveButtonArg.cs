using IVSMlib.VsmCanvas.CellUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using IVSMlib.VsmCanvas.LineUI;
using IVSMlib.VsmCanvas.CellUI;

namespace IVSMlib.EventsSupport
{
    public class MoveButtonArg
    {
        public MoveLineButton.Bindex ButtinIndex;

        public MoveLineButton.AddLine AddLineHandler;

        public MoveLineButton.GetStartPoint GetStartPointHandler;

        public Point StartPoint;
    }
}
