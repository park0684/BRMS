using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using common.DTOs;
using common.Model;
using common.Interface;

namespace Product.Presenters
{
    public class ProductListPresenter
    {
        IProductListView _view;
        ProductSearchModel _model;

        public ProductListPresenter(IProductListView view)
        {
            _view = view;
            _model = new ProductSearchModel();
            _view.AddProcutEvent += AddProduct;
            _view.SelectCategoryEvent += SelectCategory;
        }

        private void SelectCategory(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddProduct(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
