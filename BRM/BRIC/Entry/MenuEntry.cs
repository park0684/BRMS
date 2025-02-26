using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.Interface;
using BRIC.Presenters;

namespace BRIC.Entry
{
    public static class MenuEntry
    {
        public static void Connect(IProductListView view, Action<string, int> opnePopup, Action<string, int, Delegate> openPopupWhithCallback)
        {
            _ = new ProductListPresenter(view, opnePopup, openPopupWhithCallback);
        }
        public static void Connect(ILogView view, Action<string, int> opnePopup, Action<string, int, Delegate> openPopupWhithCallback)
            => _ = new ProductLogPresenter(view, opnePopup, openPopupWhithCallback);
        public static void Connect(IEmployeeListView view, Action<string, int> openPopup)
            => _ = new EmployeeListPresenter(view, openPopup);
        public static void Connect(IProductDetailView view,int code, Action<string, int> opnePopup, Action<string, int, Delegate> openPopupWithCallback)
        => _ = new ProductDetailPresenter(view, code, opnePopup, openPopupWithCallback);
        public static void Connect(ICategoryBoradView view,int type,  Action<int, int, int> onPopup)
            => _ = new CategoryBoardPresenter(view, type, onPopup);
        public static void Connect(ISupplierSearchView view,  Action<int> onPopup)
            => _ = new SupplierSearchPresenter(view, onPopup);
    }
}
