using DomCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.PropsHolders
{
    public class DateProps:Props
    {

        public string Title { get; set; }

        public delegate DateTime GetCurrentValue();

        public GetCurrentValue GetCurrentValueDelegate;

        public delegate void SetProperty(DateTime props);

        public SetProperty SetPropertyDelegate;

        public override void GetNode(ref Node root)
        {
            //List<Node.NodeFeature> feature = new List<Node.NodeFeature>();

            //feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), Name));

            //root.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), GetCurrentValueDelegate(), feature);
        }

    }
}
