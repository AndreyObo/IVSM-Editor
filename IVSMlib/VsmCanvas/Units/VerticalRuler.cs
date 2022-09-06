using IVSMlib.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace IVSMlib.VsmCanvas.Units
{
    class VerticalRuler : TableUI
    {
        private Int32 RWidth = 15;
        private Int32 RHeight = 300;

        private Point Loc;

        private Brush drawBrush;

        private bool IsVisible;

        private MainTableVM Owner;
        private List<VisualButton> Buttons = new List<VisualButton>();
        private VisualButton SelectedButton;

       // private bool ShowAddButtons;
  //      private bool ShowDeleteButtons;

        public delegate void InsertButton(Int32 from, Int32 to);
        public event InsertButton InsertButtonClick;

        public delegate void DeleteButton(Int32 column);
        public event DeleteButton DeleteButtonClick;

        public delegate void RowButtonClick(Int32 column);
        public event RowButtonClick RowButtonClickEvent;

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

        public VerticalRuler(MainTableVM _owner)
        {
            IsVisible = true;
            Owner = _owner;
            Loc = new Point(1, 1);
            drawBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#DBDBDB");
   //         ShowAddButtons = false;
           // ShowDeleteButtons = false;
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

        public Int32 GetWidth() => RWidth;

        public void SetHeigth(Int32 _height) => RHeight = _height;

        public Int32 id { get; set; }

        public void SetLocation(Point _loc)
        {
            Loc = _loc;

        }

        public void ShowDeleteButton()
        {
            //if (ShowAddButtons)
            //{
            //    ShowAddButtons = false;
            //    Buttons.Clear();
            //    SelectedButton = null;
            //}
            //double y_offset = Loc.Y;
            //Int32 index = 0;
            //foreach (double height in Owner.RowHeight)
            //{
            //    VisualButton del_button = new VisualButton();
            //    del_button.SetSize(RWidth, height);
            //    del_button.SetLocation(Loc.X, y_offset);
            //    del_button.Tag = index++;
            //    del_button.SetButtonColor(Colors.Red);
            //    del_button.FocusBrush = Brushes.Yellow;
            //    Buttons.Add(del_button);
            //    y_offset += height;
            //    ShowDeleteButtons = true;
            //}
            //DrawUI();
        }

        public void UpdateButtons()
        {
            Buttons.Clear();
            double y_offset = Loc.Y;
            Int32 index = 0;
            foreach (double height in Owner.RowHeight)
            {
                VisualButton del_button = new VisualButton();
                del_button.SetSize(RWidth, height-18);
                del_button.SetLocation(Loc.X, y_offset+9);
                del_button.Tag = index++;
                del_button.SetButtonColor(Colors.Transparent);
                del_button.FocusBrush = Brushes.WhiteSmoke;
                Buttons.Add(del_button);
                y_offset += height;
           //     ShowDeleteButtons = true;
            }
        }

        public void HideDeleteButton()
        {
           // ShowDeleteButtons = false;
         //   Buttons.Clear();
          //  SelectedButton = null;
         //   DrawUI();
        }

        public void ShowRowAddButton()
        {
            //double y_offset = Loc.Y;
            ////    Int32 col_index = 0;
            //Int32 index = 0;
            //foreach (double height in Owner.RowHeight)
            //{
            //    if (index != 0)
            //    {
            //        VisualButton c_add_top = new VisualButton();
            //        c_add_top.SetSize(RWidth, height / 2);
            //        c_add_top.SetLocation(Loc.X, y_offset);
            //        c_add_top.Tag = new InsertBetween(index - 1, index);
            //        c_add_top.SetButtonColor(Colors.LightBlue);
            //        Buttons.Add(c_add_top);

            //    }
            //    y_offset += height / 2;


            //    if (index != Owner.RowHeight.Count - 1)
            //    {
            //        VisualButton c_add_bottom = new VisualButton();
            //        c_add_bottom.SetSize(RWidth, height / 2);
            //        c_add_bottom.SetLocation(Loc.X, y_offset);
            //        c_add_bottom.Tag = new InsertBetween(index, index + 1);
            //        c_add_bottom.SetButtonColor(Colors.LightBlue);
            //        Buttons.Add(c_add_bottom);
            //        y_offset += height / 2;
            //    }
            //    index++;
            //}
            //ShowAddButtons = true;
            //DrawUI();
        }

        public void HideRowAddButton()
        {
            //ShowAddButtons = false;
            //Buttons.Clear();
            //SelectedButton = null;
            //DrawUI();
        }


        public override void DrawUI()
        {

            DrawingContext dc = this.RenderOpen();

            if (IsVisible)
            {
                dc.DrawRectangle(drawBrush, new Pen(Brushes.DarkGray, 0.5), new Rect(Loc.X, Loc.Y, RWidth, RHeight));

  //              if (ShowAddButtons || ShowDeleteButtons)
               // {
                    foreach (VisualButton button in Buttons)
                    {
                        button.Draw(dc);
                    }
             //   }
            }
            dc.Close();
        }


        //public override void DrawUI()
        //{

        //    DrawingContext dc = this.RenderOpen();

        //    dc.DrawRectangle(drawBrush, new Pen(Brushes.DarkGray, 0.5), new Rect(Loc.X, Loc.Y, RWidth, RHeight));

        //    if (ShowButtons)
        //    {
        //        foreach (VisualButton button in Buttons)
        //        {
        //            button.Draw(dc);
        //        }
        //    }

        //    dc.Close();
        //}

        public override void MouseEnter()
        {
            //   throw new NotImplementedException();
        }

        public override void MouseMove(Point e)
        {
        //    if (ShowAddButtons || ShowDeleteButtons)
          //  {
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

                if (SelectedButton != null)
                {
                    SelectedButton.Unselect();
                    SelectedButton = null;

                }
          //  }
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
                    RowButtonClickEvent?.Invoke((int)Buttons[i].Tag);
                }
                //    if (ShowDeleteButtons)
                //    {
                //        DeleteButtonClick?.Invoke((int)Buttons[i].Tag);
                //    }
                //    if (ShowAddButtons)
                //    {
                //        InsertButtonClick?.Invoke(((InsertBetween)Buttons[i].Tag).From, ((InsertBetween)Buttons[i].Tag).To);
                //    }


            }
           // }
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
