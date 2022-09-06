using IVSMlib.ViewModel.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.VsmCanvas.CellUI.Interface
{
    public interface IDocument
    {
        void AddDocumentToList(DocumentUnit doc);
        void DelDocumentFromList(DocumentUnit doc);
        bool HaveThis(DocumentUnit doc);
    }
}
