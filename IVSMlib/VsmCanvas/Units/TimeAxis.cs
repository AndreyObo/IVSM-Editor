using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using IVSMlib.Types;

namespace IVSMlib.VsmCanvas.Units
{
    public class TimeAxis:DrawingVisual
    {
        private List<double> ColumnWidth;

        private Point Loc = new Point();

        private Size size;

        private Brush ActionAxisBrush;
        private Brush WasteAxisBrush;
        private Brush MoveAxisBrush;

        private Brush columnBrush;

        private struct AxisHeight
        {
            public double Action;
            public double Waste;
            public double Move;
        }

        private AxisHeight axis_height;

        private struct AxisY
        {
            public double Action;
            public double Waste;
            public double Move;
        }

        private AxisY axis_y;

        private struct AxisTitle
        {
            public static string Action = "Обработка";
            public static string Waste = "Ожидание";
            public static string Move = "Перемещение";

        }

        private class AxisText
        {
            public string ActionText;
            public string WasteText;
        }

        private class Axis_Times
        {
            public Time ActionTime;
            public Time WasteTime;
        }

        private List<Axis_Times> AxisTimes;

        private List<Time> MoveAxisTimes;

        private Time.Type ActionAxisMeg;
        private Time.Type WasteAxisMeg;
        private Time.Type MoveAxisMeg;

        private List<AxisText> AxisTextList;
        private List<string> MoveTextList;

        public TimeAxis()
        {
            ActionAxisBrush = Brushes.LightGreen;
            WasteAxisBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFB6B6");
            MoveAxisBrush = Brushes.LightGray;

            columnBrush = Brushes.DarkGray;

            AxisTextList = new List<AxisText>();
            MoveTextList = new List<string>();

            AxisTimes = new List<Axis_Times>();

            MoveAxisTimes = new List<Time>();

            size = new Size();
            axis_height = new AxisHeight();
            SetSize(100, 30);
            SetLoc(0, 0);
        }

        public void InizialazeColumnList(List<double> c_w)
        {
            ColumnWidth = c_w;
          
        }

        public void InitAxis()
        {
            for (int i = 0; i <= ColumnWidth.Count - 2; i++)
            {
                var axis_t = new AxisText();
                axis_t.ActionText = "0";
                axis_t.WasteText = "0";
                AxisTextList.Add(axis_t);

                //-------------------------
                var axis_time = new Axis_Times();
                axis_time.ActionTime = new Time(0, Time.Type.Second);
                axis_time.WasteTime = new Time(0, Time.Type.Second);

                AxisTimes.Add(axis_time);
                //-------------------------
            }
            for (int i = 0; i <= ColumnWidth.Count - 2; i++)
            {
                MoveTextList.Add("0");
                MoveAxisTimes.Add(new Time(0, Time.Type.Second));
            }
            int p = 0;

        //    DrawUI();
        }
            

        public void AddColumn()
        {
            var axis_t = new AxisText();
            axis_t.ActionText = "0";
            axis_t.WasteText = "0";
            AxisTextList.Add(axis_t);

            var axis_time = new Axis_Times();
            axis_time.ActionTime = new Time(0, Time.Type.Second);
            axis_time.WasteTime = new Time(0, Time.Type.Second);

            AxisTimes.Add(axis_time);
            MoveAxisTimes.Add(new Time(0, Time.Type.Second));

            MoveTextList.Add("0");
        }

        public void SetSize(double width, double height)
        {
            size.Width = width;
            size.Height = height;
            axis_height.Action = height * 0.33;
            axis_height.Waste = height * 0.33;
            axis_height.Move = height * 0.33;
        }

        public void UpdateWidth(double width)
        {
            size.Width = width;
        }

        public void SetLoc(double x, double y)
        {
            Loc.X = x;
            Loc.Y = y;

            axis_y.Action = Loc.Y;
            axis_y.Move = Loc.Y + axis_height.Action;
            axis_y.Waste = Loc.Y + axis_height.Action + axis_height.Move;
        }

        public Time.Type GetActionAxisMeg() => ActionAxisMeg;
        public Time.Type GetWasteAxisMeg() => WasteAxisMeg;
        public Time.Type GetMoveAxisMeg() => MoveAxisMeg;

        public void SetActionMeg(Time.Type meg)
        {
            if(ActionAxisMeg != meg)
            {
                ActionAxisMeg = meg;
                DrawUI();
            }
            
        }

        public void SetWasteMeg(Time.Type meg)
        {
            if (WasteAxisMeg != meg)
            {
                WasteAxisMeg = meg;
                DrawUI();
            }  
        }

        public void SetMoveMeg(Time.Type meg)
        {
            if (MoveAxisMeg != meg)
            {
                MoveAxisMeg = meg;
                DrawUI();
            }          
        }

        public void SetActionTime(Time time, Int32 col)
        {
            AxisTimes[col].ActionTime = time;
            DrawUI();
        }

