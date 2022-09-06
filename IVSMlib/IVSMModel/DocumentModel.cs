using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVSMlib.IVSMModel
{
    public class DocumentModel
    {
        public string Name { get; set; }

        public string Owner { get; set; }

        public DateTime Date { get; set; }

        public string PhotoSrc { get; set; }

        public string Comment { get; set; }
    }
}
