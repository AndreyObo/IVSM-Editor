using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using IVSMlib.EventsSupport;
using IVSMlib.IVSMModel;
using IVSMlib.ViewModel.Units;
using IVSMlib.VsmCanvas.CellUI.Interface;
using IVSMlib.VsmCanvas.LineUI;
using IVSMlib.VsmCanvas.Units;
using IVSMlib.PropsHolders;
using IVSMlib.Types;
using IVSMlib.PropsHolders.VisualProps;
using IVSMlib.TableDom.CellDom;
using DomCore;

namespace IVSMlib.VsmCanvas.CellUI
{
    public class ActionCell : Cell, IConnecting, IProps, ITime, IDom, IDocument
    {
        private const Int32 BorderSize=2;
        private Brush BorderBrush;
        private Pen BorderPen;
        public bool IsBorderTransparant;

        private bool ShowMoveButton;
        private ActionModel action_model { get; set; }

        public List<Line> LinesLeftTop { get; set; }
        public List<Line> LinesRightTop { get; set; }
        public List<Line> LinesLeftBottom { get; set; }
        public List<Line> LinesRightBottom { get; set; }

        public List<Line> LinesRightCenter { get; set; }
        public List<Line> LinesLeftCenter { get; set; }

        public List<List<Line>> AllLines { get; set; }

        private Size LineBoxSize;

        public struct CornerBrushes
        {
            public Brush TopLeft;
            public Brush TopRight;
            public Brush BottomLeft;
            public Brush BottomRight;
        }

        private Brush SelectBorderBrush;
        private Pen SelectBorderPen;

        private Pen FocusPen;

        public CornerBrushes CellCornerBrushes;

        private MoveLineButton LeftTopBtn;
        private MoveLineButton RightTopBtn;
        private MoveLineButton LeftBottomBtn;
        private MoveLineButton RightBottomBtn;

        private MoveLineButton LeftCenterBtn;
        private MoveLineButton RightCenterBtn;

        private List<MoveLineButton> CellButtons;

        private MoveLineButton SelectedButton;

        private void InitCorerBrushes()
        {
            CellCornerBrushes = new CornerBrushes();
            CellCornerBrushes.TopLeft = Brushes.LightGray;
            CellCornerBrushes.TopRight = Brushes.LightGray;
            CellCornerBrushes.BottomLeft = Brushes.LightGray;
            CellCornerBrushes.BottomRight = Brushes.LightGray;
        }

        private void InitLinesLists()
        {
            LinesLeftTop = new List<Line>();
            LinesRightTop = new List<Line>();
            LinesLeftBottom = new List<Line>();
            LinesRightBottom = new List<Line>();

            LinesRightCenter = new List<Line>();
            LinesLeftCenter = new List<Line>();

            AllLines = new List<List<Line>>();

            AllLines.Add(LinesLeftTop);
            AllLines.Add(LinesRightTop);
            AllLines.Add(LinesLeftBottom);
            AllLines.Add(LinesRightBottom);
            AllLines.Add(LinesRightCenter);
            AllLines.Add(LinesLeftCenter);

        }

        public delegate void MouseDownE(ActionCell sender);
        public event MouseDownE MouseDownEvent;

        public delegate void MoveButtonClick(ActionCell sender, MoveButtonArg arg);
        public event MoveButtonClick MoveButtonClickEvent;

        public delegate void UpdateTime();
        public UpdateTime UpdateTimeCallbak;


