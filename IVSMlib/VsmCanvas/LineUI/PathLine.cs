using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


using DomCore;
using IVSMlib.TableDom.LineDom;
using IVSMlib.IVSMModel;
using IVSMlib.PropsHolders;
using IVSMlib.Types;
using IVSMlib.VsmCanvas.CellUI;
using IVSMlib.VsmCanvas.CellUI.Interface;
using IVSMlib.VsmCanvas.LineUI.Interface;
using IVSMlib.PropsHolders.VisualProps;
using IVSMlib.Language;

namespace IVSMlib.VsmCanvas.LineUI
{
    public class PathLine : Line, IProps, ILTime, IDom
    {
        private MoveModel move_model;

        public enum Dash { NoDash, Dash}
        public Dash LineDash;

        public class LinePoint
        {
            public double X1;
            public double Y1;
            public double X2;
            public double Y2;

            public LinePoint(double x1, double y1, double x2, double y2)
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }
        }

        public List<LinePoint> LinePath;

        private Pen LinePen;

        public delegate void UpdateTime(Int32 f_coll, Int32 s_coll);
        public UpdateTime UpdateTimeCallbak;

        //  private double start_x;
        //   private double start_y;


        //----------------------------


        //--********************************
        private Brush deb_brush_1;
        private Brush deb_brush_2;
        //---******************************

        private Brush DrawBrush;
        private Brush SelectedBrush;

        private PropsHolder MovePropsHolder;

        private void CreatePropertyHolders()
        {
            MovePropsHolder = new PropsHolder();
            MovePropsHolder.OwnerType = PropsHolder.Type.PathLine;


            //   PropsHolders = new List<Props>();

            StringProps OrgamizationHolder = new StringProps();

            OrgamizationHolder.PropsEditType = StringProps.EditType.Line;
            OrgamizationHolder.Title = "Тип перемещения";
            OrgamizationHolder.GetCurrentValueDelegate = GetMoveType;
            OrgamizationHolder.SetPropertyDelegate = SetMoveType;
            //      OrgamizationHolder.StringType = StringProps.Type.Line;

            MovePropsHolder.PropsList.Add(OrgamizationHolder);



            TimeProps tm = new TimeProps();
            tm.Title = "Время перемещения";
            tm.GetCurrentValueDelegate = GetMoveTime;
            tm.SetPropertyDelegate = SetMoveTime;

            MovePropsHolder.PropsList.Add(tm);


            StringProps Comment = new StringProps();
            Comment.PropsEditType = StringProps.EditType.MultiLine;
            Comment.Title = "Комментарии";
            Comment.GetCurrentValueDelegate = GetComment;
            Comment.SetPropertyDelegate = SetComment;

            MovePropsHolder.PropsList.Add(Comment);

            ColorProps bcolor = new ColorProps();

            bcolor.FillType = ColorProps.FType.OnlyFill;
            bcolor.SetTitle("Цвет рамки");
            bcolor.GetCurrentColorDelegate = GetColor;
            bcolor.SetColorDelegate = SetColor;

            MovePropsHolder.VisualPropsList.Add(bcolor);

            DropListProps DashType = new DropListProps();
            DashType.SetTitle("Тип линии");
            DashType.Items.Add(StringRes.GetPropsString(StringRes.PropsToken.NoDash));
            DashType.Items.Add(StringRes.GetPropsString(StringRes.PropsToken.Dash));
            DashType.GetCurrentValueDelegate += GetDash;
            DashType.SetPropertyDelegate += SetDash;

            MovePropsHolder.VisualPropsList.Add(DashType);

        }

        public Color GetColor() => ((SolidColorBrush)DrawBrush).Color;
        public void SetColor(Color c)
        {
            DrawBrush = new SolidColorBrush(c);
        }

        public string GetDash()
        {
            string s = "";
            switch(LineDash)
            {
                case Dash.NoDash:
                    s = StringRes.GetPropsString(StringRes.PropsToken.NoDash);
                    break;
                case Dash.Dash:
                    s = StringRes.GetPropsString(StringRes.PropsToken.Dash);
                    break;
            }

            return s;
        }

        public void SetDash(string s)
        {
          
            if(s == StringRes.GetPropsString(StringRes.PropsToken.Dash))
            {
                LineDash = Dash.Dash;
                LinePen.DashStyle = DashStyles.Dash;
                DrawUI();
                return;
            }
            if(s == StringRes.GetPropsString(StringRes.PropsToken.NoDash))
            {
                LineDash = Dash.NoDash;
                LinePen.DashStyle = DashStyles.Solid;
                DrawUI();
            }
        }

