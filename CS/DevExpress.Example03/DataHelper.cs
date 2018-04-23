using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpress.Example03 {

    public static class DataHelper {

        private static Random _Randomizer = new Random();

        private static string[] _Departments = new string[] { "Managers", "Developers", "Testing department", "Translators", "Mass media" };

        #region Names

        private static string[] _Names = new string[]{
            "Darrel Surename"     ,
            "Ivor Surename"       ,
            "Eleanor Surename"    ,
            "Maxwell Surename"    ,
            "Latifah Surename"    ,
            "Montana Surename"    ,
            "Yen Surename"        ,
            "Charity Surename"    ,
            "Herrod Surename"     ,
            "Gretchen Surename"   ,
            "Jada Surename"       ,
            "Hunter Surename"     ,
            "Haviva Surename"     ,
            "Chastity Surename"   ,
            "Jaden Surename"      ,
            "Ori Surename"        ,
            "Iona Surename"       ,
            "Caesar Surename"     ,
            "Sharon Surename"     ,
            "Zia Surename"        ,
            "MacKensie Surename"  ,
            "Edan Surename"       ,
            "Judah Surename"      ,
            "Joy Surename"        ,
            "Shay Surename"       ,
            "Alan Surename"       ,
            "Yuri Surename"       ,
            "Kiara Surename"      ,
            "Nita Surename"       ,
            "Shad Surename"       ,
            "Kimberly Surename"   ,
            "Fallon Surename"     ,
            "Abdul Surename"      ,
            "Adrienne Surename"   ,
            "Octavius Surename"   ,
            "Britanni Surename"   ,
            "Ainsley Surename"    ,
            "Buffy Surename"      ,
            "Ila Surename"        ,
            "Candace Surename"    ,
            "Madison Surename"    ,
            "Allen Surename"      ,
            "Fritz Surename"      ,
            "Curran Surename"     ,
            "Mariko Surename"     ,
            "Rylee Surename"      ,
            "Garrett Surename"    ,
            "Emery Surename"      ,
            "Tashya Surename"     ,
            "Fay Surename"        ,
            "Kadeem Surename"     ,
            "Phoebe Surename"     ,
            "Walter Surename"     ,
            "Maggy Surename"      ,
            "Wilma Surename"      ,
            "Yvonne Surename"     ,
            "Bert Surename"       ,
            "Harlan Surename"     ,
            "Valentine Surename"  ,
            "Perry Surename"      ,
            "Barclay Surename"    ,
            "Seth Surename"       ,
            "Mannix Surename"     ,
            "Bruce Surename"      ,
            "Althea Surename"     ,
            "Nevada Surename"     ,
            "Raya Surename"       ,
            "Octavius Surename"   ,
            "Mark Surename"       ,
            "Wing Surename"       ,
            "Julian Surename"     ,
            "Adam Surename"       ,
            "Rhiannon Surename"   ,
            "Neil Surename"       ,
            "Jessica Surename"    ,
            "Azalia Surename"     ,
            "Chelsea Surename"    ,
            "Mira Surename"       ,
            "Jorden Surename"     ,
            "Amir Surename"       ,
            "Zelda Surename"      ,
            "Kathleen Surename"   ,
            "Nevada Surename"     ,
            "Grace Surename"      ,
            "Meghan Surename"     ,
            "Patrick Surename"    ,
            "Sopoline Surename"   ,
            "Eleanor Surename"    ,
            "Ariana Surename"     ,
            "Honorato Surename"   ,
            "Ava Surename"        ,
            "Macon Surename"      ,
            "Leonard Surename"    ,
            "Amelia Surename"     ,
            "Emi Surename"        ,
            "Amity Surename"      ,
            "Mannix Surename"     ,
            "Sophia Surename"     ,
            "Chloe Surename"      ,
            "Dexter Surename"     

        };

        #endregion Names

        public static List<Employee> GenerateEmployees(int amount){
            List<Employee> result = new List<Employee>();
            if(amount <= 0) {
                return result;
            }

            int i = -1;
            while(++i < amount) {
                var employee = new Employee();
                employee.Name = _Names[_Randomizer.Next(_Names.Length)];
                employee.Gender = _Randomizer.Next(2)>0? Gender.Male : Gender.Female;
                employee.Department = _Departments[_Randomizer.Next(_Departments.Length)];
                employee.Age = _Randomizer.Next(50) + 20;
                result.Add(employee);
            }

            return result;
        }

    }
}
