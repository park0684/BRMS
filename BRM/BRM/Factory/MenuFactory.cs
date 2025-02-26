using BRIC.Entry;
using BRM.Views;
using common.Interface;
using System;
using System.Collections.Generic;
using common.Menu;
using common.DTOs;
using System.Linq;
using common.Helpers;


namespace BRM.Factory
{
    public class MenuFactory: IViewFactory
    {

        private readonly Dictionary<string, IView> _cache = new Dictionary<string, IView>();
        private IMenuPresenterFactory _presenterFactory;
        //Action<string, int> _openPopup;
        //Action<string, int, Delegate> _openPopupWhitCallbak;

        public MenuFactory(IMenuPresenterFactory presenterFactory )
        {
            //_openPopup = openPopup;
            //_openPopupWhitCallbak = openPopupWithResult;
            _presenterFactory = presenterFactory;
        }
        public IView CreateView(string menuKey)
        {
            // 사이드 메뉴 실행 시 권한이 있는지 확인
            int permissionCode = MenuPermissionCodes.GetCode(menuKey);
            AuthorityHelper.CheckOrThrow(permissionCode); // 권한이 없을 경우 메시지 발생

            switch (menuKey)
            {
                case Menukeys.ProductList:
                    var pdtListView = new ProductListView();
                    _presenterFactory.CreateProductListPresenter(pdtListView);
                    return pdtListView;

                case Menukeys.ProductLog:
                    var pdtLogView = new LogView();
                    _presenterFactory.CreateProductLogPresenter(pdtLogView);
                    return pdtLogView;
                case Menukeys.EmployeeList:
                    var empListView = new EmployeeListView();
                    _presenterFactory.CreateEmployeeListPresenter(empListView);
                    return empListView;
                case Menukeys.EmplyeeLog:
                    var empLogView = new LogView();
                    _presenterFactory.CreateEmployeeLogPresenter(empLogView);
                    return empLogView;
                case Menukeys.AccessLog:
                    var accLogView = new LogView();
                    _presenterFactory.CreateAccessLogPresneter(accLogView);
                    return accLogView;
                // 추가 메뉴는 여기에 확장
                default:
                    return null;
            }
        }

        /// <summary>
        /// 반화값이 필요 없는 팝업 메뉴
        /// </summary>
        /// <param name="menuKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public IView OpenPopup(string menuKey,int code)
        {
            switch (menuKey)
            {
                case Menukeys.ProductDetail:
                    var pdtDetail = new ProductDetailView();
                    _presenterFactory.CreateProductDetailPresenter(pdtDetail,code);
                    return pdtDetail;
                case Menukeys.EmployeeDetail:
                    var empDetail = new EmployeeDetailView();
                    _presenterFactory.CreateEmployeeDetailPresnter(empDetail, code);
                    return empDetail;
                default:
                    return null;
            }
        }

        private static IView Connect<TView>(TView view, Action<TView> connector) where TView : IView
        {
            connector(view);
            return view;
        }

        /// <summary>
        /// 반환값이 필요한 팝업 메뉴
        /// </summary>
        /// <param name="menuKey"></param>
        /// <param name="code"></param>
        /// <param name="callback"></param>
        public void OpenCallBackPopup(string menuKey, int code, Delegate callback)
        {

            switch (menuKey)
            {
                case Menukeys.CategoryBoard:
                    var catBoard = new CategoryBoradView();
                    MenuEntry.Connect(catBoard, code, (Action<int, int, int>)callback);
                    break;
                case Menukeys.SupplierSearch:
                    var supplierSearch = new SupplierSearchView();
                    MenuEntry.Connect(supplierSearch, (Action<int>)callback);
                    break;
            }
        }

        public MenuItemDto GetDefinition(string menuKey)
        {
            return MenuRegistry.GetSideMenuList().SelectMany(c => c.MenuItems).FirstOrDefault(m => m.Menukey == menuKey);
        }
    }
}
