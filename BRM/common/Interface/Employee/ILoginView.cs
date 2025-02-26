using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Interface
{
    public interface ILoginView
    {
        string EmployeeName { set; }
        int EmployeeCode { get; }
        string Passwordk { get; }
        
        event EventHandler LoginEvent;
        event EventHandler CloseFormEvent;
        event EventHandler GetEmployeeNameEvent;

        void CloseForm();
        void ShowMessage(string message);
        void ShowForm();
        void FocusMove();
        void GetEmpInof();
    }
}
