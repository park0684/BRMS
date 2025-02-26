using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace common.Interface
{
    public interface ISupplierSearchView :IView
    {
        int SupplierCode { get; }

        event EventHandler CloseFormEvent;
        event EventHandler<int> SelectedSupplierEvent;

        void CloseForm();
        void ShowForm();
        void ShowMessage(string message);
        void BindingDataTable(DataTable result);
    }
}
