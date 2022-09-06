using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using DomCore;
using IVSMlib.IVSMModel;
using IVSMlib.PropsHolders;
using IVSMlib.PropsHolders.VisualProps;
using IVSMlib.Types;
using IVSMlib.VsmCanvas.CellUI.Interface;
using IVSMlib.TableDom.CellDom;

namespace IVSMlib.VsmCanvas.CellUI
{
    public class PlayerCell : Cell, IProps, IDom
    {
        private const Int32 BorderSize = 2;
        private Brush BorderBrush;
        private Pen BorderPen;
        public bool IsBorderTransparant;

        public PlayerModel ThisModel { get; set; }

        private string _playerName;

        public string PlayerName
        {
            get { return _playerName; }
            set { _playerName = value; }
        }

        private string _unit;

        public string UnitName
        {
            get { return _unit; }
            set { _unit = value; }
        }

        private bool IsMouseEnter;

        private bool AutoFontSize;

        private double cur_font_size;

        private Brush SelectBorderBrush;
        private Pen SelectBorderPen;

        private Pen FocusPen;

        private void CreatePropertyHolders()
        {
            PlayerPropsHolder = new PropsHolder();
            PlayerPropsHolder.OwnerType = PropsHolder.Type.Player;

            PlayerPropsHolder.GetLableCallback = GetLable;
            PlayerPropsHolder.SetLableCallback = SetLable;

            //   PropsHolders = new List<Props>();

            StringProps OrgamizationHolder = new StringProps();

            OrgamizationHolder.PropsEditType = StringProps.EditType.Line;
            OrgamizationHolder.Title = "Наименование";
            OrgamizationHolder.GetCurrentValueDelegate = GetOrgName;
            OrgamizationHolder.SetPropertyDelegate = SetOrgName;
            OrgamizationHolder.Name = "Organization";
            //      OrgamizationHolder.StringType = StringProps.Type.Line;

            PlayerPropsHolder.PropsList.Add(OrgamizationHolder);
            // PropsHolders.Add(OrgamizationHolder);

            StringProps DepartmentHolder = new StringProps();
            DepartmentHolder.PropsEditType = StringProps.EditType.Line;
            DepartmentHolder.Title = "Подразделение";
            DepartmentHolder.GetCurrentValueDelegate = GetDepName;
            DepartmentHolder.SetPropertyDelegate = SetDepName;
            //  DepartmentHolder.StringType = StringProps.Type.Text;

            //       PropsHolders.Add(DepartmentHolder);
            PlayerPropsHolder.PropsList.Add(DepartmentHolder);

            StringProps Comment = new StringProps();
            Comment.PropsEditType = StringProps.EditType.MultiLine;
            Comment.Title = "Комментарии";
            Comment.GetCurrentValueDelegate = GetComment;
            Comment.SetPropertyDelegate = SetComment;

            PlayerPropsHolder.PropsList.Add(Comment);

            //TimeProps tm = new TimeProps();
            //tm.Title = "Время ожидания";
            //tm.GetCurrentValueDelegate = GetTime;
            //tm.SetPropertyDelegate = SetTime;

            //PlayerPropsHolder.PropsList.Add(tm);

            //--------visual-----------------

            ColorProps cp = new ColorProps();

            cp.FillType = ColorProps.FType.OnlyFill;
            cp.SetTitle("Цвет");
            cp.GetCurrentColorDelegate = GetCurrentColor;
            cp.SetColorDelegate = SetColor;

            PlayerPropsHolder.VisualPropsList.Add(cp);

            ColorProps border_color = new ColorProps();

            border_color.FillType = ColorProps.FType.Transparant;
            border_color.SetTitle("Цвет рамки");
            border_color.GetCurrentColorDelegate = GetCurrentBorderColor;
            border_color.SetColorDelegate = SetBorderColor;

            PlayerPropsHolder.VisualPropsList.Add(border_color);

            DropCheckListProps TextSize = new DropCheckListProps();
            TextSize.SetTitle("Размер шрифта");
            for (int i = 8; i <= 70; i = i + 2)
            {
                TextSize.Items.Add(i.ToString());
            }
            TextSize.GetCurrentValueDelegate += GetFontSize;
            TextSize.SetPropertyDelegate += SetTextSize;
            TextSize.OnCheckDelegate += FontSizeMode;
            TextSize.GetCurrentCheckDelegate += GetEditMode;

            PlayerPropsHolder.VisualPropsList.Add(TextSize);
        }

