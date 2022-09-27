using IVSMlib.VsmCanvas.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using IVSMlib.VsmCanvas.CellUI.Interface;
using IVSMlib.TableDom.CellDom;
using IVSMlib.EventsSupport;
using IVSMlib.VsmCanvas.LineUI;
using System.Globalization;
using IVSMlib.ViewModel.Units;
using IVSMlib.PropsHolders;
using IVSMlib.Types;
using IVSMlib.IVSMModel;
using DomCore;

namespace IVSMlib.VsmCanvas.CellUI
{
    public class ConditionCell : Cell, IConnecting, IProps, ITime, IDom, IDocument
    {
        private bool ShowMoveButton;
        private Pen pen;

        public List<Line> LinesTop { get; set; }
        public List<Line> LinesRight { get; set; }
        public List<Line> LinesLeft { get; set; }
        public List<Line> LinesBottom { get; set; }

        private List<List<Line>> AllLines;

        private MoveLineButton TopBtn;
        private MoveLineButton RightBtn;
        private MoveLineButton BottomBtn;
        private MoveLineButton LeftBtn;

        private MoveLineButton SelectedButton;

        private List<MoveLineButton> CellButtons;

        public delegate void MouseDownE(ConditionCell sender);
        public event MouseDownE MouseDownEvent;

        public delegate void MoveButtonClick(ConditionCell sender, MoveButtonArg arg);
        public event MoveButtonClick MoveButtonClickEvent;

        private Point CusomLocation;
        private double CustomSize;

        private FormattedText A_lbl;
        private FormattedText B_lbl;
        private FormattedText C_lbl;

        private void CreatePropertyHolders()
        {
            ConditionPropsHolder = new PropsHolder();
            ConditionPropsHolder.OwnerType = PropsHolder.Type.Condition;

            StringProps OrgamizationHolder = new StringProps();

            OrgamizationHolder.PropsEditType = StringProps.EditType.Line;
            OrgamizationHolder.Title = "Название операции";
            OrgamizationHolder.GetCurrentValueDelegate = GetActName;
            OrgamizationHolder.SetPropertyDelegate = SetActName;
            //      OrgamizationHolder.StringType = StringProps.Type.Line;

            ConditionPropsHolder.PropsList.Add(OrgamizationHolder);

            StringProps ConditionAHolder = new StringProps();

            ConditionAHolder.PropsEditType = StringProps.EditType.Line;
            ConditionAHolder.Title = "Вариант А";
            ConditionAHolder.GetCurrentValueDelegate = Get_A_Conditon;
            ConditionAHolder.SetPropertyDelegate = Set_A_Conditon;

            ConditionPropsHolder.PropsList.Add(ConditionAHolder);

            StringProps ConditionBHolder = new StringProps();

            ConditionBHolder.PropsEditType = StringProps.EditType.Line;
            ConditionBHolder.Title = "Вариант B";
            ConditionBHolder.GetCurrentValueDelegate = Get_B_Conditon;
            ConditionBHolder.SetPropertyDelegate = Set_B_Conditon;

            ConditionPropsHolder.PropsList.Add(ConditionBHolder);

            StringProps ConditionCHolder = new StringProps();

            ConditionCHolder.PropsEditType = StringProps.EditType.Line;
            ConditionCHolder.Title = "Вариант C";
            ConditionCHolder.GetCurrentValueDelegate = Get_C_Conditon;
            ConditionCHolder.SetPropertyDelegate = Set_C_Conditon;

            ConditionPropsHolder.PropsList.Add(ConditionCHolder);

            //----------------------------------------------

            TimeProps tm_a = new TimeProps();
            tm_a.Title = "Время операции";
            tm_a.GetCurrentValueDelegate = GetActionTime;
            tm_a.SetPropertyDelegate = SetActionTime;

            ConditionPropsHolder.PropsList.Add(tm_a);


            TimeProps tm = new TimeProps();
            tm.Title = "Время ожидания";
            tm.GetCurrentValueDelegate = GetWatingTime;
            tm.SetPropertyDelegate = SetWatingTime;

            ConditionPropsHolder.PropsList.Add(tm);

            DocumentListProps doc_props = new DocumentListProps();
            doc_props.Title = "Документы";
            doc_props.GetCurrentValueDelegate = GetActionDocList;
            doc_props.AddDocumentCallback = AddDocumentToList;
            doc_props.DeleteDocumentCallback = DelDocumentFromList;
            ConditionPropsHolder.PropsList.Add(doc_props);

            StringProps Comment = new StringProps();
            Comment.PropsEditType = StringProps.EditType.MultiLine;
            Comment.Title = "Комментарии";
            Comment.GetCurrentValueDelegate = GetComment;
            Comment.SetPropertyDelegate = SetComment;

            ConditionPropsHolder.PropsList.Add(Comment);

        }

