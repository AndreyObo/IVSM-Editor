using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IVSMlib.VsmCanvas.Units
{
    public class VisualButton
    { 
        public string Name { get; set; }

        public object Tag { get; set; }

        private Point Location;

        private Size size;

        private Brush ButtonBrush;
        public Brush FocusBrush;
        private Brush DrawBrush;

        public void SetButtonColor(Color c)
        {
            ButtonBrush = new SolidColorBrush(c);
            DrawBrush = ButtonBrush;
        }

        public VisualButton()
        {
            Name = "DefButton";
            ButtonBrush = new SolidColorBrush(Colors.Gray);
            ButtonBrush.Opacity = 0.7;
            FocusBrush = Brushes.Gray;


            Location = new Point();
            size = new Size();

            DrawBrush = ButtonBrush;
        }

        public void SetLocation(double x, double y)
        {
            Location.X = x;
            Location.Y = y;
        }

        public void SetSize(double width, double height)
        {
            size.Width = width;
            size.Height = height;
        }

        public Size GetSize() => size;


        public bool CheckHit(Point e)
        {
            if ((e.X > Location.X) && (e.X < Location.X + size.Width) && (e.Y > Location.Y) && (e.Y < Location.Y + size.Height))
            {
                return true;
            }
            return false;
        }

        public void Select()
        {
            DrawBrush = FocusBrush;
        }

        public void Unselect()
        {
            DrawBrush = ButtonBrush;
        }
            

        public void Draw(DrawingContext dc)
        {
            dc.DrawRectangle(DrawBrush, new Pen(Brushes.Transparent, 1), new Rect(Location.X, Location.Y, size.Width, size.Height));
        }
    }
}
