using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSMlib.ViewModel;
using IVSMlib.VsmCanvas;
using IVSMlib.VsmCanvas.CellUI;
using IVSMlib.VsmCanvas.LineUI;

using IVSMlib.Global;
using IVSMlib.EventsSupport;
using IVSMlib.Construct.Actions;
using IVSMlib.VsmCanvas.CellUI.Interface;
using IVSMlib.VsmCanvas.LineUI.Interface;
using IVSMlib.VsmCanvas.Types;
using IVSMlib.Types;
using System.Windows.Media;
using IVSMlib.VsmCanvas.MarksUI;
using IVSMlib.ViewModel.Units;

namespace IVSMlib.Construct
{

    public class MapConstructor
    {
        private List<IAction> ActionsList = new List<IAction>();

        private void CreateActionsList()
        {
            ActionsList = new List<IAction>();
            ActionsList.Add(new NoneAction(this));

            CreateMoveLineAction = new CreateSimpleLineAction(this);

            ActionsList.Add(CreateMoveLineAction);

            CreatePathMoveAction = new CreatePathMoveAction(this);

            ActionsList.Add(CreatePathMoveAction);

            DragAndDropCellAction = new DragAndDropCellAction(this);

            ActionsList.Add(DragAndDropCellAction);
        }

        public MainTableVM Table;

        public PropertyBarVM PropertyBar;

        public TableUI FocusedItem;
        public TableUI SelectedItem;

        private CreateSimpleLineAction CreateMoveLineAction;
        private CreatePathMoveAction CreatePathMoveAction;
        public DragAndDropCellAction DragAndDropCellAction;

        public Actions CurreantAction;

        public enum Actions { None, CreateMoveLine, CreatePathLine, DragCell, RedirectLine}

        public enum Mode { View, Edit }

        private Mode CurrentMode;

        public bool IsMouseDown;

        public Cell DragCell;

        public MapConstructor(MainTableVM _table)
        {
            Table = _table;
            CreateActionsList();
            CurreantAction = Actions.None;
        }

        public void MouseDownEvent(Point e, MouseState state)
        {
            TableUI element = Table.FindeUI(e);

            if (!ActionsList[(int)CurreantAction].MouseDownAction(e, state))
            {

                if (SelectedItem != element && element != null)
                {
                    if (SelectedItem != null)
                    {
                        SelectedItem.Unselect();
                        SelectedItem.DrawUI();
                    }
                  
                        SelectedItem = element;
                        SelectedItem.Select();
                        SelectedItem.DrawUI();

                    if(SelectedItem is IProps item)
                    {
                        PropertyBar.BuildPropertyList(item.GetPropsHolder());
                    }
                }

             
            }

            if (element != null && state.RightButton == System.Windows.Input.MouseButtonState.Released)
            {
                if (element is Cell)
                {
                    if (!(element is EmptyCell))
                    {
                        IsMouseDown = true;
                    }
                }
                element.MouseDown(e);
            }
        }

        public void MouseMoveEvent(Point e)
        {
            ActionsList[(int)CurreantAction].MouseMoveAction(e);
        }

        public void MouseUpEvent(Point e)
        {
            ActionsList[(int)CurreantAction].MouseUpAction(e);
            IsMouseDown = false;
        }

       

        public void EmptyCellMouseDown(Cell sender)
        {
            if (CurreantAction == Actions.None && CurrentMode == Mode.Edit)
            {
                if (sender.TableIndex.Column == 0)
                {

                    PlayerCell player_cell = new PlayerCell();

                      Table.AddCell(player_cell, sender.TableIndex.Row, sender.TableIndex.Column);
                }
                else
                {
                    if (Table.RowColumn[sender.TableIndex.Row][0] is EmptyCell)
                    {
                        MessageBox.Show("Player is nor define");
                        return;
                    }
                    if (GlobalStore.CreatedItem == GlobalStore.TableItems.Action)
                    {
                        ActionCell action_cell = new ActionCell();
                        AttachEventActonCell(ref action_cell);

                        Table.AddCell(action_cell, sender.TableIndex.Row, sender.TableIndex.Column);
                    }
                    else
                    {
                        ConditionCell condition_cell = new ConditionCell();
                        AttachEventConditionCell(ref condition_cell);

                        Table.AddCell(condition_cell, sender.TableIndex.Row, sender.TableIndex.Column);
                    }
                }
            }
        }

