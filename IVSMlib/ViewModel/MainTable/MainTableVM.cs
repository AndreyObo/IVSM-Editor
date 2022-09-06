using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

using IVSMlib.VsmCanvas;
using IVSMlib.VsmCanvas.Units;
using IVSMlib.VsmCanvas.CellUI;
using IVSMlib.VsmCanvas.MarksUI;
using IVSMlib.IVSMModel;
using IVSMlib.Construct;
using IVSMlib.VsmCanvas.Types;
using IVSMlib.Types;
using IVSMlib.TableDom;
using DomCore;
using DomCore.Writers;
using IVSMlib.Global;
using DomCore.Parser;
using IVSMlib.Link;
using IVSMlib.Windows;
using IVSMlib.Utils;

namespace IVSMlib.ViewModel
{
    public class MainTableVM : DependencyObject, INotifyPropertyChanged
    {
        private const double MIN_COLUMN_WIDTH= 70;
        private const double MIN_ROW_HEIGHT = 70;

        public MainTable window;

  //      public IVSM IVSM_Model;

        public MapConstructor MapConstructorInstance;
        private CreateMarkAction CreateMarkActionInstance;
        private MoveMarkAction MoveMarkInstance;
        private SetMarkSizeAction SetMarkSizeInstace;

        public PropertyBarVM PropertyBar;
        private ControlBarVM ControlBar;

        private Window ColWindow;
        private bool IsColWindowVisible;

        //   private bool Debug = false;

        private List<IAction> TableActions = new List<IAction>();

        public List<Mark> MarksList = new List<Mark>();

        private Int32 TimeLableOffset = 100;

        private void CreateActions()
        {
            TableActions.Add(new SetHorizontalColumnAction(this));
            TableActions.Add(new SetVerticalColumnAction(this));
            //     TableActions.Add(new CreateMoveLineAction(this));
            CreateMarkActionInstance = new CreateMarkAction(this);
            TableActions.Add(CreateMarkActionInstance);
            MoveMarkInstance = new MoveMarkAction(this);
            TableActions.Add(MoveMarkInstance);
            TableActions.Add(new NoneAction(this));

            SetMarkSizeInstace = new SetMarkSizeAction(this);
            TableActions.Add(SetMarkSizeInstace);

            TableActions.Add(new RulerOptionAction(this));
        }

        public ICommand Delete { get; set; }

        private class SetHorizontalColumnAction : IAction
        {
            private MainTableVM owner;

            public SetHorizontalColumnAction(MainTableVM _owner)
            {
                owner = _owner;
            }
            public void MouseDownAction(Point e, MouseState state)
            {
                // throw new NotImplementedException();
                owner.CurrentAction = Actions.None;
            }

            public void MouseMoveAction(Point e)
            {
               // bool cheek = false;
               // if (e.X > owner.RullerThikknes && e.X < owner.TableWidth + owner.RullerThikknes-Column.BoxWidth())
              //  {
                    if (((Column)owner.SelectedColumn).PrevColumn == null)
                    {
                    if (e.X > owner.RullerThikknes + owner.TimeLableOffset && (e.X < ((Column)owner.SelectedColumn).NextColumn.GetPosition().X - 60))
                        ((Column)owner.SelectedColumn).SetLocation(e.X - owner.RullerThikknes, e.Y);
                     //   owner.SelectedColumn.DrawUI(new Point(e.X - owner.RullerThikknes, e.Y));
                        return;
                    }

                    if (((Column)owner.SelectedColumn).NextColumn == null)
                    {
                       if(e.X < owner.TableWidth + owner.RullerThikknes - Column.BoxWidth()-20 && (e.X > ((Column)owner.SelectedColumn).PrevColumn.GetPosition().X + 60))
                        ((Column)owner.SelectedColumn).SetLocation(e.X - owner.RullerThikknes, e.Y);
                    //owner.SelectedColumn.DrawUI(new Point(e.X - owner.RullerThikknes, e.Y));
                    return;
                    }

                if ((e.X < ((Column)owner.SelectedColumn).NextColumn.GetPosition().X - 60) && (e.X > ((Column)owner.SelectedColumn).PrevColumn.GetPosition().X + 60))
                {
                    ((Column)owner.SelectedColumn).SetLocation(e.X - owner.RullerThikknes, e.Y);
                    //  owner.SelectedColumn.DrawUI(new Point(e.X - owner.RullerThikknes, e.Y));
                }
             //   }
            }

            public void MouseUpAction(Point e)
            {
                Int32 first_colum_index=0;
                Int32 second_colum_index = 0;
                if (((Column)owner.SelectedColumn).PrevColumn == null)
                {
                    owner.ColumnWidth[0] = ((Column)owner.SelectedColumn).GetPosition().X;
                    owner.ColumnWidth[1] = ((Column)owner.SelectedColumn).NextColumn.GetPosition().X - ((Column)owner.SelectedColumn).GetPosition().X;
                    first_colum_index = 0;
                    second_colum_index = 1;
                }
                else if(((Column)owner.SelectedColumn).NextColumn == null)
                {
                    first_colum_index = owner.ColumnWidth.Count() - 1;
                    second_colum_index = ((Column)owner.SelectedColumn).Index;
                    owner.ColumnWidth[first_colum_index] = owner.TableWidth - ((Column)owner.SelectedColumn).GetPosition().X;
                    owner.ColumnWidth[second_colum_index] = ((Column)owner.SelectedColumn).GetPosition().X - ((Column)owner.SelectedColumn).PrevColumn.GetPosition().X;
                }
                else
                {
                    first_colum_index = ((Column)owner.SelectedColumn).Index;
                    second_colum_index = ((Column)owner.SelectedColumn).Index + 1;
                    owner.ColumnWidth[first_colum_index] = ((Column)owner.SelectedColumn).GetPosition().X - ((Column)owner.SelectedColumn).PrevColumn.GetPosition().X;
                    owner.ColumnWidth[second_colum_index] = ((Column)owner.SelectedColumn).NextColumn.GetPosition().X - ((Column)owner.SelectedColumn).GetPosition().X;
                   
                }

                owner.horisontal_ruler.UpdateButtons();
                owner.horisontal_ruler.DrawUI();

                owner.SetCellLoc();
               // owner.ChangeCellWidth(first_colum_index, second_colum_index);
                owner.ChangeCellSize();
                owner.CurrentAction = Actions.None;
                owner.SelectedColumn = null;

                //   owner.TimeAxisUI.InizialazeColumnList(owner.ColumnWidth);
                owner.TimeAxisUI.DrawUI();
                //foreach (double width in owner.ColumnWidth)
                //{
                //    Console.WriteLine(width.ToString());
                //}

            }
        }

        private class SetVerticalColumnAction : IAction
        {
            private MainTableVM owner;

            public SetVerticalColumnAction(MainTableVM _owner)
            {
                owner = _owner;
            }
            public void MouseDownAction(Point e, MouseState state)
            {
              //  throw new NotImplementedException();
            }

            public void MouseMoveAction(Point e)
            {
                //if (e.Y > owner.RullerThikknes && e.Y < owner.TableHeigth + owner.RullerThikknes- Row.BoxHeigth())
                //{
                //    owner.SelectedColumn.DrawUI(new Point(e.X, e.Y-owner.RullerThikknes));
                //}

                if (((Row)owner.SelectedColumn).PrevRow == null)
                {
                    if (e.Y > owner.RullerThikknes + 20+ owner.TableOffset.Y && (e.Y < (((Row)owner.SelectedColumn).NextRow.GetPosition().Y+ owner.TableOffset.Y - 60)))
                        ((Row)owner.SelectedColumn).SetLocation(e.X, e.Y - owner.RullerThikknes - owner.TableOffset.Y);
                //    owner.SelectedColumn.DrawUI(new Point(e.X , e.Y - owner.RullerThikknes- owner.TableOffset.Y));
                    return;
                }

                if (((Row)owner.SelectedColumn).NextRow == null)
                {
                    if (e.Y < owner.TableHeigth+ owner.TableOffset.Y + owner.RullerThikknes - Row.BoxHeigth() - 20 && (e.Y > (((Row)owner.SelectedColumn).PrevRow.GetPosition().Y + 60)))
                        ((Row)owner.SelectedColumn).SetLocation(e.X, e.Y - owner.RullerThikknes - owner.TableOffset.Y);
                    //    owner.SelectedColumn.DrawUI(new Point(e.X, e.Y - owner.RullerThikknes- owner.TableOffset.Y));
                    return;
                }

                if ((e.Y < (((Row)owner.SelectedColumn).NextRow.GetPosition().Y+ owner.TableOffset.Y - 60)) && (e.Y > (((Row)owner.SelectedColumn).PrevRow.GetPosition().Y+ owner.TableOffset.Y + 60)))
                {
                    ((Row)owner.SelectedColumn).SetLocation(e.X, e.Y - owner.RullerThikknes - owner.TableOffset.Y);
                    //owner.SelectedColumn.DrawUI(new Point(e.X, e.Y - owner.RullerThikknes- owner.TableOffset.Y));
                }
            }

            public void MouseUpAction(Point e)
            {
                if (((Row)owner.SelectedColumn).PrevRow == null)
                {
                    owner.RowHeight[0] = ((Row)owner.SelectedColumn).GetPosition().Y;
                    owner.RowHeight[1] = ((Row)owner.SelectedColumn).NextRow.GetPosition().Y - ((Row)owner.SelectedColumn).GetPosition().Y;

                }
                else if (((Row)owner.SelectedColumn).NextRow == null)
                {
                    owner.RowHeight[owner.RowHeight.Count() - 1] = owner.TableHeigth - ((Row)owner.SelectedColumn).GetPosition().Y;
                    owner.RowHeight[((Row)owner.SelectedColumn).Index] = ((Row)owner.SelectedColumn).GetPosition().Y - ((Row)owner.SelectedColumn).PrevRow.GetPosition().Y;
                }
                else
                {
                    owner.RowHeight[((Row)owner.SelectedColumn).Index] = ((Row)owner.SelectedColumn).GetPosition().Y - ((Row)owner.SelectedColumn).PrevRow.GetPosition().Y;
                    owner.RowHeight[((Row)owner.SelectedColumn).Index + 1] = ((Row)owner.SelectedColumn).NextRow.GetPosition().Y - ((Row)owner.SelectedColumn).GetPosition().Y;
                }
                owner.SetCellLoc();
                owner.ChangeCellSize();
                owner.CurrentAction = Actions.None;
                owner.SelectedColumn = null;

                Console.WriteLine("---------rowwss---------");

                foreach (double height in owner.RowHeight)
                {
                    Console.WriteLine(height.ToString());
                }
                owner.CurrentAction = Actions.None;
                owner.SelectedColumn = null;
            }
        }