        private void CreatePropertyHolders()
        {
            ActionPropsHolder = new PropsHolder();
            ActionPropsHolder.OwnerType = PropsHolder.Type.Action;

            ActionPropsHolder.GetLableCallback = GetLable;
            ActionPropsHolder.SetLableCallback = SetLable;

            StringProps OrgamizationHolder = new StringProps();

            OrgamizationHolder.PropsEditType = StringProps.EditType.Line;
            OrgamizationHolder.Title = "Название операции";
            OrgamizationHolder.GetCurrentValueDelegate = GetActName;
            OrgamizationHolder.SetPropertyDelegate = SetActName;

            ActionPropsHolder.PropsList.Add(OrgamizationHolder);


            TimeProps tm_a = new TimeProps();
            tm_a.Title = "Время операции";
            tm_a.GetCurrentValueDelegate = GetActionTime;
            tm_a.SetPropertyDelegate = SetActionTime;

            ActionPropsHolder.PropsList.Add(tm_a);

        
            TimeProps tm = new TimeProps();
            tm.Title = "Время ожидания";
            tm.GetCurrentValueDelegate = GetWatingTime;
            tm.SetPropertyDelegate = SetWatingTime;

            ActionPropsHolder.PropsList.Add(tm);

            DocumentListProps doc_props = new DocumentListProps();
            doc_props.Title = "Документы";
            doc_props.GetCurrentValueDelegate = GetActionDocList;
            doc_props.AddDocumentCallback = AddDocumentToList;
            doc_props.DeleteDocumentCallback = DelDocumentFromList;
            ActionPropsHolder.PropsList.Add(doc_props);

            StringProps Comment = new StringProps();
            Comment.PropsEditType = StringProps.EditType.MultiLine;
            Comment.Title = "Комментарии";
            Comment.GetCurrentValueDelegate = GetComment;
            Comment.SetPropertyDelegate = SetComment;

            ActionPropsHolder.PropsList.Add(Comment);

            //------------Visual----------------------

            ColorProps cp = new ColorProps();

            cp.FillType = ColorProps.FType.OnlyFill;
            cp.SetTitle("Цвет");
            cp.GetCurrentColorDelegate = GetCurrentColor;
            cp.SetColorDelegate = SetColor;

            ActionPropsHolder.VisualPropsList.Add(cp);


            ColorProps border_color = new ColorProps();

            border_color.FillType = ColorProps.FType.Transparant;
            border_color.SetTitle("Цвет рамки");
            border_color.GetCurrentColorDelegate = GetCurrentBorderColor;
            border_color.SetColorDelegate = SetBorderColor;

            ActionPropsHolder.VisualPropsList.Add(border_color);
        }

        public PropsHolder ActionPropsHolder { get; set; }

        public Color GetCurrentColor() => ((SolidColorBrush)(DrawBrush)).Color;
        public void SetColor(Color c) {
            DrawBrush = new SolidColorBrush(c);
            DrawUI();
        }

        public Color GetCurrentBorderColor() => ((SolidColorBrush)(BorderPen.Brush)).Color;
        public void SetBorderColor(Color c)
        {
            if(c == Colors.Transparent)
            {
                IsBorderTransparant = true;
            }
            else
            {
                IsBorderTransparant = false;
            }

            BorderPen = new Pen(new SolidColorBrush(c), BorderSize);
            DrawUI();
        }

        public string GetActName() => action_model.Action;
        public void SetActName(string name)
        {
            action_model.Action = name;
            Lable = name;
            UpdateFormatCellText();
            DrawUI();
        }

        public Time GetActionTime() => action_model.ActionTime;
        public void SetActionTime(Time time)
        {
            action_model.ActionTime = time;
            if(UpdateTimeCallbak != null)
            {
                UpdateTimeCallbak.Invoke();
            }
        } 

        public Time GetWatingTime() => action_model.WaitingTime;

        public void SetWatingTime(Time time) { 
            
            action_model.WaitingTime = time;

            if (UpdateTimeCallbak != null)
            {
                UpdateTimeCallbak.Invoke();
            }
        }

        public string GetComment() => action_model.Comment;
        public void SetComment(string text) => action_model.Comment = text;

        public List<DocumentUnit> GetActionDocList() => ActionDocuments;

        public void AddDocumentToList(DocumentUnit doc)
        {
            ActionDocuments.Add(doc);
            DrawUI();
        }

        public void DelDocumentFromList(DocumentUnit doc)
        {
            ActionDocuments.Remove(doc);
            DrawUI();
        }
            

        private string GetLable()
        {
            return Lable;
        }

        private void SetLable(string lb)
        {
            Lable = lb;
            UpdateFormatCellText();
            DrawUI();
        }

        private string Lable;

        private FormattedText FormantCellText;

        private double cur_font_size;

        private bool IsMouseEnter;


        private List<DocumentUnit> ActionDocuments;

        public ActionCell()
        {
            BorderBrush = Brushes.DarkGray;
            BorderPen = new Pen(BorderBrush, BorderSize);
            IsBorderTransparant = false;


            cur_font_size = 14;
            action_model = new ActionModel();
            ActionDocuments = new List<DocumentUnit>();
            action_model.ActionTime = new Time(10, Time.Type.Minute);
            action_model.WaitingTime = new Time(5, Time.Type.Minute);
            Lable = " ";

            this.BackgoundBrush = Brushes.LightPink;
            this.FocusedBrush = Brushes.LightGreen;
            this.SelectedBrush = Brushes.LightSalmon;

            Color color = (Color)ColorConverter.ConvertFromString("#e05858");

            SelectBorderBrush = new SolidColorBrush(color);
            SelectBorderPen = new Pen(SelectBorderBrush, 3);

            FocusPen = new Pen(Brushes.LightGray, 3);

            DrawBrush = BackgoundBrush;
            CellMargin = new Margin();
            CellMargin.Left = 10;
            CellMargin.Top = 10;
            ShowMoveButton = true;
            base.IsSelect = false;

            IsMouseEnter = false;

            size.Width = 20;
            size.Height = 20;

            LineBoxSize = new Size(10, 10);

            FormantCellText = new FormattedText(" ", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), cur_font_size, Brushes.Black);

