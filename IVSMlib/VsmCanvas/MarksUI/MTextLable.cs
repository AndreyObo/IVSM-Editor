using System.Globalization;
using System.Windows;
using System.Windows.Media;
using IVSMlib.PropsHolders;
using IVSMlib.PropsHolders.VisualProps;

using IVSMlib.VsmCanvas.Units;
using IVSMlib.Utils;
using IVSMlib.VsmCanvas.CellUI.Interface;
using DomCore;
using IVSMlib.TableDom.MarkDom;

namespace IVSMlib.VsmCanvas.MarksUI
{
    public class MTextLable:Mark, ISize,IProps, IDom
    {
        private double MinWidth = 30;
        private double MinHeigth = 30;
        
        private Brush SizeMarkBrush;

        private Brush DrawBrush;

        private Brush FillBrush;
        private Brush FocusedBrush;
        private Brush SelectBrush;

        public bool IsFillTransparant;
        public bool IsBorderTransparant;

        private Brush BorderBrush;

        private Pen SelectPen;

        private Brush TextBrush;
        private string FontFamily;

        private bool Visible;
        private bool IsSelect;
        private bool SBSelect;

        private double PenWidth;

        private FormattedText FormantLableText;

        private double TextFontSize;
        private VisualButton SizeButton;

        private string Text;

        public PropsHolder TextPropsHolder { get; set; }

        private void CreatePropertyHolders()
        {
            TextPropsHolder = new PropsHolder();

            TextPropsHolder.OwnerType = PropsHolder.Type.Default;

            StringProps Text = new StringProps();
            Text.PropsEditType = StringProps.EditType.MultiLine;
            Text.Title = "Text";
            Text.GetCurrentValueDelegate = GetText;
            Text.SetPropertyDelegate += SetText;

            TextPropsHolder.VisualPropsList.Add(Text);

            ColorProps c_text = new ColorProps();

            c_text.FillType = ColorProps.FType.OnlyFill;
            c_text.SetTitle("Цвет текста");
            c_text.GetCurrentColorDelegate = GetTextColor;
            c_text.SetColorDelegate = SetTextColor;

            TextPropsHolder.VisualPropsList.Add(c_text);

            ColorProps cp = new ColorProps();

            cp.FillType = ColorProps.FType.Transparant;
            cp.SetTitle("Цвет фона");
            cp.GetCurrentColorDelegate = GetBackColor;
            cp.SetColorDelegate = SetBackColor;

            TextPropsHolder.VisualPropsList.Add(cp);

            ColorProps bcolor = new ColorProps();

            bcolor.FillType = ColorProps.FType.Transparant;
            bcolor.SetTitle("Цвет рамки");
            bcolor.GetCurrentColorDelegate = GetBorderColor;
            bcolor.SetColorDelegate = SetBorderColor;

            TextPropsHolder.VisualPropsList.Add(bcolor);

            StringProps BorderWidth = new StringProps();
            BorderWidth.PropsEditType = StringProps.EditType.Line;
            BorderWidth.Title = "Толщина линии";
            BorderWidth.GetCurrentValueDelegate = GetBorderWidth;
            BorderWidth.SetChangedValueDelegate += SetBorderWidth;

            TextPropsHolder.VisualPropsList.Add(BorderWidth);

            DropListProps TextFontName = new DropListProps();
            TextFontName.SetTitle("Шрифт");
            TextFontName.Items = IVSMUtils.GetSysFontsNames();

            TextFontName.GetCurrentValueDelegate += GetFontName;
            TextFontName.SetPropertyDelegate += SetTextFont;

            TextPropsHolder.VisualPropsList.Add(TextFontName);

            DropListProps TextSize = new DropListProps();
            TextSize.SetTitle("Размер шрифта");
            for(int i=8; i <=70; i=i + 2)
            {
                TextSize.Items.Add(i.ToString());
            }
            TextSize.GetCurrentValueDelegate += GetFontSize;
            TextSize.SetPropertyDelegate += SetTextSize;

            TextPropsHolder.VisualPropsList.Add(TextSize);

            DropListProps TextAlight = new DropListProps();
            TextAlight.SetTitle("Выравнивание");
            TextAlight.Items.Add("Слева");
            TextAlight.Items.Add("По центру");
            TextAlight.Items.Add("Справа");
            TextAlight.GetCurrentValueDelegate += GetCurrentAlight;
            TextAlight.SetPropertyDelegate += SetTextAlight;

            TextPropsHolder.VisualPropsList.Add(TextAlight);
        }