        private class NoneAction : IAction
        {
            private MainTableVM owner;

            public NoneAction(MainTableVM _owner)
            {
                owner = _owner;
            }

            public void MouseDownAction(Point e, MouseState state)
            {
                Console.WriteLine("Table VmClick clickkkk");
                if (owner.IsColWindowVisible == true)
                {
                    owner.ColWindow.Close();
                    owner.ColWindow = null;
                    owner.IsColWindowVisible = false;

                }
                TableUI finde_ui = owner.FindeUI(e);

                if(finde_ui == null)
                {
                    owner.UnselectTableUI();
             //       owner.PropertyBar.Clear();
                 

                }

             //   Console.WriteLine("From main VM - "+finde_ui.ToString());

              
                if(finde_ui is Mark mark)
                {
                   // mark.MouseDown(e);
                   if(mark is ISize size_mark)
                    {
                        if(size_mark.IsSetSizeMode())
                        {
                            owner.SetMarkSizeInstace.StartPos = new Point(mark.GetLocation().X, mark.GetLocation().Y);
                            owner.SetMarkSizeInstace.Mark = size_mark;
                            owner.CurrentAction = Actions.SetMarkSize;
                            return;
                        }
                    }

                    owner.SelectTableUI(mark);
                    owner.CurrentAction = Actions.MoveMark;
                    owner.MoveMarkInstance.SelectedMark = mark;
                    owner.MapConstructorInstance.UnselectCell();
                    
                    owner.MoveMarkInstance.StartPoint = new Point(e.X - mark.GetLocation().X, e.Y - mark.GetLocation().Y);

                    if (finde_ui is IProps props_ui)
                    {
                        owner.PropertyBar.BuildPropertyList(props_ui.GetPropsHolder());
                    }

                    return;

                }
                // Console.WriteLine(owner.FieldCanvas.GetVisual(e).ToString());
                if (e.X > owner.RullerThikknes && e.X < owner.RullerThikknes + owner.TableWidth && e.Y > owner.TableOffset.Y + owner.RullerThikknes && e.Y < owner.TableOffset.Y + owner.RullerThikknes + owner.TableHeigth)
                {
                    owner.UnselectTableUI();
                    owner.PropertyBar.Clear();
                    owner.MapConstructorInstance.MouseDownEvent(e, state);
                    return;
                }
              
                

                if (finde_ui != null)
                {
                    if ((finde_ui is Column) /*&& (((TableFile)finde_ui).IsActive)*/)
                    {
                      //  owner.StartPos.X = e.X - ((TableFiled)finde_ui).GetPosition().X;
                        owner.SelectedColumn = (TableUI)finde_ui;
                        owner.CurrentAction = Actions.SetColumnH;
                    }
                    if ((finde_ui is Row) /*&& (((TableFiled)finde_ui).IsActive)*/)
                    {
                        owner.SelectedColumn = (TableUI)finde_ui;
                        owner.CurrentAction = Actions.SetColumnV;
                    }
                    if(finde_ui is TButton)
                    {
                        finde_ui.MouseDown(e);
                    }
                    if(finde_ui is HorisontalRuler || finde_ui is VerticalRuler)
                    {
                        finde_ui.MouseDown(e);
                    }
                    owner.UnselectTableUI();
                }
            }

            public void MouseMoveAction(Point e)
            {
                DrawingVisual finde_ui = owner.FieldCanvas.GetVisual(e);

                TableUI element = owner.FindeUI(e);

                if (finde_ui == null)
                {
                    if (owner.FocusedUI != null)
                    {
                        owner.FocusedUI.MouseLeave();
                        owner.FocusedUI = null;
                        return;
                    }
                }

                if (owner.FocusedUI != null && owner.FocusedUI != element)
                {
                    owner.FocusedUI.MouseLeave();
                 //   Console.WriteLine("Leveeee");
                    owner.FocusedUI = null;
                }
                else if (owner.FocusedUI != null && element == owner.FocusedUI)
                {
                    owner.FocusedUI.MouseMove(e);
                    owner.FocusedUI.DrawUI();
                  //  Console.WriteLine("Moveee");
                }
                else if (element != null)
                {
                    owner.FocusedUI = element;
                    owner.FocusedUI.MouseEnter();
                  //  Console.WriteLine("Entereeee");

                }

                //----------------------------------------
                if (e.X > owner.RullerThikknes && e.X  < owner.RullerThikknes + owner.TableWidth && e.Y > owner.TableOffset.Y + owner.RullerThikknes && e.Y < owner.TableOffset.Y + owner.RullerThikknes+owner.TableHeigth)
                {
                    if (owner.IsCursorChanged)
                    {
                        owner.FieldCanvas.Cursor = Cursors.Arrow;
                    }
                    owner.MapConstructorInstance.MouseMoveEvent(e);
                    return;
                }
                //----------------------------------------
                //   Console.WriteLine("mmm");
                //      Point mouse_click = new Point(e.X, e.Y);
            //    DrawingVisual finde_ui = owner.FieldCanvas.GetVisual(e);

          //      TableUI element = owner.FindeUI(e);

                //if (finde_ui == null)
                //{
                //    if (owner.FocusedUI != null)
                //    {
                //        owner.FocusedUI.MouseLeave();
                //        owner.FocusedUI = null;
                //        return;
                //    }
                //}

                //if (owner.FocusedUI != null && owner.FocusedUI != element)
                //{
                //    owner.FocusedUI.MouseLeave();
                //}
                //if (owner.FocusedUI != null && element == owner.FocusedUI)
                //{
                //    owner.FocusedUI.MouseMove(e);
                //}
                //if (element != null)
                //{
                //    owner.FocusedUI = element;
                //    owner.FocusedUI.MouseEnter();
                //}


                bool cheek_goal = false;

                if (finde_ui != null)
                {
                    
                    if ((finde_ui is Column) /*&& (((TableFiled)finde_ui).IsActive)*/)
                    {
                        owner.FieldCanvas.Cursor = Cursors.SizeWE;
                        owner.IsCursorChanged = true;
                        cheek_goal = true;


                    }
                    if ((finde_ui is Row)/* && (((TableFiled)finde_ui).IsActive)*/)
                    {
                        owner.FieldCanvas.Cursor = Cursors.SizeNS;
                        owner.IsCursorChanged = true;
                        cheek_goal = true;
                    }

                    if (!cheek_goal && owner.IsCursorChanged)
                    {
                        owner.FieldCanvas.Cursor = Cursors.Arrow;
                        owner.IsCursorChanged = false;
                    }

                }
            }

            public void MouseUpAction(Point e)
            {
                owner.MapConstructorInstance.MouseUpEvent(e);
            }
        }

        private class CreateMarkAction : IAction
        {
            private MainTableVM owner;
            public Mark InsertMark;
           

            public CreateMarkAction(MainTableVM _owner)
            {
                owner = _owner;
            }

            public void MouseDownAction(Point e, MouseState state)
            {
                if (state.LeftButton == MouseButtonState.Pressed)
                {
                    owner.CurrentAction = Actions.None;
                    owner.MarksList.Add(InsertMark);
                    InsertMark = null;
                }

                if (state.RightButton == MouseButtonState.Pressed)
                {
                    owner.FieldCanvas.DeleteVisual(InsertMark);
                    owner.CurrentAction = Actions.None;
                    InsertMark = null;
                }
            }

            public void MouseMoveAction(Point e)
            {
                InsertMark.SetLocation(e.X, e.Y);
            }

            public void MouseUpAction(Point e)
            {
                
            }
        }

        private class MoveMarkAction:IAction
        {
            private MainTableVM owner;
            public Mark SelectedMark;
            public Point StartPoint;

            public MoveMarkAction(MainTableVM _owner)
            {
                owner = _owner;
            }

            public void MouseDownAction(Point e, MouseState state)
            {
              //  throw new NotImplementedException();
            }

            public void MouseMoveAction(Point e)
            {
                SelectedMark.SetLocation(e.X - StartPoint.X, e.Y - StartPoint.Y);
            }

            public void MouseUpAction(Point e)
            {
                owner.CurrentAction = Actions.None;
            }
        }

        private class SetMarkSizeAction : IAction
        {
            public Point StartPos;
            public ISize Mark;
            private MainTableVM Owner;

            public SetMarkSizeAction(MainTableVM _owner)
            {
                Owner = _owner;
            }

            public void MouseDownAction(Point e, MouseState state)
            {
              //  throw new NotImplementedException();
            }

            public void MouseMoveAction(Point e)
            {
                Mark.SetSize(e.X - StartPos.X, e.Y - StartPos.Y);
            }

            public void MouseUpAction(Point e)
            {
                Owner.CurrentAction = Actions.None;
            }
        }

        private class RulerOptionAction : IAction
        {

            private MainTableVM owner;

            public RulerOptionAction(MainTableVM _owner)
            {
                owner = _owner;
            }

            public void MouseDownAction(Point e, MouseState state)
            {
                if(owner.FocusedUI != null)
                {
                    owner.FocusedUI.MouseDown(e);
                 //   owner.CurrentAction = Actions.None;
                }
            }

