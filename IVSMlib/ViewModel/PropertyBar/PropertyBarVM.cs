using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using IVSMlib.ViewModel.ControlBar.Items;
using IVSMlib.PropsHolders;
using IVSMlib.PropsHolders.VisualProps;
using System.Windows;
using System.Windows.Input;

namespace IVSMlib.ViewModel
{
    public class PropertyBarVM : INotifyPropertyChanged
    {
        private const Int32 StructPropsPage = 0;
        private const Int32 VisualPropsPage = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<Item> CurrentItemList;
        private List<Item> CurrentVisualItemList;

        private bool StrucPageIsDraw;
        private bool ViuslPageIsDraw;

        private Int32 active_tab_page;

        public Int32 ActiveTabPage
        {
            get
            {
                return active_tab_page;
            }
            set
            {
                active_tab_page = value;
                ChangeTabIndex(active_tab_page);
                OnPropertyChanged("ActiveTabPage");
            }
        }

        private PropsHolder CurrentHolder;

        private Canvas DataPropsField;
        private Canvas VisualPropsField;

        

        private Visibility header_bar_vis;

        public Visibility HeaderBarVis
        {
            get
            {
                return header_bar_vis;
            }
            set
            {
                header_bar_vis = value;
                OnPropertyChanged("HeaderBarVis");
            }
        }


        private string lable_text;

        public string LableText
        {
            get { return lable_text; }
            set
            {
                lable_text = value;
                OnPropertyChanged("LableText");
            }
        }

        private double header_bar_height;

        public double HeaderBarHeight
        {
            get
            {
                return header_bar_height;
            }
            set
            {
                header_bar_height = value;
                OnPropertyChanged("HeaderBarHeight");
            }
        }

        public PropertyBarVM(Canvas _filed, Canvas _filed_visual)
        {
            DataPropsField = _filed;

            VisualPropsField = _filed_visual;

            CurrentItemList = new List<Item>();
            CurrentVisualItemList = new List<Item>();

            ActiveTabPage = 0;

            HeaderBarHeight = 70;

            StrucPageIsDraw = false;
            ViuslPageIsDraw = false;
        }


        private void MakeHeader(PropsHolder props_holder)
        {
            switch(props_holder.OwnerType)
            {
                case PropsHolder.Type.Player:
                    LableText = "Участник процесса";
                    break;
                case PropsHolder.Type.Document:
                    LableText = "Документ";
                    break;
                case PropsHolder.Type.Action:
                    LableText = "Действие";
                    break;
                case PropsHolder.Type.PathLine:
                    LableText = "Перемещение";
                    break;
                case PropsHolder.Type.Condition:
                    LableText = "Действие с условием";
                    break;
                case PropsHolder.Type.Problem:
                    LableText = "Проблема";
                    break;
                case PropsHolder.Type.Decision:
                    LableText = "Решение";
                    break;
                case PropsHolder.Type.Default:
                    LableText = "";
                    break;
            }

            //if (props_holder.GetLableCallback != null)
            //{
            //    ItemText = props_holder.GetLableCallback();
            //}
            //else
            //{
            //    ItemText = "";
            //}
        }

        private double DrawStringProps(StringProps sp, double disp)
        {
            if (sp.PropsEditType == StringProps.EditType.Line)
            {
                TextBoxItem item = new TextBoxItem();
                //  item.SetWidth(DataPropsField.ActualWidth);
                item.SetWidth(DataPropsField.Width);
                item.SetTop(disp);
                item.BindingProps(sp);
                foreach (UIElement el in item.GetUIElement())
                {
                    DataPropsField.Children.Add(el);

                }
                CurrentItemList.Add(item);
                return item.GetHeight();
            }
            if (sp.PropsEditType == StringProps.EditType.MultiLine)
            {
                TextAreaItem item = new TextAreaItem();
                item.SetWidth(DataPropsField.Width);
                item.SetTop(disp);
                item.BindingProps(sp);
                foreach (UIElement el in item.GetUIElement())
                {
                    DataPropsField.Children.Add(el);

                }
                CurrentItemList.Add(item);
                return item.GetHeight();
            }
            return 0;
        }

        private double DrawStringPropsVisual(StringProps sp, double disp)
        {
            if (sp.PropsEditType == StringProps.EditType.Line)
            {
                TextBoxItem item = new TextBoxItem();
                //  item.SetWidth(DataPropsField.ActualWidth);
                item.SetWidth(DataPropsField.Width);
                item.SetTop(disp);
                item.BindingProps(sp);
                foreach (UIElement el in item.GetUIElement())
                {
                    VisualPropsField.Children.Add(el);

                }
                CurrentVisualItemList.Add(item);
                return item.GetHeight();
            }
            if (sp.PropsEditType == StringProps.EditType.MultiLine)
            {
                TextAreaItem item = new TextAreaItem();
                item.SetWidth(DataPropsField.Width);
                item.SetTop(disp);
                item.BindingProps(sp);
                foreach (UIElement el in item.GetUIElement())
                {
                    VisualPropsField.Children.Add(el);

                }
                CurrentVisualItemList.Add(item);
                return item.GetHeight();
            }
            return 0;
        }

