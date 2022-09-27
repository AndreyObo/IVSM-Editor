using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using DomCore;
using IVSMlib.IVSMModel;
using IVSMlib.PropsHolders;
using IVSMlib.VsmCanvas.CellUI.Interface;
using IVSMlib.VsmCanvas.Units;
using IVSMlib.TableDom.MarkDom;
using System.Globalization;

namespace IVSMlib.VsmCanvas.MarksUI
{
    public class MProblem:Mark, IProps, ISize, IDom
    {
        private double MinWidth = 30;
        private double MinHeigth = 30;

        private Brush DrawBrush;

        private Brush FillBrush;
        private Brush FocusedBrush;
        private Brush SelectBrush;

        private bool Visible;
        private bool IsSelect;

        private VisualButton SizeButton;
        private bool SBSelect;

        private ProblemModel Model;

        public PropsHolder ProblemPropsHolder { get; set; }

        private Int32 Num;
        private bool IsNumMode;

        private struct FigurePoint
        {
            public double mark_width;
            public double mark_height;
            public double mark_x_offest;
            public double mark_y_offest;
            public double mark_circle_size;
            public double mark_circle_x_offset;
            public double mark_circle_y_offset;
        }

        FigurePoint points = new FigurePoint();

        private void CreatePropertyHolders()
        {
            ProblemPropsHolder = new PropsHolder();
            ProblemPropsHolder.OwnerType = PropsHolder.Type.Problem;

            StringProps ProblemDescribe = new StringProps();

            ProblemDescribe.PropsEditType = StringProps.EditType.MultiLine;
            ProblemDescribe.Title = "Описание проблемы";
            ProblemDescribe.GetCurrentValueDelegate = GetProblem;
            ProblemDescribe.SetPropertyDelegate = SetProblem;

            ProblemPropsHolder.PropsList.Add(ProblemDescribe);
        }

        private void UpdatePoints()
        {
            points.mark_width = Size.Width * 0.12f;
            points.mark_height = Size.Height * 0.5f;
            points.mark_x_offest = points.mark_width / 2;
            points.mark_y_offest = points.mark_height * 0.3f;

            points.mark_circle_size = points.mark_width * 1.4f;

            points.mark_circle_x_offset = points.mark_circle_size / 2;

            points.mark_circle_y_offset = Size.Height * 0.1f;
        }

        public void SetNum(Int32 n) => Num = n;
        public void SetNumberMode(bool mode)
        {
            IsNumMode = mode;
            DrawUI();
        }

        public MProblem()
        {
            Num = 0;
            IsNumMode = false;
            Model = new ProblemModel();

            FillBrush = Brushes.Red;
            FocusedBrush = Brushes.DarkOrange;
            SelectBrush = Brushes.DarkRed;
            DrawBrush = FillBrush;

            Size = new Size(40, 40);
            Location = new Point(30, 30);
            Visible = true;

            IsSelect = false;
            SBSelect = false;

            SizeButton = new VisualButton();
            SizeButton.SetSize(10, 10);
            SizeButton.SetButtonColor(Colors.LightBlue);
            SizeButton.SetLocation(1, 1);
            UpdatePoints();
            CreatePropertyHolders();
        }

        public Size GetSize() => this.Size;

        public string GetProblem() => Model.Describe;
        public void SetProblem(string s) => Model.Describe = s;

        public override void MouseDown(Point e)
        {

        }

        public override void MouseEnter()
        {
            if (IsSelect == false)
            {
                DrawBrush = FocusedBrush;
                DrawUI();
            }
        }

        public override void MouseLeave()
        {
            if (IsSelect == false)
            {
                DrawBrush = FillBrush;
                DrawUI();
            }
        }

        public override void MouseMove(Point e)
        {
            if (SizeButton.CheckHit(e) && SBSelect == false)
            {
                SizeButton.Select();
                SBSelect = true;
                DrawUI();
                return;
            }

            if (SizeButton.CheckHit(e) == false && SBSelect == true)
            {

                SizeButton.Unselect();
                SBSelect = false;
                DrawUI();
                return;

            }
        }

        public override void MouseUp(Point e)
        {

        }

        public override void Select()
        {
            DrawBrush = SelectBrush;
            IsSelect = true;
            DrawUI();
        }

        public override void Unselect()
        {
            DrawBrush = FillBrush;
            IsSelect = false;
            DrawUI();
        }

        public override void DrawUI()
        {
            DrawingContext dc = this.RenderOpen();
            if (Visible == true)
            {
                if (IsNumMode)
                {
                    dc.DrawRoundedRectangle(DrawBrush, new Pen(FillBrush, 0), new Rect(Location.X, Location.Y, Size.Width, Size.Height), Size.Width / 2, Size.Height / 2);
                    FormattedText TitleText = new FormattedText(Num.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 14, Brushes.White);
                    dc.DrawText(TitleText, new Point((Location.X + Size.Width / 2)-3, (Location.Y + Size.Height / 2) - 7));
                }
                else
                {
                    dc.DrawRoundedRectangle(DrawBrush, new Pen(FillBrush, 0), new Rect(Location.X, Location.Y, Size.Width, Size.Height), Size.Width / 2, Size.Height / 2);
                  
                    dc.DrawRectangle(Brushes.White, new Pen(FillBrush, 0), new Rect((Location.X + Size.Width / 2) - points.mark_x_offest, Location.Y + points.mark_y_offest, points.mark_width, points.mark_height));
                    dc.DrawRoundedRectangle(Brushes.White, new Pen(FillBrush, 0), new Rect((Location.X + Size.Width / 2) - points.mark_circle_x_offset, Location.Y + points.mark_y_offest + points.mark_height + points.mark_circle_y_offset, points.mark_circle_size, points.mark_circle_size), 10, 10);
                }
            }

            if (IsSelect)
            {
              
                SizeButton.Draw(dc);
            }
            dc.Close();
        }

        public override void SetLocation(double x, double y)
        {
            Location.X = x;
            Location.Y = y;

            SizeButton.SetLocation(x + Size.Width, y + Size.Height);

            DrawUI();
        }

        public override void Show()
        {      
            Visible = true;
            DrawUI();
        }

        public override void Hide()
        {
            Visible = false;
            DrawUI();
        }

        public override Point GetLocation()
        {
            return new Point(Location.X, Location.Y);
        }

        public PropsHolder GetPropsHolder()
        {
            return ProblemPropsHolder;
        }

        public bool IsSetSizeMode()
        {
           return SBSelect;
        }

        public void SetSize(double width_inc, double height_inc)
        {
            if (width_inc > MinWidth)
            {
                Size.Width = width_inc;
                Size.Height = width_inc;
            }
            else if (height_inc > MinHeigth)
            {
                Size.Height = height_inc;
                Size.Width = height_inc;
            }

            UpdatePoints();

            SizeButton.SetLocation(Location.X + Size.Width, Location.Y + Size.Height);
            DrawUI();
        }

        public Node CreateDomNode()
        {
           return MProblemDom.Get().CreateRootNode(this);
        }
    }
}
