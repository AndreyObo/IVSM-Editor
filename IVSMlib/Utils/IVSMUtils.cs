using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.Utils
{
    public static class IVSMUtils
    {
        public static String HexConverter(System.Windows.Media.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static List<string> GetSysFontsNames()
        {
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();

            FontFamily[] fontFamilies;

            fontFamilies = installedFontCollection.Families;

            List<string> sys_fonts_name = new List<string>();

            int count = fontFamilies.Length;

            for (int j = 0; j < count; ++j)
            {
                sys_fonts_name.Add(fontFamilies[j].Name);
            }

            return sys_fonts_name;

        }
    }
}
