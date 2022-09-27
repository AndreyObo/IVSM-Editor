using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using IVSMlib.Link;
using IVSMlib.ViewModel;
using IVSMlib.ViewModel.Units;

namespace IVSMlib.VsmCanvas.Units
{
    public class ProblemDocumentTable: DrawingVisual
    {
        private string DocsLable = "Документы";
        private string ProblemLable = "Проблемы";

        public bool Showdoc;
        public bool ShowProblem;

        public Point TablePosition;

        private double heigth;

        public struct Problem
        {
            public Int32 number;
            public string description;

            public Problem(int number, string description)
            {
                this.number = number;
                this.description = description;
            }
        }

        public List<Problem> ProblemList = new List<Problem>();

        public ProblemDocumentTable()
        {
            TablePosition = new Point(1, 1);
            heigth = 30;
            Showdoc = true;
            ShowProblem = true;
        }

        public double GetHeight() => heigth;

        public void Draw()
        {
            DrawingContext dc = this.RenderOpen();

            double x_offset = 0;
            double y_offset = 0;
            
            if (Showdoc)
            {
                FormattedText TitleText = new FormattedText(DocsLable, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 14, Brushes.Black);

                dc.DrawText(TitleText, TablePosition);

               
                foreach (DocumentUnit doc in DocConnector.CurrentDocList)
                {
                    dc.DrawRoundedRectangle(doc.DocColor, null, new Rect(TablePosition.X + x_offset + 15, TablePosition.Y + 40, 30, 30), 15, 15);
                    FormattedText doc_title = new FormattedText(doc.GetDocName(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);
                    doc_title.MaxTextWidth = 80;
                    dc.DrawText(doc_title, new Point(TablePosition.X + x_offset, TablePosition.Y + 80));
                    x_offset += 90;
                }

                //--------------------------------------------------------------------

                y_offset = 130;
            }

            x_offset = 0;
            if (ShowProblem)
            {
                FormattedText PrText = new FormattedText(ProblemLable, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 14, Brushes.Black);
                dc.DrawText(PrText, new Point(TablePosition.X, TablePosition.Y + y_offset));

                foreach (Problem pr in ProblemList)
                {
                    FormattedText pr_title = new FormattedText(pr.number.ToString(), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 18, Brushes.Red);
                    dc.DrawText(pr_title, new Point(TablePosition.X, TablePosition.Y + y_offset + 25));

                    FormattedText pr_desc = new FormattedText(pr.description, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 16, Brushes.Black);
                    dc.DrawText(pr_desc, new Point(TablePosition.X + 20, TablePosition.Y + y_offset + 25));
                    y_offset += 30;
                }
            }
            heigth = y_offset + 30;

            dc.Close();
        }
    }
}
