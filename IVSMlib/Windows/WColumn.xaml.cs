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
    /// Логика взаимодействия для WColumn.xaml
    /// </summary>
    public partial class WColumn : Window
    {
        private Int32 column;

        public delegate void InsertCol(Int32 col);
        public event InsertCol InsertColClick;

        public delegate void DelCol(Int32 col);
        public event DelCol DelColClick;

        //public delegate void LeftCol(Int32 col);
        //public event LeftCol LeftColClick;

        public WColumn(Int32 col,Int32 col_cout)
        {
           
            InitializeComponent();

            if (col == 0)
            {
                Left_Button.Visibility = Visibility.Hidden;
            }
            if (col == col_cout - 1)
            {
                Right_Button.Visibility = Visibility.Hidden;
            }

            column = col;
        }

        private void Left_Col(object sender, RoutedEventArgs e)
        {
            this.Close();
            InsertColClick?.Invoke(column);
            //LeftColClick?.Invoke(column);
        }

        private void Del_Col(object sender, RoutedEventArgs e)
        {
            this.Close();
            DelColClick?.Invoke(column);
        }

        private void Right_Col(object sender, RoutedEventArgs e)
        {
            this.Close();
            InsertColClick?.Invoke(column+1);
        }
    }
}