        public void SetWasteTime(Time time, Int32 col)
        {
            AxisTimes[col].WasteTime = time;
        }

        public void SetMoveTime(Time time, Int32 col_one, Int32 col_two)
        {
            //    MessageBox.Show(col_one.ToString() + " - " + col_two.ToString());
            MoveAxisTimes[col_one] = time;
            DrawUI();
        }

        public void SetActionTime(string text, Int32 col)
        {
            AxisTextList[col].ActionText = text;
            DrawUI();
        }

        public void SetWasteTime(string text, Int32 col)
        {
            AxisTextList[col].WasteText = text;
            DrawUI();
        }


        public void DrawUI()
        {
            DrawingContext dc = this.RenderOpen();


            dc.DrawRectangle(ActionAxisBrush, new Pen(Brushes.Transparent, 1), new Rect(Loc.X , axis_y.Action, size.Width, axis_height.Action));
            dc.DrawRectangle(WasteAxisBrush, new Pen(Brushes.Transparent, 1), new Rect(Loc.X, axis_y.Move, size.Width, axis_height.Waste));
            dc.DrawRectangle(MoveAxisBrush, new Pen(Brushes.Transparent, 1), new Rect(Loc.X, axis_y.Waste, size.Width, axis_height.Move));

            double column_pos_x = 0;
            double prev_col_x = 0;

            for(Int32 col_index=0; col_index <=ColumnWidth.Count-1; col_index++)
            {
                column_pos_x += ColumnWidth[col_index];

                if (col_index == 0)
                {
                    dc.DrawLine(new Pen(Brushes.DarkGray, 1), new Point(Loc.X + column_pos_x, Loc.Y), new Point(Loc.X + column_pos_x, Loc.Y + size.Height));
                   
                }
                else
                {
                    FormattedText action_t = MakeActionText(col_index - 1);

                    if (action_t != null)
                    {
                        dc.DrawText(action_t, new Point((Loc.X + prev_col_x + ColumnWidth[col_index] / 2) - action_t.Width/2, Loc.Y + 1));
                    }


                    FormattedText waste_t = MakeWasteText(col_index - 1);
                    if (waste_t != null)
                    {
                        dc.DrawText(waste_t, new Point((Loc.X + prev_col_x + ColumnWidth[col_index] / 2) - waste_t.Width/2, Loc.Y + axis_height.Action + 1));
                    }


                    dc.DrawLine(new Pen(Brushes.DarkGray, 1), new Point(Loc.X + column_pos_x, Loc.Y), new Point(Loc.X + column_pos_x, Loc.Y + axis_y.Waste));

                   
                }
                if (col_index != 0)
                {
                    double x_pos = ((column_pos_x - prev_col_x) / 2) + prev_col_x;
                    dc.DrawLine(new Pen(Brushes.DarkGray, 1), new Point(Loc.X + x_pos, Loc.Y+axis_y.Waste), new Point(Loc.X + x_pos, Loc.Y + size.Height));

                    //-------------------------------------------------------------------
                    if (col_index < ColumnWidth.Count - 1)
                    {
                        FormattedText move_t = MakeMoveText(col_index - 1);
                      
                        if (move_t != null)
                        {
                            dc.DrawText(move_t, new Point(Loc.X + column_pos_x - move_t.Width / 2, Loc.Y + axis_y.Waste));
                        }
                    }
                }
                prev_col_x += ColumnWidth[col_index];
            }

            //DrawLable-------------------
            FormattedText ActionText= new FormattedText(AxisTitle.Action, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);
      
            dc.DrawText(ActionText, new Point(Loc.X+3, axis_y.Action+3));
            FormattedText WasteText = new FormattedText(AxisTitle.Waste, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);
            dc.DrawText(WasteText, new Point(Loc.X + 3, axis_y.Move + 3));
            FormattedText MoveText = new FormattedText(AxisTitle.Move, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);
            dc.DrawText(MoveText, new Point(Loc.X + 3, axis_y.Waste + 3));

            dc.Close();
        }

        private FormattedText MakeActionText(Int32 col)
        {
            if(AxisTimes[col].ActionTime.Count == 0)
            {
                return null;
            }
           return new FormattedText(AxisTimes[col].ActionTime.GetIn(ActionAxisMeg).ToString("0.0"), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);           
        }

        private FormattedText MakeWasteText(Int32 col)
        {
            if(AxisTimes[col].WasteTime.Count == 0)
            {
                return null;
            }

            return new FormattedText(AxisTimes[col].WasteTime.GetIn(WasteAxisMeg).ToString("0.0"), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);
        }

        private FormattedText MakeMoveText(Int32 col)
        {
            if (MoveAxisTimes[col].Count == 0)
            {
                return null;
            }

            return new FormattedText(MoveAxisTimes[col].GetIn(MoveAxisMeg).ToString("0.0"), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);
        }

    }
}
