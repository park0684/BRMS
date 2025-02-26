using ClubManagement.Common.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManagement.View
{
    public interface IMainView
    {
        void SetMenuItems(Dictionary<string, List<MenuItemInfo>> groupedItems);
        void LoadContent(Form form);
    }
}
