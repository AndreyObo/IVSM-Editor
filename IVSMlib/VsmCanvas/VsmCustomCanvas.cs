using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IVSMlib.VsmCanvas
{
    public class VsmCustomCanvas:Canvas
    {
        //public VsmCustomCanvas()
        //{
        //    Console.WriteLine("Canvas is created!!!!!!!!!!!!");
        //}


        private List<DrawingVisual> visuals = new List<DrawingVisual>();

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }
        protected override int VisualChildrenCount
        {
            get
            {
                return visuals.Count;
            }
        }

        public void AddVisual(DrawingVisual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        public void DeleteVisual(DrawingVisual visual)
        {
            visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        public void AddVisualToBegin(DrawingVisual visual)
        {
            visuals.Insert(1,visual);

            base.AddVisualChild(visual); 
            base.AddLogicalChild(visual);
        }

        public DrawingVisual GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            if (hitResult != null)
            {
                return hitResult.VisualHit as DrawingVisual;
            }
            return null;
        }
    }
}
