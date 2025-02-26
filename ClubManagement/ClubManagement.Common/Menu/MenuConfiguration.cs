using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Common.Menu
{
    public static class MenuConfiguration
    {
        public static List<MenuItemInfo> GetMenuItems(Action<string> menuClickHandler)
        {
            return new List<MenuItemInfo>
            {
                new MenuItemInfo { Category = "모임관리", Title = "스코어보드", ClickAction = () => menuClickHandler("ScoreBoard") },
                new MenuItemInfo { Category = "모임관리", Title = "모임관리", ClickAction = () => menuClickHandler("MatchList") },
                new MenuItemInfo { Category = "회원관리", Title = "회비관리", ClickAction = () => menuClickHandler("DuesManage") },
                new MenuItemInfo { Category = "회원관리", Title = "회원조회", ClickAction = () => menuClickHandler("MemberList") },
            };
        }
    }
}
