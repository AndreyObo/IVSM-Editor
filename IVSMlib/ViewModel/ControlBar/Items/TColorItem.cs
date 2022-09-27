using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using IVSMlib.Global;
using IVSMlib.PropsHolders;
using IVSMlib.PropsHolders.VisualProps;


namespace IVSMlib.ViewModel.ControlBar.Items
{
    public class TColorItem : Item
    {
        private TextBlock ItemLable;
        private Rectangle rect;

        private Button TransparantButton;

        BrushConverter converter;

        private ColorProps Props;

        private Color p_color;

        private Line Separatior;

        public TColorItem()
        {
            converter = new BrushConverter();

            ItemLable = new TextBlock();
            Canvas.SetLeft(ItemLable, SideMargin);
            ItemLable.Foreground = (SolidColorBrush)converter.ConvertFrom("#5E5E5E");
            ItemLable.FontSize = 14;

            rect = new Rectangle();
            Canvas.SetLeft(rect, SideMargin);
            rect.Height = 20;
            rect.MouseDown += ColorButtonClick;
            rect.MouseEnter += RectEnter;
            rect.MouseLeave += RectLeave;

            TransparantButton = new Button();
            Canvas.SetLeft(TransparantButton, SideMargin);
            TransparantButton.Height = 25;
            TransparantButton.Padding = new Thickness(5,0,5,0);
            TransparantButton.Content = "Прозрачный";
            TransparantButton.Background = new SolidColorBrush(Colors.Transparent);
            TransparantButton.Click += ColorTransparantCheck;


            rect.StrokeThickness = 0;
            rect.Stroke = Brushes.LightGreen;

            Separatior = new Line();
            Separatior.X1 = 0;

            Separatior.StrokeThickness = 1;
            Separatior.Stroke = (SolidColorBrush)converter.ConvertFrom("#C8C8C8");
        }

        private void ColorTransparantCheck(object sender, RoutedEventArgs e)
        {
          
            Props.SetColorDelegate(Colors.Transparent);
            rect.Fill = new SolidColorBrush(Colors.Transparent);

        }

        private void RectEnter(object sender, MouseEventArgs e)
        {
            rect.StrokeThickness = 1;
        }

        private void RectLeave(object sender, MouseEventArgs e)
        {
            rect.StrokeThickness = 0;
        }

        private void ColorButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();

            int[] OleColors = new int[GlobalStore.LastColors.Length + 1];

            for (int i = 0; i <= GlobalStore.LastColors.Length - 1; i++)
            {
                OleColors[i] = System.Drawing.ColorTranslator.ToOle(GlobalStore.LastColors[i]);
            }

            System.Drawing.Color cur_color = System.Drawing.Color.FromArgb(Props.GetCurrentColorDelegate().A, Props.GetCurrentColorDelegate().R, Props.GetCurrentColorDelegate().G, Props.GetCurrentColorDelegate().B);

            OleColors[GlobalStore.LastColors.Length] = System.Drawing.ColorTranslator.ToOle(cur_color);

            dlg.CustomColors = OleColors;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Props.SetColorDelegate(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                rect.Fill = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                p_color = Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B);
                GlobalStore.AddLastColor(dlg.Color);
            }
        }

        public override void BindingProps(Props props)
        {
            Props = (ColorProps)props;
            ItemLable.Text = Props.GetTitle();
            rect.Fill = new SolidColorBrush(Props.GetCurrentColorDelegate());

            p_color = Props.GetCurrentColorDelegate();
        }

        public override double GetHeight() => 115;

        public override IEnumerable<UIElement> GetUIElement()
        {
            return new List<UIElement>
            {
                ItemLable,
                rect,
                TransparantButton,
                Separatior
            };
        }

        public override void SetTop(double top_y)
        {
            Canvas.SetTop(ItemLable, top_y + 5);
            Canvas.SetTop(rect, top_y + 35);
            Canvas.SetTop(TransparantButton, top_y + 60);

            Separatior.Y1 = Separatior.Y2 = top_y + 105;
        }

        public override void SetWidth(double widht)
        {
            ItemLable.Width = widht - SideMargin * 2;
            rect.Width = widht - SideMargin * 2;
            
            Separatior.X2 = widht;
        }
    }
}
