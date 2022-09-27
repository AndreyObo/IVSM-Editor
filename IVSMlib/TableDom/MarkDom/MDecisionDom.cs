using DomCore;
using IVSMlib.VsmCanvas;
using IVSMlib.VsmCanvas.MarksUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.TableDom.MarkDom
{
    public class MDecisionDom : TableUIDom
    {
        private static readonly Lazy<MDecisionDom> MDecisionDomInstance = new Lazy<MDecisionDom>(() => new MDecisionDom());

        private MDecisionDom() { }

        public static MDecisionDom Get()
        {
            return MDecisionDomInstance.Value;
        }

        public override TableUI CreateInstanse(Node ui_node)
        {
            MDecision decision = new MDecision();

            foreach (Node.NodeItem<object> d_item in ui_node.GetNodes())
            {
                if (d_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Location))
                {
                    double x = Convert.ToDouble(d_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.X)));
                    double y = Convert.ToDouble(d_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Y)));
                    decision.SetLocation(x, y);
                }
                if (d_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Size))
                {
                    double widht = Convert.ToDouble(d_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Width)));
                    double height = Convert.ToDouble(d_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Height)));

                    decision.SetSize(widht, height);
                }
                if (d_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Title))
                {
                    decision.SetDecision(d_item.value.ToString());
                }
            }
            return decision;
        }

        public override Node CreateRootNode(TableUI table_ui)
        {
            MDecision decision = (MDecision)table_ui;

            Node decision_node = new Node(Lexer.GetUITag(Lexer.UIToken.Desicion));

            Node loc_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Location));

            loc_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.X), decision.GetLocation().X.ToString());
            loc_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Y), decision.GetLocation().Y.ToString());

            decision_node.AddNodeItem<Node>(Lexer.GetUITag(Lexer.UIToken.TextLable), loc_node);

            Node size_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Size));
            size_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Width), decision.GetSize().Width.ToString());
            size_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Height), decision.GetSize().Height.ToString());

            decision_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Size), size_node);

            decision_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Title), decision.GetDecision());

            return decision_node;
        }
    }
}
