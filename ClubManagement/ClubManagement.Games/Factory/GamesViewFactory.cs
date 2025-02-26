using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Common.Factory;
using ClubManagement.Games.Presenters;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Factory
{
    public class GamesViewFactory : IViewFactory
    {
        public Form CreateView(string menuKey)
        {
            switch (menuKey)
            {
                case "MatchList":
                    var matcthView = new MatchListView();
                    var matchRepo = new MatchRepository();
                    var presenter = new MatchListPresenter(matcthView, matchRepo);
                    return matcthView;

                //case "DuesManage":
                //    var duesView = new ScoreBoadView();
                //    var duesRepo = new DuesRepsotiry();
                //    var udesPres = new DuesManagePresenter(duesView, duesRepo);
                //    return duesView;

            }
            return null;

        }
    }
}