            public void MouseMoveAction(Point e)
            {
                TableUI element = owner.FindeUI(e);

                if (element is HorisontalRuler || element is VerticalRuler)
                {
                    if (owner.FocusedUI != null && owner.FocusedUI != element)
                    {
                        owner.FocusedUI.MouseLeave();
                        //   Console.WriteLine("Leveeee");
                        owner.FocusedUI = null;
                    }
                    else if (owner.FocusedUI != null && element == owner.FocusedUI)
                    {
                        owner.FocusedUI.MouseMove(e);
                        owner.FocusedUI.DrawUI();
                        //  Console.WriteLine("Moveee");
                    }
                    else if (element != null)
                    {
                        owner.FocusedUI = element;
                        owner.FocusedUI.MouseEnter();
                        //  Console.WriteLine("Entereeee");

                    }
                }
            }

            public void MouseUpAction(Point e)
            {
               // throw new NotImplementedException();
            }
        }

        public enum Actions {SetColumnH, SetColumnV, CreateMark, MoveMark, None, SetMarkSize, RulerOptions }


        public Actions CurrentAction;
        public VsmCustomCanvas FieldCanvas { get; set; }

     //   public Command MouseDownCommand;

        public ICommand mouse_down { get; set; }
        public ICommand mouse_up { get; set; }

    //    private HColumnUI SelectedColumn;
     //   private VColumnUI SelectedColumnV;

        private TableUI SelectedColumn;

        private TableUI FocusedUI;

        private TableUI SelectedUI;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        //--------------------------------------------------

        private TButton AddColumnButton;
        private TButton AddRowButton;

        //----------------------------------------------------
        private HorisontalRuler horisontal_ruler;
        private VerticalRuler vertical_ruler;

        private DrawingVisual WorkingZoneBorder;

        private DrawingVisual RullerBox;

        //-------------------------------------------------

        private Column FirstColumn;
        private Column LastColumn;
        private Row FirstRow;
        private Row LastRow;

        //----------------------------------------------------

        public double TableWidth = 500;
        public double TableHeigth = 300;

        public Int32 RullerThikknes=15;

        public Point TableOffset = new Point(0, 70); //Global Table offset 

        //----------------------------------------------------
        public TimeAxis TimeAxisUI;
        //----------------------------------------------------
        // private MoveLine SelectedLine;

        //----------------------------------------------------

        private double x;
        private double y;


        public MouseButtonState MouseLeftButton { get; set; }
        public MouseButtonState MouseRightButton { get; set; }
        //    private Point StartPos = new Point(0, 0);

        //---------------------------
        //  private bool IsMouseDown;
        //---------------------------

        //---------------------------------------------------?????????????????????
        //      private List<Column> Columns = new List<Column>();
        //  private List<Row> Rows = new List<Row>();

        public List<List<Cell>> RowColumn { get; set; }
        public List<IVSMlib.VsmCanvas.LineUI.Line> MapLines { get; set; }
        //---------------------------------------------------?????????????????????

        public List<double> ColumnWidth = new List<double>();
        //{
        //    0,0,0,0
        //};

        public List<double> RowHeight = new List<double>();
        //{
        //    0,0,0,0
        //};


        //-------------------------------------------------------------------------

        public double x_pos
        {
            get { return x; }
            set { x = value; OnPropertyChanged("x_pos"); MouseMove(); }
        }

        public double y_pos
        {
            get { return y; }
            set { y = value; OnPropertyChanged("y_pos"); }
        }

        //----------------------------------------------

        private Boolean IsCursorChanged;

        public void SetMapTitle(string title)
        {
            GlobalStore.CurrentIVSM.MapName = title;
        }


        public MainTableVM(VsmCustomCanvas canvas)
        {

            Delete = new Command(DeleteCommand);
          //  IVSM_Model = new IVSM();

            GlobalStore.CurrentIVSM = new IVSM();
          //  ControlBar.TableName = GlobalStore.CurrentIVSM.MapName;

            MapLines = new List<VsmCanvas.LineUI.Line>();
            FocusedUI = null;
            MapConstructorInstance = new MapConstructor(this);
            FieldCanvas = canvas;
            IsCursorChanged = false;
            CreateActions();
            CurrentAction = Actions.None;
            Column.X_Offset = RullerThikknes;
            Column.Y_offset = (int)TableOffset.Y;
            Row.Y_Offset = RullerThikknes+ (int)TableOffset.Y;

            IsColWindowVisible = false;

            //    FirstColumn = new Column();
            //    FirstColumn.Index = 0;
            //    FirstColumn.SetColumnWidth(TableHeigth);
            //    FirstColumn.SetActiveMode(true);
            //    FirstColumn.SetLocation(100, 0);
            ////    FirstColumn.DrawUI(new Point(100, 0));

            //    FirstColumn.PrevColumn = null;


            //    var c_two = new Column();
            //    c_two.Index = 1;
            //    c_two.SetColumnWidth(TableHeigth);
            //    c_two.SetLocation(170, 0);
            // //   c_two.DrawUI(new Point(170, 0));

            //    FirstColumn.NextColumn = c_two;

            //    c_two.PrevColumn = FirstColumn;


            //    LastColumn = new Column();
            //    LastColumn.Index = 2;
            //    LastColumn.SetColumnWidth(TableHeigth);
            //    LastColumn.SetLocation(250, 0);
            //    //   c_three.DrawUI(new Point(250, 0));
            //    LastColumn.NextColumn = null;

            //    LastColumn.PrevColumn = c_two;
            //    c_two.NextColumn = LastColumn;
            CreateRullers();
            CreateNewColumns(new List<double>() { 100, 120, 60 });
            CreateNewRows(new List<double>() { 30, 50, 70 });

            for(int i=0; i<=GlobalStore.LastColors.Length-1; i++)
            {
                GlobalStore.LastColors[i] = System.Drawing.Color.FromArgb(GlobalStore.DefaultCellColor.A,
                                                                          GlobalStore.DefaultCellColor.R,
                                                                          GlobalStore.DefaultCellColor.G,
                                                                          GlobalStore.DefaultCellColor.B);
            }
            

            //    FirstRow = new Row();
            //    FirstRow.Index = 0;
            //    FirstRow.SetRowWidht(TableWidth);
            //    FirstRow.SetActiveMode(true);
            //    FirstRow.SetLocation(0, 20);
            ////    FirstRow.DrawUI(new Point(0, 20));

            //    FirstRow.PrevRow = null;

            //    var r_two = new Row();
            //    r_two.Index = 1;
            //    r_two.SetRowWidht(TableWidth);
            //    r_two.SetActiveMode(true);
            //    r_two.SetLocation(0, 70);
            //   // r_two.DrawUI(new Point(0, 70));
            //    r_two.PrevRow = FirstRow;

            //    FirstRow.NextRow = r_two;

            //    LastRow = new Row();
            //    LastRow.Index = 2;
            //    LastRow.SetRowWidht(TableWidth);
            //    LastRow.SetActiveMode(true);
            //    LastRow.SetLocation(0, 140);
            // //   r_tree.DrawUI(new Point(0, 140));

            //    r_two.NextRow = LastRow;

            //    LastRow.PrevRow = r_two;

            //    LastRow.NextRow = null;

            SetDefaultCellSize();


            //horisontal_ruler = new HorisontalRuler(this);
            //horisontal_ruler.InsertButtonClick += InsertColBetwwen;
            //horisontal_ruler.DeleteButtonClick += DeleteColumn;
            //horisontal_ruler.SetWidth((int)TableWidth);
            //horisontal_ruler.SetLocation(new Point(RullerThikknes, +TableOffset.Y));
            //horisontal_ruler.DrawUI();

            //vertical_ruler = new VerticalRuler(this);
            //vertical_ruler.InsertButtonClick += InsertRowBetwwen;
            //vertical_ruler.DeleteButtonClick += DeleteRow;
            //vertical_ruler.SetHeigth((int)TableHeigth);
            //vertical_ruler.SetLocation(new Point(0, RullerThikknes + TableOffset.Y));
            //vertical_ruler.DrawUI();

            RullerBox = new DrawingVisual();
            DrawRullerBox();

            //WorkingZoneBorder = new DrawingVisual();
            //DrawBorder();


            //FieldCanvas.AddVisual(WorkingZoneBorder);
            //FieldCanvas.AddVisual(horisontal_ruler);
            //FieldCanvas.AddVisual(vertical_ruler);

            //  FieldCanvas.AddVisual(FirstColumn);
            //  FieldCanvas.AddVisual(c_two);
            //  FieldCanvas.AddVisual(LastColumn);
          //  FieldCanvas.AddVisual(FirstRow);
         //   FieldCanvas.AddVisual(r_two);
         //   FieldCanvas.AddVisual(LastRow); 

            //     mouseDD = Mouse_Down;

            // MouseDownCommand = new Command(Mouse_Down);

            mouse_down = new Command(Mouse_Down);

            mouse_up = new Command(Mouse_Up);

        //    IsMouseDown = false;

            TimeAxisUI = new TimeAxis();
            TimeAxisUI.SetSize(TableWidth, 60);
            TimeAxisUI.SetLoc(RullerThikknes, 0);
      
            TimeAxisUI.InizialazeColumnList(ColumnWidth);
            TimeAxisUI.InitAxis();
            TimeAxisUI.DrawUI();

            FieldCanvas.AddVisual(TimeAxisUI);

            CreateDefaultCell();
            CreateTableButtons();

            horisontal_ruler.UpdateButtons();
            horisontal_ruler.DrawUI();

            vertical_ruler.UpdateButtons();
            vertical_ruler.DrawUI();
            //            int i = 0;

        }

        public void SetControlBar(ControlBarVM Cb)
        {
            ControlBar = Cb;
        }

