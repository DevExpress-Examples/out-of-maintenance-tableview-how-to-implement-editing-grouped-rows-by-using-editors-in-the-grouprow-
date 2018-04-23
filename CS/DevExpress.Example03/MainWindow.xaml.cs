using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;

namespace DevExpress.Example03 {

    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.DataContext = this;
        }

        protected ObservableCollection<Employee> _Employees;

        public ObservableCollection<Employee> Employees {
            get {
                if(this._Employees == null) {
                    this._Employees = new ObservableCollection<Employee>(DataHelper.GenerateEmployees(200));
                }

                return this._Employees;
            }
        }

        public Array GenderValues {
            get {
                return Enum.GetValues(typeof(Gender));
            }
        }

    }
    
}