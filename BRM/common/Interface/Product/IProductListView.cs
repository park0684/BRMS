using System;
using System.Data;

namespace common.Interface
{
    public interface IProductListView:IView
    {
        string Category { set; }
        string SerchWord { get; }
        int Status { get; set; }
        int DateType1 { get; set; }
        int DateType2 { get; set; }
        DateTime Type1FromDate { get; set; }
        DateTime Type1ToDate { get; set; }
        DateTime Type2FromDate { get; set; }
        DateTime Type2ToDate { get; set; }

        event EventHandler SelectCategoryEvent;
        event EventHandler AddProcutEvent;
        event EventHandler SearchEvent;
        event EventHandler<int> SelectProcutEvent;
        void BindingDataTable(DataTable result);
        void ShowMessage(string message);
    }
}
