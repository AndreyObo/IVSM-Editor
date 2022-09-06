using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IVSMlib.VsmCanvas.Types;

namespace IVSMlib.ViewModel
{
    public interface IAction
    {
        void MouseDownAction(Point e, MouseState state);
        void MouseMoveAction(Point e);
        void MouseUpAction(Point e);
    }
}
