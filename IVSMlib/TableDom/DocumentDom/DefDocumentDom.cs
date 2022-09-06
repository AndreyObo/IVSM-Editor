using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomCore;
using IVSMlib.Utils;
using IVSMlib.ViewModel.Units;

namespace IVSMlib.TableDom.DocumentDom
{
    public class DefDocumentDom
    {
        private static readonly Lazy<DefDocumentDom> DefDocumentDomInstance = new Lazy<DefDocumentDom>(() => new DefDocumentDom());

        private DefDocumentDom() { }

        public static DefDocumentDom Get()
        {
            return DefDocumentDomInstance.Value;
        }

        private const string NameProps = "DocumentName";
        private const string CommentProps = "Comment";

        public Node CreateRootNode(DocumentUnit doc)
        {
            Node doc_node = new Node(Lexer.GetUITag(Lexer.UIToken.Document));

            doc_node.AddFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id), doc.Id.ToString());

            //---------------------decoration------------------------------------

            Node decoration = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration));

            decoration.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Fill), IVSMUtils.HexConverter(doc.GetCurrentColor()));

            doc_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration), decoration);
            //------------------props------------------------

            Node props_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Props));

            List<Node.NodeFeature> name_feature = new List<Node.NodeFeature>();
            name_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), NameProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), doc.GetDocName(), name_feature);

            List<Node.NodeFeature> comment_feature = new List<Node.NodeFeature>();
            comment_feature.Add(new Node.NodeFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name), CommentProps));

            props_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem), doc.GetComment(), comment_feature);

            doc_node.AddNodeItem<Node>(Lexer.GetPropertyTag(Lexer.PropertyToken.Props), props_node);

            //doc_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Title), doc.GetDocName());
            //doc_node.AddNodeItem<string>(le)

            return doc_node;
        }

        public List<DocumentUnit> ParseNode(Node doc_node)
        {
            List<DocumentUnit> documents_list = new List<DocumentUnit>();
            if(doc_node == null)
            {
                return documents_list;
            }

            //Int32 doc_id = Convert.ToInt32(doc_node.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id)));

            foreach (Node.NodeItem<object> doc in doc_node.GetNodes())
            {

                Node doc_n = (Node)doc.value;
                Int32 doc_id = Convert.ToInt32(doc_n.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Id)));

                DocumentUnit document = new DocumentUnit();
                document.Id = doc_id;
              //  Console.WriteLine("doc item name -- " + doc_item.name);

                foreach (Node.NodeItem<object> doc_item in doc_n.GetNodes())
                {
                    

                    if (doc_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Decoration))
                    {
                        Node decoration = (Node)doc_item.value;

                        foreach (Node.NodeItem<object> items in decoration.GetNodes())
                        {
                            if (items.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Fill))
                            {
                                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml((string)items.value);
                                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                                document.SetColor(newColor);
                            }
                        }

                    }

                    //----------------------Props-----------------------------------------

                    if (doc_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Props))
                    {
                        Node props = (Node)doc_item.value;
                        //   Console.WriteLine("Props");

                        foreach (Node.NodeItem<object> items in props.GetNodes())
                        {
                            if (items.name == Lexer.GetPropertyTag(Lexer.PropertyToken.PropItem))
                            {
                                if (items.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == NameProps)
                                {
                                    document.SetDocName((string)items.value);
                                }

                                if (items.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Name)) == CommentProps)
                                {
                                    document.SetComment((string)items.value);
                                }
                            }
                        }

                    }
                   
                }
                documents_list.Add(document);
            }
            return documents_list;
        }
    }
}
