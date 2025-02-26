using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace common.Interface
{
    public interface IEmployeeDetailView
    {
        int EmployeeCode { set; }
        string EmployeeName { get; set; }
        string EmployeePhon { get; set; }
        string EmployeeLevel { get; set; }
        int EmployeeStatus { get; set; }
        string EmployeeEmail { get; set; }
        string EmployeeAddress { get; set; }
        string Employeememo { get; set; }
        DateTime EmployeeIdate { set; }
        DateTime EmployeeUdate { set; }
        DateTime FromDate { get; }
        DateTime ToDate { get; }

        event EventHandler CloseFormEvent;
        event EventHandler SetAuthorityEvent;
        event EventHandler SetPasswordEvent;
        event EventHandler SaveEvent;
        event EventHandler LogSearchEvent;

        void ShowForm();
        void CloseForm();
        void ShowMessage(string message);
        void SetComboBoxItems(Dictionary<int, string> items);
        void SetTabpage(bool isNew, Dictionary<string, string> columns = null);
        void DataGridViewBinding(DataTable result);
    }
}
