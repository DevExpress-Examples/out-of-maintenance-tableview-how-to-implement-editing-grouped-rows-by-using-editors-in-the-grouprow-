Imports DevExpress.Xpf.Editors
Imports DevExpress.Xpf.Editors.Settings
Imports DevExpress.Xpf.Grid
Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Linq

Namespace DevExpress.Example03

    Partial Public Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            Me.DataContext = Me
        End Sub

        Protected _Employees As ObservableCollection(Of Employee)

        Public ReadOnly Property Employees() As ObservableCollection(Of Employee)
            Get
                If Me._Employees Is Nothing Then
                    Me._Employees = New ObservableCollection(Of Employee)(DataHelper.GenerateEmployees(200))
                End If

                Return Me._Employees
            End Get
        End Property

        Public ReadOnly Property GenderValues() As Array
            Get
                Return System.Enum.GetValues(GetType(Gender))
            End Get
        End Property

    End Class

End Namespace