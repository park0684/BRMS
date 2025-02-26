using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.Interface;
using BRIC.Services;
using BRIC.Repositories;

namespace BRIC.Presenters
{
    public class CategoryBoardPresenter
    {
        ICategoryBoradView _view;
        CategoryService _service;
        IProductRepository _repository;
        Action<int, int, int> _onSelected;
        public CategoryBoardPresenter(ICategoryBoradView view,int type, Action<int, int, int> onSelected)
        {
            _view = view;
            _repository = new ProductRepository();
            _service = new CategoryService(_repository);
            _onSelected = onSelected;
            _view.SetIsEidtMod(type);
            
            var result = _service.LoadCategory(0, 0, _view.CategoryStatus);
            _view.CategoryTopBindign(result);
            _view.SelectCategoryTopEvent += LoadCategoryMid;
            _view.SelectCategoryMidEvent += LoadcategoryBot;
            _view.SaveEvent += SelectCategory;
            _view.ShowForm();
        }

        private void SelectCategory(object sender, EventArgs e)
        {
            int top = _view.GetCurrentTop();
            int mid = _view.GetCurrentMid();
            int bot = _view.GetCurrentBot();
            _onSelected?.Invoke(top, mid, bot);
            _view.CloseForm();
        }

        private void LoadcategoryBot(object sender, EventArgs e)
        {
            int top = _view.GetCurrentTop();
            int mid = _view.GetCurrentMid();
            var result = _service.LoadCategory(top, mid, _view.CategoryStatus);
            _view.CategoryBotBindign(result);
        }

        private void LoadCategoryMid(object sender, EventArgs e)
        {
            int top = _view.GetCurrentTop();
            var result = _service.LoadCategory(top, 0, _view.CategoryStatus);
            _view.CategoryMidBindign(result);
            var row = result.Rows[0];
            int mid = Convert.ToInt32(row[0]);
            result = _service.LoadCategory(top, mid, _view.CategoryStatus);
            _view.CategoryBotBindign(result);
        }
    }
}
