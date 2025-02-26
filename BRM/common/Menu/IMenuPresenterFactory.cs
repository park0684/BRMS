using common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Menu
{
    public interface IMenuPresenterFactory
    {
        void CreateProductListPresenter(IProductListView view);
        void CreateProductDetailPresenter(IProductDetailView view, int code);
        void CreateProductLogPresenter(ILogView view);
        void CreateEmployeeListPresenter(IEmployeeListView view);
        void CreateEmployeeLogPresenter(ILogView view);
        void CreateEmployeeDetailPresnter(IEmployeeDetailView view, int code);
        void CreateAccessLogPresneter(ILogView view);

    }
}
