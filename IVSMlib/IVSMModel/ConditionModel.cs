using IVSMlib.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.IVSMModel
{
    public class ConditionModel
    {
        public string Action { get; set; }
        public string Document { get; set; }
        public Time ActionTime { get; set; }
        public Time WaitingTime { get; set; }

        public string A_Condition { get; set; }
        public string B_Condition { get; set; }
        public string C_Condition { get; set; }

        public string Comment { get; set; }
    }
}
