using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


using IVSMlib.Language;
using IVSMlib.PropsHolders;
using IVSMlib.Types;

namespace IVSMlib.ViewModel.ControlBar.Items
{
    public class TimeItem : Item
    {
        private TextBox EditTextBox;
        private TextBlock ItemLable;
        private ComboBox EditComboBox;

        private Line Separatior;

        BrushConverter converter;

        private TimeProps Prop;

        private void TextBoxMouseLeave(object sender, RoutedEventArgs e)
        {

            Time.Type old_type = Prop.GetCurrentValueDelegate().Measure;
            Int32 res;
            if(int.TryParse(((TextBox)sender).Text, out res))
            {
                Prop.SetPropertyDelegate(new Time(res, old_type));
            }
            else
            {
                MessageBox.Show("Введено недопустимое значение");
                ((TextBox)sender).Text = "0";
            }
        }

        private void ComboBoxSelect(object sender, SelectionChangedEventArgs e)
        {
            double old_time = Prop.GetCurrentValueDelegate().Count;
            switch (((ComboBox)sender).SelectedIndex)
            {
                case 0:
                    Prop.SetPropertyDelegate(new Time(old_time, Time.Type.Day));
                    break;
                case 1:
                    Prop.SetPropertyDelegate(new Time(old_time, Time.Type.Hour));
                    break;
                case 2:
                    Prop.SetPropertyDelegate(new Time(old_time, Time.Type.Minute));
                    break;
                case 3:
                    Prop.SetPropertyDelegate(new Time(old_time, Time.Type.Second));
                    break;
            }
                
        }

        public TimeItem()
        {
            converter = new BrushConverter();

            ItemLable = new TextBlock();
            Canvas.SetLeft(ItemLable, SideMargin);
            ItemLable.Foreground = (SolidColorBrush)converter.ConvertFrom("#5E5E5E");
            ItemLable.FontSize = 14;

            EditTextBox = new TextBox();
            EditTextBox.FontSize = 14;
            EditTextBox.Height = 30;
            EditTextBox.Background = (SolidColorBrush)converter.ConvertFrom("#EEEEEE");
            EditTextBox.BorderBrush = (SolidColorBrush)converter.ConvertFrom("#B1B1B1");
            EditTextBox.LostFocus += TextBoxMouseLeave;
            Canvas.SetLeft(EditTextBox, SideMargin);

            EditComboBox = new ComboBox();
            EditComboBox.Items.Add(StringRes.GetPropsString(StringRes.PropsToken.Days));
            EditComboBox.Items.Add(StringRes.GetPropsString(StringRes.PropsToken.Hours));
            EditComboBox.Items.Add(StringRes.GetPropsString(StringRes.PropsToken.Minutes));
            EditComboBox.Items.Add(StringRes.GetPropsString(StringRes.PropsToken.Seconds));
            EditComboBox.FontSize = 14;
            EditComboBox.Height = 30;
            EditComboBox.Background = (SolidColorBrush)converter.ConvertFrom("#EEEEEE");
            EditComboBox.BorderBrush = (SolidColorBrush)converter.ConvertFrom("#B1B1B1");
            EditComboBox.SelectionChanged += ComboBoxSelect;
            Canvas.SetLeft(EditComboBox, SideMargin);

            Separatior = new Line();
            Separatior.X1 = 0;

            Separatior.StrokeThickness = 1;
            Separatior.Stroke = (SolidColorBrush)converter.ConvertFrom("#C8C8C8");
        }


        public override void BindingProps(Props props)
        {
            Prop = (TimeProps)props;
            ItemLable.Text = Prop.Title;
            EditTextBox.Text = Prop.GetCurrentValueDelegate().Count.ToString();
            EditComboBox.SelectedIndex = TimeToIndex(Prop.GetCurrentValueDelegate().Measure);
        }

        private Int32 TimeToIndex(Time.Type time_type)
        {
            switch (time_type)
            {
                case Time.Type.Day: return 0;
                case Time.Type.Hour: return 1;
                case Time.Type.Minute: return 2;
                case Time.Type.Second: return 3;
            }
            return -1;
        }

        public override void SetTop(double top_y)
        {
            Canvas.SetTop(ItemLable, top_y + 5);
            Canvas.SetTop(EditTextBox, top_y + 35);
            Canvas.SetTop(EditComboBox, top_y + 35);

            Separatior.Y1 = Separatior.Y2 = top_y + 85;
        }

        public override void SetWidth(double widht)
        {
            ItemLable.Width = widht - SideMargin * 2;
            EditTextBox.Width = (widht - SideMargin * 2)*0.60;
            EditComboBox.Width = (widht - SideMargin * 2)*0.35;
            Canvas.SetLeft(EditComboBox, SideMargin+ EditTextBox.Width+((widht - SideMargin * 2) * 0.05));
            Separatior.X2 = widht;
        }

        public override double GetHeight() => 100;

        public override IEnumerable<UIElement> GetUIElement()
        {
            return new List<UIElement>
            {
                EditTextBox,
                ItemLable,
                EditComboBox,
                Separatior
            };
        }
    }
}
