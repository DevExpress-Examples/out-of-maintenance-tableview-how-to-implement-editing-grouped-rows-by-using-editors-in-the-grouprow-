Imports DevExpress.Mvvm

Namespace DevExpress.Example03

    Public Enum Gender
        Male
        Female
    End Enum

    Public Class Employee
        Inherits BindableBase

        Protected _Name As String

        Public Property Name() As String
            Get
                Return Me._Name
            End Get

            Set(ByVal value As String)
                Me.SetProperty(Me._Name, value, "Name")
            End Set
        End Property

        Protected _Age As Integer

        Public Property Age() As Integer
            Get
                Return Me._Age
            End Get

            Set(ByVal value As Integer)
                Me.SetProperty(Me._Age, value, "Age")
            End Set
        End Property

        Protected _Gender As Gender

        Public Property Gender() As Gender
            Get
                Return Me._Gender
            End Get

            Set(ByVal value As Gender)
                Me.SetProperty(Me._Gender, value, "Gender")
            End Set
        End Property

        Protected _Department As String

        Public Property Department() As String
            Get
                Return Me._Department
            End Get

            Set(ByVal value As String)
                Me.SetProperty(Me._Department, value, "Department")
            End Set
        End Property

        Protected _IsInvited As Boolean

        Public Property IsInvited() As Boolean
            Get
                Return Me._IsInvited
            End Get

            Set(ByVal value As Boolean)
                Me.SetProperty(Me._IsInvited, value, "IsInvited")
            End Set
        End Property
    End Class
End Namespace