        private void CreateRullers()
        {
            horisontal_ruler = new HorisontalRuler(this);
            horisontal_ruler.InsertButtonClick += InsertColBetwwen;
            horisontal_ruler.DeleteButtonClick += DeleteColumn;
            horisontal_ruler.ColumnButtonClickEvent += ColumnButtonClick;
            horisontal_ruler.SetWidth((int)TableWidth);
            horisontal_ruler.SetLocation(new Point(RullerThikknes, +TableOffset.Y));
            

            vertical_ruler = new VerticalRuler(this);
            vertical_ruler.InsertButtonClick += InsertRowBetwwen;
            vertical_ruler.DeleteButtonClick += DeleteRow;
            vertical_ruler.RowButtonClickEvent += RowButtonClick;
            vertical_ruler.SetHeigth((int)TableHeigth);
            vertical_ruler.SetLocation(new Point(0, RullerThikknes + TableOffset.Y));
            vertical_ruler.DrawUI();

            WorkingZoneBorder = new DrawingVisual();
            DrawBorder();


            FieldCanvas.AddVisual(WorkingZoneBorder);
            FieldCanvas.AddVisual(horisontal_ruler);
            FieldCanvas.AddVisual(vertical_ruler);
        }

        private void CreateTableButtons()
        {
            AddColumnButton = new TButton();

            AddColumnButton.SetLocation(new Point(TableOffset.X + TableWidth+RullerThikknes, TableOffset.Y + RullerThikknes));
            AddColumnButton.SetSize(40, TableHeigth);
            AddColumnButton.SetImage(AppDomain.CurrentDomain.BaseDirectory + "/icons/add_ic.png", 30, 30);
            AddColumnButton.Click += ColumnButtonClick;
            FieldCanvas.AddVisual(AddColumnButton);

            AddRowButton = new TButton();
            AddRowButton.SetLocation(new Point(TableOffset.X + RullerThikknes, TableOffset.Y + RullerThikknes + TableHeigth));
            AddRowButton.SetSize(TableWidth, 40);
            AddRowButton.SetImage(AppDomain.CurrentDomain.BaseDirectory + "/icons/add_ic.png", 30, 30);
            AddRowButton.Click += RowButtonClick;
            FieldCanvas.AddVisual(AddRowButton);
    }

        private void ColumnButtonClick(int col)
        {
            if (IsColWindowVisible == false)
            {
                ColWindow = new WColumn(col, ColumnWidth.Count());
                ((WColumn)ColWindow).DelColClick += DeleteColumn;
                ((WColumn)ColWindow).InsertColClick += InsertColl;
            }

            //var point = Mouse.GetPosition(Application.Current.MainWindow);


            //ColWindow.Left = point.X + WindowExtensions.GetWindowLeft(Window.GetWindow(window));
            //ColWindow.Top = point.Y + WindowExtensions.GetWindowTop(Window.GetWindow(window));

            double l_offset = ColumnWidth[0] + vertical_ruler.GetWidth();

            for(int i =1; i<=col;i++)
            {
                l_offset += ColumnWidth[i];
            }

            l_offset -= ColumnWidth[col]/2;
            l_offset -= ColWindow.Width / 2;
            double d= WindowExtensions.GetWindowTop(Window.GetWindow(window));

            ColWindow.Left = l_offset + WindowExtensions.GetWindowLeft(Window.GetWindow(window));
            ColWindow.Top = d + 55 + TableOffset.Y + horisontal_ruler.GetHeight() + ColWindow.Height;

            IsColWindowVisible = true;

            ColWindow.Owner = Application.Current.MainWindow;


            ColWindow.Show();
           // ColWindow.Focus();
            // Console.WriteLine(col.ToString());
        }

        private void RowButtonClick(int row)
        {
          // Console.WriteLine("Row is ---->" + row.ToString());
            if (IsColWindowVisible == false)
            {
                ColWindow = new WRow(row, RowHeight.Count());
                ((WRow)ColWindow).DelRowClick += DeleteRow;
                ((WRow)ColWindow).InsertRowClick += InsertRow;
            }

            //var point = Mouse.GetPosition(Application.Current.MainWindow);


            //ColWindow.Left = point.X + WindowExtensions.GetWindowLeft(Window.GetWindow(window));
            //ColWindow.Top = point.Y + WindowExtensions.GetWindowTop(Window.GetWindow(window));

            double l_offset = RowHeight[0]/* + horisontal_ruler.GetHeight()*/;

            for (int i = 1; i <= row; i++)
            {
                l_offset += RowHeight[i];
            }

            l_offset -= RowHeight[row] / 2;
            l_offset -= ColWindow.Height;
            double d = WindowExtensions.GetWindowTop(Window.GetWindow(window));

            ColWindow.Left = vertical_ruler.GetWidth() + WindowExtensions.GetWindowLeft(Window.GetWindow(window));
            ColWindow.Top = d + 55 + TableOffset.Y + horisontal_ruler.GetHeight() + ColWindow.Height + l_offset;

            IsColWindowVisible = true;

            ColWindow.Owner = Application.Current.MainWindow;


            ColWindow.Show();
            // ColWindow.Focus();
            // Console.WriteLine(col.ToString());
        }

        private void CreateDefaultCell(Int32 row_count=4, Int32 column_count=4)
        {
            if (RowColumn == null)
            {
                RowColumn = new List<List<Cell>>(row_count);
            }
            else
            {
                RowColumn.Clear();
            }

            for(int r_c=0; r_c <=row_count-1; r_c++)
            {
                RowColumn.Add(new List<Cell>());
            }
            //{
            //    new List<Cell>(),
            //    new List<Cell>(),
            //    new List<Cell>(),
            //    new List<Cell>()
            //};

            for (int i = 0; i <= row_count - 1; i++)
            {
                for (int j = 0; j <= column_count - 1; j++)
                {
                    EmptyCell cell = new EmptyCell();
                    cell.MouseDownEvent += MapConstructorInstance.EmptyCellMouseDown;
                    cell.SetWidth(ColumnWidth[j]);
                    cell.SetHeight(RowHeight[i]);
                    cell.TableIndex = new Cell.CellIndex(i, j);

                //    Console.WriteLine(ColumnWidth[j].ToString() + " " + RowHeight[i].ToString());
                    FieldCanvas.AddVisual(cell);
                    RowColumn[i].Add(cell);

                }
            }

            SetCellLoc();
        }

        public void MakeEmptyCell(Int32 row, Int32 col)
        {
            EmptyCell cell = new EmptyCell();
            cell.MouseDownEvent += MapConstructorInstance.EmptyCellMouseDown;
            cell.SetWidth(ColumnWidth[col]);
            cell.SetHeight(RowHeight[row]);
            cell.TableIndex = new Cell.CellIndex(row, col);

            //    Console.WriteLine(ColumnWidth[j].ToString() + " " + RowHeight[i].ToString());
            //    FieldCanvas.AddVisual(cell);
            FieldCanvas.AddVisualToBegin(cell);
            RowColumn[row][col] = null;
            RowColumn[row][col] = cell;
            SetCellLoc();

        }

        private void CreateNewColumns(List<double> columns)
        {
            Column col = LastColumn;

            while(col != null)
            {
                FieldCanvas.DeleteVisual(col);

                col = col.PrevColumn;
                if (col != null)
                {
                    col.NextColumn = null;
                }
            }

            LastColumn = null;
            FirstColumn = null;

            FirstColumn = new Column();
            FirstColumn.Index = 0;
            FirstColumn.SetColumnWidth(TableHeigth);
            FirstColumn.SetActiveMode(true);
            FirstColumn.SetLocation(columns[0], 0);
            //    FirstColumn.DrawUI(new Point(100, 0));
            FirstColumn.PrevColumn = null;

            FieldCanvas.AddVisual(FirstColumn);

            Column Cur_column = FirstColumn;

            double loc = columns[0];

            for (int i = 1; i <=columns.Count()-1; i++)
            {
                Column column = new Column();
                column.Index = i;
                column.SetColumnWidth(TableHeigth);
                column.SetActiveMode(true);
                loc += columns[i];
                column.SetLocation(loc, 0);

                FieldCanvas.AddVisual(column);

             
                column.PrevColumn = Cur_column;
            

                Cur_column.NextColumn = column;

                Cur_column = column;
            }

            LastColumn = Cur_column;
            LastColumn.NextColumn = null;
        }

        private void CreateNewRows(List<double> rows)
        {
            Row l_row = LastRow;

            while (l_row != null)
            {
                FieldCanvas.DeleteVisual(l_row);

                l_row = l_row.PrevRow;
                if (l_row != null)
                {
                    l_row.NextRow = null;
                }
            }

            LastRow = null;
            FirstRow = null;

            FirstRow = new Row();
            FirstRow.Index = 0;
            FirstRow.SetRowWidht(TableWidth);
            FirstRow.SetActiveMode(true);
            FirstRow.SetLocation(0, rows[0]);

            FieldCanvas.AddVisual(FirstRow);

            Row Cur_row = FirstRow;

            double loc = rows[0];

            for (int i = 1; i <= rows.Count() - 1; i++)
            {
                Row row = new Row();
                row.Index = i;
                row.SetRowWidht(TableWidth);
                row.SetActiveMode(true);
                loc += rows[i];
                row.SetLocation(0, loc);

                FieldCanvas.AddVisual(row);

                row.PrevRow = Cur_row;


                Cur_row.NextRow = row;

                Cur_row = row;
            }

            LastRow = Cur_row;
            LastRow.NextRow = null;
        }



