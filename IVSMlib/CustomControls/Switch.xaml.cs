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
    /// Логика взаимодействия для Switch.xaml
    /// </summary>
    public partial class Switch : UserControl
    {

        public bool IsEditMode
        {
            get { 
                return (bool)GetValue(MyPropertyProperty); 
            }
            set { 
               
                //Console.WriteLine("Settt");
                SetValue(MyPropertyProperty, value); 
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("IsEditMode", typeof(bool), typeof(Switch), new PropertyMetadata(true, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool val = (bool)e.NewValue;
            var sw = (Switch)d;
            if (val == true)
            {
               
                sw.OnEdit();
            }
            else
            {
                sw.OffEdit();
            }
        }

        private bool IsEdit;
        private double OffEditLeft = 23;
        private double OnEditLeft = 0;
        private Color c_red;
        private Color c_green;

        private Brush Edit_Brush;
        private Brush View_Brush;


        public Switch()
        {
            InitializeComponent();
            c_red = System.Windows.Media.Color.FromRgb(225, 36, 0);

            Edit_Brush = new SolidColorBrush(c_red);

            c_green = System.Windows.Media.Color.FromRgb(6, 125, 70);
            IsEdit = true;

            View_Brush = new SolidColorBrush(c_green);

            swich_el.Fill = Edit_Brush;

            IsEditMode = true;

        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsEdit == false)
            {
                OnEdit();
                IsEditMode = true;
            }
            else
            {
                OffEdit();
                IsEditMode = false;
            }
        }

        private void OnEdit()
        {
            //    Canvas.SetLeft(swich_el, OnEditLeft);
            swich_el.Fill = Edit_Brush;
            Lable.Text = "Редактирование";
            IsEdit = true;
            

            TranslateTransform trans = new TranslateTransform();
            swich_el.RenderTransform = trans;

            DoubleAnimation XTrans = new DoubleAnimation();
            XTrans.From = OffEditLeft;
            XTrans.To = OnEditLeft;
            XTrans.Duration = TimeSpan.FromMilliseconds(170);
            trans.BeginAnimation(TranslateTransform.XProperty, XTrans);

            //swich_el.rende
        }

        private void OffEdit()
        {
            //     Canvas.SetLeft(swich_el, OffEditLeft);
            swich_el.Fill = View_Brush;
            Lable.Text = "Просмотр";

            TranslateTransform trans = new TranslateTransform();
            swich_el.RenderTransform = trans;

            DoubleAnimation XTrans = new DoubleAnimation();
            XTrans.From = OnEditLeft;
            XTrans.To = OffEditLeft;
            XTrans.Duration = TimeSpan.FromMilliseconds(170);
            trans.BeginAnimation(TranslateTransform.XProperty, XTrans);

            IsEdit = false;
           
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            rect.Fill = Brushes.LightGray;
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            rect.Fill = Brushes.White;
        }
    }
}
