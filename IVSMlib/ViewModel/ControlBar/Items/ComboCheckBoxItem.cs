using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using IVSMlib.PropsHolders;


namespace IVSMlib.ViewModel.ControlBar.Items
{
    public class ComboCheckBoxItem:Item
    {
        private ComboBox EditComboBox;
        private TextBlock ItemLable;
        private CheckBox ItemCheck;

        private Line Separatior;

        public delegate void SetText(string text);

        DropCheckListProps Props;
        BrushConverter converter;



        public ComboCheckBoxItem()
        {
            converter = new BrushConverter();

            ItemLable = new TextBlock();
            Canvas.SetLeft(ItemLable, SideMargin);
            ItemLable.Foreground = (SolidColorBrush)converter.ConvertFrom("#5E5E5E");
            ItemLable.FontSize = 14;

            EditComboBox = new ComboBox();
            EditComboBox.FontSize = 14;
            EditComboBox.Height = 30;
            EditComboBox.Background = (SolidColorBrush)converter.ConvertFrom("#EEEEEE");
            EditComboBox.BorderBrush = (SolidColorBrush)converter.ConvertFrom("#B1B1B1");

            EditComboBox.SelectionChanged += ItemChange;
            Canvas.SetLeft(EditComboBox, SideMargin);

            ItemCheck = new CheckBox();
            ItemCheck.FontSize = 12;
            ItemCheck.Click += CheckEvent;
            Canvas.SetLeft(ItemCheck, SideMargin);

            Separatior = new Line();
            Separatior.X1 = 0;

            Separatior.StrokeThickness = 1;
            Separatior.Stroke = (SolidColorBrush)converter.ConvertFrom("#C8C8C8");
        }

        private void CheckEvent(object sender, RoutedEventArgs e)
        {
            Props.OnCheckDelegate?.Invoke(((CheckBox)sender).IsChecked.Value);
        }

        private void ItemChange(object sender, SelectionChangedEventArgs e)
        {
            // Console.WriteLine(((ComboBox)sender).SelectedValue.ToString());
            Props.SetPropertyDelegate?.Invoke(((ComboBox)sender).SelectedValue.ToString());
        }

        public override void BindingProps(Props props)
        {
            Props = (DropCheckListProps)props;
            ItemLable.Text = Props.GetTitle();
            foreach (string s in Props.Items)
            {
                EditComboBox.Items.Add(s);
            }
            EditComboBox.Text = Props.GetCurrentValueDelegate();
        }

        public void SetLableText(string text)
        {
            ItemLable.Text = text;
        }

        public void SetTextBoxText(string text)
        {
            EditComboBox.Text = text;
        }

        public override void SetTop(double top_y)
        {
            Canvas.SetTop(ItemLable, top_y + 5);
            Canvas.SetTop(EditComboBox, top_y + 35);
            Canvas.SetTop(ItemCheck, top_y + 55);

            Separatior.Y1 = Separatior.Y2 = top_y + 90;
        }

        public override void SetWidth(double widht)
        {
            ItemLable.Width = widht - SideMargin * 2;
            EditComboBox.Width = widht - SideMargin * 2;
            Separatior.X2 = widht;
        }

        public override IEnumerable<UIElement> GetUIElement()
        {
            return new List<UIElement>
            {
                EditComboBox,
                ItemLable,
                ItemCheck,
                Separatior
            };

        }

        public override double GetHeight() => 110;
    }
}
