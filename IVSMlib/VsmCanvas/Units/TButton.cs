using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IVSMlib.VsmCanvas.Units
{
    public class TButton : TableUI
    {
        private Point Location;
        private Size size;
        private string lable;
        private Brush DrawBrush;
        private Pen DrawPen;
        private bool IsVisible;
        private bool IsImage;
        private Size ImageSize;

        private ImageSource source;

        public delegate void click(TButton sender, object arg);

        public event click Click;

        public TButton()
        {
            size.Width = 20;
            size.Height = 10;
            IsImage = false;
            Location.X = 1;
            Location.Y = 1;
            DrawBrush = Brushes.LightGray;
            DrawPen = new Pen(Brushes.Gray, 0);
            ImageSize = new Size(20, 10);
            IsVisible = true;
        }

        public void Show()
        {
            IsVisible = true;
            DrawUI();
        }
        public void Hide()
        {
            IsVisible = false;
            DrawUI();
        }

        public void SetSize(double widht, double height)
        {
            size.Width = widht;
            size.Height = height;
            DrawUI();
        }

        public void SetLocation(Point loc)
        {
            Location.X = loc.X;
            Location.Y = loc.Y;
            DrawUI();
        }

        public void SetImage(string _source, Int32 width, Int32 height)
        {
            IsImage = true;
            ImageSize.Width = width;
            ImageSize.Height = height;
            source = new BitmapImage(new Uri(_source));
            DrawUI();
        }

        public override void DrawUI()
        {
            DrawingContext dc = this.RenderOpen();

            if (IsVisible)
            {
                dc.DrawRectangle(DrawBrush, DrawPen, new Rect(Location.X, Location.Y, this.size.Width, this.size.Height));

                if(IsImage)
                {
                    if (size.Width > size.Height)
                    {
                        dc.DrawImage(source, new Rect(Location.X + this.size.Width / 2, (Location.Y+size.Height/2)-ImageSize.Height/2, 30, 30));
                    }
                    else
                    {
                        dc.DrawImage(source, new Rect((Location.X + this.size.Width / 2)-ImageSize.Width/2, Location.Y+size.Height/2, 30, 30));
                    }
                }
            }

            dc.Close();
        }

        public override void MouseDown(Point e)
        {
            Click.Invoke(this, null);
        }

        public override void MouseEnter()
        {
            DrawBrush = Brushes.Gray;
            DrawUI();
        }

        public override void MouseLeave()
        {
            DrawBrush = Brushes.LightGray;
            DrawUI();
        }

        public override void MouseMove(Point e)
        {

        }

        public override void MouseUp(Point e)
        {

        }

        public override void Select()
        {

        }

        public override void Unselect()
        {

        }
    }
}
