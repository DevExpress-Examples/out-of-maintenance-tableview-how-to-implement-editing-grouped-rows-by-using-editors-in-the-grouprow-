// Developer Express Code Central Example:
// How to create Checkable Grouping in GridControl
// 
// The example demonstrates how to add the CheckBox functionality to each GroupRow
// in TableView. The basic idea is to allow a user to easily check or uncheck the
// necessary group of items in GridControl.
// 
// The functionality is realized as
// behavior in the CheckableGroupBehavior class, which can be attached to
// GridControl. It automatically sets GroupValueTemplate using the GroupCheckBox
// class, which is inherited from the CheckBox class. The CheckableGroupBehavior's
// CheckableProperty must be set and has to contain the name of the property in a
// row data object, which will be used to check items. The property has to be of
// the Boolean type.
// 
// You can easily add the same functionality to your project by
// using the CheckableGroupBehavior class and attaching it as behavior to your
// GridControl.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=T127563

using DevExpress.Xpf.Grid;
using DevExpress.Mvvm.UI.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace DevExpress.Example03 {

    public class CheckableGroupBehavior : Behavior<GridControl> {

        #region Fields

        protected bool _LocalSet;

        #endregion Fields

        #region Checkable

        public string CheckableProperty {
            get { return (string)GetValue(CheckablePropertyProperty); }
            set { SetValue(CheckablePropertyProperty, value); }
        }

        public static readonly DependencyProperty CheckablePropertyProperty =
            DependencyProperty.Register("CheckableProperty", typeof(string), typeof(CheckableGroupBehavior), new PropertyMetadata(string.Empty));

        #endregion Checkable

        #region CheckList

        public static Dictionary<int, bool?> GetCheckList(DependencyObject obj) {
            return (Dictionary<int, bool?>)obj.GetValue(CheckListProperty);
        }

        public static void SetCheckList(DependencyObject obj, Dictionary<int, bool?> value) {
            obj.SetValue(CheckListProperty, value);
        }

        public static readonly DependencyProperty CheckListProperty =
            DependencyProperty.RegisterAttached("CheckList", typeof(Dictionary<int, bool?>), typeof(CheckableGroupBehavior), new PropertyMetadata(new Dictionary<int, bool?>(), IsCheckListPropertyChanged));

        #endregion CheckList

        public static event Action<DependencyObject, DependencyPropertyChangedEventArgs> IsCheckListChanged;
        
        protected static void IsCheckListPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
            if(IsCheckListChanged != null) {
                IsCheckListChanged(obj, args);
            }
        }

        protected void OnIsCheckListChanged(object sender, DependencyPropertyChangedEventArgs args) {
            if(args.NewValue == null) {
                return;
            }

            if(this._LocalSet) { return; }

            this._LocalSet = true;
            var grid = this.AssociatedObject;
            var table = this.AssociatedObject.View as TableView;
            var rowHandle = grid.GetSelectedRowHandles().First();
            var checkStates = (args.NewValue as Dictionary<int, bool?>);
            int groupLevel = grid.GetRowLevelByRowHandle(rowHandle);
            bool? value = checkStates.Keys.Contains(groupLevel) ? checkStates[groupLevel] : false;
            this.WorkOutGroup(rowHandle, value);
            this._LocalSet = false;
        }
        
        protected override void OnAttached() {
            base.OnAttached();
            if(this.AssociatedType != typeof(GridControl) || this.CheckableProperty == string.Empty) {
                return;
            }
            
            this.AssociatedObject.Loaded += GridLoaded;
        }

        protected void AttachItem(object obj) {
            var iNotifyPropertyChanged = (INotifyPropertyChanged)obj;
            if(iNotifyPropertyChanged != null) {
                iNotifyPropertyChanged.PropertyChanged += this.PropertyChanged;
            }
        }

        protected void GridLoaded(object sender, RoutedEventArgs e) {
            if(this.AssociatedObject.View.GetType() == typeof(TableView)) {
                (this.AssociatedObject.View as TableView).GroupValueTemplate = this.GetGroupValueTemplate();
            }

            if(this.AssociatedObject.GroupCount > 0) {
                this.AssociatedObject_EndGrouping(null, null);
            }

            var items = this.AssociatedObject.ItemsSource as IEnumerable<object>;
            if(items != null) {
                foreach(var item in items) {
                    this.AttachItem(item);
                }
            }

            INotifyCollectionChanged collection = (INotifyCollectionChanged)this.AssociatedObject.ItemsSource;
            if(collection != null) {
                collection.CollectionChanged += CollectionChanged;
            }

            IsCheckListChanged += OnIsCheckListChanged;
            this.AssociatedObject.EndGrouping += AssociatedObject_EndGrouping;
            (this.AssociatedObject.View as TableView).CellValueChanged += GridCellValueChanged;
            (this.AssociatedObject.View as TableView).CellValueChanging += CellValueChanging;
        }

        void AssociatedObject_EndGrouping(object sender, RoutedEventArgs e) {
            if(this.AssociatedObject.GroupCount == 0) { return; }

            var res = this.GetAllRowHandles();
            List<int> parents = new List<int>();
            int j = res.Count;
            while(--j >= 0) {
                var parent = this.AssociatedObject.GetParentRowHandle(res[j]);
                if(!parents.Contains(parent)) {
                    parents.Add(parent);
                    do {
                        parent = this.AssociatedObject.GetParentRowHandle(parent);
                        if(!parents.Contains(parent) && this.AssociatedObject.IsValidRowHandle(parent)) {
                            parents.Add(parent);
                        }

                    } while(this.AssociatedObject.IsValidRowHandle(parent));
                }
            }

            int g = this.AssociatedObject.GroupCount;
            while(--g >= 0) {
                var levelItems = parents.Where(c => this.AssociatedObject.GetRowLevelByRowHandle(c) == g);
                if(levelItems == null) { continue; }
                foreach(var item in levelItems) {
                    var b = this.GetRowChildrenState(item);
                    var dic = (Dictionary<int, bool?>)this.AssociatedObject.GetRowState(item).GetValue(CheckableGroupBehavior.CheckListProperty);
                    dic[g] = b;
                    this._LocalSet = true;
                    this.AssociatedObject.GetRowState(item).SetValue(CheckableGroupBehavior.CheckListProperty, new Dictionary<int, bool?>(dic));
                    this._LocalSet = false;
                }
            }
        }

        protected List<int> GetAllRowHandles() {
            List<int> result = new List<int>();
            foreach(var item in this.AssociatedObject.ItemsSource as IEnumerable<object>) {
                int handle = this.AssociatedObject.FindRowByValue("", item);
                result.Add(handle);
            }

            return result;
        }

        protected void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            switch(e.Action) {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    foreach(var item in e.NewItems) {
                        this.AttachItem(item);
                    }
                    break;
                default:
                    break;
            }
        }

        public void PropertyChanged(object obj, PropertyChangedEventArgs args) {
            if(!this._LocalSet && args.PropertyName == this.CheckableProperty) {
                var grid = this.AssociatedObject;
                int rowHandle = grid.FindRowByValue("", obj);
                this.WorkOutCell(rowHandle);
            }
        }

        protected void CellValueChanging(object sender, CellValueChangedEventArgs e) {
            var grid = this.AssociatedObject;
            grid.View.PostEditor();
        }

        protected void GridCellValueChanged(object sender, CellValueChangedEventArgs e) {
            if(e.Column.FieldName == this.CheckableProperty && !this._LocalSet) {
                this._LocalSet = true;
                this.WorkOutCell(e.RowHandle);
                this._LocalSet = false;
            }
        }

        protected void WorkOutCell(int rowHandle) {
            var grid = this.AssociatedObject;
            int groupRowHandle = grid.GetParentRowHandle(rowHandle);
            if(grid.IsValidRowHandle(groupRowHandle) && grid.IsGroupRowHandle(groupRowHandle)) {
                bool? value = this.GetRowChildrenState(groupRowHandle);
                var dic = new Dictionary<int, bool?>(this.GetUpdatedDic(groupRowHandle, value));
                grid.GetRowState(groupRowHandle).SetCurrentValue(CheckableGroupBehavior.CheckListProperty, dic);
                this.WorkOutCell(groupRowHandle);
            }
        }

        protected bool? GetRowChildrenState(int rowHandle) {
            var grid = this.AssociatedObject;
            bool? result = null, b;
            if(grid.IsValidRowHandle(rowHandle) && grid.IsGroupRowHandle(rowHandle)) {
                int count = grid.GetChildRowCount(rowHandle);
                int i = -1;
                while(++i < count) {
                    var childHandle = grid.GetChildRowHandle(rowHandle, i);
                    int childLevel = grid.GetRowLevelByRowHandle(childHandle);
                    if(grid.IsGroupRowHandle(childHandle)) {
                        var dic = (Dictionary<int, bool?>)grid.GetRowState(childHandle).GetValue(CheckableGroupBehavior.CheckListProperty);
                        b = dic[childLevel];
                    } else {
                        var column = grid.Columns.Where(c => c.FieldName == this.CheckableProperty).FirstOrDefault();
                        if(column != null) {
                            b = (bool)grid.GetCellValue(childHandle, column);
                        } else {
                            b = false;
                        }
                    }

                    if(b.HasValue) {
                        if(result.HasValue) {
                            if(result.Value ^ b.Value) {
                                return null;
                            } 
                        } else {
                            result = b;
                        }
                    } else {
                        return null;
                    }
                }
            }

            return result;
        }

        protected Dictionary<int, bool?> GetUpdatedDic(int rowHandle, bool? value) {
            Dictionary<int, bool?> result = null;
            if(this.AssociatedObject.IsValidRowHandle(rowHandle)){
                int groupLevel = this.AssociatedObject.GetRowLevelByRowHandle(rowHandle);
                var rowState = this.AssociatedObject.GetRowState(rowHandle);
                result = (Dictionary<int, bool?>)rowState.GetValue(CheckableGroupBehavior.CheckListProperty);
                result[groupLevel] = value;
            }

            return result;
        }

        protected void WorkOutGroup(int rowHandle, bool? value) {
            var grid = this.AssociatedObject;
            var parentRowState = grid.GetRowState(rowHandle);
            if(grid.IsValidRowHandle(rowHandle) && grid.IsGroupRowHandle(rowHandle)) {
                int count = grid.GetChildRowCount(rowHandle);
                int i = -1;
                while(++i < count) {
                    var childHandle = grid.GetChildRowHandle(rowHandle, i);
                    if(grid.IsGroupRowHandle(childHandle)) {
                        var rowState = this.AssociatedObject.GetRowState(childHandle);
                        rowState.SetCurrentValue(CheckableGroupBehavior.CheckListProperty, new Dictionary<int, bool?>(this.GetUpdatedDic(childHandle, value)));
                        this.WorkOutGroup(childHandle, value);
                    } else {
                        var column = grid.Columns.Where(c => c.FieldName == this.CheckableProperty).FirstOrDefault();
                        if(column != null) {
                            grid.SetCellValue(childHandle, column, value);
                        }
                    }
                }

                this.WorkOutCell(rowHandle);
            }
        }

        #region Template For GroupRowValue

        protected DataTemplate GetGroupValueTemplate() {
            string xamlDataTemplate = "<DataTemplate>";
            xamlDataTemplate += "<self:GroupCheckBox Content=\"{Binding Value}\" Margin=\"8,0,0,0\"  CheckStates=\"{Binding Path=RowData.RowState.(self:CheckableGroupBehavior.CheckList), Mode=TwoWay}\"/>";
            xamlDataTemplate += "</DataTemplate>";
            ParserContext context = new ParserContext();
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("self", "clr-namespace:DevExpress.Example03;assembly=DevExpress.Example03");
            DataTemplate template = (DataTemplate)XamlReader.Parse(xamlDataTemplate, context);
            return template;
        }

        #endregion Template for GroupRowValue
    }
}