        public void AddDocumentToList(DocumentUnit doc)
        {
            ConditionDocuments.Add(doc);
            DrawUI();
        }

        public void DelDocumentFromList(DocumentUnit doc)
        {
            ConditionDocuments.Remove(doc);
            DrawUI();
        }

        private List<DocumentUnit> ConditionDocuments;

        public PropsHolder ConditionPropsHolder { get; set; }

        public string GetActName() => condition_model.Action;
        public void SetActName(string name) => condition_model.Action = name;

        public Time GetActionTime() => condition_model.ActionTime;
        public void SetActionTime(Time time) => condition_model.ActionTime = time;

        public Time GetWatingTime() => condition_model.WaitingTime;
        public void SetWatingTime(Time time) => condition_model.WaitingTime = time;

        //--------Conditions---------------------
        public string Get_A_Conditon() => condition_model.A_Condition;
        public void Set_A_Conditon(string text) => condition_model.A_Condition = text;

        public string Get_B_Conditon() => condition_model.B_Condition;
        public void Set_B_Conditon(string text) => condition_model.B_Condition = text;

        public string Get_C_Conditon() => condition_model.C_Condition;
        public void Set_C_Conditon(string text) => condition_model.C_Condition = text;
        //---------------------------------------

        public string GetComment() => condition_model.Comment;
        public void SetComment(string text) => condition_model.Comment = text;

        public List<DocumentUnit> GetActionDocList() => ConditionDocuments;

        private ConditionModel condition_model;

        public ConditionCell()
        {
            condition_model = new ConditionModel();
            condition_model.ActionTime = new Time(1, Time.Type.Minute);
            condition_model.WaitingTime = new Time(1, Time.Type.Minute);

            pen = new Pen(Brushes.Transparent, 0);
            CellMargin = new Margin();
            size.Width = 50;
            size.Height = 50;
            SetMargin(10, 10);

            ShowMoveButton = true;

            BackgoundBrush = Brushes.Yellow;
            DrawBrush = BackgoundBrush;
            FocusedBrush = Brushes.Aqua;
            SelectedBrush = Brushes.Orange;
            InitButtons();
            InitLinesList();

            CusomLocation = new Point(1, 1);
            CustomSize = 20;

            A_lbl = new FormattedText("A", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);
            B_lbl = new FormattedText("B", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);
            C_lbl = new FormattedText("C", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);

            ConditionDocuments = new List<DocumentUnit>();
            CreatePropertyHolders();
        }

        public override void SetLocation(double x, double y)
        {
            Loc.X = x;
            Loc.Y = y;
            
            UpdateRel();
           
            UpdateInnerMes();
            UpdateButtonPos();
            SetLineLoc();

            DrawUI();
        }

        public override void SetSize(double _width, double _height)
        {
            size.Width = _width;
            size.Height = _height;
            
            UpdateRel();
            
            UpdateButtonPos();

            UpdateInnerMes();
            UpdateButtonPos();
            SetLineLoc();
            //-----------------------
            DrawUI();
        }

