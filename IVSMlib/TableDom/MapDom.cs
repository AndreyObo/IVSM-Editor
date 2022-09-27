using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSMlib.ViewModel;
using DomCore;
using IVSMlib.VsmCanvas.CellUI.Interface;
using IVSMlib.VsmCanvas.LineUI;
using IVSMlib.ViewModel.Units;
using IVSMlib.Link;
using IVSMlib.VsmCanvas.MarksUI;
using System.Windows;
using System.Windows.Media;
using System.Drawing;
using IVSMlib.TableDom.DocumentDom;
using IVSMlib.VsmCanvas.CellUI;
using IVSMlib.TableDom.CellDom;
using IVSMlib.TableDom.LineDom;
using IVSMlib.IVSMModel;
using IVSMlib.Global;
using IVSMlib.TableDom.MarkDom;

namespace IVSMlib.TableDom
{
    public static class MapDom
    {
        public static List<Linker.ActionLink> ActionLineLinks = new List<Linker.ActionLink>();
        public static List<Linker.ConditionLink> ConditionLineLinks = new List<Linker.ConditionLink>();
        public static List<Linker.DocumentLink> DocumentLinks = new List<Linker.DocumentLink>();

        public static void CreateMapDom(MainTableVM table_vm, ref DomManager manager)
        {

            List<DomManager.BlockFeature> fatures = new List<DomManager.BlockFeature>
            {
                new DomManager.BlockFeature(Lexer.GetMapTag(Lexer.MapToken.Width), table_vm.TableWidth.ToString()),
                new DomManager.BlockFeature(Lexer.GetMapTag(Lexer.MapToken.Heigth), table_vm.TableHeigth.ToString()),
                new DomManager.BlockFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Title), GlobalStore.CurrentIVSM.MapName),

            };
            manager.OpenBlock(Lexer.GetMapTag(Lexer.MapToken.Table), fatures);


          

            //-----------------------RowDefinition--------------------------------

            Node row_definition = new Node(Lexer.GetMapTag(Lexer.MapToken.RowDefinition));

            for (Int32 i = 0; i <= table_vm.RowHeight.Count() - 1; i++)
            {
                row_definition.AddNodeItem<string>(Lexer.GetMapTag(Lexer.MapToken.Row), table_vm.RowHeight[i].ToString());
            }

            manager.AddNode(row_definition);

            //-----------------------ColumnDefinition--------------------------------------------


            Node column_definition = new Node(Lexer.GetMapTag(Lexer.MapToken.ColumnDefinition));

            for (Int32 i = 0; i <= table_vm.ColumnWidth.Count() - 1; i++)
            {
                column_definition.AddNodeItem<string>(Lexer.GetMapTag(Lexer.MapToken.Column), table_vm.ColumnWidth[i].ToString());
            }

            manager.AddNode(column_definition);

            
            if (table_vm.IsEmptyTable() == false)
            {
                manager.OpenBlock(Lexer.GetMapTag(Lexer.MapToken.Cells));

                for (int row = 0; row <= table_vm.RowHeight.Count() - 1; row++)
                {
                    for (int col = 0; col <= table_vm.ColumnWidth.Count() - 1; col++)
                    {
                        if (table_vm.RowColumn[row][col] is IDom DomCell)
                        {
                            manager.AddNode(DomCell.CreateDomNode());
                        }
                    }
                }

                manager.CloseBlock();
            }

            if (table_vm.MapLines.Count() != 0)
            {
                manager.OpenBlock(Lexer.GetMapTag(Lexer.MapToken.Lines));

                foreach (Line line in table_vm.MapLines)
                {
                    if (line is IDom dom_line)
                    {
                        manager.AddNode(dom_line.CreateDomNode());
                    }
                }
                manager.CloseBlock();
            }

