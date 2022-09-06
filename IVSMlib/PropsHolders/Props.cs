using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomCore;

namespace IVSMlib.PropsHolders
{
    public abstract class Props
    {
        protected string Title;

        public string Name;

        public void SetTitle(string title)
        {
            Title = title;
        }

        public string GetTitle() => Title;

        public abstract void GetNode(ref Node root);
    }
}
