using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.PropsHolders
{
    public class PropsHolder
    {
        public enum Type {Player, Action, Condition, MoveLine, PathLine, Document, Problem, Decision, Default}

        public Type OwnerType;

        public List<Props> PropsList;

        public List<Props> VisualPropsList;

        public PropsHolder()
        {
            PropsList = new List<Props>();
            VisualPropsList = new List<Props>();
        }

        public delegate void SetLable(string lable);
        public SetLable SetLableCallback;

        public delegate string GetLable();
        public GetLable GetLableCallback;
    }
}
