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
    public class MProblemDom : TableUIDom
    {
        private static readonly Lazy<MProblemDom> MProblemDomInstance = new Lazy<MProblemDom>(() => new MProblemDom());

        private MProblemDom() { }

        public static MProblemDom Get()
        {
            return MProblemDomInstance.Value;
        }

        public override TableUI CreateInstanse(Node ui_node)
        {
            MProblem problem = new MProblem();

            foreach (Node.NodeItem<object> problem_item in ui_node.GetNodes())
            {
                if (problem_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Location))
                {
                    double x = Convert.ToDouble(problem_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.X)));
                    double y = Convert.ToDouble(problem_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Y)));
                    problem.SetLocation(x, y);
                }
                if (problem_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Size))
                {
                    double widht = Convert.ToDouble(problem_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Width)));
                    double height = Convert.ToDouble(problem_item.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Height)));

                    problem.SetSize(widht, height);
                }
                if (problem_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Title))
                {
                    problem.SetProblem(problem_item.value.ToString());
                }
            }
                return problem;
        }

        public override Node CreateRootNode(TableUI table_ui)
        {
            MProblem problem = (MProblem)table_ui;

            Node problem_node = new Node(Lexer.GetUITag(Lexer.UIToken.Problem));

            Node loc_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Location));

            loc_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.X), problem.GetLocation().X.ToString());
            loc_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Y), problem.GetLocation().Y.ToString());

            problem_node.AddNodeItem<Node>(Lexer.GetUITag(Lexer.UIToken.TextLable), loc_node);

            Node size_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Size));
            size_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Width), problem.GetSize().Width.ToString());
            size_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Height), problem.GetSize().Height.ToString());

            problem_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Size), size_node);

            problem_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Title), problem.GetProblem());

            return problem_node;
        }
    }
}