        public string GetText()
        {
            return Text;
        }

        public Size GetSize() => this.Size;

        public void SetText(string bw)
        {
            Text = bw;
            TextAlignment alight = FormantLableText.TextAlignment;
            FormantLableText = new FormattedText(Text, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), TextFontSize, TextBrush);
            FormantLableText.TextAlignment = alight;
            UpdateFormatCellText();
            DrawUI();
        }

        public Color GetTextColor() => ((SolidColorBrush)(TextBrush)).Color;
        public void SetTextColor(Color c)
        {
            TextBrush = new SolidColorBrush(c);
            FormantLableText.SetForegroundBrush(TextBrush);
            DrawUI();
        }

        public Color GetBackColor() => ((SolidColorBrush)(FillBrush)).Color;
        public void SetBackColor(Color c)
        {
            if(c == Colors.Transparent)
            {
                IsFillTransparant = true;
            }
            else
            {
                IsFillTransparant = false;
            }
            FillBrush = new SolidColorBrush(c);
            DrawBrush = FillBrush;
            DrawUI();
        }

        public Color GetBorderColor() => ((SolidColorBrush)(BorderBrush)).Color;
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
            BorderBrush = new SolidColorBrush(c);
            DrawUI();
        }


        public string GetBorderWidth()
        {
            return PenWidth.ToString();
        }

        public void SetBorderWidth(string bw)
        {
            double width;
            if(double.TryParse(bw,out width)) {
                if (width >= 0 && width < 10)
                {
                    PenWidth = width;
                    DrawUI();
                }
            }
        }

        public string GetFontName()
        {
            return FontFamily;
        }

        public void SetTextFont(string font)
        {
            if (FontFamily != font)
            {
                FormantLableText.SetFontFamily(font);
                FontFamily = font;
                DrawUI();
            }
        }

        public string GetFontSize()
        {
            return TextFontSize.ToString();
        }

        public void SetTextSize(string size)
        {
            double text_size;
            if (double.TryParse(size, out text_size))
            {
                FormantLableText.SetFontSize(text_size);
                TextFontSize = text_size;
                DrawUI();
            }
        }

        public string GetTextAlight() => FormantLableText.TextAlignment.ToString();

        public void SetTextAlightS(string s)
        {
            switch(s)
            {
                case "Left":
                    FormantLableText.TextAlignment = TextAlignment.Left;
                    break;
                case "Center":
                    FormantLableText.TextAlignment = TextAlignment.Center;
                    break;
                case "Right":
                    FormantLableText.TextAlignment = TextAlignment.Right;
                    break;
            }
        }

        public string GetCurrentAlight()
        {
            if (FormantLableText.TextAlignment == TextAlignment.Left)
            {
                return "Слева";
            }

            if (FormantLableText.TextAlignment == TextAlignment.Center)
            {
                return "По центру";
            }

            if (FormantLableText.TextAlignment == TextAlignment.Right)
            {
                return "Справа";
            }

            return "";
        }

        private void SetTextAlight(string bw)
        {
           switch(bw)
            {
                case "Слева":
                    if (FormantLableText.TextAlignment != TextAlignment.Left)
                    {
                        FormantLableText.TextAlignment = TextAlignment.Left;
                        DrawUI();
                    }
                    break;
                case "По центру":
                    if (FormantLableText.TextAlignment != TextAlignment.Center)
                    {
                        FormantLableText.TextAlignment = TextAlignment.Center;
                        DrawUI();
                    }
                    break;
                case "Справа":
                    if (FormantLableText.TextAlignment != TextAlignment.Right)
                    {
                        FormantLableText.TextAlignment = TextAlignment.Right;
                        DrawUI();
                    }
                    break;
            }
        }

        public MTextLable()
        {
            IsFillTransparant=false;
            IsBorderTransparant=false;
            SBSelect = false;

            SelectPen = new Pen(Brushes.Gray, 1);
            SelectPen.DashStyle = DashStyles.Dash;

            PenWidth = 2;


            FillBrush = Brushes.LightSkyBlue;
            //  FocusedBrush = Brushes.LightGreen;
            FocusedBrush = new SolidColorBrush(Colors.LightGray);
            FocusedBrush.Opacity = 0.4;
            SelectBrush = Brushes.DarkGreen;
            DrawBrush = FillBrush;

            BorderBrush = Brushes.Blue;

            TextBrush = Brushes.Black;
            TextFontSize = 14;
            Text = "Text";

            Size = new Size(70, 50);
            Location = new Point(30, 30);
            Visible = true;

            SizeButton = new VisualButton();
            SizeButton.SetSize(10, 10);
            SizeButton.SetButtonColor(Colors.LightGray);
            SizeButton.SetLocation(1, 1);

            SizeMarkBrush = new SolidColorBrush(Colors.LightBlue);
            SizeMarkBrush.Opacity = 0.5;

            IsSelect = false;

            CreatePropertyHolders();

            //UpdateFormatCellText();
            //  CurrentAlignment = TextAlignment.Left;
            FontFamily = "Verdana";
            FormantLableText = new FormattedText(Text, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FontFamily), TextFontSize, TextBrush);

        }

        private void UpdateFormatCellText()
        {
           
            FormantLableText.MaxTextWidth = Size.Width - 3;

            FormantLableText.MaxTextHeight = Size.Height - 3;
        }

        public override void MouseDown(Point e)
        {
            //if(SizeButton.CheckHit(e))
            //{
            //    MessageBox.Show("fdfdf");
            //}
        }

        public override void MouseEnter()
        {
            //if (IsSelect == false)
            //{
            //    DrawBrush = FocusedBrush;
            //    DrawUI();
            //}
        }

        public override void MouseLeave()
        {
            //if (IsSelect == false)
            //{
            //    DrawBrush = FillBrush;
            //    DrawUI();
            //}
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

            if(SizeButton.CheckHit(e) ==false && SBSelect == true)
            {
              
                SizeButton.Unselect();
                SBSelect = false;
                DrawUI();
                return;
                
            }
        }

        public override void MouseUp(Point e)
        {
            //  throw new NotImplementedException();
        }

        public override void Select()
        {
        //    DrawBrush = SelectBrush;
            IsSelect = true;
            DrawUI();
        }

        public override void Unselect()
        {
         //   DrawBrush = FillBrush;
            IsSelect = false;
            DrawUI();
        }

        public override void DrawUI()
        {
            DrawingContext dc = this.RenderOpen();
            if (Visible == true)
            {
                dc.DrawRoundedRectangle(DrawBrush, new Pen(BorderBrush, PenWidth), new Rect(Location.X, Location.Y, Size.Width, Size.Height), 5, 5);
                double l_y = ((Size.Height - 10) / 2 - FormantLableText.Height / 2);
                dc.DrawText(FormantLableText, new Point(Location.X + 3, Location.Y + 3 + l_y));
            }

            if(IsSelect)
            {
                //  dc.DrawRectangle(SizeMarkBrush, new Pen(BorderBrush, 0), new Rect((Location.X + Size.Width / 2)-4, Location.Y - 35, 8, 30));
                dc.DrawRoundedRectangle(Brushes.Transparent, SelectPen, new Rect(Location.X-3, Location.Y-3, Size.Width+6, Size.Height+6), 1, 1);
                SizeButton.Draw(dc);
                // dc.DrawRectangle(SizeMarkBrush, new Pen(BorderBrush, 0), new Rect((Location.X + Size.Width / 2) - 4, Location.Y - 35, 8, 30));
            }

            dc.Close();
        }

        public override void SetLocation(double x, double y)
        {
            Location.X = x;
            Location.Y = y;

            SizeButton.SetLocation(x + Size.Width+3, y+Size.Height+3);

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

        public bool IsSetSizeMode()
        {
            return SBSelect;
        }

        public void SetSize(double width_inc, double height_inc)
        {
            if (width_inc > MinWidth)
            {
                Size.Width = width_inc;
                UpdateFormatCellText();
            }
            if (height_inc > MinHeigth)
            {
                Size.Height = height_inc;
                UpdateFormatCellText();
            }

            SizeButton.SetLocation(Location.X + Size.Width, Location.Y + Size.Height); 
            DrawUI();
        }

        public PropsHolder GetPropsHolder()
        {
            return TextPropsHolder;
        }

        public Node CreateDomNode()
        {
            return MTextLableDom.Get().CreateRootNode(this);
        }
    }
}
