using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManagement.Common.Factory
{
    public class CompositeViewFactory:IViewFactory
    {
        private readonly List<IViewFactory> _factories;

        public CompositeViewFactory(params IViewFactory[] factories)
        {
            _factories = new List<IViewFactory>(factories);
        }

        public Form CreateView(string menuKey)
        {
            foreach (var factory in _factories)
            {
                var view = factory.CreateView(menuKey);
                if (view != null)
                    return view;
            }
            return null;
        }
    }
}
