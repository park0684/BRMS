using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManagement.Common.Factory
{
    public interface IViewFactory
    {
        Form CreateView(string menuKey);
    }
}
