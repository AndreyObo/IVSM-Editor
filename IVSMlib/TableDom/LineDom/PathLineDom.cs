using DomCore;
using IVSMlib.Language;
using IVSMlib.TableDom.TypesDom;
using IVSMlib.Utils;
using IVSMlib.VsmCanvas;
using IVSMlib.VsmCanvas.LineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.TableDom.LineDom
{
    public class PathLineDom : TableUIDom
    {
        public const string MoveTypeProps = "MoveType";
        public const string TimeProps = "Time";
        public const string CommentProps = "Comment";

        private static readonly Lazy<PathLineDom> PathLineDomInstance = new Lazy<PathLineDom>(() => new PathLineDom());

        private PathLineDom() { }

        public static PathLineDom Get()
        {
            return PathLineDomInstance.Value;
        }

        public override TableUI CreateInstanse(Node ui_node)
        {
            PathLine line = new PathLine();
            Int32 id = Convert.ToInt32(ui_node.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id)));
            line.Id = id;

            Node decoration = (Node)ui_node.FindeNode(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration));

            if (decoration != null)
            {
                foreach (Node.NodeItem<object> props_item in decoration.GetNodes())
                {
                    if (props_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Fill))
                    {
                        System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml((string)props_item.value);
                        System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                        line.SetColor(newColor);
                    }

                    if (props_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.DashStyle))
                    {
                        if ((string)props_item.value == Lexer.GetPropertyTag(Lexer.PropertyToken.Dash))
                        {
                            line.SetDash(PathLine.Dash.Dash);
                        }
                        else
                        {
                            line.SetDash(PathLine.Dash.NoDash);
                        }
                    }
                }
            }
            Node props = (Node)ui_node.FindeNode(Lexer.GetPropertyTag(Lexer.PropertyToken.Props));

            foreach (Node.NodeItem<object> props_item in props.GetNodes())
            {
                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == TimeProps)
                {
                    line.SetMoveTime(TimeDom.DecodeTime(((string)props_item.value)));
                }
                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == MoveTypeProps)
                {
                    line.SetMoveType((string)props_item.value);
                }
                if (props_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == CommentProps)
                {
                    line.SetComment((string)props_item.value);
                }
            }

            Node line_path = (Node)ui_node.FindeNode(Lexer.GetPropertyTag(Lexer.PropertyToken.LinePath));

            List<PathLine.LinePoint> line_points = new List<PathLine.LinePoint>();

            foreach (Node.NodeItem<object> line_item in line_path.GetNodes())
            {
                double x1 = Convert.ToDouble(line_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.X1)));
                double y1 = Convert.ToDouble(line_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Y1)));
                double x2 = Convert.ToDouble(line_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.X2)));
                double y2 = Convert.ToDouble(line_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Y2)));

                line_points.Add(new PathLine.LinePoint(x1, y1, x2, y2));
            }

            line.AddPath(line_points);
            return line;
        }

        public override Node CreateRootNode(TableUI table_ui)
        {
            PathLine line = (PathLine)table_ui;

            Node line_node = new Node(Lexer.GetUITag(Lexer.UIToken.Line));

            line_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), line.Id.ToString());

            Node decorations = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration));
            decorations.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Fill), IVSMUtils.HexConverter(line.GetColor()));
            if (line.GetDash() == StringRes.GetPropsString(StringRes.PropsToken.Dash))
            {
                decorations.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.DashStyle), Lexer.GetPropertyTag(Lexer.PropertyToken.Dash));
            }
            else
            {
                decorations.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.DashStyle), Lexer.GetPropertyTag(Lexer.PropertyToken.NoDash));
            }

            line_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration), decorations);

            //--------props--------------
            Node props_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Props));

            List<Node.NodeFeature> type_feature = new List<Node.NodeFeature>();
            type_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), MoveTypeProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), line.GetMoveType(), type_feature);


            //Node act_time_item = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem));
            //act_time_item.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), TimeProps);
            //act_time_item.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), TimeDom.GetNode(line.GetMoveTime()));

            //   props_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), act_time_item);

            //------time
            List<Node.NodeFeature> act_time_feature = new List<Node.NodeFeature>();
            act_time_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), TimeProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), TimeDom.CodeTime(line.GetMoveTime()), act_time_feature);
            //-----------------


            List<Node.NodeFeature> comment_feature = new List<Node.NodeFeature>();
            comment_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), CommentProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), line.GetComment(), comment_feature);


            line_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Props), props_node);

            //---------------------------

            Node line_path = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.LinePath));

            foreach(PathLine.LinePoint line_p in line.LinePath)
            {
                Node line_point = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Line));
                line_point.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.X1), line_p.X1.ToString());
                line_point.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Y1), line_p.Y1.ToString());
                line_point.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.X2), line_p.X2.ToString());
                line_point.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Y2), line_p.Y2.ToString());

                line_path.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Line), line_point);
            }

            line_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.LinePath), line_path);

            return line_node;
        }
    }
}
