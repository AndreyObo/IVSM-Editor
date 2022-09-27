using DomCore;
using IVSMlib.TableDom.TypesDom;
using IVSMlib.ViewModel.Units;
using IVSMlib.VsmCanvas;
using IVSMlib.VsmCanvas.CellUI;
using IVSMlib.VsmCanvas.LineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.TableDom.CellDom
{
    public class ConditionDom : TableUIDom
    {
        public const string LineTop = "Top";
        public const string LineLeft = "Left";
        public const string LineBottom = "Bottom";
        public const string LineRight = "Right";

        public const string ActionProps = "Action";
        public const string ACaseProps = "ACase";
        public const string BCaseProps = "BCase";
        public const string CCaseProps = "CCase";
        public const string ActionTime = "ActionTime";
        public const string WatingTime = "WatingTime";
        public const string CommentProps = "Comment";

        private static readonly Lazy<ConditionDom> ConditionDomInstance = new Lazy<ConditionDom>(() => new ConditionDom());

        private ConditionDom() { }

        public static ConditionDom Get()
        {
            return ConditionDomInstance.Value;
        }

        public override TableUI CreateInstanse(Node ui_node)
        {
            ConditionCell condition_cell = new ConditionCell();

            Int32 row = Convert.ToInt32(ui_node.GetFeature(Lexer.GetMapTag(Lexer.MapToken.Row)));
            Int32 col = Convert.ToInt32(ui_node.GetFeature(Lexer.GetMapTag(Lexer.MapToken.Column)));

            condition_cell.TableIndex.Row = row;
            condition_cell.TableIndex.Column = col;

            Node decoration = (Node)ui_node.FindeNode(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration));

            //foreach (Node.NodeItem<object> props_item in decoration.GetNodes())
            //{
            //    if (props_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Fill))
            //    {
            //        System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml((string)props_item.value);
            //        System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            //        condition_cell.SetColor(newColor);
            //    }
            //}

            Node props = (Node)ui_node.FindeNode(Lexer.GetPropertyTag(Lexer.PropertyToken.Props));

            //  Node act_time = props.FindeNode(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem));

            foreach (Node.NodeItem<object> props_item in props.GetNodes())
            {
                //Console.WriteLine(props_item.name);
                //Console.WriteLine(props_item.value.ToString());
                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == ActionProps)
                {
                    condition_cell.SetActName((string)props_item.value);
                }
                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == ActionTime)
                {
                    condition_cell.SetActionTime(TimeDom.DecodeTime(((string)props_item.value)));
                }

                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == WatingTime)
                {
                    condition_cell.SetWatingTime(TimeDom.DecodeTime(((string)props_item.value)));
                }

                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == ACaseProps)
                {
                    condition_cell.Set_A_Conditon((string)props_item.value);
                }

                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == BCaseProps)
                {
                    condition_cell.Set_B_Conditon((string)props_item.value);
                }

                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == CCaseProps)
                {
                    condition_cell.Set_C_Conditon((string)props_item.value);
                }

                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == CommentProps)
                {
                    condition_cell.SetComment((string)props_item.value);
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
            CreateLinesCallbak(condition_cell, lines);

            Node documents = (Node)ui_node.FindeNode(Lexer.GetMapTag(Lexer.MapToken.Documents));
            if (documents != null)
            {
                CreateDocumentsCallback(condition_cell, documents);
            }

            return condition_cell;
        }

        private void CreateDocumentsCallback(ConditionCell cell, Node documents_node)
        {
            foreach (Node.NodeItem<object> doc_id_item in documents_node.GetNodes())
            {
                if (doc_id_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Id))
                {
                    MapDom.DocumentLinks.Add(new Linker.DocumentLink(cell, Convert.ToInt32(doc_id_item.value)));
                }
            }
        }

        private void CreateLinesCallbak(ConditionCell condition_cell, Node lines_node)
        {
            foreach (Node.NodeItem<object> line_item in lines_node.GetNodes())
            {
                if (line_item.name == Lexer.GetUITag(Lexer.UIToken.Line))
                {
                    Int32 id = Convert.ToInt32(line_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id)));
                    string link_type = line_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type));
                    CreateLink(id, link_type, condition_cell);
                   
                }
             
            }
        }

        private void CreateLink(Int32 id, string type, ConditionCell cell)
        {
            switch (type)
            {
                case LineTop:
                    MapDom.ConditionLineLinks.Add(new Linker.ConditionLink(cell, Linker.ConditionLinkType.Top, id));
                    break;
                case LineLeft:
                    MapDom.ConditionLineLinks.Add(new Linker.ConditionLink(cell, Linker.ConditionLinkType.Left, id));
                    break;
                case LineBottom:
                    MapDom.ConditionLineLinks.Add(new Linker.ConditionLink(cell, Linker.ConditionLinkType.Bottom, id));
                    break;
                case LineRight:
                    MapDom.ConditionLineLinks.Add(new Linker.ConditionLink(cell, Linker.ConditionLinkType.Right, id));
                    break; 
            }
        }

        public override Node CreateRootNode(TableUI table_ui)
        {
            ConditionCell cell = (ConditionCell)table_ui;

            Node condition_node = new Node(Lexer.GetUITag(Lexer.UIToken.Condition));

            condition_node.AddFeature(Lexer.GetMapTag(Lexer.MapToken.Row), cell.TableIndex.Row.ToString());
            condition_node.AddFeature(Lexer.GetMapTag(Lexer.MapToken.Column), cell.TableIndex.Column.ToString());


            Node props_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Props));

            CreatePropsNode(ref props_node, cell);

            condition_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Props), props_node);

            if (cell.GetActionDocList().Count != 0)
            {
                Node doc_list = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.DocumentsList));

                foreach (DocumentUnit doc in cell.GetActionDocList())
                {
                    doc_list.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), doc.Id.ToString());
                }

                condition_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.DocumentsList), doc_list);
            }

            CreateLinesNode(ref condition_node, cell);

            return condition_node;
        }

        private void CreatePropsNode(ref Node props_node, ConditionCell cell)
        {
            List<Node.NodeFeature> name_feature = new List<Node.NodeFeature>();
            name_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), ActionProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), cell.GetActName(), name_feature);

            List<Node.NodeFeature> a_case_feature = new List<Node.NodeFeature>();
            a_case_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), ACaseProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), cell.Get_A_Conditon(), a_case_feature);

            List<Node.NodeFeature> b_case_feature = new List<Node.NodeFeature>();
            b_case_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), BCaseProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), cell.Get_B_Conditon(), b_case_feature);

            List<Node.NodeFeature> c_case_feature = new List<Node.NodeFeature>();
            c_case_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), CCaseProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), cell.Get_C_Conditon(), c_case_feature);


            List<Node.NodeFeature> act_time_feature = new List<Node.NodeFeature>();
            act_time_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), ActionTime));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), TimeDom.CodeTime(cell.GetActionTime()), act_time_feature);

            List<Node.NodeFeature> waste_time_feature = new List<Node.NodeFeature>();
            waste_time_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), WatingTime));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), TimeDom.CodeTime(cell.GetWatingTime()), waste_time_feature);

            List<Node.NodeFeature> comment_feature = new List<Node.NodeFeature>();
            comment_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), CommentProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), cell.GetComment(), comment_feature);
        }

        private void CreateLinesNode(ref Node condition_node, ConditionCell cell)
        {
            Node lines = null;


            foreach (Line line in cell.LinesTop)
            {
                if (lines == null) lines = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList));

                Node lines_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Line));
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type), LineTop);
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), line.Id.ToString());
                lines.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Line), lines_node);
            }

            foreach (Line line in cell.LinesLeft)
            {
                if (lines == null) lines = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList));

                Node lines_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Line));
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type), LineLeft);
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), line.Id.ToString());
                lines.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Line), lines_node);
            }

            foreach (Line line in cell.LinesBottom)
            {
                if (lines == null) lines = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList));

                Node lines_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Line));
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type), LineBottom);
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), line.Id.ToString());
                lines.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Line), lines_node);
            }

            foreach (Line line in cell.LinesRight)
            {
                if (lines == null) lines = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList));

                Node lines_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Line));
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Type), LineRight);
                lines_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), line.Id.ToString());
                lines.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Line), lines_node);
            }

            if (lines != null)
            {
                condition_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.LinesList), lines);
            }
        }
    }
}