        private void UpdateInnerMes()
        {
            if (RelSize.Width < RelSize.Height)
            {
                CustomSize = RelSize.Width;
                CusomLocation.X = RelLocation.X;
                CusomLocation.Y = RelLocation.Y +RelSize.Height/2 - CustomSize / 2;
               
            }
            else
            {
                CustomSize = RelSize.Height;
                CusomLocation.X = RelLocation.X + RelSize.Width / 2 - CustomSize / 2;
                CusomLocation.Y = RelLocation.Y;
                
            }


        }


        public override void MouseMove(Point e)
        { 
                for (int i = 0; i <= CellButtons.Count - 1; i++)
                {
                    if (CellButtons[i].CheckHit(e))
                    {
                        if (CellButtons[i] == SelectedButton)
                        {
                            return;
                        }
                        else
                        {
                            SelectedButton?.Unselect();
                            CellButtons[i].Select();
                            SelectedButton = CellButtons[i];

                            return;
                        }

                    }
                }

                if (SelectedButton != null)
                {
                    SelectedButton.Unselect();
                    SelectedButton = null;

                }
            

        }

        private void InitLinesList()
        {
            LinesTop = new List<Line>();
            LinesRight = new List<Line>();
            LinesLeft = new List<Line>();
            LinesBottom = new List<Line>();

            AllLines = new List<List<Line>>();

            AllLines.Add(LinesTop);
            AllLines.Add(LinesRight);
            AllLines.Add(LinesLeft);
            AllLines.Add(LinesBottom);
        }

        private void InitButtons()
        {
            CellButtons = new List<MoveLineButton>();


            TopBtn = new MoveLineButton();
            TopBtn.SetSize(10, 10);
            TopBtn.Name = "Top";
            TopBtn.LineList = LinesTop;
            TopBtn.AddLineDelegate = AddTopLine;
            TopBtn.GetStartPointDelegate = GetTopPoint;
            TopBtn.ButtonIndex = new MoveLineButton.Bindex(MoveLineButton.Side.N, 0);

            CellButtons.Add(TopBtn);

            RightBtn = new MoveLineButton();
            RightBtn.SetSize(10, 10);
            RightBtn.Name = "Top";
            RightBtn.LineList = LinesRight;
            RightBtn.AddLineDelegate = AddRightLine;
            RightBtn.GetStartPointDelegate = GetRightPoint;
            RightBtn.ButtonIndex = new MoveLineButton.Bindex(MoveLineButton.Side.E, 0);


            CellButtons.Add(RightBtn);

            BottomBtn = new MoveLineButton();
            BottomBtn.SetSize(10, 10);
            BottomBtn.Name = "Top";
            BottomBtn.LineList = LinesBottom;
            BottomBtn.AddLineDelegate = AddBottomLine;
            BottomBtn.GetStartPointDelegate = GetBottomPoint;
            BottomBtn.ButtonIndex = new MoveLineButton.Bindex(MoveLineButton.Side.S, 0);


            CellButtons.Add(BottomBtn);

            LeftBtn = new MoveLineButton();
            LeftBtn.SetSize(10, 10);
            LeftBtn.Name = "Top";
            LeftBtn.LineList = LinesLeft;
            LeftBtn.AddLineDelegate = AddLeftLine;
            LeftBtn.GetStartPointDelegate = GetLefPoint;
            LeftBtn.ButtonIndex = new MoveLineButton.Bindex(MoveLineButton.Side.W, 0);


            CellButtons.Add(LeftBtn);
        }

