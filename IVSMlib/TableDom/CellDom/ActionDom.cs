using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSMlib.VsmCanvas;
using IVSMlib.VsmCanvas.CellUI;
using IVSMlib.Utils;
using IVSMlib.TableDom.TypesDom;
using DomCore;
using IVSMlib.ViewModel.Units;
using IVSMlib.VsmCanvas.LineUI;
using IVSMlib.Construct;
using System.Windows.Media;

namespace IVSMlib.TableDom.CellDom
{
    public class ActionDom : TableUIDom
    {
        public const string ActionProps = "Action";
        public const string ActionTime = "ActionTime";
        public const string WatingTime = "WatingTime";
        public const string CommentProps = "Comment";


        public const string LineLeftTop = "LeftTop";
        public const string LineLeftMiddle = "LeftMiddle";
        public const string LineLeftBottom = "LeftBottom";
        public const string LineRightTop = "RightTop";
        public const string LineRightMiddle = "RightMiddle";
        public const string LineRightBottom = "RightBottom";

        private static readonly Lazy<ActionDom> ActionDomInstance = new Lazy<ActionDom>(() => new ActionDom());

        private ActionDom() { }

        public static ActionDom Get()
        {
            return ActionDomInstance.Value;
        }

        public override TableUI CreateInstanse(Node ui_node)
        {
            ActionCell action_cell = new ActionCell();

            Int32 row = Convert.ToInt32(ui_node.GetFeature(Lexer.GetMapTag(Lexer.MapToken.Row)));
            Int32 col = Convert.ToInt32(ui_node.GetFeature(Lexer.GetMapTag(Lexer.MapToken.Column)));

            action_cell.TableIndex.Row = row;
            action_cell.TableIndex.Column = col;

            Node decoration = (Node)ui_node.FindeNode(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration));

            foreach (Node.NodeItem<object> props_item in decoration.GetNodes())
            {
                if (props_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Fill))
                {
                    System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml((string)props_item.value);
                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    action_cell.SetColor(newColor);
                }

