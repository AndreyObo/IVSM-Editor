using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomCore;
using IVSMlib.Types;

namespace IVSMlib.PropsHolders
{
    public class TimeProps:Props
    {
        public string Title { get; set; }

        public delegate Time GetCurrentValue();

        public GetCurrentValue GetCurrentValueDelegate;

        public delegate void SetProperty(Time props);

        public SetProperty SetPropertyDelegate;

        public override void GetNode(ref Node root)
        {
            //List<Node.NodeFeature> feature = new List<Node.NodeFeature>();

            //feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), Name));

            //root.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), GetCurrentValueDelegate(), feature);
        }
    }
}
