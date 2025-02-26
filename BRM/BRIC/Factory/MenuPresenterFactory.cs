using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.Menu;
using common.Interface;
using BRIC.Presenters;

namespace BRIC.Factory
{
    public class MenuPresenterFactory : IMenuPresenterFactory
    {
        private readonly Action<string, int> _openPopup;
        private readonly Action<string, int, Delegate> _openPopupWithCallback;

        public MenuPresenterFactory(Action<string, int> openPopup, Action<string, int, Delegate> openPopupWithCallback)
        {
            _openPopup = openPopup;
            _openPopupWithCallback = openPopupWithCallback;
        }

        public void CreateAccessLogPresneter(ILogView view)
        {
            _ = new AccessLogPresenter(view, _openPopup);
        }

        public void CreateEmployeeDetailPresnter(IEmployeeDetailView view, int code)
        {
            _ = new EmployeeDetailPresenter(view, code, _openPopup, _openPopupWithCallback);
        }

        public void CreateEmployeeListPresenter(IEmployeeListView view)
        {
            _ = new EmployeeListPresenter(view, _openPopup);
        }

        public void CreateEmployeeLogPresenter(ILogView view)
        {
            _ = new EmployeeLogPresenter(view, _openPopup,_openPopupWithCallback);
        }

        public void CreateProductDetailPresenter(IProductDetailView view, int code)
        {
            _ = new ProductDetailPresenter(view, code , _openPopup, _openPopupWithCallback);
        }

        public void CreateProductListPresenter(IProductListView view)
        => _ = new ProductListPresenter(view, _openPopup, _openPopupWithCallback);

        public void CreateProductLogPresenter(ILogView view)
            => _ = new ProductLogPresenter(view, _openPopup, _openPopupWithCallback);

    }
}
