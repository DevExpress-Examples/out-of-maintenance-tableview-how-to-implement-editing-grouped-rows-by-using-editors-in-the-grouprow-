using System.ComponentModel;

namespace DevExpress.Example03 {

    public enum Gender { Male, Female }
    
    public class Employee : INotifyPropertyChanged {

        
        protected string _Name;

        public string Name {
            get {
                return this._Name;
            }

            set {
                if(this._Name != value) {
                    this._Name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }
        
        protected int _Age;

        public int Age {
            get {
                return this._Age;
            }

            set {
                if(this._Age != value) {
                    this._Age = value;
                    this.OnPropertyChanged("Age");
                }
            }
        }
        protected Gender _Gender;

        public Gender Gender {
            get {
                return this._Gender;
            }

            set {
                if(this._Gender != value) {
                    this._Gender = value;
                    this.OnPropertyChanged("Gender");
                }
            }
        }

        
        protected string _Department;

        public string Department {
            get {
                return this._Department;
            }

            set {
                if(this._Department != value) {
                    this._Department = value;
                    this.OnPropertyChanged("Department");
                }
            }
        }
        
        protected bool _IsInvited;

        public bool IsInvited {
            get {
                return this._IsInvited;
            }

            set {
                if(this._IsInvited != value) {
                    this._IsInvited = value;
                    this.OnPropertyChanged("IsInvited");
                }
            }
        }

        public void OnPropertyChanged(string info) {
            if(this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
