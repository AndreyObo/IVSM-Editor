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

using IVSMlib.ViewModel;

namespace IVSMlib
{
    /// <summary>
    /// Логика взаимодействия для DocumentBar.xaml
    /// </summary>
    public partial class DocumentBar : UserControl
    {
        public DocumentBar()
        {
            InitializeComponent();
            this.DataContext = new DocumentBarVM();
        }
    }
}
