using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;


namespace IVSMlib.VsmCanvas.MouseBehavior
{
    class MouseBehavior : Behavior<Canvas>
    {
   //     public delegate void MouseDown();

        public static readonly DependencyProperty MouseYProperty = DependencyProperty.Register(
         "MouseY", typeof(double), typeof(MouseBehavior), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty MouseXProperty = DependencyProperty.Register(
           "MouseX", typeof(double), typeof(MouseBehavior), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty LeftMouseBS = DependencyProperty.Register(
          "LeftButtonState", typeof(MouseButtonState), typeof(MouseBehavior), new PropertyMetadata(default(MouseButtonState)));

        public static readonly DependencyProperty RightMouseBS = DependencyProperty.Register(
         "RightButtonState", typeof(MouseButtonState), typeof(MouseBehavior), new PropertyMetadata(default(MouseButtonState)));

        public ICommand MouseUpEv
        {
            get { return (ICommand)GetValue(MouseUpEvProperty); }
            set { SetValue(MouseUpEvProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseUpEv.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseUpEvProperty =
            DependencyProperty.Register("MouseUpEv", typeof(ICommand), typeof(MouseBehavior), new PropertyMetadata(null));


        public ICommand MouseDownEv
        {
            get { return (ICommand)GetValue(MouseDownEvProperty); }
            set { SetValue(MouseDownEvProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseDownEv.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseDownEvProperty =
            DependencyProperty.Register("MouseDownEv", typeof(ICommand), typeof(MouseBehavior), new PropertyMetadata(null));



        public double MouseY
        {
            get { return (double)GetValue(MouseYProperty); }
            set { SetValue(MouseYProperty, value); }
        }

        public double MouseX
        {
            get { return (double)GetValue(MouseXProperty); }
            set { SetValue(MouseXProperty, value); }
        }

        public MouseButtonState LeftButtonState
        {
            get { return (MouseButtonState)GetValue(LeftMouseBS); }
            set { SetValue(LeftMouseBS, value); }
        }

        public MouseButtonState RightButtonState
        {
            get { return (MouseButtonState)GetValue(RightMouseBS); }
            set { SetValue(RightMouseBS, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.MouseDown += OnClick;
            AssociatedObject.MouseMove += MouseMove;
            AssociatedObject.MouseUp += AssociatedObject_MouseUp;
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
        }

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
          //  ((MainTableVM)(AssociatedObject.DataContext)).FieldCanvas = (VsmCanvas)AssociatedObject;
        }

        private void AssociatedObject_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUpEv.Execute(null);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseDown -= OnClick;
            AssociatedObject.MouseMove -= MouseMove;
        }
        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine(e.LeftButton.ToString());
            //     Console.WriteLine(e.RightButton.ToString());
            LeftButtonState = e.LeftButton;
            RightButtonState = e.RightButton;
            MouseDownEv.Execute(null);
            // Console.WriteLine("Clekk");
            //Point p = e.GetPosition((Canvas)sender);
            //MouseX = p.X;
            //MouseY = p.Y;
            //SrMap.ChangeColor((Canvas)sender);
            //MessageBox.Show("x - " + p.X.ToString());
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition((Canvas)sender);
            MouseX = p.X;
            MouseY = p.Y;
            //   MessageBox.Show("x - " + p.X.ToString());
        }
    }
}
