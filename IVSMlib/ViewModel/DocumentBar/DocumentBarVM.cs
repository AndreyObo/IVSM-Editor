using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSMlib.ViewModel.Units;
using IVSMlib.Link;
using System.Windows.Input;
using System.Windows.Media;
using IVSMlib.Global;
using System.Windows;

namespace IVSMlib.ViewModel
{
    public class DocumentBarVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddDocCommand { get; set; }
        public ICommand DelDocCommand { get; set; }

        public ObservableCollection<DocumentUnit> DocumentUnitList { get; set; }

        private DocumentUnit selected_document;

        public DocumentUnit SelectedUnit
        {
            get
            {
                return selected_document;
            }
            set
            {
                selected_document = value;
                if (selected_document != null)
                {
                    PropertyBar.BuildPropertyList(selected_document.DocumentPropsHolder);
                    MainTable.UnselectCell();
                }
                OnPropertyChanged("SelectedUnit");
            }
        }

        private PropertyBarVM PropertyBar;
        private MainTableVM MainTable;

        public DocumentBarVM()
        {
            AddDocCommand = new Command(add_doc);
            DelDocCommand = new Command(del_doc);

            DocumentUnitList = new ObservableCollection<DocumentUnit>();

            //for (int i = 1; i <=3; i++)
            //{
            //    string s = "Документ_" + i.ToString();
            //    DocumentUnitList.Add(new DocumentUnit(s));
            //}
           
        //    DocumentUnitList.Add(new DocumentUnit());
          //  DocumentUnitList.Add(new DocumentUnit());

            DocConnector.CurrentDocList = DocumentUnitList;
        }

        private void add_doc()
        {

            int num = DocumentUnitList.Count;

            bool name_exist;

            string s;

            do
            {
                name_exist = false;
                s = "Документ_" + num.ToString();
                foreach (DocumentUnit doc in DocumentUnitList)
                {
                    if(doc.DocLable == s)
                    {
                        name_exist = true;
                        num++;
                        break;
                    }
                }
            } while (name_exist == true);

            DocumentUnit document = new DocumentUnit(s);
            document.Id =  GlobalStore.CurrentIVSM.GetDocId();

            DocumentUnitList.Add(document);
        }

        private void del_doc()
        {
            if(SelectedUnit !=null)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Вы хотите удалить документ - \""+SelectedUnit.DocName+"\"", "Удаление документа", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MainTable.MapConstructorInstance.RemoveDocumentFromAll(SelectedUnit);
                    DocumentUnitList.Remove(SelectedUnit);
                    SelectedUnit = null;
                    PropertyBar.Clear();
                }
               
            }
        }

        public void SetPropertyBarVM(PropertyBarVM propery_bar_vm)
        {
            PropertyBar = propery_bar_vm;
        }

        public void SetMainTableVM(MainTableVM tb)
        {
            MainTable = tb;
        }

        public void ChangeColor(Color color)
        {
            if (SelectedUnit != null)
            {
                SelectedUnit.DocColor = new SolidColorBrush(color);
            }
        }
    }
}
