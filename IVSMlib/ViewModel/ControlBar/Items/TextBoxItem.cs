using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using IVSMlib.PropsHolders;

namespace IVSMlib.ViewModel.ControlBar.Items
{
    public class TextBoxItem:Item
    {
        private TextBox EditTextBox;
        private TextBlock ItemLable;

        private Line Separatior;

        public delegate void SetText(string text);   

        StringProps Props;
        BrushConverter converter;

        private void TextBoxMouseLeave(object sender, RoutedEventArgs e)
        {
            Props.SetPropertyDelegate?.Invoke(((TextBox)sender).Text);
        }

        public TextBoxItem()
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
            EditTextBox.TextChanged += TextChange;
            Canvas.SetLeft(EditTextBox, SideMargin);

            Separatior = new Line();
            Separatior.X1 = 0;

            Separatior.StrokeThickness = 1;
            Separatior.Stroke = (SolidColorBrush)converter.ConvertFrom("#C8C8C8");
        }

        private void TextChange(object sender, TextChangedEventArgs e)
        {
            Props.SetChangedValueDelegate?.Invoke(((TextBox)sender).Text);
        }

        public override void BindingProps(Props props)
        {
            Props = (StringProps)props;
            ItemLable.Text = Props.Title;
            EditTextBox.Text = Props.GetCurrentValueDelegate();
        }

        public void SetLableText(string text)
        {
            ItemLable.Text = text;
        }

        public void SetTextBoxText(string text)
        {
            EditTextBox.Text = text;
        }

        public override void SetTop(double top_y)
        {
            Canvas.SetTop(ItemLable, top_y + 5);
            Canvas.SetTop(EditTextBox, top_y + 35);

            Separatior.Y1 = Separatior.Y2 = top_y + 85;
        }

        public override void SetWidth(double widht)
        {
            ItemLable.Width = widht- SideMargin*2;
            EditTextBox.Width = widht- SideMargin*2;
            Separatior.X2 = widht;
        }

        public override IEnumerable<UIElement> GetUIElement()
        {
            return new List<UIElement>
            {
                EditTextBox,
                ItemLable,
                Separatior
            };

        }

        public override double GetHeight() => 100;
    }
}
