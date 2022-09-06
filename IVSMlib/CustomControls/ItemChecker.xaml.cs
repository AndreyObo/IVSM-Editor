using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IVSMlib.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для ItemChecker.xaml
    /// </summary>
    public partial class ItemChecker : UserControl
    {
        // private struct 


        public string Mode
        {
            get { return (string)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("Mode", typeof(string), typeof(ItemChecker), new PropertyMetadata("Operation"));



        private double current_pos;

        private Brush switcher_brush;
        private Brush switcher_brush_focus;
        private Brush FocusBrush;
        public ItemChecker()
        {
            InitializeComponent();
            current_pos = Canvas.GetLeft(swither);
            switcher_brush = new SolidColorBrush(Color.FromRgb(143, 143, 154));
            switcher_brush_focus = new SolidColorBrush(Color.FromRgb(119, 119, 124));
            swither.Fill = switcher_brush;
            FocusBrush = Brushes.WhiteSmoke;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void operation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TranslateTransform trans = new TranslateTransform();
            swither.RenderTransform = trans;

            DoubleAnimation XTrans = new DoubleAnimation();
            XTrans.From = 100;
            XTrans.To = 0;
            XTrans.Duration = TimeSpan.FromMilliseconds(250);
            trans.BeginAnimation(TranslateTransform.XProperty, XTrans);
            current_pos = 1;

            Mode = "Operation";
        }

        private void condition_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TranslateTransform trans = new TranslateTransform();
            swither.RenderTransform = trans;

            DoubleAnimation XTrans = new DoubleAnimation();
            XTrans.From = 0;
            XTrans.To = 100;
            XTrans.Duration = TimeSpan.FromMilliseconds(250);
            trans.BeginAnimation(TranslateTransform.XProperty, XTrans);

            Mode = "Condition";
            //    current_pos = 100;
        }

        private void swither_MouseEnter(object sender, MouseEventArgs e)
        {
           
        }

        private void swither_MouseLeave(object sender, MouseEventArgs e)
        {
           
        }

        private void ButtonEnter(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = FocusBrush;
        }

        private void ButtonLeave(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = Brushes.White;
        }
    }
}
