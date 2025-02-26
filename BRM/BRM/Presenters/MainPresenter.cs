using BRM.Factory;
using common.Interface;
using common.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using common.DTOs;

namespace BRM.Presenters
{
    public class MainPresenter
    {
        IMainView _view;
        private readonly Dictionary<string, IView> _loadedViews = new Dictionary<string, IView>();
        private readonly IViewFactory _viewFactory;
        public MainPresenter(IMainView view, IViewFactory viewFactory)
        {
            _view = view;
            _viewFactory = viewFactory;
            _view.OnClickMenu += HandleMenu;
            _view.OnClickSearch += HandleSearch;
            _view.DBConfigEvent += ExecuteDBConfig;
            var menuList = MenuRegistry.GetSideMenuList();
            _view.CreateMenu(menuList);
        }

        private void ExecuteDBConfig(object sender, EventArgs e)
        {
            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DBConfig.exe");

            if (File.Exists(exePath))
            {
                System.Diagnostics.Process.Start(exePath);
            }
            else
            {
                _view.ShowMessage("DB 설정 프로그램을 찾을 수 없습니다.");
            }
        }

        private void HandleSearch(object sender, EventArgs e)
        {
            foreach (var view in _loadedViews.Values)
            {
                
            }
        }

        private void HandleMenu(object sender, string e)
        {
            //if (!_loadedViews.TryGetValue(e, out var view))
            //{
            //    view = _viewFactory.CreateView(e);
            //    if (view != null)
            //        _loadedViews[e] = view;
            //}

            //if (view != null)
            //    _view.LoadControlToPanel(view.AsControl());
            //_view.MenuTitle = MenuRegistry.GetMenuDisplay(e);
            try
            {
                var menuDef = _viewFactory.GetDefinition(e);
                if (menuDef == null)
                {
                    return;
                }
                if (menuDef.OpenType == ViewOpenType.Panel)
                {
                    if (!_loadedViews.TryGetValue(e, out var view))
                    {
                        view = _viewFactory.CreateView(e);
                        if (view != null)
                            _loadedViews[e] = view;
                    }
                    if (view != null)
                        _view.LoadControlToPanel(view.AsControl());
                    _view.MenuTitle = MenuRegistry.GetMenuDisplay(e);
                }
                else if (menuDef.OpenType == ViewOpenType.Popup)
                {
                    var popupview = _viewFactory.OpenPopup(e, 0);
                }
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }
        public void OpenPopup(string menuKey, int code)
        {
            try 
            {
                var popupView = _viewFactory.OpenPopup(menuKey, code);
                if (popupView == null) return;

                var method = popupView.GetType().GetMethod("SetCode", new[] { typeof(int) });
                method?.Invoke(popupView, new object[] { code });

                var showForm = popupView.GetType().GetMethod("ShowForm");
                showForm?.Invoke(popupView, null);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

    }
}
