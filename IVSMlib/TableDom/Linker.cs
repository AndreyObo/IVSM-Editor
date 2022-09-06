using IVSMlib.VsmCanvas.CellUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVSMlib.VsmCanvas.CellUI.Interface;

namespace IVSMlib.TableDom
{
    public static class Linker
    {
        public enum ActionLinkType {LTop, LMiddle, LBottom, RTop, RMiddle, RBottom }
        public enum ConditionLinkType { Left, Bottom, Right, Top }

        public struct ActionLink
        {
            public ActionCell cell;
            public ActionLinkType type;
            public Int32 line_id;

            public ActionLink(ActionCell cell, ActionLinkType type, int line_id)
            {
                this.cell = cell;
                this.type = type;
                this.line_id = line_id;
            }
        }

        public struct ConditionLink
        {
            public ConditionCell cell;
            public ConditionLinkType type;
            public Int32 line_id;

            public ConditionLink(ConditionCell cell, ConditionLinkType type, int line_id)
            {
                this.cell = cell;
                this.type = type;
                this.line_id = line_id;
            }
        }

        public struct DocumentLink
        {
            public IDocument cell;
            public Int32 doc_id;

            public DocumentLink(IDocument cell, int doc_id)
            {
                this.cell = cell;
                this.doc_id = doc_id;
            }
        }
    }
}
