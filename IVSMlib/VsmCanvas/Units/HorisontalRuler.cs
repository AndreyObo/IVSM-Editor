using IVSMlib.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IVSMlib.VsmCanvas.Units
{
    class HorisontalRuler : TableUI
    {
        private Int32 RWidth = 300;
        private Int32 RHeight = 15;

        private Point Loc;

        private bool IsVisible;

        private Brush drawBrush;

        private MainTableVM Owner;
        private List<VisualButton> Buttons = new List<VisualButton>();

        private VisualButton SelectedButton;

        public delegate void InsertButton(Int32 from, Int32 to);
        public event InsertButton InsertButtonClick;

        public delegate void DeleteButton(Int32 column);
        public event DeleteButton DeleteButtonClick;

        public delegate void ColumnButtonClick(Int32 column);
        public event ColumnButtonClick ColumnButtonClickEvent;

        public HorisontalRuler(MainTableVM _owner)
        {
            IsVisible = true;
            Loc = new Point(1, 1);
            drawBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#DBDBDB");
            Owner = _owner;

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

        private struct InsertBetween
        {
            public Int32 From;
            public Int32 To;

            public InsertBetween(int from, int to)
            {
                From = from;
                To = to;
            }
        } 

        public Int32 GetHeight() => RHeight;

        public Int32 id { get; set; }

        public void SetWidth(Int32 _width)
        {
            RWidth = _width;
        }

        public void SetLocation(Point _loc)
        {
            Loc = _loc;
            
        }

        public void ShowDeleteButton()
        {
          
        }

        public void UpdateButtons()
        {
            double x_offset = Loc.X;
            Int32 index = 0;
            Buttons.Clear();
            foreach (double width in Owner.ColumnWidth)
            {
                VisualButton del_button = new VisualButton();
                del_button.SetSize(width-18, RHeight);
                del_button.SetLocation(x_offset+9, Loc.Y);
                del_button.Tag = index++;
                del_button.SetButtonColor(Colors.Transparent);
                del_button.FocusBrush = Brushes.WhiteSmoke;
                Buttons.Add(del_button);
                x_offset += width;
            }

        }

        public void ShowColumnAddButton()
        {

        }

        public void HideDeleteButton()
        {
            Buttons.Clear();
            SelectedButton = null;
            DrawUI();
        }

        public void HideColumnAddButton()
        {
            Buttons.Clear();
            SelectedButton = null;
            DrawUI();
        }

        public override void DrawUI()
        {
            
            DrawingContext dc = this.RenderOpen();

            if (IsVisible)
            {

                dc.DrawRectangle(drawBrush, new Pen(Brushes.DarkGray, 0.5), new Rect(Loc.X, Loc.Y, RWidth, RHeight));

                    foreach (VisualButton button in Buttons)
                    {
                        button.Draw(dc);
                    }
            }
         
            dc.Close();
        }

        public override void MouseEnter()
        {

        }

        public override void MouseMove(Point e)
        {
                for (int i = 0; i <= Buttons.Count - 1; i++)
                {
                    if (Buttons[i].CheckHit(e))
                    {
                        if (Buttons[i] == SelectedButton)
                        {
                            return;
                        }
                        else
                        {
                            SelectedButton?.Unselect();
                            Buttons[i].Select();
                            SelectedButton = Buttons[i];

                            return;
                        }

                    }
                }
        }

        public override void MouseLeave()
        {
            if (SelectedButton != null)
            {
                SelectedButton.Unselect();
                SelectedButton = null;

            }
            DrawUI();
        }

        public override void MouseDown(Point e)
        {

            for (int i = 0; i <= Buttons.Count - 1; i++)
            {
                if (Buttons[i].CheckHit(e))
                {
                    ColumnButtonClickEvent?.Invoke((int)Buttons[i].Tag);
                }
            }
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
