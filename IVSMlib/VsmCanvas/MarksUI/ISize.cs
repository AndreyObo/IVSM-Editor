using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.VsmCanvas.MarksUI
{
    public interface ISize
    {
        bool IsSetSizeMode();
        void SetSize(double width_inc, double height_inc);
    }
}
