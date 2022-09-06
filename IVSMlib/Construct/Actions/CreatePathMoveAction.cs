using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;



using IVSMlib.EventsSupport;
using IVSMlib.VsmCanvas.CellUI;
using IVSMlib.VsmCanvas.LineUI;
using IVSMlib.VsmCanvas.Types;
using IVSMlib.Global;

namespace IVSMlib.Construct.Actions
{
    public class CreatePathMoveAction : IAction
    {
        private Pen DemoLinePen;
        private Pen DemoPathPen;
        private DrawingVisual DemoLine;

        private DrawingVisual DemoPath;

        private MapConstructor Constructor;

        private Point StartPoint;
        public PathLine InsertLine;

        public MoveButtonArg FirstArg { get; set; }
        public Cell FirstCell;

        private List<PathLine.LinePoint> DemoLinePath;

        Boolean IsHorizontalCaptured;
        Boolean IsVerticalCaptured;

        Point draw_point = new Point();

        bool click;

        public CreatePathMoveAction(MapConstructor owner)
        {
            Constructor = owner;
            DemoLinePen = new Pen(Brushes.Red, 3);
            DemoPathPen = new Pen(Brushes.Green, 3);
            DemoLinePen.DashStyle = DashStyles.Dash;
            DemoLine = new DrawingVisual();
            DemoPath = new DrawingVisual();

            IsHorizontalCaptured = false;
            IsVerticalCaptured = false;

           

            click = false;
        }

        public void OpenEdit()
        {
            DemoLinePath = new List<PathLine.LinePoint>();
            Constructor.Table.AddVisual(DemoLine);
            Constructor.Table.AddVisual(DemoPath);

            StartPoint = FirstArg.StartPoint;
           
        }

        public void CloseEdit()
        {
            DrawingContext dc = DemoLine.RenderOpen();
            dc.Close();
            DrawingContext path_dc = DemoPath.RenderOpen();
            path_dc.Close();
            Constructor.Table.DeleteVisual(DemoLine);
            Constructor.Table.DeleteVisual(DemoPath);
            FirstArg = null;
            FirstCell = null;
            DemoLinePath = null;
        }

        public bool MouseDownAction(Point e, MouseState state)
        {

           // Console.WriteLine("Path click");
            // InsertLine.AddNewLine(e.X, e.Y);
            // InsertLine.DrawUI();
            if(state.RightButton == MouseButtonState.Pressed)
            {
                if (DemoLinePath.Count >= 2)
                {
                    StartPoint.X = DemoLinePath[DemoLinePath.Count - 1].X1;
                    StartPoint.Y = DemoLinePath[DemoLinePath.Count - 1].Y1;

                    DemoLinePath.RemoveAt(DemoLinePath.Count - 1);
                    UpdateDemoPath();
                }
                else if(DemoLinePath.Count == 1 || DemoLinePath.Count == 0)
                {
                    Constructor.CurreantAction = MapConstructor.Actions.None;
                    CloseEdit();
                    return false;
                }
                return true;
            }

            click = true;
            if (IsVerticalCaptured || IsHorizontalCaptured)
            {
                DemoLinePath.Add(new PathLine.LinePoint(StartPoint.X, StartPoint.Y, draw_point.X, draw_point.Y));
                StartPoint.X = draw_point.X;
                StartPoint.Y = draw_point.Y;
                UpdateDemoPath();
                click = false;
            }
            
            return true;
        }

        public void MouseMoveAction(Point e)
        {
            SetXPos(e);
            SetYPos(e);
            DrawDemo(draw_point);
        }

