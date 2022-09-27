using DomCore;
using IVSMlib.Utils;
using IVSMlib.VsmCanvas;
using IVSMlib.VsmCanvas.MarksUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IVSMlib.TableDom.MarkDom
{
    public class MTextLableDom : TableUIDom
    {
        private const string TransparentColor = "Transparent";

        private static readonly Lazy<MTextLableDom> MTextLableDomInstance = new Lazy<MTextLableDom>(() => new MTextLableDom());

        private MTextLableDom() { }

        public static MTextLableDom Get()
        {
            return MTextLableDomInstance.Value;
        }

        public override TableUI CreateInstanse(Node ui_node)
        {
            MTextLable text_lbl = new MTextLable();

            foreach (Node.NodeItem<object> text_item in ui_node.GetNodes())
            {
                if (text_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Location))
                {
                    double x = Convert.ToDouble(text_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.X)));
                    double y = Convert.ToDouble(text_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Y)));
                    text_lbl.SetLocation(x, y);
                }
                if (text_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Size))
                {
                    double widht = Convert.ToDouble(text_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Width)));
                    double height = Convert.ToDouble(text_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Height)));

                    text_lbl.SetSize(widht, height);
                }
                if (text_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Text))
                {
                    text_lbl.SetText(text_item.value.ToString());
                }
                if (text_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Fill))
                {
                    if (text_item.value.ToString() == TransparentColor)
                    {
                        text_lbl.SetBackColor(Colors.Transparent);
                    }
                    else
                    {
                        System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml((string)text_item.value);
                        System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                        text_lbl.SetBackColor(newColor);
                    }
                }
                if (text_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.BorderColor))
                {
                    if (text_item.value.ToString() == TransparentColor)
                    {
                        text_lbl.SetBorderColor(Colors.Transparent);
                    }
                    else
                    {
                        System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml((string)text_item.value);
                        System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                        text_lbl.SetBorderColor(newColor);
                    }
                }
                if (text_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.TextColor))
                {
                    System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml((string)text_item.value);
                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    text_lbl.SetTextColor(newColor);
                }
                if (text_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.BorderWidth))
                {

                    text_lbl.SetBorderWidth(text_item.value.ToString());
                }
                if (text_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.FontFamily))
                {

                    text_lbl.SetTextFont(text_item.value.ToString());
                }
                if (text_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.FontSize))
                {

                    text_lbl.SetTextSize(text_item.value.ToString());
                }
                if (text_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Alight))
                {

                    text_lbl.SetTextAlightS(text_item.value.ToString());
                }
            }

            return text_lbl;
        }

        public override Node CreateRootNode(TableUI text_ui)
        {
            MTextLable text = (MTextLable)text_ui;

            Node text_node = new Node(Lexer.GetUITag(Lexer.UIToken.TextLable));

            Node loc_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Location));

            loc_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.X), text.GetLocation().X.ToString());
            loc_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Y), text.GetLocation().Y.ToString());

            text_node.AddNodeItem<Node>(Lexer.GetUITag(Lexer.UIToken.TextLable), loc_node);
            Node size_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Size));
            size_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Width), text.GetSize().Width.ToString());
            size_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Height), text.GetSize().Height.ToString());

            text_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Size), size_node);

            text_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Text), text.GetText());
            if (text.IsFillTransparant)
            {
                text_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Fill), TransparentColor);
            }
            else
            {
                text_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Fill), IVSMUtils.HexConverter(text.GetBackColor()));
            }
            if(text.IsBorderTransparant)
            {
                text_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.BorderColor), TransparentColor);
            }
            else
            {
                text_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.BorderColor), IVSMUtils.HexConverter(text.GetBorderColor()));
            }
         
            text_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.TextColor), IVSMUtils.HexConverter(text.GetTextColor()));
            text_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.BorderWidth), text.GetBorderWidth());
            text_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.FontFamily), text.GetFontName());
            text_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.FontSize), text.GetFontSize());
            text_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Alight), text.GetTextAlight());

            return text_node;
        }
    }
}