        public void CreateNewTable(List<double> row_height, List<double> col_width, double width, double height)
        {
            DocConnector.CurrentDocList.Clear();

            PropertyBar.Clear();

            for (int i = 0; i <= RowHeight.Count() - 1; i++)
            {
                for (int j = 0; j <= ColumnWidth.Count() - 1; j++)
                {
                    FieldCanvas.DeleteVisual(RowColumn[i][j]);
                }
            }

            for(int i=0; i<= MarksList.Count() -1; i++)
            {
                FieldCanvas.DeleteVisual(MarksList[i]);
            }
            MarksList.Clear();

            RowHeight.Clear();
            RowHeight.AddRange(row_height);


            ColumnWidth.Clear();
            ColumnWidth.AddRange(col_width);


            TableWidth = width;
            TableHeigth = height;

  //          Console.WriteLine("width = " + width.ToString() + " height = " + height.ToString());

            foreach(IVSMlib.VsmCanvas.LineUI.Line line in MapLines)
            {
                FieldCanvas.DeleteVisual(line);
            }
            MapLines.Clear();

            List<double> n_w = new List<double>();
            n_w.AddRange(col_width);
            n_w.RemoveAt(col_width.Count() - 1);

            List<double> n_c = new List<double>();
            n_c.AddRange(row_height);
            n_c.RemoveAt(row_height.Count() - 1);

            CreateNewColumns(n_w);
            CreateNewRows(n_c);

            CreateDefaultCell(RowHeight.Count(), ColumnWidth.Count());

            horisontal_ruler.SetWidth((int)TableWidth);
            horisontal_ruler.DrawUI();
            vertical_ruler.SetHeigth((int)TableHeigth);
            vertical_ruler.DrawUI();



            //TimeAxisUI.SetSize(TableWidth, 60);

            TimeAxisUI.InizialazeColumnList(ColumnWidth);
            TimeAxisUI.InitAxis();
            //TimeAxisUI.DrawUI();
            TimeAxisUI.UpdateWidth(TableWidth);
            TimeAxisUI.DrawUI();

             DrawBorder();

            AddColumnButton.SetLocation(new Point(TableOffset.X + TableWidth + RullerThikknes, TableOffset.Y + RullerThikknes));
            AddColumnButton.SetSize(40, TableHeigth);
            AddColumnButton.DrawUI();

            AddRowButton.SetLocation(new Point(TableOffset.X + RullerThikknes, TableOffset.Y + TableHeigth + RullerThikknes));
            AddRowButton.SetSize(TableWidth, 40);
            AddRowButton.DrawUI();

            horisontal_ruler.UpdateButtons();
            horisontal_ruler.DrawUI();
        }


        private void ChangeCellSize()
        {
            for (int i = 0; i <= RowHeight.Count() - 1; i++)
            {
                for (int j = 0; j <= ColumnWidth.Count() - 1; j++)
                {
                    RowColumn[i][j].SetSize(ColumnWidth[j], RowHeight[i]);
                   
                }
            }
        }

        private void SetCellLoc()
        {
            Column col = FirstColumn;

            Row row = FirstRow;

            double x = 0;
            double y = 0;

           // Int32 row_pos;
            //Int32 coll_pos;

            //----------------------Row 0-------------------------------
            RowColumn[0][0].SetLocation(0+RullerThikknes, 0 + RullerThikknes + TableOffset.Y);

            while (col != null)
            {

                RowColumn[0][col.Index+1].SetLocation(col.GetPosition().X + RullerThikknes, 0 + RullerThikknes + TableOffset.Y);

                col = col.NextColumn;
            }

            //----------------------------------------------------------

            

            while (row !=null)
            {
                col = FirstColumn;
             
                RowColumn[row.Index+1][0].SetLocation(0 + RullerThikknes, row.GetPosition().Y + RullerThikknes + TableOffset.Y);

                while (col !=null)
                {

                    RowColumn[row.Index+1][col.Index+1].SetLocation(col.GetPosition().X + RullerThikknes, row.GetPosition().Y + RullerThikknes + TableOffset.Y);

                    col = col.NextColumn;
                }
                row = row.NextRow;
            }
        }

        private void SetDefaultCellSize()
        {
            Column col = FirstColumn;

           

            ColumnWidth.Add(col.GetPosition().X);

            col = col.NextColumn;

            while (col != null)
            {
              ColumnWidth.Add(col.GetPosition().X - col.PrevColumn.GetPosition().X);
                if(col.NextColumn == null)
                {
                    ColumnWidth.Add(TableWidth - col.GetPosition().X);
                    break;
                }
              col = col.NextColumn;
            }

            Row row = FirstRow;

            RowHeight.Add(row.GetPosition().Y);

            row = row.NextRow;
            while (row != null)
            {
                RowHeight.Add(row.GetPosition().Y - row.PrevRow.GetPosition().Y);
                if (row.NextRow == null)
                {
                    RowHeight.Add(TableHeigth - row.GetPosition().Y);
                    break;
                }
                row = row.NextRow;
            }

           
        }

        private void DrawRullerBox()
        {
            DrawingContext dc = RullerBox.RenderOpen();

            dc.DrawRectangle(Brushes.DarkGray, new Pen(Brushes.DarkGray, 1), new Rect(0, TableOffset.Y, RullerThikknes, RullerThikknes));

            dc.Close();
        }

        private void DrawBorder()
        {
            DrawingContext dc = WorkingZoneBorder.RenderOpen();

            dc.DrawRectangle(Brushes.Transparent, new Pen(Brushes.DarkGray, 1), new Rect(horisontal_ruler.GetHeight(), vertical_ruler.GetWidth()+TableOffset.Y, TableWidth, TableHeigth));

            dc.Close();
        }


        public void Mouse_Down()
        {
            //Console.WriteLine("from VM  l-  " + MouseLeftButton.ToString() + " - r -" + MouseRightButton.ToString());
            TableActions[(int)CurrentAction].MouseDownAction(new Point(x,y), new MouseState(MouseLeftButton, MouseRightButton));
           
         //   IsMouseDown = true;
        }

        public void Mouse_Up()
        {
            TableActions[(int)CurrentAction].MouseUpAction(new Point(x, y));

          //  IsMouseDown = false;
        }

        public void MouseMove()
        {
          //  if(IsMouseDown)
          //  {
                TableActions[(int)CurrentAction].MouseMoveAction(new Point(x, y));
           // }
        }

        public void AddCell(Cell cell, Int32 row, Int32 column)
        {
            Cell new_cell = cell;
            new_cell.SetSize(RowColumn[row][column].GetSize().Width, RowColumn[row][column].GetSize().Height);
            new_cell.SetLocation(RowColumn[row][column].GetLocation().X, RowColumn[row][column].GetLocation().Y);
            new_cell.TableIndex = new Cell.CellIndex(RowColumn[row][column].TableIndex.Row, RowColumn[row][column].TableIndex.Column);
         //   new_cell.MouseResultEvent += CellDownEvent;
            new_cell.DrawUI();
          

            FieldCanvas.DeleteVisual(RowColumn[row][column]);
            RowColumn[row][column] = null;
            RowColumn[row][column] = new_cell;


            FieldCanvas.AddVisual(RowColumn[row][column]);
            
        } 

        public void MoveCellTo(Cell cell, Int32 row, Int32 column)
        {
            Cell new_cell = cell;
            new_cell.SetSize(RowColumn[row][column].GetSize().Width, RowColumn[row][column].GetSize().Height);
            new_cell.SetLocation(RowColumn[row][column].GetLocation().X, RowColumn[row][column].GetLocation().Y);
            new_cell.TableIndex = new Cell.CellIndex(RowColumn[row][column].TableIndex.Row, RowColumn[row][column].TableIndex.Column);
            //   new_cell.MouseResultEvent += CellDownEvent;
            new_cell.DrawUI();


            FieldCanvas.DeleteVisual(RowColumn[row][column]);
            RowColumn[row][column] = null;
            RowColumn[row][column] = new_cell;
        }

        public TableUI FindeUI(Point e)
        {
            DrawingVisual finde_ui = FieldCanvas.GetVisual(e);
            if (finde_ui is TableUI)
            {
                return (TableUI)finde_ui;
            }
            else return null;
        }

        public void AddUI(TableUI ui)
        {
            FieldCanvas.AddVisual(ui);
        }

        public void AddVisual(DrawingVisual dw)
        {
            FieldCanvas.AddVisual(dw);
        }

        public void DeleteVisual(DrawingVisual dw)
        {
            FieldCanvas.DeleteVisual(dw);
        }

        private void ColumnButtonClick(TButton sender, object arg)
        {
            InsertLastColumn();
        }

        private void RowButtonClick(TButton sender, object arg)
        {
            InsertLastRow();
        }

        private void InsertLastRow()
        {
            int row_height = 100;

            var last_row = new Row();

            last_row.Index = LastRow.Index + 1;
            last_row.SetRowWidht(TableWidth);
            last_row.SetLocation(0, TableHeigth);
            last_row.NextRow = null;

            last_row.PrevRow = LastRow;
            LastRow.NextRow = last_row;
            LastRow = null;
            LastRow = last_row;

            LastRow.DrawUI();

            FieldCanvas.AddVisual(LastRow);

            TableHeigth += row_height;

            AddRowButton.SetLocation(new Point(TableOffset.X + RullerThikknes, TableOffset.Y + TableHeigth + RullerThikknes));

            RowHeight.Add(row_height);

            List<Cell> NewRow = new List<Cell>();

            for (int col = 0; col <= ColumnWidth.Count - 1; col++)
            {
                EmptyCell cell = new EmptyCell();
                cell.MouseDownEvent += MapConstructorInstance.EmptyCellMouseDown;
                cell.SetWidth(ColumnWidth[col]);
                cell.SetHeight(row_height);
                cell.TableIndex = new Cell.CellIndex(RowHeight.Count - 1, col);
                FieldCanvas.AddVisual(cell);
                NewRow.Add(cell);
            }

            RowColumn.Add(NewRow);

            UpdateColumnSize();

            SetCellLoc();
            DrawBorder();

            vertical_ruler.SetHeigth((int)TableHeigth);
            vertical_ruler.DrawUI();
            AddColumnButton.SetSize(40, TableHeigth);

            FieldCanvas.Height = TableHeigth + 200;
        }