        public void AttachEventActonCell(ref ActionCell action_cell)
        {
            action_cell.MouseDownEvent += ActionCellMouseDown;
            action_cell.MoveButtonClickEvent += ActionCellMoveButtomClick;
            action_cell.UpdateTimeCallbak = UpdateTimeAxis;
        }

        public void AttachEventConditionCell(ref ConditionCell condition_cell)
        {
            condition_cell.MoveButtonClickEvent += ConditionCellMoveButtonClick;
            condition_cell.MouseDownEvent += ConditionCellMouseDown;
        }


        public void ActionCellMouseDown(ActionCell sender)
        {
            
        }

        public void ConditionCellMouseDown(ConditionCell sender)
        {

        }

        public void ConditionCellMoveButtonClick(ConditionCell sender, MoveButtonArg arg)
        {
           
            if (CurreantAction == Actions.CreatePathLine)
            {
                    if (CreatePathMoveAction.CreateLine(sender, arg))
                    {

                        CurreantAction = Actions.None;
                        CreatePathMoveAction.CloseEdit();
                        sender.DrawUI();
                    }

            }
            else
            {
                if(arg.ButtinIndex.Side != MoveLineButton.Side.W)
                {
                    CreatePathMoveAction.FirstCell = sender;
                    CreatePathMoveAction.FirstArg = arg;
                    CreatePathMoveAction.OpenEdit();
                    CurreantAction = Actions.CreatePathLine;
                }
            }
        }

        public void ActionCellMoveButtomClick(ActionCell sender, MoveButtonArg arg)
        {
            if (CurreantAction == Actions.None)
            {

                CreateMoveLineAction.FirstCell = sender;

                CreatePathMoveAction.FirstCell = sender;
                CreatePathMoveAction.FirstArg = arg;
                CreatePathMoveAction.OpenEdit();

                CurreantAction = Actions.CreatePathLine;
                return;
            }

            if(CurreantAction == Actions.CreatePathLine)
            {

                if (CreatePathMoveAction.CreateLine(sender,arg))
                {
                    
                    CurreantAction = Actions.None;
                    CreatePathMoveAction.CloseEdit();

                    sender.DrawUI();
                }
            }
            
        }

        public void SetMode(Mode _mode)
        {
            CurrentMode = _mode;
          
            
                for (Int32 row = 0; row <= Table.RowColumn.Count - 1; row++)
                {
                    for (int col = 0; col <= Table.RowColumn[0].Count - 1; col++)
                    {
                      if (Table.RowColumn[row][col] is EmptyCell cell)
                      {
                        if (_mode == Mode.View)
                        {
                            cell.Disable();
                        }
                        else
                        {
                            cell.Enable();
                        }
                      }
                      else if(Table.RowColumn[row][col] is IConnecting connectElement)
                      {
                        if (_mode == Mode.View)
                        {
                            connectElement.DisableConnectButton();
                        }
                        else
                        {
                            connectElement.ActiveConnectButton();
                        }
                    }
                    }
                }
        }

        public void DeleteItem()
        {
            if(SelectedItem != null)
            {
                if(SelectedItem is Cell)
                {
                    DeleteCellItem();
                }

                if(SelectedItem is Line)
                {
                    DeleteLine();
                }
            }
        }

        private void DeleteLine()
        {
            Line del_line = (Line)SelectedItem;

            ((IConnecting)del_line.Left_Cell).DisconnectLine(del_line);
            ((IConnecting)del_line.Right_Cell).DisconnectLine(del_line);

            GlobalStore.MoveModelsStore.Add(del_line.GetModel());

            Table.DeleteVisual(del_line);

            Table.MapLines.Remove(del_line);

            SelectedItem = null; 
        }

        private void DeleteCellItem()
        {
           if(SelectedItem is IConnecting item)
            {
                foreach(List<Line> line_list in item.GetAllLinesList())
                {
                    foreach(Line line in line_list)
                    {
                        if(SelectedItem == line.Left_Cell)
                        {
                            ((IConnecting)line.Right_Cell).DisconnectLine(line);
                            Table.DeleteVisual(line);
                        }
                        else
                        {
                            ((IConnecting)line.Left_Cell).DisconnectLine(line);
                            Table.DeleteVisual(line);
                        }
                    }
                }
            }

            Table.MakeEmptyCell(((Cell)SelectedItem).TableIndex.Row, ((Cell)SelectedItem).TableIndex.Column);

            Table.DeleteVisual(SelectedItem);

            SelectedItem = null;
        }

