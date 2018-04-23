Imports DevExpress.Xpf.Editors
Imports DevExpress.Xpf.Editors.Settings
Imports DevExpress.Xpf.Grid
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Namespace DevExpress.Example03
    Public Class EditorWrapper
        Inherits ContentControl

        Public Shared ReadOnly ColumnProperty As DependencyProperty = DependencyProperty.Register("Column", GetType(ColumnBase), GetType(EditorWrapper), New PropertyMetadata(Nothing, AddressOf OnColumnChanged))
        Public Shared ReadOnly RowHandleProperty As DependencyProperty = DependencyProperty.Register("RowHandle", GetType(Integer), GetType(EditorWrapper), New PropertyMetadata(GridControl.InvalidRowHandle, AddressOf OnRowHandleChanged))

        Private Shared Sub OnColumnChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            CType(d, EditorWrapper).OnColumnChanged()
        End Sub
        Private Shared Sub OnRowHandleChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            CType(d, EditorWrapper).OnRowHandleChanged()
        End Sub

        Public Property Column() As GridColumn
            Get
                Return DirectCast(GetValue(ColumnProperty), GridColumn)
            End Get
            Set(ByVal value As GridColumn)
                SetValue(ColumnProperty, value)
            End Set
        End Property
        Public Property RowHandle() As Integer
            Get
                Return DirectCast(GetValue(RowHandleProperty), Integer)
            End Get
            Set(ByVal value As Integer)
                SetValue(RowHandleProperty, value)
            End Set
        End Property

        Public ReadOnly Property Editor() As BaseEdit
            Get
                Return TryCast(Content, BaseEdit)
            End Get
        End Property
        Public ReadOnly Property View() As TableView
            Get
                Return TryCast(Column.View, TableView)
            End Get
        End Property

        Private Sub OnColumnChanged()
            Content = Column.ActualEditSettings.CreateEditor(EmptyDefaultEditorViewInfo.Instance)
            Editor.SetBinding(BaseEdit.ShowBorderProperty, New Binding("IsKeyboardFocusWithin") With { _
                .RelativeSource = New RelativeSource With {.Mode = RelativeSourceMode.Self} _
            })
            AddHandler Editor.PreviewKeyDown, AddressOf Editor_PreviewKeyDown
            AddHandler Editor.LostKeyboardFocus, AddressOf Editor_LostKeyboardFocus
        End Sub

        Private Sub OnRowHandleChanged()
            If Editor IsNot Nothing Then
                Editor.EditValue = Nothing
            End If
        End Sub
        Private Sub Editor_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs)
            If e.Key = System.Windows.Input.Key.Enter Then
                PostValue()
            End If
        End Sub
        Private Sub Editor_LostKeyboardFocus(ByVal sender As Object, ByVal e As System.Windows.Input.KeyboardFocusChangedEventArgs)
            PostValue()
        End Sub
        Private Sub PostValue()
            If Editor.EditValue Is Nothing Then
                Return
            End If
            PostValueCore(RowHandle)
            Editor.EditValue = Nothing
        End Sub
        Private Sub PostValueCore(ByVal groupRowHandle As Integer)
            Dim count As Integer = View.Grid.GetChildRowCount(groupRowHandle)
            For i As Integer = 0 To count - 1
                Dim handle = View.Grid.GetChildRowHandle(groupRowHandle, i)
                If handle < 0 Then
                    PostValueCore(handle)
                Else
                    View.Grid.SetCellValue(handle, Column.FieldName, Editor.EditValue)
                End If
            Next i
        End Sub
    End Class
End Namespace
