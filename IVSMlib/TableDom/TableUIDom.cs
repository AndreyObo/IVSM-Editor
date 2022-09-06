using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomCore;
using IVSMlib.VsmCanvas;

namespace IVSMlib.TableDom
{
    public abstract class TableUIDom
    {
        public abstract Node CreateRootNode(TableUI table_ui);

        public abstract TableUI CreateInstanse(Node ui_node);
    }
}