        public void DeleteCell(Int32 row, Int32 col)
        {
            if (Table.RowColumn[row][col] is IConnecting item)
            {
                foreach (List<Line> line_list in item.GetAllLinesList())
                {
                    foreach (Line line in line_list)
                    {
                        if (Table.RowColumn[row][col] == line.Left_Cell)
                        {
                            ((IConnecting)line.Right_Cell).DisconnectLine(line);
                            Table.DeleteVisual(line);
                        }
                        else
                        {
                            ((IConnecting)line.Left_Cell).DisconnectLine(line);
                            Table.DeleteVisual(line);
                        }
                        Table.MapLines.Remove(line);
                    }
                }
            }

            Table.DeleteVisual(Table.RowColumn[row][col]);

            Table.MakeEmptyCell(Table.RowColumn[row][col].TableIndex.Row, Table.RowColumn[row][col].TableIndex.Column);
  
        }

        public void UpdateTimeAxis()
        {
            for (int col = 1; col <= Table.RowColumn[0].Count - 1; col++)
            {
                Time max_action_time = null;
                Time max_wate_time = null;
                
                for (Int32 row = 0; row <= Table.RowColumn.Count - 1; row++)
                {
                    if(Table.RowColumn[row][col] is ITime cell)
                    {
                        if(max_action_time == null)
                        {
                            max_action_time = cell.IGetActionTime();
                        }
                        else
                        {
                            if (cell.IGetActionTime() > max_action_time)
                            {
                                max_action_time = cell.IGetActionTime();
                            }
                        }

                        if(max_wate_time == null)
                        {
                            max_wate_time = cell.IGetWasteTime();
                        }
                        else
                        {
                            if (cell.IGetWasteTime() > max_wate_time)
                            {
                                max_wate_time = cell.IGetWasteTime();
                            }
                        }
                           
                    }
                }
                
                if(max_action_time == null)
                {
                    max_action_time = new Time(0, Time.Type.Second);
                }

                if (max_wate_time == null)
                {
                    max_wate_time = new Time(0, Time.Type.Second);
                }
                Table.TimeAxisUI.SetActionTime(max_action_time, col-1);
                Table.TimeAxisUI.SetWasteTime(max_wate_time, col-1);
                Table.TimeAxisUI.DrawUI();
            }
        }

        private bool CheckNeibCell(Int32 col_one, Int32 col_two)
        {

            if (col_one +1 == col_two)
            {
                return true;
            }
            if(col_one -1 == col_two)
            {
                return true;
            }
            return false;
        }

        public void UpdateAxisMoveTime(Int32 col_one, Int32 col_two)
        {

            if(CheckNeibCell(col_one, col_two)==false)
            {
                return;
            }

            Time max_move_time = null;

            for (Int32 row = 0; row <= Table.RowColumn.Count - 1; row++)
            {
                if (Table.RowColumn[row][col_one] is IConnecting cell)
                {

                    foreach (Line line in cell.GetAll_E_Lines())
                    {
                        if (line is ILTime t_line)
                        {
                            if (line.Left_Cell.TableIndex.Column + 1 == line.Right_Cell.TableIndex.Column)
                            {
                                if (max_move_time == null)
                                {
                                    max_move_time = t_line.GetLineMoveTime();
                                }
                                else
                                {
                                    if (t_line.GetLineMoveTime() > max_move_time)
                                    {
                                        max_move_time = t_line.GetLineMoveTime();
                                    }
                                }
                            }
                        }
                    }
                }
            }


            Table.TimeAxisUI.SetMoveTime(max_move_time, col_one - 1, col_two);
        }

        public void UnselectCell()
        {
            if (SelectedItem != null)
            {
                SelectedItem.Unselect();
                SelectedItem.DrawUI();
                SelectedItem = null;
            }
        }

        public void RediectLine()
        {

        }

        public void SetColor(Color c)
        {
            if(SelectedItem is Cell cell)
            {
                cell.SetCellColor(c);
            }
        }

        public void RemoveDocumentFromAll(DocumentUnit doc)
        {
            for(int row=0; row <= Table.RowHeight.Count()-1; row++)
            {
                for(int col=0; col <=Table.ColumnWidth.Count() -1; col++)
                {
                    if(Table.RowColumn[row][col] is IDocument doc_item)
                    {
                        if(doc_item.HaveThis(doc))
                        {
                            doc_item.DelDocumentFromList(doc);
                        }
                    }
                }
            }
        }
    }
}
