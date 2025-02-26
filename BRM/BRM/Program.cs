using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BRM.Views;
using BRM.Presenters;
using BRIC.Presenters;
using common.Interface;
using BRM.Factory;
using common.Menu;
using BRIC.Factory;

namespace BRM
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //로그인 
            var loginView = new LoginView();
            var loginPresenter = new LoginPresenter(loginView);

            //메인View
            IMainView view = new MainView();
            IMenuPresenterFactory presenterFactory = null;

            Action<string, int> openPopup = (menuKey, code) =>
            {
                var factory = new MenuFactory(presenterFactory);
                factory.OpenPopup(menuKey, code);
            };

            Action<string, int, Delegate> openCallbackPopup = (menuKey, code, callback) =>
            {
                var factory = new MenuFactory(presenterFactory);
                int paramCount = callback.Method.GetParameters().Length;
                switch (paramCount)
                {
                    case 1:
                        factory.OpenCallBackPopup(menuKey, code, (Action<int>)callback);
                        break;
                    case 3:
                        factory.OpenCallBackPopup(menuKey, code, (Action<int, int, int>)callback);
                        break;
                }
            };

            // presenterFactory 생성
            presenterFactory = new MenuPresenterFactory(openPopup, openCallbackPopup);

            IViewFactory viewFactory = new MenuFactory(presenterFactory);
            var mainPresenter = new MainPresenter(view, viewFactory);

            Application.Run((Form)view);
        }
    }
}
