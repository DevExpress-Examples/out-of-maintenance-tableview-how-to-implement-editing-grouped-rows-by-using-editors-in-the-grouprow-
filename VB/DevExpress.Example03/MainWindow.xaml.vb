Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls

Namespace DevExpress.Example03

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            Me.InitializeComponent()
            DataContext = Me
        End Sub

        Protected _Employees As ObservableCollection(Of Employee)

        Public ReadOnly Property Employees As ObservableCollection(Of Employee)
            Get
                If _Employees Is Nothing Then
                    _Employees = New ObservableCollection(Of Employee)(GenerateEmployees(200))
                End If

                Return _Employees
            End Get
        End Property
    End Class
End Namespace
