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
    /// Логика взаимодействия для WSetTableSize.xaml
    /// </summary>
    public partial class WSetTableSize : Window
    {
  //      private bool ApplayParam;
        public WSetTableSize(double table_width, double table_heigth)
        {
            InitializeComponent();
            //   ApplayParam = false;
            WidthTB.Text = table_width.ToString();
            HeightTB.Text = table_heigth.ToString();

        }

        public delegate void WinParam(double widht, double height, bool ChangeCellSize);

        public WinParam WinParamCallback;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            //if(WinParamCallback != null && ApplayParam)
            //{
            //    WinParamCallback.Invoke(12, 45, false);
            //}
        }

        private void AbortClick(object sender, RoutedEventArgs e)
        {
       //     ApplayParam = false;
            this.Close();
        }

        private void DoneClick(object sender, RoutedEventArgs e)
        {
            double width;
            double height;
            if (WinParamCallback != null)
            {
                if(double.TryParse(WidthTB.Text, out width) == false)
                {
                    MessageBox.Show("Неверное значение параметра width");
                    WidthTB.Focus();
                    return;
                }

                if (double.TryParse(HeightTB.Text, out height) == false)
                {
                    MessageBox.Show("Неверное значение параметра height");
                    HeightTB.Focus();
                    return;
                }
                WinParamCallback.Invoke(width, height, CellSizeCB.IsChecked.Value);
            }

            this.Close();
        }
    }
}
