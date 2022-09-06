using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using IVSMlib.IVSMModel.Items;

namespace IVSMlib.IVSMModel
{
   public class IVSM
   {
        public string MapName;

        private Int32 LineId;
        private Int32 DocId;

        public IVSM()
        {
            MapName = "Новая карта";
            LineId = 0;
            DocId = 0;
        }

        public Int32 GetLineId()
        {
            return LineId++;
        } 

        public void SetCurrentLineId(Int32 id)
        {
            LineId = id;
        }

        public Int32 GetDocId() => DocId++;

        public void SetCurrentDocId(Int32 id)
        {
            DocId = id;
        }
   }
}
