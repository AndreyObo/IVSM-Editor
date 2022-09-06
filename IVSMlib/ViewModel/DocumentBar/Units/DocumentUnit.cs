using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using IVSMlib.IVSMModel;
using IVSMlib.PropsHolders;
using IVSMlib.PropsHolders.VisualProps;
using IVSMlib.Global;
using IVSMlib.TableDom.DocumentDom;
using DomCore;

namespace IVSMlib.ViewModel.Units
{
    public class DocumentUnit : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private void CreatePropertyHolders()
        {  
         
            DocumentPropsHolder.OwnerType = PropsHolder.Type.Document;
            DocumentPropsHolder.GetLableCallback = GetDocLable;
            DocumentPropsHolder.SetLableCallback = SetDocLable;

            //   PropsHolders = new List<Props>();

            StringProps DocName = new StringProps();

            DocName.PropsEditType = StringProps.EditType.Line;
            DocName.Title = "Наименование";
            DocName.GetCurrentValueDelegate = GetDocName;
            DocName.SetPropertyDelegate = SetDocName;
            //      OrgamizationHolder.StringType = StringProps.Type.Line;

            DocumentPropsHolder.PropsList.Add(DocName);
            // PropsHolders.Add(OrgamizationHolder);

            StringProps DocOwner = new StringProps();
            DocOwner.PropsEditType = StringProps.EditType.Line;
            DocOwner.Title = "Кем утвержден";
            DocOwner.GetCurrentValueDelegate = GetDocOwner;
            DocOwner.SetPropertyDelegate = SetDocOwner;
            //  DepartmentHolder.StringType = StringProps.Type.Text;

            //       PropsHolders.Add(DepartmentHolder);
            DocumentPropsHolder.PropsList.Add(DocOwner);

            DateProps SingDate = new DateProps();
            SingDate.Title = "Дата утверждения";
            SingDate.GetCurrentValueDelegate = GetSingDate;
            SingDate.SetPropertyDelegate = SetSingDate;

            DocumentPropsHolder.PropsList.Add(SingDate);

            StringProps Comment = new StringProps();
            Comment.PropsEditType = StringProps.EditType.MultiLine;
            Comment.Title = "Комментарии";
            Comment.GetCurrentValueDelegate = GetComment;
            Comment.SetPropertyDelegate = SetComment;

            DocumentPropsHolder.PropsList.Add(Comment);

            ColorProps color = new ColorProps();

            color.FillType = ColorProps.FType.OnlyFill;
            color.SetTitle("Цвет");
            color.GetCurrentColorDelegate = GetCurrentColor;
            color.SetColorDelegate = SetColor;

            DocumentPropsHolder.VisualPropsList.Add(color);

        }

        public Color GetCurrentColor() => ((SolidColorBrush)(DocColor)).Color;
        public void SetColor(Color c) => DocColor = new SolidColorBrush(c);

        private string GetDocLable()
        {
            return DocLable;
        }

        private void SetDocLable(string lb)
        {
            DocLable = lb;
        }

        private string doc_lable;

        public string DocLable
        {
            get
            {
                return doc_lable;
            }
            set
            {
                doc_lable = value;
                OnPropertyChanged("DocLable");
            }
        }

        public string GetDocName() => doc_model.Name;
        public void SetDocName(string name)
        {
            doc_model.Name = name;
            DocLable = name;
        }

        public string GetDocOwner() => doc_model.Owner;
        public void SetDocOwner(string owner) => doc_model.Owner = owner;

        public DateTime GetSingDate() => doc_model.Date;
        public void SetSingDate(DateTime date) => doc_model.Date = date;

        public string GetComment() => doc_model.Comment;
        public void SetComment(string comment) => doc_model.Comment = comment;


        public PropsHolder DocumentPropsHolder { get; } = new PropsHolder(); 

        private DocumentModel doc_model;

        public Int32 Id;

        public string DocName
        {
            get
            {
                return doc_model.Name;
            }
            
        }

        private Brush doc_color;

        public Brush DocColor
        {
            get { return doc_color; }
            set
            {
                doc_color = value;
                OnPropertyChanged("DocColor");
            }
        }

        public DocumentUnit()
        {
            DocColor = Brushes.LightGreen;
            doc_model = new DocumentModel();
            doc_model.Name = " ";
            doc_model.Owner = " ";
            doc_model.Date = new DateTime(2009, 02, 15);
            DocLable = "Документ";
            CreatePropertyHolders();

           // Id = GlobalStore.CurrentIVSM.GetDocId();
        }

        public DocumentUnit(string Title)
        {
       //     Id = GlobalStore.CurrentIVSM.GetDocId();

            DocColor = Brushes.LightGreen;
            doc_model = new DocumentModel();
            doc_model.Name = " ";
            doc_model.Owner = " ";
            doc_model.Date = new DateTime(2009, 02, 15);
            DocLable = Title;
            CreatePropertyHolders();
        }

        public DocumentModel GetModel() => doc_model;

        public Node GetDomNode() => DefDocumentDom.Get().CreateRootNode(this);
    }
}
