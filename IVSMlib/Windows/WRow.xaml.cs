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
using System.Windows.Shapes;

namespace IVSMlib.Windows
{
    /// <summary>
    /// Логика взаимодействия для WRow.xaml
    /// </summary>
    public partial class WRow : Window
    {
        private Int32 row;

        public delegate void InsertRow(Int32 row);
        public event InsertRow InsertRowClick;

        public delegate void DelRow(Int32 row);
        public event DelRow DelRowClick;

        public WRow(Int32 _row, Int32 row_cout)
        {
            InitializeComponent();

            if (_row == 0)
            {
                Up_Button.Visibility = Visibility.Hidden;
            }
            if (_row == row_cout - 1)
            {
                Down_Button.Visibility = Visibility.Hidden;
            }

            row = _row;
        }

        private void Del_Row(object sender, RoutedEventArgs e)
        {
            DelRowClick?.Invoke(row);
            this.Close();
        }

        private void Up_Row(object sender, RoutedEventArgs e)
        {
            InsertRowClick?.Invoke(row);
            this.Close();
        }

        private void Down_Row(object sender, RoutedEventArgs e)
        {
            InsertRowClick?.Invoke(row+1);
            this.Close();
        }
    }
}
