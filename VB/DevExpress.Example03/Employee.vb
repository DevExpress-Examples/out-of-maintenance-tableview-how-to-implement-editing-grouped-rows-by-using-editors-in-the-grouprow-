Imports System.ComponentModel

Namespace DevExpress.Example03

    Public Enum Gender
        Male
        Female
    End Enum

    Public Class Employee
        Implements INotifyPropertyChanged


        Protected _Name As String

        Public Property Name() As String
            Get
                Return Me._Name
            End Get

            Set(ByVal value As String)
                If Me._Name <> value Then
                    Me._Name = value
                    Me.OnPropertyChanged("Name")
                End If
            End Set
        End Property

        Protected _Age As Integer

        Public Property Age() As Integer
            Get
                Return Me._Age
            End Get

            Set(ByVal value As Integer)
                If Me._Age <> value Then
                    Me._Age = value
                    Me.OnPropertyChanged("Age")
                End If
            End Set
        End Property
        Protected _Gender As Gender

        Public Property Gender() As Gender
            Get
                Return Me._Gender
            End Get

            Set(ByVal value As Gender)
                If Me._Gender <> value Then
                    Me._Gender = value
                    Me.OnPropertyChanged("Gender")
                End If
            End Set
        End Property


        Protected _Department As String

        Public Property Department() As String
            Get
                Return Me._Department
            End Get

            Set(ByVal value As String)
                If Me._Department <> value Then
                    Me._Department = value
                    Me.OnPropertyChanged("Department")
                End If
            End Set
        End Property

        Protected _IsInvited As Boolean

        Public Property IsInvited() As Boolean
            Get
                Return Me._IsInvited
            End Get

            Set(ByVal value As Boolean)
                If Me._IsInvited <> value Then
                    Me._IsInvited = value
                    Me.OnPropertyChanged("IsInvited")
                End If
            End Set
        End Property

        Public Sub OnPropertyChanged(ByVal info As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