        private void DrawStructProps(List<Props> struct_props_list) 
        {
            double disp = 1;
            foreach (Props props in struct_props_list)
            {
                if (props is StringProps sp)
                {
                    disp += DrawStringProps(sp, disp);
                }
                if (props is TimeProps tp)
                {
                    TimeItem t_item = new TimeItem();
                    t_item.SetWidth(DataPropsField.Width);
                    t_item.SetTop(disp);
                    t_item.BindingProps(tp);
                    foreach (UIElement el in t_item.GetUIElement())
                    {
                        DataPropsField.Children.Add(el);

                    }
                    CurrentItemList.Add(t_item);
                    disp += t_item.GetHeight();
                }
                if (props is DateProps dp)
                {
                    DataItem t_item = new DataItem();
                    t_item.SetWidth(DataPropsField.Width);
                    t_item.SetTop(disp);
                    t_item.BindingProps(dp);
                    foreach (UIElement el in t_item.GetUIElement())
                    {
                        DataPropsField.Children.Add(el);

                    }
                    CurrentItemList.Add(t_item);
                    disp += t_item.GetHeight();
                }
                if (props is DocumentListProps dl)
                {
                    DocumentItem d_item = new DocumentItem();
                    d_item.SetWidth(DataPropsField.Width);
                    d_item.SetTop(disp);
                    d_item.BindingProps(dl);
                    foreach (UIElement el in d_item.GetUIElement())
                    {
                        DataPropsField.Children.Add(el);

                    }
                    CurrentItemList.Add(d_item);
                    disp += d_item.GetHeight();
                }
            }
            DataPropsField.Height = disp + 30;
        }

        private void DrawVisualProps(List<Props> visual_props_list)
        {
            double disp = 1;
            foreach (Props props in visual_props_list)
            {
                if(props is ColorProps color_p)
                {
                    if (color_p.FillType == ColorProps.FType.OnlyFill)
                    {
                        ColorItem color_item = new ColorItem();
                        color_item.SetWidth(VisualPropsField.Width);
                        color_item.SetTop(disp);
                        color_item.BindingProps(props);
                        foreach (UIElement el in color_item.GetUIElement())
                        {
                            VisualPropsField.Children.Add(el);

                        }
                        CurrentVisualItemList.Add(color_item);
                        disp += color_item.GetHeight();
                    }
                    if(color_p.FillType == ColorProps.FType.Transparant)
                    {
                        TColorItem color_item = new TColorItem();
                        color_item.SetWidth(VisualPropsField.Width);
                        color_item.SetTop(disp);
                        color_item.BindingProps(props);
                        foreach (UIElement el in color_item.GetUIElement())
                        {
                            VisualPropsField.Children.Add(el);

                        }
                        CurrentVisualItemList.Add(color_item);
                        disp += color_item.GetHeight();
                    }
                }
                if(props is StringProps sp)
                {
                    disp += DrawStringPropsVisual(sp, disp);

                }
                if(props is DropListProps dp)
                {
                    IVSMComboBoxItem combo_item = new IVSMComboBoxItem();
                    combo_item.SetWidth(VisualPropsField.Width);
                    combo_item.SetTop(disp);
                    combo_item.BindingProps(dp);

                    foreach (UIElement el in combo_item.GetUIElement())
                    {
                        VisualPropsField.Children.Add(el);

                    }
                    CurrentVisualItemList.Add(combo_item);
                    disp += combo_item.GetHeight();
                }
            }
            VisualPropsField.Height = disp + 30;
        }

        private void ChangeTabIndex(Int32 index)
        {
            if(index == StructPropsPage && !StrucPageIsDraw)
            {
                if(CurrentHolder != null)
                {
                    DrawStructProps(CurrentHolder.PropsList);
                    StrucPageIsDraw = true;
                }
            }

            if (index == VisualPropsPage && !ViuslPageIsDraw)
            {
                if (CurrentHolder != null)
                {
                    DrawVisualProps(CurrentHolder.VisualPropsList);
                    ViuslPageIsDraw = true;
                }
            }
        }

        public void Clear()
        {
            StrucPageIsDraw = false;
            ViuslPageIsDraw = false;

            DataPropsField.Children.Clear();
            CurrentItemList.Clear();
            VisualPropsField.Children.Clear();
            CurrentVisualItemList.Clear();
            CurrentHolder = null;

            LableText = "";
        }

        private void BuildNewList(PropsHolder props_holder)
        {
            StrucPageIsDraw = false;
            ViuslPageIsDraw = false;

            DataPropsField.Children.Clear();
            CurrentItemList.Clear();
            VisualPropsField.Children.Clear();
            CurrentVisualItemList.Clear();
            CurrentHolder = props_holder;
            MakeHeader(CurrentHolder);


            if (ActiveTabPage == StructPropsPage)
            {
                DrawStructProps(CurrentHolder.PropsList);
                StrucPageIsDraw = true;
                
            }
            if (ActiveTabPage == VisualPropsPage)
            {
                DrawVisualProps(CurrentHolder.VisualPropsList);
                ViuslPageIsDraw = true;
            }

        }

        public void BuildPropertyList(PropsHolder props_holder)
        {

            BuildNewList(props_holder);
       
        }
    }
    }