            InitCorerBrushes();
            InitLinesLists();
            InitButtons();

            CreatePropertyHolders();

        }

        private void InitButtons()
        {
            CellButtons = new List<MoveLineButton>();

            LeftTopBtn = new MoveLineButton();
            LeftTopBtn.SetSize(10, 10);
            LeftTopBtn.Name = "LeftTop";
            LeftTopBtn.LineList = LinesLeftTop;
            LeftTopBtn.AddLineDelegate = AddLeftTopLine;
            LeftTopBtn.GetStartPointDelegate = GetLeftTopPoint;
            LeftTopBtn.ButtonIndex = new MoveLineButton.Bindex(MoveLineButton.Side.W, 3);


            CellButtons.Add(LeftTopBtn);

            RightTopBtn = new MoveLineButton();
            RightTopBtn.SetSize(10, 10);
            RightTopBtn.Name = "RightTop";
            RightTopBtn.ButtonIndex = new MoveLineButton.Bindex(MoveLineButton.Side.E, 1);
            RightTopBtn.AddLineDelegate = AddRightTopLine;
            RightTopBtn.GetStartPointDelegate = GetRightTopPoint;
            RightTopBtn.LineList = LinesRightTop;

            CellButtons.Add(RightTopBtn);


            LeftBottomBtn = new MoveLineButton();
            LeftBottomBtn.SetSize(10, 10);
            LeftBottomBtn.Name = "LeftBottom";
            LeftBottomBtn.ButtonIndex = new MoveLineButton.Bindex(MoveLineButton.Side.W, 1);
            LeftBottomBtn.AddLineDelegate = AddLeftBottomLine;
            LeftBottomBtn.GetStartPointDelegate = GetLeftBottomPoint;
            LeftBottomBtn.LineList = LinesLeftBottom;

            CellButtons.Add(LeftBottomBtn);

            RightBottomBtn = new MoveLineButton();
            RightBottomBtn.SetSize(10, 10);
            RightBottomBtn.Name = "RightBottom";
            RightBottomBtn.ButtonIndex = new MoveLineButton.Bindex(MoveLineButton.Side.E, 3);
            RightBottomBtn.LineList = LinesRightBottom;
            RightBottomBtn.AddLineDelegate = AddRightBottomLine;
            RightBottomBtn.GetStartPointDelegate = GetRightBottomPoint;
            CellButtons.Add(RightBottomBtn);

            RightCenterBtn = new MoveLineButton();
            RightCenterBtn.SetSize(10, 10);
            RightCenterBtn.Name = "RightCenter";
            RightCenterBtn.ButtonIndex = new MoveLineButton.Bindex(MoveLineButton.Side.E, 2);
            RightCenterBtn.LineList = LinesRightCenter;
            RightCenterBtn.AddLineDelegate = AddRightCenterLine;
            RightCenterBtn.GetStartPointDelegate = GetRightCenterPoint;
            CellButtons.Add(RightCenterBtn);

            LeftCenterBtn = new MoveLineButton();
            LeftCenterBtn.SetSize(10, 10);
            LeftCenterBtn.Name = "LeftCenter";
            LeftCenterBtn.ButtonIndex = new MoveLineButton.Bindex(MoveLineButton.Side.W, 2);
            LeftCenterBtn.LineList = LinesLeftCenter;
            LeftCenterBtn.AddLineDelegate = AddLeftCenterLine;
            LeftCenterBtn.GetStartPointDelegate = GetLeftCenterPoint;
            CellButtons.Add(LeftCenterBtn);
        }

        public override void SetLocation(double x, double y)
        {
            Loc.X = x;
            Loc.Y = y;
            UpdateButtonPos();
            SetLineLoc();
            DrawUI();
        }

        public override void SetSize(double _width, double _height)
        {
            size.Width = _width;
            size.Height = _height;
            UpdateButtonPos();
            SetLineLoc();
            UpdateFormatCellText();
            DrawUI();
        }

