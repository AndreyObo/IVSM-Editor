using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.TableDom
{
    public static class Lexer
    {
        //------------------------------Map Tokens ------------------------------
        public enum MapToken
        {
            IVSM,
            Table,
            Width,
            Heigth,
            RowDefinition,
            ColumnDefinition,
            RowCount,
            ColumnCount,
            Row,
            Column,
            Cells,
            Lines,
            Marks,
            Documents,
        }

        public static Dictionary<MapToken, string> MapTokens = new Dictionary<MapToken, string>
        {
            {MapToken.IVSM, "IVSM" },
            {MapToken.Table, "Table" },
            {MapToken.Width, "Width" },
            {MapToken.Heigth, "Heigth" },
            {MapToken.RowDefinition, "RowDefinition" },
            {MapToken.ColumnDefinition, "ColumnDefinition" },
            {MapToken.RowCount, "RowCount" },
            {MapToken.ColumnCount, "ColumnCount" },
            {MapToken.Row, "Row" },
            {MapToken.Column, "Column" },
            {MapToken.Cells, "Cells" },
            {MapToken.Lines, "Lines" },
            {MapToken.Marks, "Marks" },
            {MapToken.Documents, "Documents" },

        };

        //------------------------------UI Tokens ------------------------------

        public enum UIToken
        {
            Player,
            Action,
            Condition,
            Document,
            Line,
            Problem,
            Desicion,
            TextLable
        }

        public static Dictionary<UIToken, string> UITokens = new Dictionary<UIToken, string> {
            {UIToken.Player, "Player"},
            {UIToken.Action, "Action"},
            {UIToken.Condition, "Condition"},
            {UIToken.Document, "Document"},
            {UIToken.Line, "Line"},
            {UIToken.Problem, "Problem"},
            {UIToken.Desicion, "Desicion"},
            {UIToken.TextLable, "TextLable"},
        };

        //---------------------------Property token ----------------------------------------------
        public enum PropertyToken
        {
           Id,
           Name,
           Title,
           Props,
           PropItem,
           Type,
           Time,
           Measure,
           Count,
           String,
           Integer,
           Double,
           Decoration,
           DocumentsList,
           LinesList,
           LinePath,
           X1,
           Y1,
           X2,
           Y2,
           Line,
           Text,
           Color,
           FontFamily,
           FontSize,
           BorderWidth,
           BorderColor,
           Fill,
           Location,
           X,
           Y,
           Size,
           Width,
           Height,
           TextColor,
           Alight,
           Transparent,
           DashStyle,
           Dash,
           NoDash,
        }

        public static Dictionary<PropertyToken, string> PropertyTokens = new Dictionary<PropertyToken, string> {
            {PropertyToken.Id, "Id" },
            {PropertyToken.Name, "Name" },
            {PropertyToken.Title, "Title" },
            {PropertyToken.Props, "Props"},
            {PropertyToken.PropItem, "PropItem"},
            {PropertyToken.Type, "Type"},
            {PropertyToken.Time, "Time"},
            {PropertyToken.Measure, "Measure"},
            {PropertyToken.Count, "Count"},
            {PropertyToken.String, "String"},
            {PropertyToken.Integer, "Integer"},
            {PropertyToken.Double, "Double"},
            {PropertyToken.Decoration, "Decoration"},
            {PropertyToken.DocumentsList, "Documents" },
            {PropertyToken.LinesList, "Lines" },
            {PropertyToken.LinePath, "LinePath" },
            {PropertyToken.X1, "X1" },
            {PropertyToken.Y1, "Y1" },
            {PropertyToken.X2, "X2" },
            {PropertyToken.Y2, "Y2" },
            {PropertyToken.Line,"Line" },
            {PropertyToken.Text, "Text"},
            {PropertyToken.Color, "Color"},
            {PropertyToken.FontFamily, "FontFamily"},
            {PropertyToken.FontSize, "FontSize"},
            {PropertyToken.BorderWidth, "BorderWidth"},
            {PropertyToken.BorderColor, "BorderColor"},
            {PropertyToken.Fill, "Fill"},
            {PropertyToken.Location, "Location"},
            {PropertyToken.X, "X"},
            {PropertyToken.Y, "Y"},
            {PropertyToken.Size, "Size"},
            {PropertyToken.Width, "Width"},
            {PropertyToken.Height, "Height"},
            {PropertyToken.TextColor, "TextColor"},
            {PropertyToken.Alight, "Alight"},
            {PropertyToken.Transparent, "Transparent"},
            {PropertyToken.DashStyle, "DashStyle" },
            {PropertyToken.Dash, "Dash" },
            {PropertyToken.NoDash, "NoDash" },

        };
        //----------------------------------------------------------------------------------------

        public static string GetMapTag(MapToken token)
        {
            return MapTokens[token];
           
        }

        public static string GetUITag(UIToken token)
        {
            return UITokens[token];

        }

        public static string GetPropertyTag(PropertyToken token)
        {
            return PropertyTokens[token];

        }
    }
}
