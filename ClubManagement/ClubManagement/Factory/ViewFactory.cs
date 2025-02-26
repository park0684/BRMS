using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ClubManagement/Factory/ViewFactory.cs
using System.Windows.Forms;
using ClubManagement.Common.Factory;
using ClubManagement.Members.Views;

namespace ClubManagement.Factory
{
    public class ViewFactory : IViewFactory
    {
        public Form CreateView(string menuKey)
        {
            switch (menuKey)
            {
                case "MemberList": return new MemberListView();
                //case "DuesManage": return new DuesManageView();
                //case "GameManage": return new GameManageView();
                //case "ScoreBoard": return new ScoreBoardView();
                default: return null;
            }
        }
    }
}
