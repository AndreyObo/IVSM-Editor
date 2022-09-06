using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSMlib.Types;

namespace IVSMlib.VsmCanvas.CellUI.Interface
{
    public interface ITime
    {
        Time IGetActionTime();
        Time IGetWasteTime();
    }
}
