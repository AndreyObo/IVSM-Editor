using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.Types
{
    public class Time
    {
        public double Count;

        public enum Type { Day, Hour, Minute, Second }

      //  private static List<double> SecConvert = new List<double> {};

        private static double[] Seconds = new double[] {86400, 3600, 60, 1 };

        public Type Measure;

        public Time()
        {

        }

        public Time(double time, Type meg)
        {
            Count = time;
            Measure = meg;
        }

        public double GetIn(Time.Type meg)
        {
            double new_timme = Count * Seconds[(int)Measure] / Seconds[(int)meg];
            return new_timme;
        }

        public Time ConvertTo(Time time, Time.Type meg)
        {
            double new_timme = time.Count * Seconds[(int)time.Measure] / Seconds[(int)meg];
            return new Time(new_timme, meg);
        }

        public static bool operator <(Time t1, Time t2)
        {
            double t1_sec = t1.Count * Seconds[(int)t1.Measure];
            double t2_sec = t2.Count * Seconds[(int)t2.Measure];
            return t1_sec < t2_sec;
        }

        public static bool operator >(Time t1, Time t2)
        {
            double t1_sec = t1.Count * Seconds[(int)t1.Measure];
            double t2_sec = t2.Count * Seconds[(int)t2.Measure];
            return t1_sec > t2_sec;
        }
    }
}
