using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.View;
using ClubManagement.Presenter;
using ClubManagement.Common.Factory;
using ClubManagement.Factory;
using ClubManagement.Members.Factory;
using ClubManagement.Games.Factory;
namespace ClubManagement
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

            IMainView view = new MainView();
            IViewFactory viewFactory = new CompositeViewFactory
                ( 
                    new MembersViewFactory(),
                    new GamesViewFactory()
                    );
            var presenter = new MainPresenter(view, viewFactory);
            presenter.LoadMenu();

            Application.Run((Form)view);
        }
    }
}
