using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using IVSMlib.Types;
using IVSMlib.Windows.ViewModels;

namespace IVSMlib.Windows
{
    /// <summary>
    /// Логика взаимодействия для WSetAxisMeg.xaml
    /// </summary>
    /// 
    
    public partial class WSetAxisMeg : Window
    {
        public delegate void SetTimes(Time.Type act_type, Time.Type wst_type, Time.Type mv_type);
        public SetTimes SetTimesCallback;

        public WSetAxisMeg(Time.Type act_type, Time.Type wst_type, Time.Type mv_type)
        {
            InitializeComponent();

            this.DataContext = new WSetAxisMegVM(act_type, wst_type, mv_type);
        }

        private void AbortClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DoneClick(object sender, RoutedEventArgs e)
        {
            SetTimesCallback.Invoke((DataContext as WSetAxisMegVM).GetActionType(), (DataContext as WSetAxisMegVM).GetWasteType(), (DataContext as WSetAxisMegVM).GetMoveType());
        }
    }
}
