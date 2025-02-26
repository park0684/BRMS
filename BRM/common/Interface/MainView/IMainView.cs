using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.DTOs;
using System.Windows.Forms;

namespace common.Interface
{
    public interface IMainView
    {
        string MenuTitle { get; set; }

        event EventHandler<string> OnClickMenu;
        event EventHandler OnClickSearch;
        event EventHandler DBConfigEvent;
        void CreateMenu(List<MenuCategoryDto> menu);
        void LoadControlToPanel(Control control);
        void ShowMessage(string message);
    }
}
