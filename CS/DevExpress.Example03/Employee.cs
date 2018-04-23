

using DevExpress.Mvvm;

namespace DevExpress.Example03 {

    public enum Gender { Male, Female }
    
    public class Employee : BindableBase {

        protected string _Name;

        public string Name {
            get {
                return this._Name;
            }

            set {
                this.SetProperty(ref this._Name, value, "Name");
            }
        }
        
        protected int _Age;

        public int Age {
            get {
                return this._Age;
            }

            set {
                this.SetProperty(ref this._Age, value, "Age");
            }
        }

        protected Gender _Gender;

        public Gender Gender {
            get {
                return this._Gender;
            }

            set {
                this.SetProperty(ref this._Gender, value, "Gender");
            }
        }

        protected string _Department;

        public string Department {
            get {
                return this._Department;
            }

            set {
                this.SetProperty(ref this._Department, value, "Department");
            }
        }

        protected bool _IsInvited;

        public bool IsInvited {
            get {
                return this._IsInvited;
            }

            set {
                this.SetProperty(ref this._IsInvited, value, "IsInvited");
            }
        }
    }
}
