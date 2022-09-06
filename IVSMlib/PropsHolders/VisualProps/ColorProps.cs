using DomCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;



namespace IVSMlib.PropsHolders.VisualProps
{
    public class ColorProps:Props
    {
        public enum FType {OnlyFill, Transparant}
        public FType FillType;
        public delegate Color GetCurrentColor();

        public GetCurrentColor GetCurrentColorDelegate;

        public delegate void SetColor(Color _color);

        public SetColor SetColorDelegate;

        public override void GetNode(ref Node root)
        {
           // throw new NotImplementedException();
        }
    }
}
