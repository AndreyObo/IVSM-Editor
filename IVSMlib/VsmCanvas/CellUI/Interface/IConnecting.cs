using IVSMlib.VsmCanvas.LineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.VsmCanvas.CellUI.Interface
{
    public interface IConnecting
    {
        void ActiveConnectButton();
        void DisableConnectButton();

        List<List<Line>> GetAllLinesList();
        void DisconnectLine(Line disc_line);

        List<Line> GetAll_E_Lines();
    }
}
