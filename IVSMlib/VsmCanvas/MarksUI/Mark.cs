using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IVSMlib.VsmCanvas.MarksUI
{
    public abstract class Mark : TableUI
    {

        protected Point Location;

        protected Size Size;

        public abstract void SetLocation(double x, double y);

        public abstract void Show();

        public abstract void Hide();

        public abstract Point GetLocation();

        
    }
}
