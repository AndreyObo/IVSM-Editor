using IVSMlib.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.Windows.ViewModels
{
    public class WSetAxisMegVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<string> TimeMeg { get; set; } = new ObservableCollection<string>();

        private List<Time.Type> types = new List<Time.Type> { Time.Type.Day, Time.Type.Hour, Time.Type.Minute, Time.Type.Second };

     //   private static Time.Type[] types = new Time.Type[] { Time.Type.Day, Time.Type.Hour };
        private Int32 act_sel_index;
        private Int32 waste_sel_index;
        private Int32 move_sel_index;

        public Int32 ActionSelIndex
        {
            get
            {
                return act_sel_index;
            }
            set
            {
                act_sel_index = value;
                OnPropertyChanged("ActionSelIndex");
            }
        }
        public Int32 WasteSelIndex
        {
            get
            {
                return waste_sel_index;
            }
            set 
            {
                waste_sel_index = value;
                OnPropertyChanged("WasteSelIndex");
            }
        }
        public Int32 MoveSelIndex
        {
            get
            {
                return move_sel_index;
            }
            set
            {
                move_sel_index = value;
                OnPropertyChanged("MoveSelIndex");
            }
        }

        public WSetAxisMegVM(Time.Type act_type, Time.Type wst_type, Time.Type mv_type)
        {
            TimeMeg.Add("Дни");
            TimeMeg.Add("Часы");
            TimeMeg.Add("Минуты");
            TimeMeg.Add("Секунды");

            ActionSelIndex = (int)act_type;
            WasteSelIndex = (int)wst_type;

            MoveSelIndex = (int)mv_type;
        }

        public Time.Type GetActionType()
        {
            return types[ActionSelIndex];
        }

        public Time.Type GetWasteType()
        {
            return types[WasteSelIndex];
        }

        public Time.Type GetMoveType()
        {
            return types[MoveSelIndex];
        }
    }
}