        private void InsertLastColumn()
        {
            Int32 ColWidth = 100;

            var last_column = new Column();
            last_column.Index = LastColumn.Index + 1;
            last_column.SetColumnWidth(TableHeigth);
            last_column.SetLocation(TableOffset.X + TableWidth, 0);
            //   c_three.DrawUI(new Point(250, 0));
            last_column.NextColumn = null;

            last_column.PrevColumn = LastColumn;

            LastColumn.NextColumn = last_column;

            LastColumn = null;

            LastColumn = last_column;

            //          Console.WriteLine("prev - " + LastColumn.PrevColumn.Index.ToString() + "new - " + LastColumn.Index.ToString());

            LastColumn.DrawUI();

            ColumnWidth.Add(ColWidth);


            for (int row = 0; row <= RowHeight.Count - 1; row++)
            {
                EmptyCell cell = new EmptyCell();
                cell.MouseDownEvent += MapConstructorInstance.EmptyCellMouseDown;
                cell.SetWidth(ColumnWidth[ColumnWidth.Count - 1]);
                cell.SetHeight(RowHeight[row]);
                cell.TableIndex = new Cell.CellIndex(row, ColumnWidth.Count - 1);

                //    Console.WriteLine(ColumnWidth[j].ToString() + " " + RowHeight[i].ToString());
                FieldCanvas.AddVisual(cell);
                RowColumn[row].Add(cell);
            }


            TableWidth += ColWidth;
            //------------------------------------------------------------------------------------
            UpdateRowsSize();

            horisontal_ruler.SetWidth((int)TableWidth);
            horisontal_ruler.UpdateButtons();
            horisontal_ruler.DrawUI();

            SetCellLoc();
            //---------------------------------------------------------------------------------------

            AddColumnButton.SetLocation(new Point(TableOffset.X + TableWidth + RullerThikknes, TableOffset.Y + RullerThikknes));

            FieldCanvas.AddVisual(last_column);
            DrawBorder();
            AddRowButton.SetSize(TableWidth, 40);
            TimeAxisUI.AddColumn();
            TimeAxisUI.UpdateWidth(TableWidth);
            TimeAxisUI.DrawUI();

            FieldCanvas.Width = TableWidth + 100;

            horisontal_ruler.UpdateButtons();
            horisontal_ruler.DrawUI();

            // MessageBox.Show("hfghgf");
        }

        public void DeleteLastColl()
        {
            for(int i = 0; i<= RowHeight.Count-1; i++)
            {
                if(!(RowColumn[i][ColumnWidth.Count-1] is EmptyCell))
                {
                    MessageBox.Show("Ряд не пустой");
                    return;
                }
            }

            for (int i = 0; i <= RowHeight.Count - 1; i++)
            {
                FieldCanvas.DeleteVisual(RowColumn[i][ColumnWidth.Count - 1]);
                RowColumn[i].RemoveAt(ColumnWidth.Count - 1);
            }


            TableWidth -= (int)ColumnWidth[ColumnWidth.Count - 1];

            UpdateRowsSize();


            horisontal_ruler.SetWidth((int)TableWidth);
            horisontal_ruler.DrawUI();

            AddColumnButton.SetLocation(new Point(TableOffset.X + TableWidth + RullerThikknes, TableOffset.Y + RullerThikknes));

            DrawBorder();
            AddRowButton.SetSize(TableWidth, 40);
           // TimeAxisUI.AddColumn();
         


            FieldCanvas.DeleteVisual(LastColumn);

            LastColumn = LastColumn.PrevColumn;
            LastColumn.NextColumn = null;

            ColumnWidth.RemoveAt(ColumnWidth.Count - 1);

            TimeAxisUI.UpdateWidth(TableWidth);
            TimeAxisUI.DrawUI();

            FieldCanvas.Width = TableWidth + 100;
        }

        public void DeleteLastRow()
        {
            for (int i = 0; i <= ColumnWidth.Count - 1; i++)
            {
                if (!(RowColumn[RowHeight.Count -1][i] is EmptyCell))
                {
                    MessageBox.Show("Ряд не пустой");
                    return;
                }
            }

            for (int i = 0; i <= ColumnWidth.Count - 1; i++)
            {
                FieldCanvas.DeleteVisual(RowColumn[RowHeight.Count - 1][i]);
            }

            RowColumn.RemoveAt(RowColumn.Count - 1);

         

            FieldCanvas.DeleteVisual(LastRow);

            LastRow = LastRow.PrevRow;

            LastRow.NextRow = null;

          //  Column n_col = FirstColumn;

            TableHeigth -= (int)RowHeight[RowHeight.Count - 1];

            RowHeight.RemoveAt(RowHeight.Count - 1);

            //while (n_col != null)
            //{
            //    n_col.SetColumnWidth(TableHeigth);
            //    n_col.DrawUI();
            //    n_col = n_col.NextColumn;
            //}
            UpdateColumnSize();

            AddRowButton.SetLocation(new Point(TableOffset.X + RullerThikknes, TableOffset.Y + TableHeigth + RullerThikknes));
            AddColumnButton.SetSize(40, TableHeigth);

            vertical_ruler.SetHeigth((int)TableHeigth);
            vertical_ruler.DrawUI();

            DrawBorder();

            FieldCanvas.Height = TableHeigth + 100;

            vertical_ruler.UpdateButtons();
            vertical_ruler.DrawUI();
        }

        public void SetMode(MapConstructor.Mode _mode)
        {
            if(_mode == MapConstructor.Mode.Edit)
            {
                foreach(Mark mark in MarksList)
                {
                    FieldCanvas.DeleteVisual(mark);
                }
            }

            if (_mode == MapConstructor.Mode.View)
            {
                foreach (Mark mark in MarksList)
                {
                    FieldCanvas.AddVisual(mark);
                }
            }
            MapConstructorInstance.SetMode(_mode);
        }

        public void SetTableSize(double width, double height, bool is_alight_size)
        {
            if (width != TableWidth &&  width > TableWidth)
            {
                int first_colum_index = ColumnWidth.Count() - 1;

                double last_col_width = width - LastColumn.GetPosition().X;


                if (last_col_width < MIN_COLUMN_WIDTH)
                {
                    TableWidth = LastColumn.GetPosition().X + MIN_COLUMN_WIDTH;
                    ColumnWidth[first_colum_index] = MIN_COLUMN_WIDTH;

                }
                else
                {
                    TableWidth = width;
                    ColumnWidth[first_colum_index] = last_col_width;

                }

                AddColumnButton.SetLocation(new Point(TableOffset.X + TableWidth + RullerThikknes, TableOffset.Y + RullerThikknes));
                DrawBorder();
                AddRowButton.SetSize(TableWidth, 40);
                TimeAxisUI.UpdateWidth(TableWidth);
                TimeAxisUI.DrawUI();

                ChangeCellSize();

                UpdateRowsSize();

                for (int i = 0; i <= RowHeight.Count - 1; i++)
                {
                    RowColumn[i][ColumnWidth.Count - 1].DrawUI();
                }

                horisontal_ruler.SetWidth((int)TableWidth);
                horisontal_ruler.DrawUI();
                if (is_alight_size)
                {
                    AlightColumnSize();
                }
                FieldCanvas.Width = TableWidth + 100;
            }

            if(width != TableWidth && width < TableWidth)
            {
                double coll_width = width / ColumnWidth.Count();

                if(coll_width <= MIN_COLUMN_WIDTH)
                {
                    MessageBox.Show("Недопустимо малый размер ширины таблицы");
                }
                else
                {
                    //for(int col=0; col <= ColumnWidth.Count() -1; col++)
                    //{
                    //    ColumnWidth[col] = coll_width;
                    //}
                    TableWidth = width;
                    TimeAxisUI.UpdateWidth(TableWidth);
                    TimeAxisUI.DrawUI();

                    AddColumnButton.SetLocation(new Point(TableOffset.X + TableWidth + RullerThikknes, TableOffset.Y + RullerThikknes));
                    DrawBorder();
                    AddRowButton.SetSize(TableWidth, 40);
                    UpdateRowsSize();
                    //ChangeCellSize();

                    //UpdateRowsSize();

                    //for (int i = 0; i <= RowHeight.Count - 1; i++)
                    //{
                    //    RowColumn[i][ColumnWidth.Count - 1].DrawUI();
                    //}

                    horisontal_ruler.SetWidth((int)TableWidth);
                    horisontal_ruler.DrawUI();
                    AlightColumnSize();
                    FieldCanvas.Width = TableWidth + 100;
                }
            }


            if (height != TableHeigth && height > TableHeigth)
            {
                int last_row_index = RowHeight.Count() - 1;

                double last_row_height = height - LastRow.GetPosition().Y;


                if (last_row_height < MIN_ROW_HEIGHT)
                {
                    TableHeigth = LastRow.GetPosition().Y + MIN_ROW_HEIGHT;
                    RowHeight[last_row_index] = MIN_COLUMN_WIDTH;

                }
                else
                {
                    TableHeigth = height;
                    RowHeight[last_row_index] = last_row_height;

                }

                //TableHeigth = (int)height;

                AddRowButton.SetLocation(new Point(TableOffset.X + RullerThikknes, TableOffset.Y + TableHeigth + RullerThikknes));

                vertical_ruler.SetHeigth((int)TableHeigth);
                vertical_ruler.DrawUI();
                AddColumnButton.SetSize(40, TableHeigth);

               // int last_row_index = RowHeight.Count() - 1;
            //    RowHeight[last_row_index] = TableHeigth - LastRow.GetPosition().Y;
                ChangeCellSize();
                UpdateColumnSize();
                DrawBorder();
                if (is_alight_size)
                {
                    AlightRowHeight();
                }
                FieldCanvas.Height = TableHeigth + 100;
            }

            if(height != TableHeigth && height < TableHeigth)
            {
                double row_height = width / RowHeight.Count();

                if(row_height <= MIN_ROW_HEIGHT)
                {
                    MessageBox.Show("Недопустимо малый размер высоты таблицы");
                }
                else
                {
                    TableHeigth = height;
                    UpdateColumnSize();
                    AddRowButton.SetLocation(new Point(TableOffset.X + RullerThikknes, TableOffset.Y + TableHeigth + RullerThikknes));

                    vertical_ruler.SetHeigth((int)TableHeigth);
                    vertical_ruler.DrawUI();
                    AddColumnButton.SetSize(40, TableHeigth);
                    DrawBorder();
                    AlightRowHeight();
                    FieldCanvas.Height = TableHeigth + 100;
                }
            }

            vertical_ruler.UpdateButtons();
            vertical_ruler.DrawUI();

            horisontal_ruler.UpdateButtons();
            horisontal_ruler.DrawUI();
        }

