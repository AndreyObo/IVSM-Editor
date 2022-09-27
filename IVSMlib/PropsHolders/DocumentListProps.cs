using DomCore;
using IVSMlib.ViewModel.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.PropsHolders
{
    public class DocumentListProps:Props
    {
        public string Title { get; set; }

        public delegate List<DocumentUnit> GetCurrentValue();

        public GetCurrentValue GetCurrentValueDelegate;

        public delegate void AddDocument(DocumentUnit doc);

        public AddDocument AddDocumentCallback;

        public delegate void DeleteDocument(DocumentUnit doc);

        public DeleteDocument DeleteDocumentCallback;

        public override void GetNode(ref Node root)
        {
        }
    }
}
