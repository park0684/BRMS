using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Common.Menu
{
    public class MenuItemInfo
    {
        public string Category { get; set; }
        public string Title { get; set; }
        public Action ClickAction { get; set; }
    }
}