            if (DocConnector.CurrentDocList.Count() != 0)
            {
                manager.OpenBlock(Lexer.GetMapTag(Lexer.MapToken.Documents));

                foreach (DocumentUnit doc in DocConnector.CurrentDocList)
                {
                    manager.AddNode(doc.GetDomNode());
                }


                manager.CloseBlock();
            }

            if (table_vm.MarksList.Count() != 0)
            {
                manager.OpenBlock(Lexer.GetMapTag(Lexer.MapToken.Marks));

                foreach (Mark mark in table_vm.MarksList)
                {
                    if (mark is IDom dom_mark)
                    {
                        manager.AddNode(dom_mark.CreateDomNode());
                    }
                }
                //------------------------

                manager.CloseBlock();
            }

          //  manager.CloseBlock();

            manager.CloseBlock();

        }

        public static void ParseNodeToMap(Node root, MainTableVM table_vm)
        {
            if (root.GetName() != "IVSM")
            {
                MessageBox.Show("Файл некорректен");
                return;
            }

            Node table_n = (Node)root.FindeNode(Lexer.GetMapTag(Lexer.MapToken.Table));

            double table_width = Convert.ToDouble(table_n.GetFeature(Lexer.GetMapTag(Lexer.MapToken.Width)));
            double table_height = Convert.ToDouble(table_n.GetFeature(Lexer.GetMapTag(Lexer.MapToken.Heigth)));

            table_vm.FieldCanvas.Width = table_width+100;
            table_vm.FieldCanvas.Height = table_height+100;
            string map_name = table_n.GetFeature(Lexer.GetPropertyTag(Lexer.PropertyToken.Title));



            if (table_n == null)
            {
                MessageBox.Show("Файл некорректен");
                return;
            }

            //--------get row definition-----------------------------

            List<double> rows_height = new List<double>();

            Node row_def = (Node)table_n.FindeNode(Lexer.GetMapTag(Lexer.MapToken.RowDefinition));

            foreach (Node.NodeItem<object> row in row_def.GetNodes())
            {
                rows_height.Add(Convert.ToDouble(row.value));
            }

            //---------get column definition---------------------------------

            List<double> col_width = new List<double>();

            Node col_def = (Node)table_n.FindeNode(Lexer.GetMapTag(Lexer.MapToken.ColumnDefinition));

            foreach (Node.NodeItem<object> col in col_def.GetNodes())
            {
                col_width.Add(Convert.ToDouble(col.value));
            }



            table_vm.CreateNewTable(rows_height, col_width, table_width, table_height);
            DocConnector.CurrentDocList.Clear();


            ActionLineLinks.Clear();
            ConditionLineLinks.Clear();
            DocumentLinks.Clear();

            Node documents_node = (Node)table_n.FindeNode(Lexer.GetMapTag(Lexer.MapToken.Documents));

            if (documents_node != null)
            {
                DefDocumentDom.Get().ParseNode(documents_node).ForEach(doc => DocConnector.CurrentDocList.Add(doc));
            }

            //--------------Cells-------------------------------------------------------
            Node cells = (Node)table_n.FindeNode(Lexer.GetMapTag(Lexer.MapToken.Cells));

            if (cells != null)
            {
                foreach (Node.NodeItem<object> cell_node in cells.GetNodes())
                {
                    Cell cell = ParseCell((Node)cell_node.value);
                    if (cell is ActionCell act_cell)
                    {
                        table_vm.MapConstructorInstance.AttachEventActonCell(ref act_cell);
                    }
                    if (cell is ConditionCell con_cell)
                    {
                        table_vm.MapConstructorInstance.AttachEventConditionCell(ref con_cell);
                    }

                    if (cell != null)
                    {
                        table_vm.AddCell(cell, cell.TableIndex.Row, cell.TableIndex.Column);
                    }
                }
            }

            //--------------Lines-------------------------------------------------------
            Node lines = (Node)table_n.FindeNode(Lexer.GetMapTag(Lexer.MapToken.Lines));

            if (lines != null)
            {
                foreach (Node.NodeItem<object> line_node in lines.GetNodes())
                {
                    if (line_node.name == Lexer.GetUITag(Lexer.UIToken.Line))
                    {
                        PathLine line = (PathLine)PathLineDom.Get().CreateInstanse((Node)line_node.value);
                        line.UpdateTimeCallbak = table_vm.MapConstructorInstance.UpdateAxisMoveTime;
                        table_vm.MapLines.Add(line);
                        table_vm.FieldCanvas.AddVisual(line);
                    }
                }

                ConnectCells(table_vm);
                
            }

            ConnectDocumentToCells();

            Node marks = (Node)table_n.FindeNode(Lexer.GetMapTag(Lexer.MapToken.Marks));

            if (marks != null)
            {
                foreach (Node.NodeItem<object> mark_node in marks.GetNodes())
                {
                    CreateMarks(table_vm, (Node)mark_node.value);
                }
            }
            

            IVSM ivsm = new IVSM();

            ivsm.SetCurrentDocId(GetNextDocId());
            ivsm.SetCurrentLineId(GetNextLineId(table_vm));
            ivsm.MapName = map_name;

            GlobalStore.CurrentIVSM = ivsm;
         
        }

        private static void CreateMarks(MainTableVM table_v, Node marks)
        {
            if(marks.GetName() == Lexer.GetUITag(Lexer.UIToken.Problem))
            {
                MProblem problem = (MProblem)MProblemDom.Get().CreateInstanse(marks);
                table_v.MarksList.Add(problem);
                table_v.FieldCanvas.AddVisual(problem);
                problem.DrawUI();
            }
            if (marks.GetName() == Lexer.GetUITag(Lexer.UIToken.Desicion))
            {
                MDecision dec = (MDecision)MDecisionDom.Get().CreateInstanse(marks);
                table_v.MarksList.Add(dec);
                table_v.FieldCanvas.AddVisual(dec);
                dec.DrawUI();
            }
            if(marks.GetName() == Lexer.GetUITag(Lexer.UIToken.TextLable))
            {
                MTextLable text_l = (MTextLable)MTextLableDom.Get().CreateInstanse(marks);
                table_v.MarksList.Add(text_l);
                table_v.FieldCanvas.AddVisual(text_l);
                text_l.DrawUI();
            }
        }

        private static Int32 GetNextLineId(MainTableVM table_v)
        {
            int id = 0;

            foreach(Line line in table_v.MapLines)
            {
                if (line.Id > id) id = line.Id;
            }

            return id+1;
        }

        private static Int32 GetNextDocId()
        {
            int id = 0;

            foreach (DocumentUnit doc in DocConnector.CurrentDocList)
            {
                if (doc.Id > id) id = doc.Id;
            }

            return id+1;
        }

        private static void ConnectDocumentToCells()
        {
            foreach(Linker.DocumentLink doc_link in DocumentLinks)
            {
                for (int i=0; i <= DocConnector.CurrentDocList.Count() - 1; i++) {
                    if(DocConnector.CurrentDocList[i].Id == doc_link.doc_id)
                    {
                        doc_link.cell.AddDocumentToList(DocConnector.CurrentDocList[i]);
                    }
                }
            }

            DocumentLinks.Clear();
        }

        private static void ConnectCells(MainTableVM table_v)
        {
            CreateActionLinks(table_v);
            CreateConditionLinks(table_v);
        }

        private static void CreateActionLinks(MainTableVM table_v)
        {
            foreach (Linker.ActionLink cell_link in ActionLineLinks)
            {
                Line line = null;

                foreach(Line map_line in table_v.MapLines)
                {
                    if(map_line.Id == cell_link.line_id)
                    {
                        line = map_line;
                        break;
                    }
                }
               

                if (line != null)
                {
                    if (line.Left_Cell == null)
                    {
                        line.Left_Cell = cell_link.cell;
                    }
                    else
                    {
                        if (line.Left_Cell.TableIndex.Column > cell_link.cell.TableIndex.Column)
                        {
                            Cell prev_cell = line.Left_Cell;
                            line.Left_Cell = cell_link.cell;
                            line.Right_Cell = prev_cell;
                        }
                        else
                        {
                            line.Right_Cell = cell_link.cell;
                        }
                    }

                    

                    switch (cell_link.type)
                    {
                        case Linker.ActionLinkType.LTop:
                            cell_link.cell.AddLeftTopLine(line);
                         //   Console.WriteLine("top");
                            break;
                        case Linker.ActionLinkType.LMiddle:
                            cell_link.cell.AddLeftCenterLine(line);
                         //   Console.WriteLine("top");
                            break;
                        case Linker.ActionLinkType.LBottom:
                            cell_link.cell.AddLeftBottomLine(line);
                         //   Console.WriteLine("top");
                            break;
                        case Linker.ActionLinkType.RTop:
                            cell_link.cell.AddRightTopLine(line);
                        //    Console.WriteLine("top");
                            break;
                        case Linker.ActionLinkType.RMiddle:
                            cell_link.cell.AddRightCenterLine(line);
                        //    Console.WriteLine("top");
                            break;
                        case Linker.ActionLinkType.RBottom:
                            cell_link.cell.AddRightBottomLine(line);
                          //  Console.WriteLine("top");
                            break;
                    }
                }
            }

            foreach (Line line in table_v.MapLines)
            {
                line.DrawUI();
            }

          

            ActionLineLinks.Clear();
        }

        private static void CreateConditionLinks(MainTableVM table_v)
        {
            foreach (Linker.ConditionLink cell_link in ConditionLineLinks)
            {

                Line line = null;
               

                foreach (Line map_line in table_v.MapLines)
                {
                    if (map_line.Id == cell_link.line_id)
                    {
                        line = map_line;
                        break;
                    }
                }
                if (line != null)
                {
                   
                    if (line.Left_Cell == null)
                    {
                        line.Left_Cell = cell_link.cell;
                    }
                    else
                    {
                        if (line.Left_Cell.TableIndex.Column > cell_link.cell.TableIndex.Column)
                        {
                            Cell prev_cell = line.Left_Cell;
                            line.Left_Cell = cell_link.cell;
                            line.Right_Cell = prev_cell;
                           
                        }
                        else
                        {
                            line.Right_Cell = cell_link.cell;
                        }
                    }
                    switch (cell_link.type)
                    {
                        case Linker.ConditionLinkType.Left:
                            cell_link.cell.AddLeftLine(line);
                            break;
                        case Linker.ConditionLinkType.Bottom:
                            cell_link.cell.AddBottomLine(line);
                            break;
                        case Linker.ConditionLinkType.Right:
                            cell_link.cell.AddRightLine(line);
                            break;
                        case Linker.ConditionLinkType.Top:
                            cell_link.cell.AddTopLine(line);
                            break;
                    }
                }
            }

            foreach (Line line in table_v.MapLines)
            {
                line.DrawUI();
            }

            ConditionLineLinks.Clear();
        }

        private static Cell ParseCell(Node cell_node)
        {
            Console.WriteLine("cell parrsee -->" + cell_node.GetName());
            if(cell_node.GetName() == Lexer.GetUITag(Lexer.UIToken.Player)) {
                return (Cell)PlayerDom.Get().CreateInstanse(cell_node);
            }
            if(cell_node.GetName() == Lexer.GetUITag(Lexer.UIToken.Action))
            {
                return (Cell)ActionDom.Get().CreateInstanse(cell_node);
            }
            if (cell_node.GetName() == Lexer.GetUITag(Lexer.UIToken.Condition))
            {
                return (Cell)ConditionDom.Get().CreateInstanse(cell_node);
            }
            return null;
        }

       
    }
}