        public Color GetCurrentBorderColor() => ((SolidColorBrush)(BorderPen.Brush)).Color;
        public void SetBorderColor(Color c)
        {
            if (c == Colors.Transparent)
            {
                IsBorderTransparant = true;
            }
            else
            {
                IsBorderTransparant = false;
            }
            //DrawBrush = new SolidColorBrush(c);
            BorderPen = new Pen(new SolidColorBrush(c), BorderSize);
            DrawUI();
        }

        private bool GetEditMode() => AutoFontSize;

        private string GetFontSize()
        {
            return cur_font_size.ToString();
        }

        private void SetTextSize(string s)
        {
            int fs = Convert.ToInt32(s);
            cur_font_size = fs;

            UpdateFormatCellText();
        }

        private void FontSizeMode(bool state)
        {

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

        //--------------------------------------------------------------------
        public string GetOrgName() => ThisModel.Orgamization;

        public void SetOrgName(string name)
        {
            ThisModel.Orgamization = name;
            Lable = name;
            UpdateFormatCellText();
            DrawUI();
        }

        public string GetDepName() => ThisModel.Department;
        public void SetDepName(string name) => ThisModel.Department = name;

        public string GetComment() => ThisModel.Comment;
        public void SetComment(string com) => ThisModel.Comment = com;

        public Color GetCurrentColor() => ((SolidColorBrush)(DrawBrush)).Color;

        public void SetColor(Color c)
        {
            DrawBrush = new SolidColorBrush(c);
            DrawUI();
        }

        //--------------------------------------------------------------------

    //    public List<Props> PropsHolders { get; set; }

        public PropsHolder PlayerPropsHolder { get; set; }

        //-------------------debug time----------------------------
        private Time bd_time;

      //  private Time GetTime() => bd_time;
     //   private void SetTime(Time time) => bd_time = time;
        //----------------------------------------------

        private string Lable;

        private FormattedText FormantCellText;


        public PlayerCell()
        {
            BorderBrush = Brushes.DarkGray;
            BorderPen = new Pen(BorderBrush, BorderSize);
            IsBorderTransparant = false;

            AutoFontSize = true;

            cur_font_size = 14;

            bd_time = new Time();
            bd_time.Count = 80;
            bd_time.Measure = Time.Type.Hour;

            Color color = (Color)ColorConverter.ConvertFromString("#e05858");

            SelectBorderBrush = new SolidColorBrush(color);
            SelectBorderPen = new Pen(SelectBorderBrush, 3);

            FocusPen = new Pen(Brushes.LightGray, 3);


            ThisModel = new PlayerModel();
            ThisModel.Orgamization = " ";
            ThisModel.Department = " ";
            CreatePropertyHolders();
            this.BackgoundBrush = Brushes.Yellow;
            this.FocusedBrush = Brushes.Gray;
            this.SelectedBrush = Brushes.LightCyan;
            DrawBrush = BackgoundBrush;
            
            CellMargin = new Margin();
            CellMargin.Left = 5;
            CellMargin.Top = 5;

            Lable = " ";

            IsMouseEnter = false;

            size.Width = 20;
            size.Height = 20;

            FormantCellText = new FormattedText(" ", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), cur_font_size, Brushes.Black);

            //    UpdateFormatCellText();
        }

        private void DecFontSize(ref FormattedText formattedText)
        {
       //     Console.WriteLine("text width --->" + formattedText.Width.ToString());
            while (formattedText.Height >= (size.Height - (CellMargin.Top * 2)) - 5)
            {
                cur_font_size -= 2;
                if (cur_font_size < 5) return;
                formattedText.SetFontSize(cur_font_size);
                
            }
        }

        private void DecFontSizeWidth(ref FormattedText formattedText)
        {
            while (formattedText.Width >= size.Width - (CellMargin.Left * 2))
            {
                cur_font_size -= 2;
                if (cur_font_size < 5) return;
                formattedText.SetFontSize(cur_font_size);

            }
        }

        private void IncFontSize(ref FormattedText formattedText)
        {
            while (formattedText.Height <= ((size.Height - (CellMargin.Top * 2)) - 5) - 5  && cur_font_size  <= 14)
            {
                cur_font_size += 2;
                formattedText.SetFontSize(cur_font_size);

            }
        }

        private void UpdateFormatCellText()
        {
            FormantCellText = new FormattedText(Lable, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), cur_font_size, Brushes.Black);

            FormantCellText.MaxTextWidth = size.Width - (CellMargin.Left * 2);

            // formattedText.MaxTextHeight = (size.Height - (CellMargin.Top * 2)) - 5;

