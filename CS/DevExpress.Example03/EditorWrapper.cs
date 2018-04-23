using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DevExpress.Example03 {
    public class EditorWrapper : ContentControl {
        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.Register("Column", typeof(ColumnBase), typeof(EditorWrapper), new PropertyMetadata(null, OnColumnChanged));
        public static readonly DependencyProperty RowHandleProperty =
            DependencyProperty.Register("RowHandle", typeof(int), typeof(EditorWrapper), new PropertyMetadata(GridControl.InvalidRowHandle, OnRowHandleChanged));

        private static void OnColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((EditorWrapper)d).OnColumnChanged();
        }
        private static void OnRowHandleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((EditorWrapper)d).OnRowHandleChanged();
        }

        public GridColumn Column {
            get { return (GridColumn)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }
        public int RowHandle {
            get { return (int)GetValue(RowHandleProperty); }
            set { SetValue(RowHandleProperty, value); }
        }

        public BaseEdit Editor { get { return Content as BaseEdit; } }
        public TableView View { get { return Column.View as TableView; } }

        private void OnColumnChanged() {
            Content = Column.ActualEditSettings.CreateEditor(EmptyDefaultEditorViewInfo.Instance);
            Editor.SetBinding(BaseEdit.ShowBorderProperty, new Binding("IsKeyboardFocusWithin") { RelativeSource = new RelativeSource { Mode = RelativeSourceMode.Self } });
            Editor.PreviewKeyDown += Editor_PreviewKeyDown;
            Editor.LostKeyboardFocus += Editor_LostKeyboardFocus;
        }

        private void OnRowHandleChanged() {
            if(Editor != null)
                Editor.EditValue = null;
        }
        void Editor_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(e.Key == System.Windows.Input.Key.Enter)
                PostValue();
        }
        void Editor_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e) {
            PostValue();
        }
        void PostValue() {
            if(Editor.EditValue == null)
                return;
            PostValueCore(RowHandle);
            Editor.EditValue = null;
        }
        void PostValueCore(int groupRowHandle) {
            int count = View.Grid.GetChildRowCount(groupRowHandle);
            for(int i = 0; i < count; i++) {
                var handle = View.Grid.GetChildRowHandle(groupRowHandle, i);
                if(handle < 0)
                    PostValueCore(handle);
                else
                    View.Grid.SetCellValue(handle, Column.FieldName, Editor.EditValue);
            }
        }
    }
}
