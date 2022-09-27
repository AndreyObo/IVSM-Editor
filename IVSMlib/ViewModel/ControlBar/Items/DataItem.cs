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
    public class DataItem:Item
    {
        private DatePicker EditDataPicker;
        private TextBlock ItemLable;

        private Line Separatior;

        public delegate void SetText(string text);

        DateProps Props;
        BrushConverter converter;

        private void DateChange(object sender, SelectionChangedEventArgs e)
        {
        
            Props.SetPropertyDelegate(new DateTime(((DatePicker)sender).SelectedDate.Value.Year, ((DatePicker)sender).SelectedDate.Value.Month, ((DatePicker)sender).SelectedDate.Value.Day));
        }

        public DataItem()
        {
            converter = new BrushConverter();

            ItemLable = new TextBlock();
            Canvas.SetLeft(ItemLable, SideMargin);
            ItemLable.Foreground = (SolidColorBrush)converter.ConvertFrom("#5E5E5E");
            ItemLable.FontSize = 14;

            EditDataPicker = new DatePicker();
            EditDataPicker.FontSize = 14;
            EditDataPicker.Height = 30;
            EditDataPicker.Background = (SolidColorBrush)converter.ConvertFrom("#EEEEEE");
            EditDataPicker.BorderBrush = (SolidColorBrush)converter.ConvertFrom("#B1B1B1");
            EditDataPicker.SelectedDateChanged += DateChange;
            Canvas.SetLeft(EditDataPicker, SideMargin);

            Separatior = new Line();
            Separatior.X1 = 0;

            Separatior.StrokeThickness = 1;
            Separatior.Stroke = (SolidColorBrush)converter.ConvertFrom("#C8C8C8");
        }

        public override void BindingProps(Props props)
        {
            Props = (DateProps)props;
            EditDataPicker.SelectedDate = Props.GetCurrentValueDelegate();
            ItemLable.Text = Props.Title;
        }

        public void SetLableText(string text)
        {
            ItemLable.Text = text;
        }

        public void SetTextBoxText(string text)
        {
            EditDataPicker.Text = text;
        }

        public override void SetTop(double top_y)
        {
            Canvas.SetTop(ItemLable, top_y + 5);
            Canvas.SetTop(EditDataPicker, top_y + 35);

            Separatior.Y1 = Separatior.Y2 = top_y + 85;
        }

        public override void SetWidth(double widht)
        {
            ItemLable.Width = widht - SideMargin * 2;
            EditDataPicker.Width = widht - SideMargin * 2;
            Separatior.X2 = widht;
        }

        public override IEnumerable<UIElement> GetUIElement()
        {
            return new List<UIElement>
            {
                EditDataPicker,
                ItemLable,
                Separatior
            };

        }

        public override double GetHeight() => 100;
    }
}
