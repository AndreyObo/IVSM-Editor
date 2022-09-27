using System.Windows;
using System.Windows.Media;

using DomCore;
using IVSMlib.IVSMModel;
using IVSMlib.PropsHolders;
using IVSMlib.VsmCanvas.CellUI.Interface;
using IVSMlib.VsmCanvas.Units;
using IVSMlib.TableDom.MarkDom;

namespace IVSMlib.VsmCanvas.MarksUI
{
    public class MDecision:Mark, IProps, ISize, IDom
    {
        private Brush DrawBrush;

        private double MinWidth = 30;
        private double MinHeigth = 30;

        private Brush FillBrush;
        private Brush FocusedBrush;
        private Brush SelectBrush;

        private bool Visible;
        private bool IsSelect;

        private VisualButton SizeButton;
        private bool SBSelect;

        private DecisionModel Model;

        public PropsHolder DecisionPropsHolder { get; set; }

        LinePoint[] lines_vector = new LinePoint[]
           {
                new LinePoint(0.16f, 0.50f, 0.23f, 0.41f),
                new LinePoint(0.23f, 0.41f, 0.43f, 0.61f),
                new LinePoint(0.43f, 0.61f, 0.82f, 0.24f),
                new LinePoint(0.82f, 0.24f, 0.92f, 0.34f),
                new LinePoint(0.92f, 0.34f, 0.45f, 0.8f),
                new LinePoint(0.45f, 0.8f, 0.16f, 0.50f),
           };

        LinePoint[] sized;



        private struct LinePoint
        {
            public double X1;
            public double Y1;
            public double X2;
            public double Y2;

            public LinePoint(double x1, double y1, double x2, double y2)
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }
        }

        private void UpdatePoints()
        {
            for (int i = 0; i <= lines_vector.Length - 1; i++)
            {
                sized[i].X1 = Location.X + lines_vector[i].X1 * Size.Width;
                sized[i].Y1 = Location.Y + lines_vector[i].Y1 * Size.Height;
                sized[i].X2 = Location.X + lines_vector[i].X2 * Size.Width;
                sized[i].Y2 = Location.Y + lines_vector[i].Y2 * Size.Height;
            }
        }

        private void CreatePropertyHolders()
        {
            DecisionPropsHolder = new PropsHolder();
            DecisionPropsHolder.OwnerType = PropsHolder.Type.Decision;


            StringProps ProblemDescribe = new StringProps();

            ProblemDescribe.PropsEditType = StringProps.EditType.MultiLine;
            ProblemDescribe.Title = "Описание решения";
            ProblemDescribe.GetCurrentValueDelegate = GetDecision;
            ProblemDescribe.SetPropertyDelegate = SetDecision;


            DecisionPropsHolder.PropsList.Add(ProblemDescribe);
        }

        public Size GetSize() => this.Size;

        public MDecision()
        {
            Model = new DecisionModel();

            sized = new LinePoint[lines_vector.Length];

            FillBrush = Brushes.Green;
            FocusedBrush = Brushes.LightGreen;
            SelectBrush = Brushes.DarkGreen;
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

        public string GetDecision() => Model.Describe;
        public void SetDecision(string s) => Model.Describe = s;

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
                dc.DrawRoundedRectangle(DrawBrush, new Pen(FillBrush, 0), new Rect(Location.X, Location.Y, Size.Width, Size.Height), Size.Width / 2, Size.Height / 2);

                for (int i = 0; i <= sized.Length - 1; i++)
                {
                    dc.DrawLine(new Pen(Brushes.White,2), new Point(sized[i].X1, sized[i].Y1), new Point(sized[i].X2, sized[i].Y2));
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
            UpdatePoints();
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
            return DecisionPropsHolder;
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
            return MDecisionDom.Get().CreateRootNode(this);
        }
    }
}
