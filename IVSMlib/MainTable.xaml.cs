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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using IVSMlib.VsmCanvas;
using IVSMlib.ViewModel;

namespace IVSMlib
{
    /// <summary>
    /// Логика взаимодействия для MainTable.xaml
    /// </summary>
    public partial class MainTable : UserControl
    {
        public int MyProperty
        {
            get { return (int)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(MainTableVM), new PropertyMetadata(0));


        public MainTable()
        {
            InitializeComponent();
            this.DataContext = new MainTableVM(Field);

            (DataContext as MainTableVM).window = this;
        }
    }
}
