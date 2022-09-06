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

        //public static IEnumerable<Color> GetColors()
        //{
        //    return new List<Color>
        //    {
        //        LastColors[]
        //    };
        //}

        public static List<MoveModel> MoveModelsStore = new List<MoveModel>();
        public static IVSM CurrentIVSM { get; set; }
        public static string CurrentFileName= NULL_FILE;

        //    public static MainTableVM TableVM { get; set; }

        public enum TableItems { Player, Action, Condition }
        public static TableItems CreatedItem;

        //public static void CreateCell(Cell current_cell, Int32 row, Int32 column)
        //{
        //   if(column == 0)
        //   {
        //        if (current_cell is PlayerCell)
        //        {
        //            MessageBox.Show("Cell alredy have player");
        //        }
        //        Console.WriteLine("This player Cell");
        //          PlayerCell player_cell = new PlayerCell();
        //    //   ConditionCell player_cell = new ConditionCell();
        //         //   player_cell.player_model = new Player();
        //        //   CurrentMap.VsmItems[row][column] = player_cell.player_model;
        //        TableVM.AddCell(player_cell, row, column);

        //        return;
        //    }
        //   else
        //   {
        //        if (CreatedItem == TableItems.Action)
        //        {
        //            ActionCell action_cell = new ActionCell();
        //            TableVM.AddCell(action_cell, row, column);
        //        }
        //        else if(CreatedItem == TableItems.Condition)
        //        {
        //            ConditionCell action_cell = new ConditionCell();
        //            TableVM.AddCell(action_cell, row, column);
        //        }
        //    }


        //}

        //public static void ShowNeibMoveBtn(Cell cell)
        //{
        //    Int32 Column = cell.TableIndex.Column;

        //    for(Int32 row=0; row <=TableVM.RowColumn.Count-1; row++)
        //    {
        //        TableVM.RowColumn[row][Column + 1].ShowMoveButton = true;
        //        TableVM.RowColumn[row][Column +1].DrawUI();
        //    }
        //}
    }
}
