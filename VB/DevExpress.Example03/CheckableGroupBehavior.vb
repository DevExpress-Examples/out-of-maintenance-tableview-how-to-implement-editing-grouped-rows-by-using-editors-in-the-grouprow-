' Developer Express Code Central Example:
' How to create Checkable Grouping in GridControl
' 
' The example demonstrates how to add the CheckBox functionality to each GroupRow
' in TableView. The basic idea is to allow a user to easily check or uncheck the
' necessary group of items in GridControl.
' 
' The functionality is realized as
' behavior in the CheckableGroupBehavior class, which can be attached to
' GridControl. It automatically sets GroupValueTemplate using the GroupCheckBox
' class, which is inherited from the CheckBox class. The CheckableGroupBehavior's
' CheckableProperty must be set and has to contain the name of the property in a
' row data object, which will be used to check items. The property has to be of
' the Boolean type.
' 
' You can easily add the same functionality to your project by
' using the CheckableGroupBehavior class and attaching it as behavior to your
' GridControl.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=T127563

Imports DevExpress.Xpf.Grid
Imports DevExpress.Mvvm.UI.Interactivity
Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Linq
Imports System.Windows
Imports System.Windows.Markup

Namespace DevExpress.Example03

    Public Class CheckableGroupBehavior
        Inherits Behavior(Of GridControl)

        #Region "Fields"

        Protected _LocalSet As Boolean

        #End Region ' Fields

        #Region "Checkable"

        Public Property CheckableProperty() As String
            Get
                Return CStr(GetValue(CheckablePropertyProperty))
            End Get
            Set(ByVal value As String)
                SetValue(CheckablePropertyProperty, value)
            End Set
        End Property

        Public Shared ReadOnly CheckablePropertyProperty As DependencyProperty = DependencyProperty.Register("CheckableProperty", GetType(String), GetType(CheckableGroupBehavior), New PropertyMetadata(String.Empty))

        #End Region ' Checkable

        #Region "CheckList"

        Public Shared Function GetCheckList(ByVal obj As DependencyObject) As Dictionary(Of Integer, Boolean?)
            Return CType(obj.GetValue(CheckListProperty), Dictionary(Of Integer, Boolean?))
        End Function

        Public Shared Sub SetCheckList(ByVal obj As DependencyObject, ByVal value As Dictionary(Of Integer, Boolean?))
            obj.SetValue(CheckListProperty, value)
        End Sub

        Public Shared ReadOnly CheckListProperty As DependencyProperty = DependencyProperty.RegisterAttached("CheckList", GetType(Dictionary(Of Integer, Boolean?)), GetType(CheckableGroupBehavior), New PropertyMetadata(New Dictionary(Of Integer, Boolean?)(), AddressOf IsCheckListPropertyChanged))

        #End Region ' CheckList

        Public Shared Event IsCheckListChanged As Action(Of DependencyObject, DependencyPropertyChangedEventArgs)

        Protected Shared Sub IsCheckListPropertyChanged(ByVal obj As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
            RaiseEvent IsCheckListChanged(obj, args)
        End Sub

        Protected Sub OnIsCheckListChanged(ByVal sender As Object, ByVal args As DependencyPropertyChangedEventArgs)
            If args.NewValue Is Nothing Then
                Return
            End If

            If Me._LocalSet Then
                Return
            End If

            Me._LocalSet = True
            Dim grid = Me.AssociatedObject
            Dim table = TryCast(Me.AssociatedObject.View, TableView)
            Dim rowHandle = grid.GetSelectedRowHandles().First()
            Dim checkStates = (TryCast(args.NewValue, Dictionary)(Of Integer, Boolean?))
            Dim groupLevel As Integer = grid.GetRowLevelByRowHandle(rowHandle)
            Dim value? As Boolean = If(checkStates.Keys.Contains(groupLevel), checkStates(groupLevel), False)
            Me.WorkOutGroup(rowHandle, value)
            Me._LocalSet = False
        End Sub

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            If Me.AssociatedType IsNot GetType(GridControl) OrElse Me.CheckableProperty = String.Empty Then
                Return
            End If

            AddHandler Me.AssociatedObject.Loaded, AddressOf GridLoaded
        End Sub

        Protected Sub AttachItem(ByVal obj As Object)
            Dim iNotifyPropertyChanged = DirectCast(obj, INotifyPropertyChanged)
            If iNotifyPropertyChanged IsNot Nothing Then
                AddHandler iNotifyPropertyChanged.PropertyChanged, AddressOf Me.PropertyChanged
            End If
        End Sub

        Protected Sub GridLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Me.AssociatedObject.View.GetType() Is GetType(TableView) Then
                TryCast(Me.AssociatedObject.View, TableView).GroupValueTemplate = Me.GetGroupValueTemplate()
            End If

            If Me.AssociatedObject.GroupCount > 0 Then
                Me.AssociatedObject_EndGrouping(Nothing, Nothing)
            End If

            Dim items = TryCast(Me.AssociatedObject.ItemsSource, IEnumerable(Of Object))
            If items IsNot Nothing Then
                For Each item In items
                    Me.AttachItem(item)
                Next item
            End If

            Dim collection As INotifyCollectionChanged = DirectCast(Me.AssociatedObject.ItemsSource, INotifyCollectionChanged)
            If collection IsNot Nothing Then
                AddHandler collection.CollectionChanged, AddressOf CollectionChanged
            End If

            AddHandler IsCheckListChanged, AddressOf OnIsCheckListChanged
            AddHandler Me.AssociatedObject.EndGrouping, AddressOf AssociatedObject_EndGrouping
            AddHandler TryCast(Me.AssociatedObject.View, TableView).CellValueChanged, AddressOf GridCellValueChanged
            AddHandler TryCast(Me.AssociatedObject.View, TableView).CellValueChanging, AddressOf CellValueChanging
        End Sub

        Private Sub AssociatedObject_EndGrouping(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Me.AssociatedObject.GroupCount = 0 Then
                Return
            End If

            Dim res = Me.GetAllRowHandles()
            Dim parents As New List(Of Integer)()
            Dim j As Integer = res.Count
            j -= 1
            Do While j >= 0
                Dim parent = Me.AssociatedObject.GetParentRowHandle(res(j))
                If Not parents.Contains(parent) Then
                    parents.Add(parent)
                    Do
                        parent = Me.AssociatedObject.GetParentRowHandle(parent)
                        If (Not parents.Contains(parent)) AndAlso Me.AssociatedObject.IsValidRowHandle(parent) Then
                            parents.Add(parent)
                        End If

                    Loop While Me.AssociatedObject.IsValidRowHandle(parent)
                End If
                j -= 1
            Loop

            Dim g As Integer = Me.AssociatedObject.GroupCount
            g -= 1
            Do While g >= 0
                Dim levelItems = parents.Where(Function(c) Me.AssociatedObject.GetRowLevelByRowHandle(c) = g)
                If levelItems Is Nothing Then
                    g -= 1
                    Continue Do
                End If
                For Each item In levelItems
                    Dim b = Me.GetRowChildrenState(item)
                    Dim dic = CType(Me.AssociatedObject.GetRowState(item).GetValue(CheckableGroupBehavior.CheckListProperty), Dictionary(Of Integer, Boolean?))
                    dic(g) = b
                    Me._LocalSet = True
                    Me.AssociatedObject.GetRowState(item).SetValue(CheckableGroupBehavior.CheckListProperty, New Dictionary(Of Integer, Boolean?)(dic))
                    Me._LocalSet = False
                Next item
                g -= 1
            Loop
        End Sub

        Protected Function GetAllRowHandles() As List(Of Integer)
            Dim result As New List(Of Integer)()
            For Each item In TryCast(Me.AssociatedObject.ItemsSource, IEnumerable(Of Object))
                Dim handle As Integer = Me.AssociatedObject.FindRowByValue("", item)
                result.Add(handle)
            Next item

            Return result
        End Function

        Protected Sub CollectionChanged(ByVal sender As Object, ByVal e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)
            Select Case e.Action
                Case System.Collections.Specialized.NotifyCollectionChangedAction.Add, System.Collections.Specialized.NotifyCollectionChangedAction.Replace
                    For Each item In e.NewItems
                        Me.AttachItem(item)
                    Next item
                Case Else
            End Select
        End Sub

        Public Sub PropertyChanged(ByVal obj As Object, ByVal args As PropertyChangedEventArgs)
            If (Not Me._LocalSet) AndAlso args.PropertyName = Me.CheckableProperty Then
                Dim grid = Me.AssociatedObject
                Dim rowHandle As Integer = grid.FindRowByValue("", obj)
                Me.WorkOutCell(rowHandle)
            End If
        End Sub

        Protected Sub CellValueChanging(ByVal sender As Object, ByVal e As CellValueChangedEventArgs)
            Dim grid = Me.AssociatedObject
            grid.View.PostEditor()
        End Sub

        Protected Sub GridCellValueChanged(ByVal sender As Object, ByVal e As CellValueChangedEventArgs)
            If e.Column.FieldName = Me.CheckableProperty AndAlso (Not Me._LocalSet) Then
                Me._LocalSet = True
                Me.WorkOutCell(e.RowHandle)
                Me._LocalSet = False
            End If
        End Sub

        Protected Sub WorkOutCell(ByVal rowHandle As Integer)
            Dim grid = Me.AssociatedObject
            Dim groupRowHandle As Integer = grid.GetParentRowHandle(rowHandle)
            If grid.IsValidRowHandle(groupRowHandle) AndAlso grid.IsGroupRowHandle(groupRowHandle) Then
                Dim value? As Boolean = Me.GetRowChildrenState(groupRowHandle)
                Dim dic = New Dictionary(Of Integer, Boolean?)(Me.GetUpdatedDic(groupRowHandle, value))
                grid.GetRowState(groupRowHandle).SetCurrentValue(CheckableGroupBehavior.CheckListProperty, dic)
                Me.WorkOutCell(groupRowHandle)
            End If
        End Sub

        Protected Function GetRowChildrenState(ByVal rowHandle As Integer) As Boolean?
            Dim grid = Me.AssociatedObject
            Dim result? As Boolean = Nothing, b? As Boolean
            If grid.IsValidRowHandle(rowHandle) AndAlso grid.IsGroupRowHandle(rowHandle) Then
                Dim count As Integer = grid.GetChildRowCount(rowHandle)
                Dim i As Integer = -1
                i += 1
                Do While i < count
                    Dim childHandle = grid.GetChildRowHandle(rowHandle, i)
                    Dim childLevel As Integer = grid.GetRowLevelByRowHandle(childHandle)
                    If grid.IsGroupRowHandle(childHandle) Then
                        Dim dic = CType(grid.GetRowState(childHandle).GetValue(CheckableGroupBehavior.CheckListProperty), Dictionary(Of Integer, Boolean?))
                        b = dic(childLevel)
                    Else
                        Dim column = grid.Columns.Where(Function(c) c.FieldName = Me.CheckableProperty).FirstOrDefault()
                        If column IsNot Nothing Then
                            b = CBool(grid.GetCellValue(childHandle, column))
                        Else
                            b = False
                        End If
                    End If

                    If b.HasValue Then
                        If result.HasValue Then
                            If result.Value Xor b.Value Then
                                Return Nothing
                            End If
                        Else
                            result = b
                        End If
                    Else
                        Return Nothing
                    End If
                    i += 1
                Loop
            End If

            Return result
        End Function

        Protected Function GetUpdatedDic(ByVal rowHandle As Integer, ByVal value? As Boolean) As Dictionary(Of Integer, Boolean?)
            Dim result As Dictionary(Of Integer, Boolean?) = Nothing
            If Me.AssociatedObject.IsValidRowHandle(rowHandle) Then
                Dim groupLevel As Integer = Me.AssociatedObject.GetRowLevelByRowHandle(rowHandle)
                Dim rowState = Me.AssociatedObject.GetRowState(rowHandle)
                result = CType(rowState.GetValue(CheckableGroupBehavior.CheckListProperty), Dictionary(Of Integer, Boolean?))
                result(groupLevel) = value
            End If

            Return result
        End Function

        Protected Sub WorkOutGroup(ByVal rowHandle As Integer, ByVal value? As Boolean)
            Dim grid = Me.AssociatedObject
            Dim parentRowState = grid.GetRowState(rowHandle)
            If grid.IsValidRowHandle(rowHandle) AndAlso grid.IsGroupRowHandle(rowHandle) Then
                Dim count As Integer = grid.GetChildRowCount(rowHandle)
                Dim i As Integer = -1
                i += 1
                Do While i < count
                    Dim childHandle = grid.GetChildRowHandle(rowHandle, i)
                    If grid.IsGroupRowHandle(childHandle) Then
                        Dim rowState = Me.AssociatedObject.GetRowState(childHandle)
                        rowState.SetCurrentValue(CheckableGroupBehavior.CheckListProperty, New Dictionary(Of Integer, Boolean?)(Me.GetUpdatedDic(childHandle, value)))
                        Me.WorkOutGroup(childHandle, value)
                    Else
                        Dim column = grid.Columns.Where(Function(c) c.FieldName = Me.CheckableProperty).FirstOrDefault()
                        If column IsNot Nothing Then
                            grid.SetCellValue(childHandle, column, value)
                        End If
                    End If
                    i += 1
                Loop

                Me.WorkOutCell(rowHandle)
            End If
        End Sub

        #Region "Template For GroupRowValue"

        Protected Function GetGroupValueTemplate() As DataTemplate
            Dim xamlDataTemplate As String = "<DataTemplate>"
            xamlDataTemplate &= "<self:GroupCheckBox Content=""{Binding Value}"" Margin=""8,0,0,0""  CheckStates=""{Binding Path=RowData.RowState.(self:CheckableGroupBehavior.CheckList), Mode=TwoWay}""/>"
            xamlDataTemplate &= "</DataTemplate>"
            Dim context As New ParserContext()
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation")
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml")
            context.XmlnsDictionary.Add("self", "clr-namespace:DevExpress.Example03;assembly=DevExpress.Example03")
            Dim template As DataTemplate = CType(XamlReader.Parse(xamlDataTemplate, context), DataTemplate)
            Return template
        End Function

        #End Region ' Template for GroupRowValue
    End Class
End Namespace
