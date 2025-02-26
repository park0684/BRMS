using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.DTOs;
using common.Model;
using common.Interface;
using BRIC.Presenters;
using BRIC.Services;
using BRIC.Repositories;
using common.Helpers;

namespace BRIC.Presenters
{
    public class ProductListPresenter
    {
        IProductListView _view;
        ProductSearchModel _model;
        ProductService _service;
        IProductRepository _repository;
        Action<string, int> _onPopup;
        Action<string, int, Delegate> _openPopupWithCallback;

        public ProductListPresenter(IProductListView view, Action<string,int> onPopup, Action<string, int, Delegate> popupWithCallback)
        {
            _view = view;
            _onPopup = onPopup;
            _openPopupWithCallback = popupWithCallback;
            _model = new ProductSearchModel();
            _repository = new ProductRepository();
            _service = new ProductService(_repository,UserSession.CurrentUser.EmployeeCode);
            _view.AddProcutEvent += AddProduct;
            _view.SelectCategoryEvent += SelectCategory;
            _view.SearchEvent += ProductSearch;
            _view.SelectProcutEvent += LoadProductInfo;
        }

        private void LoadProductInfo(object sender, int code)
        {
            try
            {
                _onPopup?.Invoke("ProductDetail", code);
            }
             catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void ProductSearch(object sender, EventArgs e)
        {
            _model.SeachWord = _view.SerchWord;
            _model.DateType1 = _view.DateType1;
            _model.DataType2 = _view.DateType1;
            _model.Type1FromDate = _view.Type1FromDate;
            _model.Type1ToDate = _view.Type1ToDate;
            _model.Type2FromDate = _view.Type2FromDate;
            _model.Type2ToDate = _view.Type2ToDate;
            _model.Status = _view.Status;

            ProductSearchDto product = new ProductSearchDto
            {
                SeachWord = _view.SerchWord,
                CategoryBot = _model.CategoryBot,
                CategoryMid = _model.CategoryMid,
                CategoryTop = _model.CategoryTop,
                DateType1 = _model.DateType1,
                DateType2 = _model.DataType2,
                Type1FromDate = _model.Type1FromDate,
                Type1ToDate = _model.Type1ToDate,
                Type2FromDate = _model.Type2FromDate,
                Type2ToDate = _model.Type2ToDate,
                Status = _model.Status

            };
            try
            {
                var result = _service.LoadProductList(product);
                _view.BindingDataTable(result);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void SelectCategory(object sender, EventArgs e)
        {
            try
            {
                //_openPopup?.Invoke("CategoryBoard", 1);
                _openPopupWithCallback?.Invoke("CategoryBoard", 1, new Action<int, int, int>((top, mid, bot) =>
                {
                    _model.CategoryTop = top;
                    _model.CategoryMid = mid;
                    _model.CategoryBot = bot;
                    var (catKr, catEn) = _service.GetCategoryName(top,mid, bot);
                    _view.Category = catKr;


                }));
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void AddProduct(object sender, EventArgs e)
        {
            try
            {
                _onPopup?.Invoke("ProductDetail", 0);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }

        }

    }
}