        public void AlightColumnSize()
        {
            double column_size = TableWidth / ColumnWidth.Count;

            for(int col=0; col <= ColumnWidth.Count-1; col++)
            {
                ColumnWidth[col] = column_size;
            }

            double col_pos = column_size;

            Column n_col = FirstColumn;

            while (n_col != null)
            {
                n_col.SetLocation(col_pos, 0);
                n_col.DrawUI();
                n_col = n_col.NextColumn;
                col_pos += column_size;
            }

            SetCellLoc();
            ChangeCellSize();
            TimeAxisUI.DrawUI();

            horisontal_ruler.UpdateButtons();
            horisontal_ruler.DrawUI();
        }

        public void AlightRowHeight()
        {
            double row_size = TableHeigth / RowHeight.Count;

            for (int row = 0; row <= RowHeight.Count - 1; row++)
            {
                RowHeight[row] = row_size;
            }

            double row_pos = row_size;

            Row n_row = FirstRow;

            while (n_row != null)
            {
                n_row.SetLocation(0, row_pos);
                n_row.DrawUI();
                n_row = n_row.NextRow;
                row_pos += row_size;
            }

            SetCellLoc();
            ChangeCellSize();

            vertical_ruler.UpdateButtons();
            vertical_ruler.DrawUI();
        }
         
        public void AlightTableCellSize()
        {
            AlightColumnSize();
            AlightRowHeight();
        }

        public Cell GetFocucedCell()
        {
            if(FocusedUI is Cell)
            {
                return (Cell)FocusedUI;
            }
            return null;
        }
            

        private void UpdateRowsSize()
        {
            Row n_row = FirstRow;

            while (n_row != null)
            {
                n_row.SetRowWidht(TableWidth);
                n_row.DrawUI();
                n_row = n_row.NextRow;
            }
        }

        private void UpdateColumnSize()
        {
            Column n_col = FirstColumn;

            while (n_col != null)
            {
                n_col.SetColumnWidth(TableHeigth);
                n_col.DrawUI();
                n_col = n_col.NextColumn;
            }        
        }


        public void DeleteCommand()
        {
            if(SelectedUI is Mark)
            {
                FieldCanvas.DeleteVisual(SelectedUI);
                MarksList.Remove((Mark)SelectedUI);
                SelectedUI = null;
                return;
            }
            MapConstructorInstance.DeleteItem();
        }

        public void SetPropertyBarVM(PropertyBarVM pb)
        {
            MapConstructorInstance.PropertyBar = pb;
            PropertyBar = pb;
        }

        public void RedirectLine()
        {
            MapConstructorInstance.RediectLine();
        }

        public void SetAxisTimeMeg(Time.Type act_type, Time.Type wst_type, Time.Type mv_type)
        {
            Console.WriteLine(act_type.ToString() + " -  " + wst_type.ToString() + " - " + mv_type.ToString());
            TimeAxisUI.SetActionMeg(act_type);
            TimeAxisUI.SetWasteMeg(wst_type);
            TimeAxisUI.SetMoveMeg(mv_type);
        }

        public void SetColor(Color c)
        {
            MapConstructorInstance.SetColor(c);
        }

        public void SelectTableUI(TableUI ui)
        {
            if(SelectedUI != null)
            {
                SelectedUI.Unselect();
            }

            SelectedUI = ui;

            SelectedUI.Select();
        }

        public void UnselectTableUI()
        {
            if (SelectedUI != null)
            {
                SelectedUI.Unselect();
                SelectedUI = null;
            }
        }

        public void CreateProblemMark()
        {
            MProblem problem = new MProblem();

            FieldCanvas.AddVisual(problem);

            CreateMarkActionInstance.InsertMark = problem;

            CurrentAction = Actions.CreateMark;
        }

        public void CreateDecisionMark()
        {
            MDecision decision = new MDecision();

            FieldCanvas.AddVisual(decision);

            CreateMarkActionInstance.InsertMark = decision;

            CurrentAction = Actions.CreateMark;
        }

        public void CreateTextLableMark()
        {
            MTextLable text = new MTextLable();

            FieldCanvas.AddVisual(text);

            CreateMarkActionInstance.InsertMark = text;

            CurrentAction = Actions.CreateMark;
        }

        public void InsertRowColumnMode()
        {
            if(CurrentAction != Actions.RulerOptions)
            {
                CurrentAction = Actions.RulerOptions;
                horisontal_ruler.ShowColumnAddButton();
                vertical_ruler.ShowRowAddButton();
            }
            else
            {
                horisontal_ruler.HideColumnAddButton();
                vertical_ruler.HideRowAddButton();
                CurrentAction = Actions.None;
            }
           
        }

        private void InsertColl(Int32 to)
        {
            InsertColBetwwen(1, to);
        }

        private void InsertColBetwwen(Int32 from, Int32 to)
        {
            InsertLastColumn();

           // horisontal_ruler.HideColumnAddButton();
        //    horisontal_ruler.ShowColumnAddButton();

            for (int i=ColumnWidth.Count()-1; i > to; i--)
            {
                for(int row = 0; row <= RowHeight.Count - 1; row++)
                {
                    if(!(RowColumn[row][i-1] is EmptyCell))
                    {

                        Int32 row_i = RowColumn[row][i - 1].TableIndex.Row;
                        Int32 col_i = RowColumn[row][i - 1].TableIndex.Column;

                        FieldCanvas.DeleteVisual(RowColumn[row][i]);
                        RowColumn[row][i - 1].TableIndex.Row = RowColumn[row][i].TableIndex.Row;
                        RowColumn[row][i - 1].TableIndex.Column = RowColumn[row][i].TableIndex.Column;
                        RowColumn[row][i] = RowColumn[row][i - 1];
                       
                        //---------

                        //----------------
                        //EmptyCell cell = new EmptyCell();
                        //cell.MouseDownEvent += MapConstructorInstance.EmptyCellMouseDown;
                        //cell.SetWidth(ColumnWidth[i-1]);
                        //cell.SetHeight(RowHeight[row]);
                        //cell.TableIndex = new Cell.CellIndex(RowColumn[row][i - 1].TableIndex.Row, RowColumn[row][i - 1].TableIndex.Column);
                        //FieldCanvas.AddVisualToBegin(cell);
                        MakeEmptyCell(row_i, col_i);
                     //   RowColumn[row][i - 1] = cell;
                   //     RowColumn[row][i - 1].DrawUI();
                    }
                }
            }

            SetCellLoc();
            ChangeCellSize();
        }

        private void InsertRow(Int32 to)
        {
            InsertRowBetwwen(1, to);
        }

        private void InsertRowBetwwen(Int32 from, Int32 to)
        {
            InsertLastRow();

            vertical_ruler.HideRowAddButton();
            vertical_ruler.ShowRowAddButton();

       //     Console.WriteLine(from.ToString() + " -- > " + to.ToString() + " RowHeightCout - " + RowHeight.Count().ToString());

            for (int row = RowHeight.Count() - 1; row > to; row--)
            {
                for (int col = 0; col <= ColumnWidth.Count - 1; col++)
                {
                    if (!(RowColumn[row-1][col] is EmptyCell))
                    {

                        Int32 row_i = RowColumn[row-1][col].TableIndex.Row;
                        Int32 col_i = RowColumn[row-1][col].TableIndex.Column;

                        FieldCanvas.DeleteVisual(RowColumn[row][col]);

                        RowColumn[row-1][col].TableIndex.Row = RowColumn[row][col].TableIndex.Row;
                        RowColumn[row-1][col].TableIndex.Column = RowColumn[row][col].TableIndex.Column;
                        RowColumn[row][col] = RowColumn[row-1][col];

                       
                        MakeEmptyCell(row_i, col_i);
                     
                    }
                }
            }

            SetCellLoc();
            ChangeCellSize();
        }

        public void DeleteRowColumn()
        {
            if (CurrentAction != Actions.RulerOptions)
            {
                CurrentAction = Actions.RulerOptions;
                horisontal_ruler.ShowDeleteButton();
                vertical_ruler.ShowDeleteButton();
              //  vertical_ruler.ShowRowAddButton();
            }
            else
            {
                horisontal_ruler.HideDeleteButton();
                vertical_ruler.HideDeleteButton();
                CurrentAction = Actions.None;
            }
        }

        private void DeleteColumn(Int32 col)
        {
            for(Int32 row =0; row <= RowHeight.Count() -1; row++)
            {
                if(!(RowColumn[row][col] is EmptyCell))
                {
                    MapConstructorInstance.DeleteCell(row, col);
                }
            }

        

            for (int cur_col = col+1; cur_col <= ColumnWidth.Count()-1; cur_col++)
            {
                for (int row = 0; row <= RowHeight.Count - 1; row++)
                {
                    if (!(RowColumn[row][cur_col] is EmptyCell))
                    {

                        Int32 row_i = RowColumn[row][cur_col].TableIndex.Row;
                        Int32 col_i = RowColumn[row][cur_col].TableIndex.Column;

                        FieldCanvas.DeleteVisual(RowColumn[row][cur_col-1]);

                        RowColumn[row][cur_col].TableIndex.Row = RowColumn[row][cur_col-1].TableIndex.Row;
                        RowColumn[row][cur_col].TableIndex.Column = RowColumn[row][cur_col-1].TableIndex.Column;
                        RowColumn[row][cur_col-1] = RowColumn[row][cur_col];

                     
                        MakeEmptyCell(row_i, col_i);
                      
                    }
                }
            }
            SetCellLoc();
            ChangeCellSize();

            DeleteLastColl();

            //horisontal_ruler.HideDeleteButton();
            //horisontal_ruler.ShowDeleteButton();

            horisontal_ruler.UpdateButtons();
            horisontal_ruler.DrawUI();
        }