        public void SetDash(Dash style)
        {
            LineDash = style;
            if(style == Dash.Dash)
            {
                LinePen.DashStyle = DashStyles.Dash;
            }
            else if(style == Dash.NoDash)
            {
                LinePen.DashStyle = DashStyles.Solid;
            }
        }

        public string GetMoveType() => move_model.MoveType;
        public void SetMoveType(string type) => move_model.MoveType = type;

        public Time GetMoveTime() => move_model.MoveTime;
        public void SetMoveTime(Time time)
        {
            move_model.MoveTime = time;
            UpdateTimeCallbak?.Invoke(Left_Cell.TableIndex.Column, Right_Cell.TableIndex.Column);
        }

        public string GetComment() => move_model.Comment;
        public void SetComment(string text) => move_model.Comment = text;

        public PathLine()
        {
            LineDash = Dash.NoDash;

            move_model = new MoveModel();

            move_model.MoveTime = new Time(1, Time.Type.Minute);
            LinePath = new List<LinePoint>();

            DrawBrush = new SolidColorBrush(Color.FromRgb(80, 76, 75));
            SelectedBrush = Brushes.Red;

            LinePen = new Pen(DrawBrush, 3);
            //  LinePen.DashStyle = DashStyles.Dash;
            LinePen.StartLineCap = PenLineCap.Round;
            LinePen.EndLineCap = PenLineCap.Round;

            deb_brush_1 = Brushes.Red;
            deb_brush_2 = Brushes.Green;

            CreatePropertyHolders();
        }

        public void AddPath(List<LinePoint> path)
        {
            LinePath = path;
        }


        public override void DrawUI()
        {
            DrawingContext dc = this.RenderOpen();

        //    dc.DrawRectangle(deb_brush_1, new Pen(Brushes.Transparent, 0), new Rect(LinePath[0].X1, LinePath[0].Y1, 10, 10));

       //     dc.DrawRectangle(deb_brush_2, new Pen(Brushes.Transparent, 0), new Rect(LinePath[LinePath.Count-1].X2, LinePath[LinePath.Count - 1].Y2, 10, 10));
        
            foreach (LinePoint lp in LinePath)
            {
                dc.DrawLine(LinePen, new Point(lp.X1, lp.Y1), new Point(lp.X2, lp.Y2));
            }

            dc.Close();
        }

        public override void MouseDown(Point e)
        {
           // throw new NotImplementedException();
        }

        public override void MouseEnter()
        {
         //   throw new NotImplementedException();
        }

        public override void MouseLeave()
        {
          //  throw new NotImplementedException();
        }

        public override void MouseMove(Point e)
        {
         //   throw new NotImplementedException();
        }

        public override void MouseUp(Point e)
        {
        //    throw new NotImplementedException();
        }

        public override void Select()
        {
            LinePen.Brush = SelectedBrush;
            DrawUI();
        }

        public override void Unselect()
        {
            LinePen.Brush = DrawBrush;
            DrawUI();
        }

        public override void SetStartPoint(double x, double y)
        {
            if(LinePath.Count==1)
            {
                LinePath[0].X1 = x;
                LinePath[0].Y1 = y;
                return;
            }
            if (LinePath[0].X1 == LinePath[0].X2)
            {
                LinePath[0].X1 = x;
                LinePath[0].Y1 = y;

                LinePath[0].X2 = x;

                LinePath[1].X1 = LinePath[0].X1;
            }

            if (LinePath[0].Y1 == LinePath[0].Y2)
            {
                LinePath[0].X1 = x;
                LinePath[0].Y1 = y;

                LinePath[0].Y2 = y;

                LinePath[1].Y1 = LinePath[0].Y1;
            }
        }

        public override void SetEndPoint(double x, double y)
        {
            if (LinePath.Count == 1)
            {
                LinePath[0].X2 = x;
                LinePath[0].Y2 = y;
                return;
            }

            LinePath[LinePath.Count-1].X2 = x;
            LinePath[LinePath.Count - 1].Y2 = y;

            LinePath[LinePath.Count - 1].Y1 = y;

            LinePath[LinePath.Count - 2].Y2 = LinePath[LinePath.Count - 1].Y1;
        }

        public PropsHolder GetPropsHolder() => MovePropsHolder;

        public override MoveModel GetModel()
        {
            return move_model;
        }

        public Time GetLineMoveTime()
        {
            return move_model.MoveTime;
        }

        public Node CreateDomNode()
        {
            return PathLineDom.Get().CreateRootNode(this);
        }
    }
}
