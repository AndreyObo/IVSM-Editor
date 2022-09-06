using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IVSMlib.Utils;
using IVSMlib.ViewModel;
using IVSMlib.Windows.ViewModels;

namespace IVSMlib.Windows
{
    /// <summary>
    /// Логика взаимодействия для WSaveAsImage.xaml
    /// </summary>
    public partial class WSaveAsImage : Window
    {
        public delegate void SaveParam(string file_name, ImportManager.ImgFormat format, bool show_time_t, bool show_doc, bool show_problem);
        public event SaveParam SaveParamEvent;

        public WSaveAsImage()
        {
            InitializeComponent();
            //DataContext = new WSaveAsImageVM(SaveField, table);
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save_dlg = new SaveFileDialog();
           // open_dlg.Filter = "Png(*.png)|*.pmg|Jpg(*.jpg)|*.jpg|Bmp(*.bmp)|*.bmp";

            if (save_dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            // получаем выбранный файл
            FileTB.Text = save_dlg.FileName;
           // string filename = open_dlg.FileName;
            // читаем файл в строку
        }

        private void AbrortClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if(FileTB.Text.Length == 0)
            {
                System.Windows.MessageBox.Show("Имя файла не указанно");
                FileTB.Focus();
                return;
            }

            ImportManager.ImgFormat format = ImportManager.ImgFormat.PNG;

            switch(FormatCB.Text)
            {
                case "PNG":
                    format = ImportManager.ImgFormat.PNG;
                    break;
                case "JPG":
                    format = ImportManager.ImgFormat.JPG;
                    break;
                case "BMP":
                    format = ImportManager.ImgFormat.BMP;
                    break;
            }

            SaveParamEvent?.Invoke(FileTB.Text, format, TimeCB.IsChecked.Value, DocCB.IsChecked.Value, PropblemCB.IsChecked.Value);
            this.Close();
        }
    }
}
