using IVSMlib.VsmCanvas;
using IVSMlib.VsmCanvas.CellUI;
using IVSMlib.VsmCanvas.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IVSMlib.Construct.Actions
{
    public class NoneAction : IAction
    {
        private MapConstructor Constructor;

        private Int32 MouseCount;

        public NoneAction(MapConstructor owner)
        {
            Constructor = owner;
            MouseCount = 0;
        }

        public bool MouseDownAction(Point e, MouseState state)
        {
            return false;
        }

        public void MouseMoveAction(Point e)
        {
          
          if(Constructor.IsMouseDown)
          {
                MouseCount++;
                if(MouseCount > 5)
                {
                  //  Console.WriteLine("gfg");
                    MouseCount = 0;
                    Constructor.DragAndDropCellAction.OpenDrag((Cell)Constructor.SelectedItem, e);
                    Constructor.CurreantAction = MapConstructor.Actions.DragCell;
                   
                }
          }
        }

        public void MouseUpAction(Point e)
        {
           
          //  Constructor.CurreantAction = MapConstructor.Actions.None;
            //  throw new NotImplementedException();
        }
    }
}
