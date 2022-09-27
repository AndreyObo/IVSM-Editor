using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomCore;
using IVSMlib.TableDom;

namespace IVSMlib.PropsHolders
{
    public class StringProps:Props
    {
        public enum EditType { Line, MultiLine}
        public EditType PropsEditType;

        
        public string Title { get; set; }

        public delegate string GetCurrentValue();

        public GetCurrentValue GetCurrentValueDelegate;

        public delegate void SetProperty(string props);

        public SetProperty SetPropertyDelegate;

        public delegate void SetChangedValue(string props);
        public SetChangedValue SetChangedValueDelegate;

        public override void GetNode(ref Node root)
        {
            List<Node.NodeFeature> feature = new List<Node.NodeFeature>();

            feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), Name));

            root.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), GetCurrentValueDelegate(), feature);
        }
    }
}