        public override Size GetContentSize()
        {
            return new Size(this.size.Width - CellMargin.Left * 2, this.size.Height - CellMargin.Top * 2);
        }


        private void UpdateButtonPos()
        {
            LeftTopBtn.SetLocation(Loc.X + CellMargin.Left, Loc.Y + CellMargin.Top);
            RightTopBtn.SetLocation((Loc.X + size.Width) - CellMargin.Left - RightTopBtn.GetSize().Width, Loc.Y + CellMargin.Top);
            LeftBottomBtn.SetLocation(Loc.X + CellMargin.Left, Loc.Y + size.Height - CellMargin.Top - LineBoxSize.Height);
            RightBottomBtn.SetLocation((Loc.X + size.Width) - CellMargin.Left - RightBottomBtn.GetSize().Width, Loc.Y + size.Height - CellMargin.Top - LineBoxSize.Height);

            RightCenterBtn.SetLocation((Loc.X + size.Width) - CellMargin.Left - RightCenterBtn.GetSize().Width, Loc.Y + size.Height  -size.Height/2 - RightCenterBtn.GetSize().Height / 2);
            LeftCenterBtn.SetLocation(Loc.X + CellMargin.Left, Loc.Y + size.Height  - size.Height / 2 - RightCenterBtn.GetSize().Height / 2);
        }

        public override void Select()
        {
            base.IsSelect = true;
            IsMouseEnter = false;
            
        }

        public override void Unselect()
        {
            base.IsSelect = false;
        }


        private void UpdateFormatCellText()
        {
            FormantCellText = new FormattedText(Lable, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), cur_font_size, Brushes.Black);

            FormantCellText.MaxTextWidth = size.Width - (CellMargin.Left * 2);

            // formattedText.MaxTextHeight = (size.Height - (CellMargin.Top * 2)) - 5;

            FormantCellText.TextAlignment = TextAlignment.Center;

            Console.WriteLine("\nUpdate Funct IN text_height - " + FormantCellText.Height.ToString());

            if (FormantCellText.Height >= (((size.Height - (CellMargin.Top * 2)) - 5) - 15))
            {
                //DecFontSize(FormantCellText);
                //FormantCellText.SetFontSize(cur_font_size);
                double cell_height = ((size.Height - (CellMargin.Top * 2)) - 5) - 15;
                while (FormantCellText.Height >= cell_height)
                {

                    cur_font_size -= 2;
                    if (cur_font_size < 4) return;
                    FormantCellText.SetFontSize(cur_font_size);
                    Console.WriteLine("insede decing main text_h -  " + FormantCellText.Height + " cell_h - " + cell_height.ToString());

                }
                return;
            }
            if (FormantCellText.Height <= (((size.Height - (CellMargin.Top * 2)) - 5) - 15))
            {
                //  IncFontSize(ref FormantCellText);
                double cell_height = ((size.Height - (CellMargin.Top * 2)) - 5) - 15;
                while (FormantCellText.Height < cell_height && cur_font_size <= 14)
                {

                    cur_font_size += 2;
                    FormantCellText.SetFontSize(cur_font_size);
                    if(FormantCellText.Height >= cell_height)
                    {
                        cur_font_size -= 2;
                        FormantCellText.SetFontSize(cur_font_size);
                        break;
                    }
                    Console.WriteLine("insede while incing text_h -  " + FormantCellText.Height + " cell_h - " + cell_height.ToString());

                }
            }

