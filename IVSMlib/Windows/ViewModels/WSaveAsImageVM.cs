using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSMlib.ViewModel;
using IVSMlib.VsmCanvas;

namespace IVSMlib.Windows.ViewModels
{
    public class WSaveAsImageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // private MainTableVM Table;
        // private VsmCustomCanvas SaveCanvas { get; set; }

        // public WSaveAsImageVM(VsmCustomCanvas canvas, MainTableVM table)
        // {
        //     Table = table;
        //     SaveCanvas = canvas;
        //     CreatePicture();
        // }

        // private void CreatePicture()
        // {
        ////     Table.FieldCanvas.DeleteVisual(Table.TimeAxisUI);
        //     SaveCanvas.AddVisual(Table.TimeAxisUI);
        // }
    }
}
