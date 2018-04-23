Imports DevExpress.Xpf.Editors
Imports DevExpress.Xpf.Editors.Settings
Imports DevExpress.Xpf.Grid
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

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

    End Class

End Namespace