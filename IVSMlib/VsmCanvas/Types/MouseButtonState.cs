using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IVSMlib.VsmCanvas.Types
{
    public struct MouseState
    {
        public MouseButtonState LeftButton;
        public MouseButtonState RightButton;

        public MouseState(MouseButtonState leftButton, MouseButtonState rightButton)
        {
            LeftButton = leftButton;
            RightButton = rightButton;
        }
    }
}