        public override void DrawUI()
        {
            DrawingContext dc = this.RenderOpen();

            PathFigure path_figure = new PathFigure();

            path_figure.StartPoint = new Point(CusomLocation.X + RelSize.Width / 2, CusomLocation.Y);

            //--------------------------------------------------
            PolyLineSegment check_path = new PolyLineSegment();
            check_path.Points = new PointCollection();

            check_path.Points.Add(new Point(CusomLocation.X + CustomSize / 2, CusomLocation.Y));
            check_path.Points.Add(new Point(CusomLocation.X + CustomSize, CusomLocation.Y + CustomSize / 2));
            check_path.Points.Add(new Point(CusomLocation.X + CustomSize / 2, CusomLocation.Y + CustomSize));
            check_path.Points.Add(new Point(CusomLocation.X, CusomLocation.Y + CustomSize / 2));
            check_path.Points.Add(new Point(CusomLocation.X + CustomSize / 2, CusomLocation.Y));



            path_figure.Segments.Add(check_path);

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(path_figure);

            dc.DrawGeometry(DrawBrush, pen, geometry);

            if(ShowMoveButton)
            {
                TopBtn.Draw(dc);
                RightBtn.Draw(dc);
                BottomBtn.Draw(dc);
                LeftBtn.Draw(dc);
            }

            double x_disp = 15;
            bool center_is_passed = false;

            foreach (DocumentUnit doc_u in ConditionDocuments)
            {
                if((Loc.X + size.Width - x_disp) - (CusomLocation.X + CustomSize / 2) <= 7 && !center_is_passed)
                {
                    //x_disp += 17;
                    x_disp -= 15;
                    x_disp += (((Loc.X + size.Width - x_disp) - (CusomLocation.X + CustomSize / 2))*2)+10;
                    center_is_passed = true;
                }
                    
                dc.DrawRoundedRectangle(doc_u.DocColor, new Pen(Brushes.Transparent, 0), new Rect(new Point(Loc.X + size.Width - x_disp, Loc.Y + size.Height - (CellMargin.Top) - 2), new Size(10, 10)), 5, 5);
                x_disp += 15;
            }

            if (IsSelect)
            {
                dc.DrawText(A_lbl, new Point((CusomLocation.X + CustomSize / 2) - TopBtn.GetSize().Width / 2, CusomLocation.Y+TopBtn.GetSize().Height));
                dc.DrawText(B_lbl, new Point(CusomLocation.X + CustomSize - TopBtn.GetSize().Width*2, (CusomLocation.Y + CustomSize / 2) - RightBtn.GetSize().Height / 2));
                dc.DrawText(C_lbl, new Point((CusomLocation.X + CustomSize / 2) - TopBtn.GetSize().Width / 2, CusomLocation.Y + CustomSize - (C_lbl.Height /2) - RightBtn.GetSize().Height*2));
            }

            //---------------------------------------------------


            dc.Close();
        }

      
        private void UpdateButtonPos()
        {
            TopBtn.SetLocation((CusomLocation.X + CustomSize / 2) - TopBtn.GetSize().Width / 2, CusomLocation.Y);
            RightBtn.SetLocation(CusomLocation.X + CustomSize - TopBtn.GetSize().Width, (CusomLocation.Y + CustomSize / 2) - RightBtn.GetSize().Height/2);
            BottomBtn.SetLocation((CusomLocation.X + CustomSize / 2) - TopBtn.GetSize().Width / 2, CusomLocation.Y + CustomSize - RightBtn.GetSize().Height);
            LeftBtn.SetLocation(CusomLocation.X, (CusomLocation.Y + CustomSize / 2) - RightBtn.GetSize().Height / 2);
        }

        public override void Select()
        {
            DrawBrush = SelectedBrush;
            IsSelect = true;
        }

        public override void Unselect()
        {
            DrawBrush = BackgoundBrush;
            IsSelect = false;
        }

        public override void MouseEnter()
        {
            if (!IsSelect)
            {
                DrawBrush = FocusedBrush;
                
            }
            DrawUI();
        }

        public override void MouseLeave()
        {
            if (!IsSelect)
            {
                DrawBrush = BackgoundBrush;
                
            }
            DrawUI();
        }

        public override void MouseDown(Point e)
        {
            if (SelectedButton != null && ShowMoveButton)
            {
                MoveButtonArg arg = new MoveButtonArg();

                arg.ButtinIndex = new MoveLineButton.Bindex(SelectedButton.ButtonIndex.Side, SelectedButton.ButtonIndex.Pos);
                arg.AddLineHandler = SelectedButton.AddLineDelegate;
                arg.StartPoint = SelectedButton.GetStartPointDelegate();


                MoveButtonClickEvent.Invoke(this, arg);
            }
            MouseDownEvent.Invoke(this);
        }

