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
    public class ColorItem : Item
    {
        private TextBlock ItemLable;
     //   private Button ColorButton;
        private Rectangle rect;    

        BrushConverter converter;

        private ColorProps Props;

        private Line Separatior;

        public ColorItem()
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
            

            rect.StrokeThickness = 0;
            rect.Stroke = Brushes.LightGreen;

            //ColorButton = new Button();
            //ColorButton.BorderThickness = new Thickness(0);
            //ColorButton.Height = 20;
            //ColorButton.Content = "";
            //ColorButton.Click += ColorButtonClick;

            Separatior = new Line();
            Separatior.X1 = 0;

            Separatior.StrokeThickness = 1;
            Separatior.Stroke = (SolidColorBrush)converter.ConvertFrom("#C8C8C8");
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

            int[] OleColors = new int[GlobalStore.LastColors.Length+1];

            for (int i = 0; i <= GlobalStore.LastColors.Length - 1; i++)
            {
                //OleColors[i] = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(GlobalStore.LastColors[i].A, GlobalStore.LastColors[i].R,
                //    GlobalStore.LastColors[i].G, GlobalStore.LastColors[i].B));
                OleColors[i] = System.Drawing.ColorTranslator.ToOle(GlobalStore.LastColors[i]);
            }

            System.Drawing.Color cur_color = System.Drawing.Color.FromArgb(Props.GetCurrentColorDelegate().A, Props.GetCurrentColorDelegate().R, Props.GetCurrentColorDelegate().G, Props.GetCurrentColorDelegate().B);

            OleColors[GlobalStore.LastColors.Length] = System.Drawing.ColorTranslator.ToOle(cur_color);
            //    GlobalStore.LastColors[i].G, GlobalStore.LastColors[i].B));

            dlg.CustomColors = OleColors;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Props.SetColorDelegate(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                rect.Fill = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                GlobalStore.AddLastColor(dlg.Color);
            }
        }

        public override void BindingProps(Props props)
        {
            Props = (ColorProps)props;
            ItemLable.Text = Props.GetTitle();
            rect.Fill =  new SolidColorBrush(Props.GetCurrentColorDelegate());
        //    ColorButton.Content = rect;
        //    ColorButton.Background = 
            
        }

        public override double GetHeight() => 80;

        public override IEnumerable<UIElement> GetUIElement()
        {
            return new List<UIElement>
            {       
                ItemLable,
                rect,
                Separatior
            };
        }

        public override void SetTop(double top_y)
        {
            Canvas.SetTop(ItemLable, top_y + 5);
            Canvas.SetTop(rect, top_y + 35);

            Separatior.Y1 = Separatior.Y2 = top_y + 75;
        }

        public override void SetWidth(double widht)
        {
            ItemLable.Width = widht - SideMargin * 2;
          //  ColorButton.Width = widht - SideMargin * 2;
            rect.Width = widht - SideMargin * 2;
            Separatior.X2 = widht;
        }
    }
}