        private void SetXPos(Point e)
        {
            if (!IsVerticalCaptured)
            {
                if (e.X + 10 >= StartPoint.X && e.X - 10 <= StartPoint.X)
                {
                    IsVerticalCaptured = true;
                    draw_point.X = StartPoint.X;
               //     DrawDemo(draw_point);
                    return;
                }
                else
                {
                    if (e.X - StartPoint.X < 0)
                    {
                        draw_point.X = e.X + 5;
                    }
                    else
                    {
                        draw_point.X = e.X - 5;
                    }
                }
            }

            if (IsVerticalCaptured)
            {
                if (StartPoint.X - 10 < e.X && e.X < StartPoint.X + 10)
                {
                    return;
                }
                else
                {
                    //  InsertLine.X2 = mouse_x;

                    // draw_point.X = e.X-5;
                    if (e.X - StartPoint.X < 0)
                    {
                        draw_point.X = e.X + 5;
                    }
                    else
                    {
                        draw_point.X = e.X - 5;
                    }
                    IsVerticalCaptured = false;
                    // DrawDemo(draw_point);
                }

            }
        }

        private void SetYPos(Point e)
        {

            if (!IsHorizontalCaptured)
            {
                if (e.Y + 10 >= StartPoint.Y && e.Y - 10 <= StartPoint.Y)
                {
                    IsHorizontalCaptured = true;
                    draw_point.Y = StartPoint.Y;
                 //   DrawDemo(draw_point);
                    return;
                }
                else
                {

                    draw_point.Y = e.Y-5;
                }
            }

            if (IsHorizontalCaptured)
            {
                if (StartPoint.Y - 10 < e.Y && e.Y < StartPoint.Y + 10)
                {
                    return;
                }
                else
                {
                    //  InsertLine.X2 = mouse_x;
                    draw_point.Y = e.Y-5;
                    IsHorizontalCaptured = false;
                }

            }
        }

        private void DrawDemo(Point e)
        {
            DrawingContext dc = DemoLine.RenderOpen();

            dc.DrawLine(DemoLinePen, StartPoint, e);

            dc.Close();
        }
            
        public void MouseUpAction(Point e)
        {
           // throw new NotImplementedException();
        }

        private void UpdateDemoPath()
        {
            DrawingContext dc = DemoPath.RenderOpen();

            foreach (PathLine.LinePoint lp in DemoLinePath)
            {
                dc.DrawLine(DemoPathPen, new Point(lp.X1, lp.Y1), new Point(lp.X2, lp.Y2));
            }

            dc.Close();
        }

        public bool CreateLine(Cell SecondCell, MoveButtonArg Secarg)
        {
            if (SecondCell == FirstCell)
            {
                MessageBox.Show("Выбранная ячейка недоступна.");
                if (DemoLinePath.Count != 0)
                {
                    StartPoint.X = DemoLinePath[DemoLinePath.Count - 1].X1;
                    StartPoint.Y = DemoLinePath[DemoLinePath.Count - 1].Y1;
                    DemoLinePath.RemoveAt(DemoLinePath.Count - 1);
                    UpdateDemoPath();
                }
                return false;
            }

            PathLine line = new PathLine();
            line.Id = GlobalStore.CurrentIVSM.GetLineId();

            //-----------------------------------
            if(FirstCell.TableIndex.Column < SecondCell.TableIndex.Column)
            {
                line.Left_Cell = FirstCell;
                line.Right_Cell = SecondCell;
            }
            else
            {
                line.Left_Cell = SecondCell;
                line.Right_Cell = FirstCell;
            }
            
            //-----------------------------------


            line.UpdateTimeCallbak = Constructor.UpdateAxisMoveTime;
            if (click)
            {
                  DemoLinePath.Add(new PathLine.LinePoint(StartPoint.X, StartPoint.Y, Secarg.StartPoint.X, Secarg.StartPoint.Y));
            }
            click = false;
            line.AddPath(DemoLinePath);
            Constructor.Table.AddUI(line);
            Constructor.Table.MapLines.Add(line);
            FirstArg.AddLineHandler(line);
            Secarg.AddLineHandler(line);

            line.DrawUI();

           // Console.WriteLine(Secarg.StartPoint.ToString());
            return true;
        }


    }
}
