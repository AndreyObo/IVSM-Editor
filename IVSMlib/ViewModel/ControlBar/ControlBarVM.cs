using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

using IVSMlib.Windows;
using IVSMlib.Global;
using IVSMlib.Types;
using System.Windows;

namespace IVSMlib.ViewModel
{
    public class ControlBarVM: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private MainTableVM mainTable;

        public void SetTAbleVM(MainTableVM tb)
        {
            mainTable = tb;
        }

        private bool _actionbtn;
        private bool _condition_btn;

        private string t_name;

        public string TableName
        {
            get
            {
                return t_name;
            }
            set
            {
                t_name = value;
                mainTable.SetMapTitle(t_name);
                OnPropertyChanged("TableName");
            }
        }

        private double c_height;

        public double CHeight
        {
            get
            {
                return c_height;
            }
            set
            {
                c_height = value;
            }
        }

        private string mode;

        public string ItemMode
        {
            get
            {
                return mode;
            }
            set
            {
                Console.WriteLine(value);
                mode = value;
                if(value =="Operation")
                {
                    GlobalStore.CreatedItem = GlobalStore.TableItems.Action;
                }
                if(value == "Condition")
                {
                    GlobalStore.CreatedItem = GlobalStore.TableItems.Condition;
                }
                OnPropertyChanged("ItemMode");
            }
        }

        private bool edit_m;

        public bool EditMode
        {
            get
            {
                return edit_m;
            }
            set
            {
                edit_m = value;
                SwitchClick(value);
                OnPropertyChanged("EditMode");
            }
        }

        //------------------------------------------------------------

        public bool ActionBtnState
        {
            get { return _actionbtn; }
            set
            {
                if(value == true)
                {
                    ConditionBtnState = false;
                    GlobalStore.CreatedItem = GlobalStore.TableItems.Action;
                }

                _actionbtn = value;
                OnPropertyChanged("ActionBtnState");

            }
        }

        public bool ConditionBtnState
        {
            get { return _condition_btn; }
            set
            {
                if(value == true)
                {
                    ActionBtnState = false;
                    GlobalStore.CreatedItem = GlobalStore.TableItems.Condition;
                }
                _condition_btn = value;
                OnPropertyChanged("ConditionBtnState");
            }
        }
        public ICommand NewMapCommand { get; set; }

        public ICommand LogoClickCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand ColorButtonCommand { get; set; }

        public ICommand DeleteButtonCommand { get; set; }

        public ICommand DeleteColumnCommand { get; set; }

        public ICommand DeleteRowCommand { get; set; }

        public ICommand TableSizeCommand { get; set; }

        public ICommand AlightColumnCommand { get; set; }

        public ICommand AlightRowCommand { get; set; }

        public ICommand AlightTableCommand { get; set; }

        public ICommand RedirectLineCommand { get; set; }

        public ICommand AxisMegCommand { get; set; }

        public ICommand ProblemCreateCommand { get; set; }

        public ICommand DecisionCreateCommand { get; set; }

        public ICommand TextLableCreateCommand { get; set; }
        public ICommand InsertRowColumnCommand { get; set; }
        public ICommand DelRowColumnCommand { get; set; }
        public ICommand SaveDocCommand { get; set; }
        public ICommand OpenDocCommand { get; set; }
        public ICommand ClearTableCommand { get; set; }
        public ICommand SaveImgCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public ICommand HelpCommand { get; set; }

        private DocumentBarVM DocBarVm;

        private bool SwicthBlock;

        public ControlBarVM()
        {
            LogoClickCommand = new Command(LogoClick);

            t_name = "Новая карта";
            _actionbtn = true;
            _condition_btn = false;

            SwicthBlock = false;

            GlobalStore.CreatedItem = GlobalStore.TableItems.Action;

            SaveCommand = new Command(SaveClick);

            NewMapCommand = new Command(NewMapClick);

            ColorButtonCommand = new Command(ColorButtonClick);
            DeleteButtonCommand = new Command(DeleteButtonClick);
            DeleteColumnCommand = new Command(DeleteColumnClick);
            DeleteRowCommand = new Command(DeleteRowClick);
            TableSizeCommand = new Command(TableSizeClick);

            AlightColumnCommand = new Command(AlightColumnClick);
            AlightRowCommand = new Command(AlightRowClick);

            AlightTableCommand = new Command(AlightTableClick);

            RedirectLineCommand = new Command(RedirectLineClick);

            AxisMegCommand = new Command(AxisMegClick);

            ProblemCreateCommand = new Command(ProblemCreateClick);

            DecisionCreateCommand = new Command(DecisionCreateClick);
            TextLableCreateCommand = new Command(TextLableCreateClick);
            InsertRowColumnCommand = new Command(InsertRowColumnClick);
            DelRowColumnCommand = new Command(DelRowColumnClick);
            SaveDocCommand = new Command(SaveDocClick);
            OpenDocCommand = new Command(OpenDocClick);
            ClearTableCommand = new Command(ClearTableClick);
            SaveImgCommand = new Command(SaveImgClick);
            CloseCommand = new Command(CloseClick);

            HelpCommand = new Command(HelpClick);
        }

