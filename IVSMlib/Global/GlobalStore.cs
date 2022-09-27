using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using IVSMlib.IVSMModel;
using IVSMlib.ViewModel;
using IVSMlib.VsmCanvas.CellUI;
//using IVSMlib.IVSMModel.Items;

namespace IVSMlib.Global
{
    public static class GlobalStore
    {
        public const string NULL_FILE = "NullFile";
        public static Color DefaultCellColor { get; } = (Color)ColorConverter.ConvertFromString("#e05858");

        public static System.Drawing.Color[] LastColors { get; private set; } = new System.Drawing.Color[5];
        private static Int32 color_index=0;

        public static void AddLastColor(System.Drawing.Color color)
        {
            for(int i=0; i<=LastColors.Length -1; i++)
            {
                if (color == LastColors[i]) return;
            }

            if(color_index == LastColors.Length-1)
            {
                color_index = 0;
            }

            LastColors[color_index] = color;
            color_index++;
        }


        public static List<MoveModel> MoveModelsStore = new List<MoveModel>();
        public static IVSM CurrentIVSM { get; set; }
        public static string CurrentFileName= NULL_FILE;


        public enum TableItems { Player, Action, Condition }
        public static TableItems CreatedItem;

    }
}
