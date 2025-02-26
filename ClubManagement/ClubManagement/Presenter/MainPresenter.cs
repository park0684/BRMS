using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Common.Menu;
using ClubManagement.View;
using ClubManagement.Common.Factory;
using System.Windows.Forms;

namespace ClubManagement.Presenter
{
    public class MainPresenter
    {
        IMainView _view;
        IViewFactory _viewFactory;
        private readonly Dictionary<string, Form> _cachedViews = new Dictionary<string, Form>();
        public MainPresenter(IMainView view, IViewFactory viewFactory)
        {
            _view = view;
            _viewFactory = viewFactory;
        }

        
        public void LoadMenu()
        {
            List<MenuItemInfo> items = MenuConfiguration.GetMenuItems(HandleMenuClick);
            Dictionary<string, List<MenuItemInfo>> grouped = new Dictionary<string, List<MenuItemInfo>>();

            foreach (var item in items)
            {
                if (!grouped.ContainsKey(item.Category))
                    grouped[item.Category] = new List<MenuItemInfo>();
                grouped[item.Category].Add(item);
            }

            _view.SetMenuItems(grouped);
        }

        private void HandleMenuClick(string menuKey)
        {
            if(!_cachedViews.TryGetValue(menuKey, out var form) || form.IsDisposed)
            {
                form = _viewFactory.CreateView(menuKey);
                if (form != null)
                    _cachedViews[menuKey] = form;
            }

            if (form != null)
                _view.LoadContent(form);
        }
    }
}
