using IVSMlib.VsmCanvas.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IVSMlib.Construct
{
    public interface IAction
    {
        bool MouseDownAction(Point e, MouseState state);
        void MouseMoveAction(Point e);
        void MouseUpAction(Point e);
    }
}
