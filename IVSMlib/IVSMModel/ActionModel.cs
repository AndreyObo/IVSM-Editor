using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSMlib.Types;

namespace IVSMlib.IVSMModel
{
    public class ActionModel
    {
        public string Action { get; set; }
        public string Document { get; set; }
        public Time ActionTime { get; set; }
        public Time WaitingTime { get; set; }
        public string Comment { get; set; }
    }
}
