using IVSMlib.ViewModel;
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
using IVSMlib.Language;

namespace IVSMlib
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            StringRes.LoadDictionary();
            InitializeComponent();
            (CB.DataContext as ControlBarVM).SetTAbleVM((MainTableVM)table.DataContext);
            (CB.DataContext as ControlBarVM).SetDocBar((DocumentBarVM)DocBar.DataContext);
            (table.DataContext as MainTableVM).SetPropertyBarVM((PropertyBarVM)PropertyBar.DataContext);
            (table.DataContext as MainTableVM).SetControlBar((ControlBarVM)CB.DataContext);
            (DocBar.DataContext as DocumentBarVM).SetPropertyBarVM((PropertyBarVM)PropertyBar.DataContext);
            (DocBar.DataContext as DocumentBarVM).SetMainTableVM((MainTableVM)table.DataContext);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
         //   Console.WriteLine("Main clickkkk");
        }
    }
}