        public override void MouseUp(Point e)
        {
           
        }

        public void ActiveConnectButton()
        {
            ShowMoveButton = true;
            DrawUI();
        }

        public void DisableConnectButton()
        {
            ShowMoveButton = false;
            DrawUI();
        }

        public void AddLeftLine(Line line)
        {
            line.SetEndPoint(CusomLocation.X, CusomLocation.Y + CustomSize / 2);
            line.DrawUI();
            LinesLeft.Add(line);
        }

        public void AddBottomLine(Line line)
        {
            line.SetStartPoint(CusomLocation.X + CustomSize / 2, CusomLocation.Y + CustomSize);
            line.DrawUI();
            LinesBottom.Add(line);
        }

        public void AddRightLine(Line line)
        {
            line.SetStartPoint(CusomLocation.X + CustomSize, CusomLocation.Y + CustomSize / 2);
            line.DrawUI();
            LinesRight.Add(line);
        }

        public void AddTopLine(Line line)
        {
            line.SetStartPoint(CusomLocation.X + CustomSize / 2, CusomLocation.Y);
            line.DrawUI();
            LinesTop.Add(line);
        }

        public Point GetLefPoint() => new Point(CusomLocation.X, CusomLocation.Y + CustomSize / 2);

        public Point GetTopPoint() => new Point(CusomLocation.X + CustomSize / 2, CusomLocation.Y);

        public Point GetRightPoint() => new Point(CusomLocation.X + CustomSize, CusomLocation.Y + CustomSize / 2);

        public Point GetBottomPoint() => new Point(CusomLocation.X + CustomSize / 2, CusomLocation.Y + CustomSize);

        private void SetLineLoc()
        {
            foreach (Line line in LinesLeft)
            {
                line.SetEndPoint(CusomLocation.X, CusomLocation.Y + CustomSize / 2);
                line.DrawUI();
            }

            foreach (Line line in LinesTop)
            {
                line.SetStartPoint(CusomLocation.X + CustomSize / 2, CusomLocation.Y);
                line.DrawUI();
            }

            foreach (Line line in LinesRight)
            {
                line.SetStartPoint(CusomLocation.X + CustomSize, CusomLocation.Y + CustomSize / 2);
                line.DrawUI();
            }

            foreach (Line line in LinesBottom)
            {
                line.SetStartPoint(CusomLocation.X + CustomSize / 2, CusomLocation.Y + CustomSize);
                line.DrawUI();
            }
        }

        public PropsHolder GetPropsHolder() => ConditionPropsHolder;

        public List<List<Line>> GetAllLinesList()
        {
            return AllLines;
        }

        public void DisconnectLine(Line disc_line)
        {

            for (int i = 0; i <= LinesTop.Count - 1; i++)
            {
                if (LinesTop[i] == disc_line)
                {
                    LinesTop.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i <= LinesRight.Count - 1; i++)
            {
                if (LinesRight[i] == disc_line)
                {
                    LinesRight.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i <= LinesLeft.Count - 1; i++)
            {
                if (LinesLeft[i] == disc_line)
                {
                    LinesLeft.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i <= LinesBottom.Count - 1; i++)
            {
                if (LinesBottom[i] == disc_line)
                {
                    LinesBottom.RemoveAt(i);
                    return;
                }
            }

        }

        public Time IGetActionTime()
        {
            return condition_model.ActionTime;
        }

        public Time IGetWasteTime()
        {
            return condition_model.WaitingTime;
        }

        public List<Line> GetAll_E_Lines()
        {
            List<Line> e_lines = new List<Line>();

            e_lines.AddRange(LinesRight);
            e_lines.AddRange(LinesTop);
            e_lines.AddRange(LinesBottom);

            return e_lines;
        }

        public Node CreateDomNode()
        {
            return ConditionDom.Get().CreateRootNode(this);
        }

        public bool HaveThis(DocumentUnit doc)
        {
            if(ConditionDocuments.Contains(doc))
            {
                return true;
            }
            return false;
        }
    }
}
