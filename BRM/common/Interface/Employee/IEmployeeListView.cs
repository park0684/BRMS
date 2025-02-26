using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace common.Interface
{
    public interface IEmployeeListView : IView
    {
        int Status { get; }

        event EventHandler SearchEvent;
        event EventHandler AddEmployeeEvent;
        event EventHandler<int> SelectEmployeeEvent;

        void EmpListBinding(DataTable resutl);
        void ShowMessage(string message);
        void SetComboBoxItems(Dictionary<int, string> items);
        void SetDataGridColumns(Dictionary<string, string> columns);


    }
}
