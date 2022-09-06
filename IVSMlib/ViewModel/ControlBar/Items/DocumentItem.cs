using IVSMlib.PropsHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using IVSMlib.Link;
using IVSMlib.ViewModel.Units;

namespace IVSMlib.ViewModel.ControlBar.Items
{
    public class DocumentItem : Item
    {
        private TextBlock ItemLable;
        private ComboBox DocComboBox;
        private Button AddDocButton;
        private ListBox DocListBox;
        private Button DelDocButton;

        private Line Separatior;

        BrushConverter converter;

        private DocumentListProps Props;

        private void AddDocButtonClick(object sender, RoutedEventArgs e)
        {
            if(DocComboBox.SelectedIndex !=-1)
            {
              //  bool doc_is_exist = false;
                foreach(DocumentUnit du in Props.GetCurrentValueDelegate())
                {
                    if (du.DocLable == DocComboBox.Text)
                    {
                    //    Console.WriteLine(DocComboBox.Text);
                        MessageBox.Show("Данный документ уже имееться в списке");
                        return;
                    }
                }
                    //  Props.AddDocumentCallback(du);
                foreach (DocumentUnit docs in DocConnector.CurrentDocList)
                {

                        if (docs.DocLable == DocComboBox.Text)
                        {
                            Props.AddDocumentCallback(docs);
                            DocListBox.Items.Add(docs.DocLable);
                            break;
                        }
                }
             
            }
                    
        }

        private void DelDocButtonClick(object sender, RoutedEventArgs e)
        {
            if (DocListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Ничего не выбранно");
                return;
            }

            foreach (DocumentUnit docs in DocConnector.CurrentDocList)
            {

                if (docs.DocLable == DocListBox.Items[DocListBox.SelectedIndex].ToString())
                {
                    Props.DeleteDocumentCallback(docs);
                    DocListBox.Items.RemoveAt(DocListBox.SelectedIndex);
                    break;
                }
            }
            //   var s = DocListBox.Items[DocListBox.SelectedIndex].ToString();
            // MessageBox.Show(s);
        }

        public DocumentItem()
        {
            converter = new BrushConverter();

            ItemLable = new TextBlock();
            Canvas.SetLeft(ItemLable, SideMargin);
            ItemLable.Foreground = (SolidColorBrush)converter.ConvertFrom("#5E5E5E");
            ItemLable.FontSize = 14;

            DocComboBox = new ComboBox();
            DocComboBox.FontSize = 14;
            DocComboBox.Height = 30;
            DocComboBox.Background = (SolidColorBrush)converter.ConvertFrom("#EEEEEE");
            DocComboBox.BorderBrush = (SolidColorBrush)converter.ConvertFrom("#B1B1B1");
           // DocComboBox.SelectionChanged += ComboBoxSelect;
            Canvas.SetLeft(DocComboBox, SideMargin);

            AddDocButton = new Button();
            AddDocButton.Foreground = (SolidColorBrush)converter.ConvertFrom("#5E5E5E");
            AddDocButton.Height = 30;
            AddDocButton.Content = "Добавить";
            AddDocButton.Click += AddDocButtonClick;
            AddDocButton.BorderThickness = new Thickness(0);

            DocListBox = new ListBox();
            DocListBox.Height = 60;
            DocListBox.Foreground = (SolidColorBrush)converter.ConvertFrom("#5E5E5E");
            Canvas.SetLeft(DocListBox, SideMargin);


            DelDocButton = new Button();
            DelDocButton.Foreground = (SolidColorBrush)converter.ConvertFrom("#5E5E5E");
            DelDocButton.BorderThickness = new Thickness(0);
            DelDocButton.Height = 30;
            DelDocButton.Content = "Удалить";
            DelDocButton.Click += DelDocButtonClick;
            Canvas.SetLeft(DelDocButton, SideMargin);

            Separatior = new Line();
            Separatior.X1 = 0;

            Separatior.StrokeThickness = 1;
            Separatior.Stroke = (SolidColorBrush)converter.ConvertFrom("#C8C8C8");
        }

        public override void BindingProps(Props props)
        {
            Props = (DocumentListProps)props;
            ItemLable.Text = Props.Title;
            foreach(DocumentUnit u in DocConnector.CurrentDocList)
            {
                DocComboBox.Items.Add(u.DocLable);
            }
            foreach(DocumentUnit doc in Props.GetCurrentValueDelegate())
            {
                DocListBox.Items.Add(doc.DocLable);
            }
         //   throw new NotImplementedException();
        }

        public override double GetHeight() => 215;

        public override IEnumerable<UIElement> GetUIElement()
        {
            return new List<UIElement>
            {
              ItemLable,
              DocComboBox,
              AddDocButton,
              DocListBox,
              DelDocButton,
              Separatior,
            };
        }

        public override void SetTop(double top_y)
        {
            Canvas.SetTop(ItemLable, top_y + 5);
            Canvas.SetTop(DocComboBox, top_y + 40);
            Canvas.SetTop(AddDocButton, top_y + 40);
            Canvas.SetTop(DocListBox, top_y + 80);
            Canvas.SetTop(DelDocButton, top_y + 150);
            Canvas.SetTop(Separatior, top_y + 200);
        }

        public override void SetWidth(double widht)
        {
            ItemLable.Width = widht - SideMargin * 2;
            DocComboBox.Width = (widht - SideMargin * 2) * 0.60;
            AddDocButton.Width = (widht - SideMargin * 2) * 0.35;
            Canvas.SetLeft(AddDocButton, SideMargin + DocComboBox.Width + ((widht - SideMargin * 2) * 0.05));
            DocListBox.Width = widht - SideMargin * 2;
            DelDocButton.Width= widht - SideMargin * 2;
            Separatior.X2 = widht;
        }
    }
}
