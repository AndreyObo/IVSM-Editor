using DomCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.PropsHolders
{
    public class DropCheckListProps:Props
    {
        public List<string> Items = new List<string>();

        public delegate string GetCurrentValue();

        public GetCurrentValue GetCurrentValueDelegate;

        public delegate void SetProperty(string props);

        public SetProperty SetPropertyDelegate;

        public delegate bool GetCurrentCheck();

        public GetCurrentCheck GetCurrentCheckDelegate;

        public delegate void OnCheck(bool state);

        public OnCheck OnCheckDelegate;

        public override void GetNode(ref Node root)
        {
           
        }
    }
}