            Console.WriteLine("\nUpdate Funct OUT text_height - " + FormantCellText.Height.ToString());
        }


        public override void DrawUI()
        {
            DrawingContext dc = this.RenderOpen();

            dc.DrawRectangle(DrawBrush, new Pen(Brushes.Transparent, 1), new Rect(Loc.X + CellMargin.Left, Loc.Y + CellMargin.Top, this.size.Width - CellMargin.Left * 2, this.size.Height - CellMargin.Top * 2));

            dc.DrawRectangle(Brushes.Transparent, BorderPen, new Rect(Loc.X + CellMargin.Left, Loc.Y + CellMargin.Top, this.size.Width - CellMargin.Left * 2, this.size.Height - CellMargin.Top * 2));

            if(IsSelect)
            {
                dc.DrawRectangle(Brushes.Transparent, SelectBorderPen, new Rect(Loc.X + CellMargin.Left-2, Loc.Y + CellMargin.Top-2, (this.size.Width - CellMargin.Left * 2)+4, (this.size.Height - CellMargin.Top * 2)+4));
            }
            if(IsMouseEnter)
            {
                dc.DrawRectangle(Brushes.Transparent, FocusPen, new Rect(Loc.X + CellMargin.Left - 2, Loc.Y + CellMargin.Top - 2, (this.size.Width - CellMargin.Left * 2) + 4, (this.size.Height - CellMargin.Top * 2) + 4));
            }


            double l_y = ((size.Height - (CellMargin.Top * 2)) / 2 - FormantCellText.Height / 2);

            dc.DrawText(FormantCellText, new Point(Loc.X + CellMargin.Left, Loc.Y + CellMargin.Top + l_y));

            if (ShowMoveButton)
            {
                LeftTopBtn.Draw(dc);
                RightTopBtn.Draw(dc);
                LeftBottomBtn.Draw(dc);
                RightBottomBtn.Draw(dc);

                RightCenterBtn.Draw(dc);
                LeftCenterBtn.Draw(dc);
            }

            double x_disp = 10 + CellMargin.Left;

            foreach(DocumentUnit doc_u in ActionDocuments)
            {
                dc.DrawRoundedRectangle(doc_u.DocColor, new Pen(Brushes.Transparent, 0), new Rect(new Point(Loc.X + size.Width - CellMargin.Left-x_disp, Loc.Y + size.Height - (CellMargin.Top*2)-2), new Size(10,10)), 5, 5);
                x_disp += 15;
            }

            dc.Close();
        }

        public override void MouseMove(Point e)
        {
         
            for(int i=0; i <=CellButtons.Count -1; i++)
            {
                if(CellButtons[i].CheckHit(e))
                {
                    if(CellButtons[i]==SelectedButton)
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

            if(SelectedButton != null)
            {
                SelectedButton.Unselect();
                SelectedButton = null;

            }

        }

        public override void MouseEnter()
        {
            if (IsSelect==false)
            {
                IsMouseEnter = true;
                DrawUI();

            }
           
        }


        public override void MouseLeave()
        {

            if (IsSelect == false)
            {
                IsMouseEnter = false;
                DrawUI();

            }

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

        public override CellMouseResult GetMouseDownResult(Point e)
        {
            CellMouseResult res = new CellMouseResult();
            res.sender = this;
            if (SelectedButton != null)
            {
                switch (SelectedButton.Name)
                {
                    case "LeftTop":
                        res.CellResult = CellMouseResult.Result.LeftTopBtn;
                        res.e = new Point(Loc.X + CellMargin.Left, Loc.Y + CellMargin.Top);
                        break;
                    case "RightTop":
                        res.CellResult = CellMouseResult.Result.RightTopBtn;
                        res.e = new Point(Loc.X + size.Width - CellMargin.Left, Loc.Y + CellMargin.Top);
                        break;
                    case "LeftBottom":
                        res.CellResult = CellMouseResult.Result.LeftBottomBtn;
                        res.e = new Point(Loc.X + CellMargin.Left, Loc.Y + size.Height - CellMargin.Top);
                        break;
                    case "RightBottom":
                        res.CellResult = CellMouseResult.Result.RightBottomBtn;
                        res.e = new Point(Loc.X + size.Width - CellMargin.Left, Loc.Y + size.Height - CellMargin.Top);
                        break;
                }
            }
            else
            {
                res.CellResult = CellMouseResult.Result.CellClick;
            }
            return res;
        }

        public void AddLeftTopLine(Line line)
        {
            line.SetEndPoint(Loc.X + CellMargin.Left, Loc.Y + CellMargin.Top);
            LinesLeftTop.Add(line);
        }

        public void AddLeftBottomLine(Line line)
        {
            line.SetEndPoint(Loc.X + CellMargin.Left, Loc.Y +size.Height - CellMargin.Top);
            LinesLeftBottom.Add(line);
        }

        public void AddRightTopLine(Line line)
        {
           line.SetStartPoint(Loc.X+size.Width - CellMargin.Left, Loc.Y + CellMargin.Top);
            LinesRightTop.Add(line);
        }

        public void AddRightBottomLine(Line line)
        {
            line.SetStartPoint(Loc.X + size.Width - CellMargin.Left, Loc.Y + size.Height - CellMargin.Top);
            LinesRightBottom.Add(line);
        }

        public void AddRightCenterLine(Line line)
        {
            line.SetStartPoint(Loc.X + size.Width - CellMargin.Left, Loc.Y + size.Height - (size.Height/2));
            LinesRightCenter.Add(line);
        }

        public void AddLeftCenterLine(Line line)
        {
            line.SetEndPoint(Loc.X + CellMargin.Left, Loc.Y + size.Height - (size.Height / 2));
            LinesLeftCenter.Add(line);
        }

        //-------------------------------------------------------------

        public Point GetLeftTopPoint()=> new Point(Loc.X + CellMargin.Left, Loc.Y + CellMargin.Top);

        public Point GetLeftBottomPoint() => new Point(Loc.X + CellMargin.Left, Loc.Y + size.Height - CellMargin.Top);

        public Point GetRightTopPoint() => new Point(Loc.X + size.Width - CellMargin.Left, Loc.Y + CellMargin.Top);

        public Point GetRightBottomPoint() => new Point(Loc.X + size.Width - CellMargin.Left, Loc.Y + size.Height - CellMargin.Top);

        public Point GetRightCenterPoint()=> new Point(Loc.X + size.Width - CellMargin.Left, Loc.Y + size.Height - (size.Height/2));

        public Point GetLeftCenterPoint()=> new Point(Loc.X + CellMargin.Left, Loc.Y + size.Height - (size.Height / 2));

        public void SetLineLoc()
        {
            foreach(Line line in LinesLeftTop)
            {
                line.SetEndPoint(Loc.X + CellMargin.Left, Loc.Y + CellMargin.Top);
                line.DrawUI();
            }
            foreach (Line line in LinesLeftBottom)
            {
                line.SetEndPoint(Loc.X + CellMargin.Left, Loc.Y + size.Height - CellMargin.Top);
                line.DrawUI();
            }

            foreach (Line line in LinesRightTop)
            {
                line.SetStartPoint(Loc.X + size.Width - CellMargin.Left, Loc.Y + CellMargin.Top);
                line.DrawUI();
            }

            foreach (Line line in LinesRightBottom)
            {
                line.SetStartPoint(Loc.X + size.Width - CellMargin.Left, Loc.Y + size.Height - CellMargin.Top);
                line.DrawUI();
            }

            foreach (Line line in LinesLeftCenter)
            {
                line.SetEndPoint(Loc.X + CellMargin.Left, Loc.Y + size.Height - (size.Height / 2));
                line.DrawUI();
            }

            foreach (Line line in LinesRightCenter)
            {
                line.SetStartPoint(Loc.X + size.Width - CellMargin.Left, Loc.Y + size.Height - (size.Height / 2));
                line.DrawUI();
            }
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

        public PropsHolder GetPropsHolder() => ActionPropsHolder;

        public List<List<Line>> GetAllLinesList()
        {
            return AllLines;
        }

        public void DisconnectLine(Line disc_line)
        {

            for (int i = 0; i <= LinesLeftTop.Count - 1; i++)
            {
                if(LinesLeftTop[i]==disc_line)
                {
                    LinesLeftTop.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i <= LinesRightTop.Count - 1; i++)
            {
                if (LinesRightTop[i] == disc_line)
                {
                    LinesRightTop.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i <= LinesLeftBottom.Count - 1; i++)
            {
                if (LinesLeftBottom[i] == disc_line)
                {
                    LinesLeftBottom.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i <= LinesRightBottom.Count - 1; i++)
            {
                if (LinesRightBottom[i] == disc_line)
                {
                    LinesRightBottom.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i <= LinesRightCenter.Count - 1; i++)
            {
                if (LinesRightCenter[i] == disc_line)
                {
                    LinesRightCenter.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i <= LinesLeftCenter.Count - 1; i++)
            {
                if (LinesLeftCenter[i] == disc_line)
                {
                    LinesLeftCenter.RemoveAt(i);
                    return;
                }
            }

        }

       public Time IGetActionTime()
       {
            return action_model.ActionTime;
       }

        public Time IGetWasteTime()
        {
            return action_model.WaitingTime;
        }

        public List<Line> GetAll_E_Lines()
        {
            List<Line> e_lines = new List<Line>();

            e_lines.AddRange(LinesRightTop);
            e_lines.AddRange(LinesRightCenter);
            e_lines.AddRange(LinesRightBottom);

            return e_lines;
        }

        public Node CreateDomNode()
        {
            return ActionDom.Get().CreateRootNode(this);
        }

        public bool HaveThis(DocumentUnit doc)
        {
            if (ActionDocuments.Contains(doc))
            {
                return true;
            }
            return false;
        }

    }
}
