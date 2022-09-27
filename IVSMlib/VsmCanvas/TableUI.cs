using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IVSMlib.VsmCanvas
{
    public abstract class TableUI : DrawingVisual
    {

       public abstract void MouseEnter();
       public abstract void MouseMove(Point e);
       public abstract void MouseLeave();

       public abstract void MouseDown(Point e);
       public abstract void MouseUp(Point e);

       public abstract void Select();
       public abstract void Unselect();

       public abstract void DrawUI();

    }
}