        private void DeleteRow(Int32 row)
        {
            for (Int32 col = 0; col <= ColumnWidth.Count() - 1; col++)
            {
                if (!(RowColumn[row][col] is EmptyCell))
                {
                    MapConstructorInstance.DeleteCell(row, col);
                }
            }

            for (int cur_row = row+1; cur_row <=RowHeight.Count() - 1; cur_row++)
            {
                for (int col = 0; col <= ColumnWidth.Count - 1; col++)
                {
                    if (!(RowColumn[cur_row][col] is EmptyCell))
                    {

                        Int32 row_i = RowColumn[cur_row][col].TableIndex.Row;
                        Int32 col_i = RowColumn[cur_row][col].TableIndex.Column;

                        FieldCanvas.DeleteVisual(RowColumn[cur_row - 1][col]);

                        RowColumn[cur_row][col].TableIndex.Row = RowColumn[cur_row - 1][col].TableIndex.Row;
                        RowColumn[cur_row][col].TableIndex.Column = RowColumn[cur_row - 1][col].TableIndex.Column;
                        RowColumn[cur_row-1][col] = RowColumn[cur_row][col];


                        MakeEmptyCell(row_i, col_i);

                    }
                }
            }

            DeleteLastRow();

           // vertical_ruler.HideDeleteButton();
         //   vertical_ruler.ShowDeleteButton();

            SetCellLoc();
            ChangeCellSize();

            vertical_ruler.UpdateButtons();
            vertical_ruler.DrawUI();
        }

        private void ClearTable()
        {
            for (int i = 0; i <= RowHeight.Count() - 1; i++)
            {
                for (int j = 0; j <= ColumnWidth.Count() - 1; j++)
                {
                    FieldCanvas.DeleteVisual(RowColumn[i][j]);
                }
            }

            for (int i = 0; i <= MarksList.Count() - 1; i++)
            {
                FieldCanvas.DeleteVisual(MarksList[i]);
            }
            MarksList.Clear();


            foreach (IVSMlib.VsmCanvas.LineUI.Line line in MapLines)
            {
                FieldCanvas.DeleteVisual(line);
            }
            MapLines.Clear();

            CreateDefaultCell(RowHeight.Count(), ColumnWidth.Count());

            DocConnector.CurrentDocList.Clear();


            //IVSM ivsm = new IVSM();

            //GlobalStore.CurrentIVSM = ivsm;
            //ControlBar.TableName = GlobalStore.CurrentIVSM.MapName;
        }

        public void ClearTableClick()
        {
            ClearTable();
        }

        public void Save()
        {
            if (GlobalStore.CurrentFileName != GlobalStore.NULL_FILE)
            {
                string filename = GlobalStore.CurrentFileName;
                DomManager manager = new DomManager("IVSM");
                MapDom.CreateMapDom(this, ref manager);

                //  manager.setWriter(new XmlWriter());

                XmlWriter writer = new XmlWriter();
                writer.SaveToFileTwo(filename, manager.GetRootNode());
            }
            else
            {
                SaveAs();
            }


            // l.Text = "Сохранение.......";
          
        }

        public void SaveAs()
        {
            //string filename = "D:\\Projects\\IVSM\\IVSMlib\\Host\\bin\\Debug\\first.xml";

            //DomManager manager = new DomManager("IVSM");
            //MapDom.CreateMapDom(this, ref manager);

            //XmlWriter writer = new XmlWriter();
            //writer.SaveToFileTwo(filename, manager.GetRootNode());

            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog1.Filter = "vsm files (*.ivsm)|*.ivsm";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            string filename = saveFileDialog1.FileName;
            GlobalStore.CurrentFileName = filename;


            // l.Text = "Сохранение.......";
            DomManager manager = new DomManager("IVSM");
            MapDom.CreateMapDom(this, ref manager);

            //  manager.setWriter(new XmlWriter());

            XmlWriter writer = new XmlWriter();
            writer.SaveToFileTwo(filename, manager.GetRootNode());
        }

        public void NewMap()
        {
            ClearTable();
            IVSM ivsm = new IVSM();

            GlobalStore.CurrentIVSM = ivsm;
            ControlBar.TableName = GlobalStore.CurrentIVSM.MapName;
            GlobalStore.CurrentFileName = GlobalStore.NULL_FILE;
        }

        public void Open()
        {
          //  string filename = "D:\\Projects\\IVSM\\IVSMlib\\Host\\bin\\Debug\\first.xml";

            DomManager manager = new DomManager("VSM");

            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "Ivsm files (*.ivsm)|*.ivsm";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            string filename = openFileDialog1.FileName;

            GlobalStore.CurrentFileName = filename;

            XMLParser parser = new XMLParser();

            MapDom.ParseNodeToMap(parser.ParseFile(filename), this);

            ControlBar.SetSwithPos(MapConstructor.Mode.View);
            MapConstructorInstance.SetMode(MapConstructor.Mode.View);
            ControlBar.TableName = GlobalStore.CurrentIVSM.MapName;
            //   ControlBar.EditMode = false;
        }


        private void IportToImage(string file_name, ImportManager.ImgFormat format, bool show_time_t, bool show_doc, bool show_problem)
        {
            SetVisibleColRow(false);

            MapConstructorInstance.UnselectCell();

            AddColumnButton.Hide();
            AddRowButton.Hide();
            horisontal_ruler.Hide();
            vertical_ruler.Hide();

            ProblemDocumentTable dt = new ProblemDocumentTable();
            dt.TablePosition.X = TableOffset.X + 10;
            dt.TablePosition.Y = TableOffset.Y + TableHeigth + 50;
            dt.Showdoc = show_doc;
            dt.ShowProblem = show_problem;

            if(show_problem)
            {
                Int32 count = 1;
                foreach (Mark mark in MarksList)
                {
                    if (mark is MProblem problem)
                    {
                        problem.SetNum(count);
                        problem.SetNumberMode(true);
                        dt.ProblemList.Add(new ProblemDocumentTable.Problem(count, problem.GetProblem()));
                        count++;

                    }
                }
            }

            FieldCanvas.AddVisual(dt);
            dt.Draw();

            Int32Rect CropRect = new Int32Rect(0,0,0,0);

            CropRect.Width = (int)TableWidth + 20;
            CropRect.Height = (int)TableOffset.Y + (int)TableHeigth + (int)dt.GetHeight()+ 50;

            if (show_time_t == false)
            {
                CropRect.Y = Convert.ToInt32(TableOffset.Y);
                CropRect.Height -= Convert.ToInt32(TableOffset.Y);
            }

            int width = (int)TableWidth + 20;
            int heigth = (int)TableOffset.Y + (int)(TableHeigth + dt.GetHeight() + 80);
            ImportManager.SetFileName(file_name);
            ImportManager.SetFormat(format);
            ImportManager.ImportImage(FieldCanvas, width, heigth, CropRect);
          //  CreateImage(dt.GetHeight());

            FieldCanvas.DeleteVisual(dt);

            AddColumnButton.Show();
            AddRowButton.Show();
            horisontal_ruler.Show();
            vertical_ruler.Show();

            if (show_problem)
            {
                foreach (Mark mark in MarksList)
                {
                    if (mark is MProblem problem)
                    {
                        problem.SetNumberMode(false);

                    }
                }
            }

            SetVisibleColRow(true);
        }

        public void SaveAsImage()
        {
            WSaveAsImage save_wimdow = new WSaveAsImage();
            save_wimdow.SaveParamEvent += IportToImage;
            save_wimdow.ShowDialog();
            return;

            SetVisibleColRow(false);

            MapConstructorInstance.UnselectCell();

            AddColumnButton.Hide();
            AddRowButton.Hide();
            horisontal_ruler.Hide();
            vertical_ruler.Hide();

          
                ProblemDocumentTable dt = new ProblemDocumentTable();
                dt.TablePosition.X = TableOffset.X + 10;
                dt.TablePosition.Y = TableOffset.Y + TableHeigth + 50;

            Int32 count = 1;
            foreach (Mark mark in MarksList)
            {
                if (mark is MProblem problem)
                {
                    problem.SetNum(count);
                    problem.SetNumberMode(true);
                    dt.ProblemList.Add(new ProblemDocumentTable.Problem(count, problem.GetProblem()));
                    count++;

                }
            }
            
            FieldCanvas.AddVisual(dt);
            dt.Draw();
          //  FieldCanvas.Height = TableHeigth + dt.GetHeight();


            CreateImage(dt.GetHeight());

       //     FieldCanvas.Height = TableHeigth - dt.GetHeight();

            FieldCanvas.DeleteVisual(dt);

            AddColumnButton.Show();
            AddRowButton.Show();
            horisontal_ruler.Show();
            vertical_ruler.Show();

            foreach (Mark mark in MarksList)
            {
                if (mark is MProblem problem)
                {
                    problem.SetNumberMode(false);
                 
                }
            }

            SetVisibleColRow(true);
        }

        private void SetVisibleColRow(bool state)
        {
            Column n_col = FirstColumn;
            while (n_col != null)
            {
                n_col.ShowHeader = state;
                n_col.DrawUI();
                n_col = n_col.NextColumn;

            }
            Row n_row = FirstRow;
            while (n_row != null)
            {
                n_row.ShowHeader = state;
                n_row.DrawUI();
                n_row = n_row.NextRow;

            }
        }

        private void CreateImage(double extra_height)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)TableWidth+20,
             (int)(FieldCanvas.RenderSize.Height+extra_height+40), 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(FieldCanvas);

            var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, (int)TableWidth + 20, (int)(FieldCanvas.RenderSize.Height + extra_height+ 30)));
            //(int)FieldCanvas.RenderSize.Width - 10

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            using (var fs = System.IO.File.OpenWrite("map.png"))
            {
                pngEncoder.Save(fs);
            }
            //WSaveAsImage meg_w = new WSaveAsImage(this);
            //meg_w.ShowDialog();
        }

        public bool IsEmptyTable()
        {
            for (int i = 0; i <= RowHeight.Count() - 1; i++)
            {
                for (int j = 0; j <= ColumnWidth.Count() - 1; j++)
                {
                   if(!(RowColumn[i][j] is EmptyCell))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void UnselectCell()
        {
            MapConstructorInstance.UnselectCell();
            UnselectTableUI();
        }


        public Time.Type GetActionAxisMeg() => TimeAxisUI.GetActionAxisMeg();
        public Time.Type GetWasteAxisMeg() => TimeAxisUI.GetWasteAxisMeg();
        public Time.Type GetMoveAxisMeg() => TimeAxisUI.GetMoveAxisMeg();
    }
}
