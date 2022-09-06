using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using IVSMlib.VsmCanvas.LineUI;

namespace IVSMlib.VsmCanvas.CellUI
{
    public class CellMouseResult
    {
        public enum Result {LeftTopBtn, RightTopBtn, LeftBottomBtn, RightBottomBtn, CellClick }
        public Result CellResult;
        public Point e;
        public Cell sender;
        public MoveLine _MoveLine;
    }
}
