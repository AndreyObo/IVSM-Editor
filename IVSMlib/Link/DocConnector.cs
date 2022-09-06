using IVSMlib.ViewModel.Units;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.Link
{
    public static class DocConnector
    {
        public static ObservableCollection<DocumentUnit> CurrentDocList { get; set; }

    }
}
