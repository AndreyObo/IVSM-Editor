using IVSMlib.EventsSupport;
using IVSMlib.VsmCanvas;
using IVSMlib.VsmCanvas.CellUI;
using IVSMlib.VsmCanvas.LineUI;
using IVSMlib.VsmCanvas.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IVSMlib.Construct.Actions
{
    class CreateSimpleLineAction : IAction
    {
        private MapConstructor Constructor;

        private DrawingVisual DemoLine;
        private Pen DemoLinePen;


        public CreateSimpleLineAction(MapConstructor owner)
        {
            Constructor = owner;
            DemoLinePen = new Pen(Brushes.Green, 3);
            DemoLinePen.DashStyle = DashStyles.Dash;
            DemoLine = new DrawingVisual();
        }

        public Cell FirstCell { get; set; }
        public MoveButtonArg FirstArg { get; set; }
        public MoveLine Line { get; set; }

        public enum LineType { Start, End }
        public LineType InsertLineType;

        public void Clear()
        {
            Line = null;
            FirstArg = null;
            FirstCell = null;
        }

        public void OpenEdit()
        {
            Constructor.Table.AddVisual(DemoLine);
        }

        public void CloseEdit()
        {
            Constructor.Table.DeleteVisual(DemoLine);
        }

        public bool CreateToConditionLine(Cell SecondCell, MoveButtonArg Secar)
        {
            //if (FirstArg.ButtinIndex.Pos != 2 || Secar.ButtinIndex.Side != MoveLineButton.Side.W)
            //{
            //    MessageBox.Show("Wrong Connector");
            //    return false;
            //}

            if(FirstArg.ButtinIndex.Side != MoveLineButton.Side.E || Secar.ButtinIndex.Side != MoveLineButton.Side.W)
            {
                MessageBox.Show("Wrong Connector");
                return false;
            }
            if(FirstCell.TableIndex.Row > SecondCell.TableIndex.Row && FirstArg.ButtinIndex.Pos !=1)
            {
                MessageBox.Show("Wrong Connector");
                return false;
            }

            if (FirstCell.TableIndex.Row < SecondCell.TableIndex.Row && FirstArg.ButtinIndex.Pos != 3)
            {
                MessageBox.Show("Wrong Connector");
                return false;
            }
            //if (FirstCell.TableIndex.Row != SecondCell.TableIndex.Row)
            //{

            //    MessageBox.Show("Wrong row");
            //    return false;
            //}
            MoveLine line = new MoveLine();
            Constructor.Table.AddUI(line);
            FirstArg.AddLineHandler(line);
            Secar.AddLineHandler(line);
            line.DrawUI();
            return true;
        }

        public bool CreatedToActionLine(Cell SecondCell, MoveButtonArg Secarg)
        {
            if(FirstArg.ButtinIndex.Side == Secarg.ButtinIndex.Side)
            {
                MessageBox.Show("WrongSide");
                return false;
            }
            if(FirstArg.ButtinIndex.Pos !=Secarg.ButtinIndex.Pos)
            {
                MessageBox.Show("Wrong Connector");
                return false;
            }
            if(FirstArg.ButtinIndex.Side == MoveLineButton.Side.E && FirstArg.ButtinIndex.Pos == 2)
            {
                if(Secarg.ButtinIndex.Pos != 2 || Secarg.ButtinIndex.Side != MoveLineButton.Side.W || SecondCell.TableIndex.Row != FirstCell.TableIndex.Row)
                {
                    MessageBox.Show("Whrong Connectog");
                    return false;
                }
            }

            MoveLine line = new MoveLine();
            Constructor.Table.AddUI(line);
            FirstArg.AddLineHandler(line);
            Secarg.AddLineHandler(line);
            line.DrawUI();
            //Line = null;
            //FirstArg = null;
            //FirstCell = null;
            return true;
        }
        
        public bool CreateToConditionLine()
        {
            return false;
        }

        public void SetLinePos(Point e)
        {
            //if (InsertLineType == LineType.Start)
            //{
            //    Line.SetEndPoint(e.X - 3, e.Y - 3);
            //}
            //if (InsertLineType == LineType.End)
            //{
            //    Line.SetStartPoint(e.X + 3, e.Y + 3);
            //}
            //Line.DrawUI();

            DrawingContext dc = DemoLine.RenderOpen();

            dc.DrawLine(DemoLinePen, FirstArg.StartPoint, new Point(e.X-3, e.Y-3));

            dc.Close();
        }

        public void MouseMoveAction(Point e)
        {
            TableUI finde_ui = Constructor.Table.FindeUI(e);

            if (finde_ui is ActionCell || finde_ui is ConditionCell)
            {
                if (Constructor.FocusedItem != null && Constructor.FocusedItem != finde_ui)
                {
                    Constructor.FocusedItem.MouseLeave();
                }
                if (finde_ui == Constructor.FocusedItem)
                {
                    Constructor.FocusedItem.MouseMove(e);
                }
                Constructor.FocusedItem = finde_ui;
                Constructor.FocusedItem.MouseEnter();
            }
            SetLinePos(e);
        }

        public void MouseUpAction(Point e)
        {
            throw new NotImplementedException();
        }

        public bool MouseDownAction(Point e, MouseState state)
        {
            return false;
        }
    }
}