                if (props_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.BorderColor))
                {
                    if((string)props_item.value == Lexer.GetPropertyTag(Lexer.PropertyToken.Transparent))
                    {
                        action_cell.SetBorderColor(Colors.Transparent);
                    }
                    else
                    {
                        System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml((string)props_item.value);
                        System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                        action_cell.SetBorderColor(newColor);
                    }  
                }
            }

            Node props = (Node)ui_node.FindeNode(Lexer.GetPropertyTag(Lexer.PropertyToken.Props));

            //  Node act_time = props.FindeNode(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem));

            foreach (Node.NodeItem<object> props_item in props.GetNodes())
            {
                //Console.WriteLine(props_item.name);
                //Console.WriteLine(props_item.value.ToString());
                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == ActionProps)
                {
                    action_cell.SetActName((string)props_item.value);
                }
                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == ActionTime)
                {
                  action_cell.SetActionTime(TimeDom.DecodeTime(((string)props_item.value)));
                }

                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == WatingTime)
                {
                    action_cell.SetWatingTime(TimeDom.DecodeTime(((string)props_item.value)));
                }

                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == CommentProps)
                {
                    action_cell.SetComment((string)props_item.value);
                }
                //if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == ActionTime)
                //{
                //    action_cell.SetActionTime(TimeDom.ParseTime((Node)props_item.value));
                //}
                //if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == WatingTime)
                //{
                //    action_cell.SetWatingTime(TimeDom.ParseTime((Node)props_item.value));
                //}

            }

            Node lines = (Node)ui_node.FindeNode(Lexer.GetMapTag(Lexer.MapToken.Lines));
            if (lines != null)
            {
                CreateLinesCallbak(action_cell, lines);
            }

            Node documents = (Node)ui_node.FindeNode(Lexer.GetMapTag(Lexer.MapToken.Documents));
            if (documents != null)
            {
                CreateDocumentsCallback(action_cell, documents);
            }

            return action_cell;
        }

        private void CreateDocumentsCallback(ActionCell action_cell, Node documents_node)
        {
            foreach (Node.NodeItem<object> doc_id_item in documents_node.GetNodes())
            {
                if(doc_id_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Id))
                {
                    MapDom.DocumentLinks.Add(new Linker.DocumentLink(action_cell, Convert.ToInt32(doc_id_item.value)));
                }
            }
        }

        private void CreateLinesCallbak(ActionCell action_cell, Node lines_node)
        {
            foreach (Node.NodeItem<object> line_item in lines_node.GetNodes())
            {
                if(line_item.name == Lexer.GetUITag(Lexer.UIToken.Line))
                {
                    Int32 id = Convert.ToInt32(line_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id)));
                    string link_type = line_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type));
                    CreateLink(id, link_type, action_cell);
                }
            }
        }

        private void CreateLink(Int32 id, string type, ActionCell cell)
        {
            switch(type)
            {
                case LineLeftTop:
                    MapDom.ActionLineLinks.Add(new Linker.ActionLink(cell, Linker.ActionLinkType.LTop, id));
                    break;
                case LineLeftMiddle:
                    MapDom.ActionLineLinks.Add(new Linker.ActionLink(cell, Linker.ActionLinkType.LMiddle, id));
                    break;
                case LineLeftBottom:
                    MapDom.ActionLineLinks.Add(new Linker.ActionLink(cell, Linker.ActionLinkType.LBottom, id));
                    break;
                case LineRightTop:
                    MapDom.ActionLineLinks.Add(new Linker.ActionLink(cell, Linker.ActionLinkType.RTop, id));
                    break;
                case LineRightMiddle:
                    MapDom.ActionLineLinks.Add(new Linker.ActionLink(cell, Linker.ActionLinkType.RMiddle, id));
                    break;
                case LineRightBottom:
                    MapDom.ActionLineLinks.Add(new Linker.ActionLink(cell, Linker.ActionLinkType.RBottom, id));
                    break;
            }
        }

        public override Node CreateRootNode(TableUI table_ui)
        {
            ActionCell cell = (ActionCell)table_ui;

            Node action_node = new Node(Lexer.GetUITag(Lexer.UIToken.Action));

            action_node.AddFeature(Lexer.GetMapTag(Lexer.MapToken.Row), cell.TableIndex.Row.ToString());
            action_node.AddFeature(Lexer.GetMapTag(Lexer.MapToken.Column), cell.TableIndex.Column.ToString());

            action_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Text), cell.GetActName());

            Node decorations = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration));
            decorations.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Fill), IVSMUtils.HexConverter(cell.GetCurrentColor()));
            if(cell.IsBorderTransparant == true)
            {
                decorations.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.BorderColor), Lexer.GetPropertyTag(Lexer.PropertyToken.Transparent));
            }
            else
            {
                decorations.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.BorderColor), IVSMUtils.HexConverter(cell.GetCurrentBorderColor()));
            }
            

            action_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration), decorations);

            Node props_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Props));

            CreatePropsNode(ref props_node, cell);

            action_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Props), props_node);

            if (cell.GetActionDocList().Count != 0)
            {
                Node doc_list = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.DocumentsList));

                foreach (DocumentUnit doc in cell.GetActionDocList())
                {
                    doc_list.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), doc.Id.ToString());
                }

                action_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.DocumentsList), doc_list);
            }

            CreateLinesNode(ref action_node, cell);

            return action_node;
        }

        private void CreateLinesNode(ref Node action_node, ActionCell cell)
        {
            Node lines=null;

            foreach(Line line in cell.LinesLeftTop)
            {
                if(lines == null) lines = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList));

                Node lines_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Line));
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type), LineLeftTop);
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), line.Id.ToString());
                lines.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Line), lines_node);
            }

            foreach (Line line in cell.LinesLeftCenter)
            {
                if (lines == null) lines = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList));

                Node lines_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Line));
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type), LineLeftMiddle);
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), line.Id.ToString());
                lines.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Line), lines_node);
            }

            foreach (Line line in cell.LinesLeftBottom)
            {
                if (lines == null) lines = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList));

                Node lines_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Line));
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type), LineLeftBottom);
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), line.Id.ToString());
                lines.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Line), lines_node);
            }

            foreach (Line line in cell.LinesRightTop)
            {
                if (lines == null) lines = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList));

                Node lines_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Line));
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type), LineRightTop);
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), line.Id.ToString());
                lines.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Line), lines_node);
            }

            foreach (Line line in cell.LinesRightCenter)
            {
                if (lines == null) lines = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList));

                Node lines_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Line));
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type), LineRightMiddle);
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), line.Id.ToString());
                lines.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Line), lines_node);
            }

            foreach (Line line in cell.LinesRightBottom)
            {
                if (lines == null) lines = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList));

                Node lines_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Line));
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type), LineRightBottom);
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), line.Id.ToString());
                lines.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Line), lines_node);
            }

            if (lines != null)
            {
                action_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList), lines);
            }
        }

        private void CreatePropsNode(ref Node props_node, ActionCell cell)
        {
            List<Node.NodeFeature> name_feature = new List<Node.NodeFeature>();
            name_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), ActionProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), cell.GetActName(), name_feature);

            //List<Node.NodeFeature> time_feature = new List<Node.NodeFeature>();
            // time_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), ActionTime));

            List<Node.NodeFeature> act_time_feature = new List<Node.NodeFeature>();
            act_time_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), ActionTime));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), TimeDom.CodeTime(cell.GetActionTime()), act_time_feature);

            List<Node.NodeFeature> waste_time_feature = new List<Node.NodeFeature>();
            waste_time_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), WatingTime));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), TimeDom.CodeTime(cell.GetWatingTime()), waste_time_feature);
            //Node act_time_item = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem));
            //act_time_item.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), ActionTime);
            //act_time_item.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), TimeDom.GetNode(cell.GetActionTime()));

            //props_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), act_time_item);

            //Node wating_time_item = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem));
            //wating_time_item.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), WatingTime);
            //wating_time_item.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), TimeDom.GetNode(cell.GetWatingTime()));

            //props_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), wating_time_item);




            List<Node.NodeFeature> comment_feature = new List<Node.NodeFeature>();
            comment_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), CommentProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), cell.GetComment(), comment_feature);
        }
    }
}
