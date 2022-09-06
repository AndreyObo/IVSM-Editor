using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IVSMlib.Language
{
    public static class StringRes
    {
        private static void AddToken<TEnum>(Dictionary<TEnum, string> dic, XDocument doc, string element) where TEnum : struct
        {

            XElement root = doc.Root.Element(element);

            foreach (XElement y in root.Elements())
            {
                TEnum token;
                if (Enum.TryParse(y.Name.ToString(), out token))
                {
                    dic.Add(token, y.Value.ToString());
                }
                else
                {
                    //BadTokens
                }

            }
        }

        public static bool LoadDictionary()
        {

            string file_name = "rus.xml";

            XDocument doc = XDocument.Load(file_name);

            if (doc.Root.Name.ToString() != "IVSMLocal")
            {
                Console.WriteLine("Incorrect file format");
                return false;
            }
            else
            {
                Console.WriteLine("languge - " + doc.Root.Attribute("language").Value);
            }

            AddToken<ControlsToken>(ControlsText, doc, "Controls");

            AddToken<PropsToken>(PropsText, doc, "Props");

            return true;
        }

        public enum ControlsToken
        {
            Player,
            Action,
            Condition,
            Move
        }


        public enum PropsToken
        {
            Name,
            CycleTime,
            Description,
            WasteType,
            Days,
            Hours,
            Minutes,
            Seconds,
            Dash,
            NoDash
        }

        public static string GetControlsString(ControlsToken token)
        {
            return ControlsText[token];
        }

        public static string GetPropsString(PropsToken token)
        {
            return PropsText[token];
        }

        private static Dictionary<ControlsToken, string> ControlsText { get; } = new Dictionary<ControlsToken, string>();

        private static Dictionary<PropsToken, string> PropsText { get; } = new Dictionary<PropsToken, string>();

    }
}