            FormantCellText.TextAlignment = TextAlignment.Center;

            if (FormantCellText.Height > size.Height - 10)
            {
                DecFontSize(ref FormantCellText);
            }
            if (FormantCellText.Height < size.Height - 10)
            {
                IncFontSize(ref FormantCellText);
            }
            //if (FormantCellText.Width > size.Width - 10)
            //{
            //    DecFontSizeWidth(ref FormantCellText);
            //}
        }

        public override void DrawUI()
        {
            DrawingContext dc = this.RenderOpen();

            dc.DrawRoundedRectangle(DrawBrush, new Pen(Brushes.Transparent, 1), new Rect(Loc.X + CellMargin.Left, Loc.Y + CellMargin.Top, this.size.Width - CellMargin.Left*2, this.size.Height - CellMargin.Top*2), 10, 10);
            
         //   dc.DrawRectangle(Brushes.Transparent, BorderPen, new Rect(Loc.X + CellMargin.Left, Loc.Y + CellMargin.Top, this.size.Width - CellMargin.Left * 2, this.size.Height - CellMargin.Top * 2));


            if (IsSelect)
            {
                dc.DrawRectangle(Brushes.Transparent, SelectBorderPen, new Rect(Loc.X + CellMargin.Left - 2, Loc.Y + CellMargin.Top - 2, (this.size.Width - CellMargin.Left * 2) + 4, (this.size.Height - CellMargin.Top * 2) + 4));
            }
            if (IsMouseEnter)
            {
                dc.DrawRectangle(Brushes.Transparent, FocusPen, new Rect(Loc.X + CellMargin.Left - 2, Loc.Y + CellMargin.Top - 2, (this.size.Width - CellMargin.Left * 2) + 4, (this.size.Height - CellMargin.Top * 2) + 4));
            }

            // FormattedText formattedText = new FormattedText(Lable, CultureInfo.GetCultureInfo("en-us"),  FlowDirection.LeftToRight,new Typeface("Verdana"), cur_font_size, Brushes.Black);



            // formattedText.MaxTextWidth = size.Width - (CellMargin.Left * 2);

            //// formattedText.MaxTextHeight = (size.Height - (CellMargin.Top * 2)) - 5;

            // formattedText.TextAlignment = TextAlignment.Center;

            // if (formattedText.Height > size.Height - 10)
            // {
            //     DecFontSize(ref formattedText);
            // }
            // if (formattedText.Height < size.Height - 10)
            // {
            //     IncFontSize(ref formattedText);
            // }

            double l_y = ((size.Height-(CellMargin.Top*2)) / 2 - FormantCellText.Height / 2);

            dc.DrawText(FormantCellText, new Point(Loc.X + CellMargin.Left, Loc.Y + CellMargin.Top+ l_y));
     
            dc.Close();
        }

        public override void Select()
        {
            //DrawBrush = SelectedBrush;
            //DrawUI();
            base.IsSelect = true;
            IsMouseEnter = false;
        }

        public override void Unselect()
        {
            //DrawBrush = BackgoundBrush;
            //DrawUI();
            base.IsSelect = false;
        }

        public override void MouseEnter()
        {
            if (IsSelect == false)
            {
                //  DrawBrush = FocusedBrush;
                IsMouseEnter = true;
                DrawUI();

            }
        }

        public override void MouseMove(Point e)
        {
          
        }

        public override void MouseLeave()
        {
            if (IsSelect == false)
            {
                //  DrawBrush = FocusedBrush;
                IsMouseEnter = false;
                DrawUI();

            }
        }

        public override void MouseDown(Point e)
        {
         //   throw new NotImplementedException();
        }

        public override void MouseUp(Point e)
        {
           // throw new NotImplementedException();
        }

        public override void SetLocation(double x, double y)
        {
            Loc.X = x;
            Loc.Y = y;
            UpdateRel();
            DrawUI();
        }

        public PropsHolder GetPropsHolder()
        {
            return PlayerPropsHolder;
        }

        public override void SetWidth(double _width)
        {
            size.Width = _width;
            UpdateFormatCellText();
            DrawUI();
        }

        public override void SetHeight(double _height)
        {
            size.Height = _height;
            UpdateFormatCellText();
            DrawUI();
        }

        public override void SetSize(double _width, double _height)
        {
            size.Width = _width;
            size.Height = _height;
            UpdateFormatCellText();
            UpdateRel();
            DrawUI();
        }

        public Node CreateDomNode()
        {
            return PlayerDom.Get().CreateRootNode(this);
        }
    }
}
