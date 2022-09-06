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
     //   private List<VisualButton> DeleteButtons = new List<VisualButton>();
        private VisualButton SelectedButton;

    //    private bool ShowAddButtons;
    //    private bool ShowDeleteButtons;

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
        //    ShowAddButtons = false;
          //  ShowDeleteButtons = true;
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
          
            //if(ShowAddButtons)
            //{
            //    ShowAddButtons = false;
            //    Buttons.Clear();
            //    SelectedButton = null;
            //}
            //double x_offset = Loc.X;
            //Int32 index = 0;
            //foreach (double width in Owner.ColumnWidth)
            //{
            //    VisualButton del_button = new VisualButton();
            //    del_button.SetSize(width, RHeight);
            //    del_button.SetLocation(x_offset, Loc.Y);
            //    del_button.Tag = index++;
            //    del_button.SetButtonColor(Colors.Red);
            //    del_button.FocusBrush = Brushes.Yellow;
            //    Buttons.Add(del_button);
            //    x_offset += width;
            //    ShowDeleteButtons = true;
            //}
            //DrawUI();
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
            //    ShowDeleteButtons = true;
            }
        //    DrawUI();
        }

        public void ShowColumnAddButton()
        {
        //    double x_offset = Loc.X;
        ////    Int32 col_index = 0;
        //    Int32 index = 0;
        //    foreach(double width in Owner.ColumnWidth)
        //    {
        //        if (index != 0)
        //        {
        //            VisualButton c_add_left = new VisualButton();
        //            c_add_left.SetSize(width / 2, RHeight);
        //            c_add_left.SetLocation(x_offset, Loc.Y);
        //            c_add_left.Tag = new InsertBetween(index-1, index);
        //            c_add_left.SetButtonColor(Colors.LightBlue);
        //            Buttons.Add(c_add_left);
                    
        //        }
        //        x_offset += width / 2;


        //        if (index != Owner.ColumnWidth.Count-1)
        //        {
        //            VisualButton c_add_right = new VisualButton();
        //            c_add_right.SetSize(width / 2, RHeight);
        //            c_add_right.SetLocation(x_offset, Loc.Y);
        //            c_add_right.Tag = new InsertBetween(index, index+1);
        //            c_add_right.SetButtonColor(Colors.LightBlue);
        //            Buttons.Add(c_add_right);
        //            x_offset += width / 2;
        //        }
        //        index++;
        //    }
        //    ShowAddButtons = true;
        //    DrawUI();
        }

        public void HideDeleteButton()
        {
          //  ShowDeleteButtons = false;
            Buttons.Clear();
            SelectedButton = null;
            DrawUI();
        }

        public void HideColumnAddButton()
        {
           // ShowAddButtons = false;
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

           //     if (ShowAddButtons || ShowDeleteButtons)
            //    {
                    foreach (VisualButton button in Buttons)
                    {
                        button.Draw(dc);
                    }
            //   }
            }
         
            dc.Close();
        }

        public override void MouseEnter()
        {
         //   throw new NotImplementedException();
        }

        public override void MouseMove(Point e)
        {
         //   if (ShowAddButtons || ShowDeleteButtons)
          // {
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

            //if (SelectedButton != null)
            //{
            //    SelectedButton.Unselect();
            //    SelectedButton = null;

            //}
            //}
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
          //  Console.WriteLine("Down");

            for (int i = 0; i <= Buttons.Count - 1; i++)
            {
                if (Buttons[i].CheckHit(e))
                {
                    //// Console.WriteLine(((InsertBetween)Buttons[i].Tag).From.ToString() + " <-- >" + ((InsertBetween)Buttons[i].Tag).To.ToString());

                    //if (ShowDeleteButtons)
                    //{
                    //    DeleteButtonClick?.Invoke((int)Buttons[i].Tag);
                    //}
                    //if(ShowAddButtons)
                    //{
                    //    InsertButtonClick?.Invoke(((InsertBetween)Buttons[i].Tag).From, ((InsertBetween)Buttons[i].Tag).To);
                    //}
                    ColumnButtonClickEvent?.Invoke((int)Buttons[i].Tag);

                //    Console.WriteLine(((int)Buttons[i].Tag).ToString());
                }
            }
        }

        public override void MouseUp(Point e)
        {
           
        }

        public override void Select()
        {
         //   throw new NotImplementedException();
        }

        public override void Unselect()
        {
         //   throw new NotImplementedException();
        }

      
    }
}
