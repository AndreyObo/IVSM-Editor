using IVSMlib.VsmCanvas.CellUI;
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
    public class DragAndDropCellAction : IAction
    {
        private MapConstructor Construcor;

        private DrawingVisual DemoCell;

        private Brush DemoCellBrush;

        private Cell DragCell;

        private Size d_cell_size;

        private Point start;

        public DragAndDropCellAction(MapConstructor owner)
        {
            Construcor = owner;
            DemoCellBrush = new SolidColorBrush(Colors.Green);
            DemoCellBrush.Opacity = 0.7;
            DemoCell = new DrawingVisual();

            d_cell_size = new Size();

            start = new Point();
        }

        public void OpenDrag(Cell drag_cell, Point e)
        {
            DragCell = drag_cell;
            d_cell_size.Width = drag_cell.GetContentSize().Width;
            d_cell_size.Height = drag_cell.GetContentSize().Height;

         //   start.X = e.X - drag_cell.GetLocation().X;
         //   start.Y = e.Y - drag_cell.GetLocation().Y;

            Construcor.Table.AddVisual(DemoCell);

        }

        public void CloseDrag()
        {
            DrawingContext dc = DemoCell.RenderOpen();
            dc.Close();

            Construcor.Table.DeleteVisual(DemoCell);
        }

        public bool MouseDownAction(Point e, MouseState state)
        {
            return false;
        }

        public void MouseMoveAction(Point e)
        {
          
            DrawingContext dc = DemoCell.RenderOpen();

            dc.DrawRectangle(DemoCellBrush, new Pen(Brushes.Transparent,1), new Rect(e.X+5, e.Y+5, d_cell_size.Width, d_cell_size.Height));

            dc.Close();
        }

        public void MouseUpAction(Point e)
        {
            CloseDrag();
            if (Construcor.Table.GetFocucedCell() is Cell target_cell)
            {
                if (DragCell is PlayerCell p_cell)
                {
                    DragPlayerCell(p_cell, target_cell);
                }
                else if (DragCell is ActionCell a_cell)
                {
                    DragActionCell(a_cell, target_cell);
                }
                else if (DragCell is ConditionCell c_cell)
                {
                    DragConditionCell(c_cell, target_cell);
                }


                Construcor.CurreantAction = MapConstructor.Actions.None;
            }
        }

        private void DragPlayerCell(PlayerCell cell, Cell target_cell)
        {
            if (target_cell.TableIndex.Column !=0)
            {
                MessageBox.Show("Player can be only in first row");
                return;
            }
            else
            {
                if(Construcor.Table.RowColumn[target_cell.TableIndex.Row][target_cell.TableIndex.Column] is PlayerCell)
                {
                    MessageBox.Show("Cell consist player");
                    return;
                }
                Construcor.Table.MakeEmptyCell(DragCell.TableIndex.Row, DragCell.TableIndex.Column);

                Construcor.Table.MoveCellTo(DragCell, target_cell.TableIndex.Row, target_cell.TableIndex.Column);

            }
        }

        private void DragActionCell(ActionCell cell, Cell target_cell)
        {
            if (Construcor.Table.RowColumn[target_cell.TableIndex.Row][0] is EmptyCell)
            {
                MessageBox.Show("Player is nor define");
             //   Construcor.CurreantAction = MapConstructor.Actions.None;
                return;
            }

            if(target_cell.TableIndex.Row > cell.TableIndex.Row)
            {
              
            }
            Construcor.Table.MakeEmptyCell(DragCell.TableIndex.Row, DragCell.TableIndex.Column);

            Construcor.Table.MoveCellTo(DragCell, target_cell.TableIndex.Row, target_cell.TableIndex.Column);
            
        }

        private void DragConditionCell(ConditionCell cell, Cell target_cell)
        {
            if (Construcor.Table.RowColumn[target_cell.TableIndex.Row][0] is EmptyCell)
            {
                MessageBox.Show("Player is nor define");
                //   Construcor.CurreantAction = MapConstructor.Actions.None;
                return;
            }

            if (target_cell.TableIndex.Row > cell.TableIndex.Row)
            {

            }
            Construcor.Table.MakeEmptyCell(DragCell.TableIndex.Row, DragCell.TableIndex.Column);

            Construcor.Table.MoveCellTo(DragCell, target_cell.TableIndex.Row, target_cell.TableIndex.Column);

        }
    }
}
