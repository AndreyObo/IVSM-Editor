using IVSMlib.VsmCanvas.CellUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSMlib.IVSMModel;

namespace IVSMlib.VsmCanvas.LineUI
{
    public abstract class Line:TableUI
    {
        public Int32 Id;

        public Cell Left_Cell;
        public Cell Right_Cell;

        public abstract void SetStartPoint(double x, double y);
        public abstract void SetEndPoint(double x, double y);

        public abstract MoveModel GetModel();
    }
}
