using IVSMlib.PropsHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IVSMlib.ViewModel.ControlBar.Items
{
    public abstract class Item
    {
        protected Point Location;
        protected Size ItemSize;

        protected static double SideMargin = 10;

        public Item()
        {
            Location = new Point(1, 1);
            ItemSize = new Size(50, 20);
        }

        public abstract IEnumerable<UIElement> GetUIElement();

        public abstract double GetHeight();

        public abstract void SetTop(double top_y);

        public abstract void SetWidth(double widht);

        public abstract void BindingProps(Props props);
    }
}