        private void NewMapClick()
        {
            mainTable.NewMap();
        }

        private void SaveClick()
        {
            mainTable.Save();
        }

        private void LogoClick()
        {
           
        }

        private void SwitchClick(bool IsEdit)
        {
          //  edit_m = value;
            if(SwicthBlock)
            {
                SwicthBlock = false;
                return;
            }

            if (IsEdit == true)
            {
                mainTable.SetMode(Construct.MapConstructor.Mode.Edit);
            }
            else
            {
                mainTable.SetMode(Construct.MapConstructor.Mode.View);
            }
        }

        public void SetSwithPos(Construct.MapConstructor.Mode mode)
        {
            SwicthBlock = true;
            if(mode == Construct.MapConstructor.Mode.Edit)
            {
                EditMode = true;
            }
            else if(mode == Construct.MapConstructor.Mode.View)
            {
                EditMode = false;
            }
        }

        private void SaveImgClick()
        {
            mainTable.SaveAsImage();
        }

        private void ClearTableClick()
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Вы хотите очистить таблицу?", "Очистка", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                mainTable.ClearTableClick();
            }
            
        }

        private void CloseClick()
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Вы хотите выйти из приложения?", "Закрыть", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                
            }
        }

        private void OpenDocClick()
        {
            mainTable.Open();
        }

        private void SaveDocClick()
        {
            mainTable.SaveAs();
        }

        private void DelRowColumnClick()
        {
            mainTable.DeleteRowColumn();
        }

        private void InsertRowColumnClick()
        {
            mainTable.InsertRowColumnMode();
        }

        private void DecisionCreateClick()
        {
            mainTable.CreateDecisionMark();
        }

        private void TextLableCreateClick()
        {
            mainTable.CreateTextLableMark();
        }

        private void ProblemCreateClick()
        {
            mainTable.CreateProblemMark();
        }

        private void AxisMegClick()
        {
            WSetAxisMeg meg_w = new WSetAxisMeg(mainTable.GetActionAxisMeg(), mainTable.GetWasteAxisMeg(), mainTable.GetMoveAxisMeg());
            meg_w.SetTimesCallback = SetAxisTimeMeg;
            meg_w.ShowDialog();
        }

        private void HelpClick()
        {
            WHelp help_window = new WHelp();
            help_window.Show();
        }

        private void SetAxisTimeMeg(Time.Type act_type, Time.Type wst_type, Time.Type mv_type)
        {
            mainTable.SetAxisTimeMeg(act_type, wst_type, mv_type);
        }

        public void SetDocBar(DocumentBarVM db)
        {
            DocBarVm = db;
        }

        private void RedirectLineClick()
        {

        }

        private void AlightColumnClick()
        {
            mainTable.AlightColumnSize();
        }

        private void AlightRowClick()
        {
            mainTable.AlightRowHeight();
        }

        private void AlightTableClick()
        {
            mainTable.AlightTableCellSize();
        }

        private void TableSizeClick()
        {
            WSetTableSize win = new WSetTableSize(mainTable.TableWidth, mainTable.TableHeigth);
            win.WinParamCallback = SetTableSizeParam;
            win.ShowDialog();
        }
        
        public void SetTableSizeParam(double widht, double height, bool ChangeCellSize)
        {
            mainTable.SetTableSize(widht, height, ChangeCellSize);
        }

        private void DeleteRowClick()
        {
            mainTable.DeleteLastRow();
        }

        private void DeleteColumnClick()
        {
            mainTable.DeleteLastColl();
        }

        private void DeleteButtonClick()
        {
            mainTable.DeleteCommand();
        }

        private void ColorButtonClick()
        {
            EditMode = false;
            Console.WriteLine("Color clik");
        }

    }
}
