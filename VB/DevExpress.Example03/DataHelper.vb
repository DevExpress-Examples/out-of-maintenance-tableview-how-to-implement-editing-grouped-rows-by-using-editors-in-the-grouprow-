Imports System
Imports System.Collections.Generic

Namespace DevExpress.Example03

    Public Module DataHelper

        Private _Randomizer As Random = New Random()

        Private _Departments As String() = New String() {"Managers", "Developers", "Testing department", "Translators", "Mass media"}

'#Region "Names"
        Private _Names As String() = New String() {"Darrel Surename", "Ivor Surename", "Eleanor Surename", "Maxwell Surename", "Latifah Surename", "Montana Surename", "Yen Surename", "Charity Surename", "Herrod Surename", "Gretchen Surename", "Jada Surename", "Hunter Surename", "Haviva Surename", "Chastity Surename", "Jaden Surename", "Ori Surename", "Iona Surename", "Caesar Surename", "Sharon Surename", "Zia Surename", "MacKensie Surename", "Edan Surename", "Judah Surename", "Joy Surename", "Shay Surename", "Alan Surename", "Yuri Surename", "Kiara Surename", "Nita Surename", "Shad Surename", "Kimberly Surename", "Fallon Surename", "Abdul Surename", "Adrienne Surename", "Octavius Surename", "Britanni Surename", "Ainsley Surename", "Buffy Surename", "Ila Surename", "Candace Surename", "Madison Surename", "Allen Surename", "Fritz Surename", "Curran Surename", "Mariko Surename", "Rylee Surename", "Garrett Surename", "Emery Surename", "Tashya Surename", "Fay Surename", "Kadeem Surename", "Phoebe Surename", "Walter Surename", "Maggy Surename", "Wilma Surename", "Yvonne Surename", "Bert Surename", "Harlan Surename", "Valentine Surename", "Perry Surename", "Barclay Surename", "Seth Surename", "Mannix Surename", "Bruce Surename", "Althea Surename", "Nevada Surename", "Raya Surename", "Octavius Surename", "Mark Surename", "Wing Surename", "Julian Surename", "Adam Surename", "Rhiannon Surename", "Neil Surename", "Jessica Surename", "Azalia Surename", "Chelsea Surename", "Mira Surename", "Jorden Surename", "Amir Surename", "Zelda Surename", "Kathleen Surename", "Nevada Surename", "Grace Surename", "Meghan Surename", "Patrick Surename", "Sopoline Surename", "Eleanor Surename", "Ariana Surename", "Honorato Surename", "Ava Surename", "Macon Surename", "Leonard Surename", "Amelia Surename", "Emi Surename", "Amity Surename", "Mannix Surename", "Sophia Surename", "Chloe Surename", "Dexter Surename"}

'#End Region  ' Names
        Public Function GenerateEmployees(ByVal amount As Integer) As List(Of Employee)
            Dim result As List(Of Employee) = New List(Of Employee)()
            If amount <= 0 Then
                Return result
            End If

            Dim i As Integer = -1
            While Threading.Interlocked.Increment(i) < amount
                Dim employee = New Employee()
                employee.Name = _Names(_Randomizer.Next(_Names.Length))
                employee.Gender = If(_Randomizer.Next(2) > 0, Gender.Male, Gender.Female)
                employee.Department = _Departments(_Randomizer.Next(_Departments.Length))
                employee.Age = _Randomizer.Next(50) + 20
                result.Add(employee)
            End While

            Return result
        End Function
    End Module
End Namespace
