using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomCore;
using IVSMlib.Types;

namespace IVSMlib.TableDom.TypesDom
{
    public static class TimeDom
    {

        public static string CodeTime(Time time)
        {
            return time.Count.ToString() + "~" + time.Measure.ToString();
        }

        public static Time DecodeTime(string time)
        {
            string count_s = "";
            string meg_s;
            char brek = '~';
            int count=0;
            while(time[count] != brek)
            {
                count_s += time[count];
                count += 1;
            }

            count += 1;
            meg_s = time.Substring(count, time.Length - count);

            //    Console.WriteLine(count_s + " ------------->>>>>> " + meg_s);
            Time time_t = new Time();
            time_t.Count = Convert.ToDouble(count_s);

            if (meg_s == Time.Type.Day.ToString())
            {
                time_t.Measure = Time.Type.Day;
            }
            if (meg_s == Time.Type.Hour.ToString())
            {
                time_t.Measure = Time.Type.Hour;
            }
            if (meg_s == Time.Type.Minute.ToString())
            {
                time_t.Measure = Time.Type.Minute;
            }
            if (meg_s == Time.Type.Second.ToString())
            {
                time_t.Measure = Time.Type.Second;
            }
            return time_t;

        }
        public static Node GetNode(Time time)
        {
            Node time_node = new Node(Lexer.GetPropertyTag(Lexer.PropertyToken.Time));
            time_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Count), time.Count.ToString());
            time_node.AddNodeItem<string>(Lexer.GetPropertyTag(Lexer.PropertyToken.Measure), time.Measure.ToString());

            return time_node;
        }

        public static Time ParseTime(Node time_node)
        {
            Time time = new Time();

            foreach (Node.NodeItem<object> time_item in time_node.GetNodes())
            {
                if(time_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Count))
                {
                    time.Count = Convert.ToDouble(time_item.value);
                }
                if (time_item.name == Lexer.GetPropertyTag(Lexer.PropertyToken.Measure))
                {
                    //string minute = Time.Type.Minute.ToString();
                    //string seconds = Time.Type.Second.ToString();

                    if ((string)time_item.value == Time.Type.Day.ToString())
                    {
                        time.Measure = Time.Type.Day;
                    }
                    if ((string)time_item.value == Time.Type.Hour.ToString())
                    {
                        time.Measure = Time.Type.Hour;
                    }
                    if ((string)time_item.value == Time.Type.Minute.ToString())
                    {
                        time.Measure = Time.Type.Minute;
                    }
                    if ((string)time_item.value == Time.Type.Second.ToString())
                    {
                        time.Measure = Time.Type.Second;
                    }
                }
            }

            return time;
        }
    }
}
