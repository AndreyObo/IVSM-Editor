using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSMlib.VsmCanvas;
using IVSMlib.VsmCanvas.CellUI;
using IVSMlib.Utils;
using DomCore;
using System.Windows.Media;

namespace IVSMlib.TableDom.CellDom
{
    public class PlayerDom : TableUIDom
    {
        private const string OrganizationProps = "Organization";
        private const string DepartmentProps = "Department";
        private const string CommentProps = "Comment";

        private static readonly Lazy<PlayerDom> PlayerDomInstance = new Lazy<PlayerDom>(() => new PlayerDom());

        private PlayerDom() { }

        public static PlayerDom Get()
        {
            return PlayerDomInstance.Value;
        }

        public override TableUI CreateInstanse(Node ui_node)
        {
            PlayerCell cell = new PlayerCell();

            Int32 row = Convert.ToInt32(ui_node.GetFeature(Lexer.GetMapTag(Lexer.MapToken.Row)));
            Int32 col = Convert.ToInt32(ui_node.GetFeature(Lexer.GetMapTag(Lexer.MapToken.Column)));

            cell.TableIndex.Row = row;
            cell.TableIndex.Column = col;

            Node decoration = (Node)ui_node.FindeNode(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration));

            foreach (Node.NodeItem<object> props_item in decoration.GetNodes())
            {
                if (props_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Fill))
                {
                    System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml((string)props_item.value);
                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    cell.SetColor(newColor);
                }
                if (props_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.BorderColor))
                {
                    if ((string)props_item.value == Lexer.GetPropertyTag(Lexer.PropertyToken.Transparent))
                    {
                        cell.SetBorderColor(Colors.Transparent);
                    }
                    else
                    {
                        System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml((string)props_item.value);
                        System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                        cell.SetBorderColor(newColor);
                    }
                }
            }

            Node props = (Node)ui_node.FindeNode(Lexer.GetPropertyTag(Lexer.PropertyToken.Props));
            foreach(Node.NodeItem<object> props_item in props.GetNodes())
            {
                if(props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == OrganizationProps) {
                    cell.SetOrgName((string)props_item.value);
                }
                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == CommentProps)
                {
                    cell.SetComment((string)props_item.value);
                }
                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == DepartmentProps)
                {
                    cell.SetDepName((string)props_item.value);
                }
            }
            return cell;
        }

        public override Node CreateRootNode(TableUI table_ui)
        {
            PlayerCell cell = (PlayerCell)table_ui;

            Node player_node = new Node(Lexer.GetUITag(Lexer.UIToken.Player));

            player_node.AddFeature(Lexer.GetMapTag(Lexer.MapToken.Row), cell.TableIndex.Row.ToString());
            player_node.AddFeature(Lexer.GetMapTag(Lexer.MapToken.Column), cell.TableIndex.Column.ToString());

        //    player_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Text), cell.GetOrgName());

            Node decorations = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration));
            decorations.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Fill), IVSMUtils.HexConverter(cell.GetCurrentColor()));
            if (cell.IsBorderTransparant == true)
            {
                decorations.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.BorderColor), Lexer.GetPropertyTag(Lexer.PropertyToken.Transparent));
            }
            else
            {
                decorations.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.BorderColor), IVSMUtils.HexConverter(cell.GetCurrentBorderColor()));
            }


            player_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration), decorations);


            //--------------------Create Props-------------------------------------
            Node props_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Props));

            CreatePropsNode(ref props_node, cell);

            player_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Props), props_node);

            return player_node;
        }

        private void CreatePropsNode(ref Node props_node, PlayerCell cell)
        {
            List<Node.NodeFeature> name_feature = new List<Node.NodeFeature>();
            name_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), OrganizationProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), cell.GetOrgName(), name_feature);

            List<Node.NodeFeature> dep_feature = new List<Node.NodeFeature>();
            dep_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), DepartmentProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), cell.GetDepName(), dep_feature);

            List<Node.NodeFeature> comment_feature = new List<Node.NodeFeature>();
            comment_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name),CommentProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), cell.GetComment(), dep_feature);
        }
    }
}
