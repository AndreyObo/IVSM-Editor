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
        }
    }
}
