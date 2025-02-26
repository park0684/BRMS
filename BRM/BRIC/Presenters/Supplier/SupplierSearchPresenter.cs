using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using common.Interface;
using BRIC.Services;
using BRIC.Repositories;

namespace BRIC.Presenters
{
    public class SupplierSearchPresenter
    {
        ISupplierSearchView _view;
        ISupplierRepository _repository;
        SupplierService _service;
        Action<int> _onSlected;

        public SupplierSearchPresenter(ISupplierSearchView view, Action< int> onSelected)
        {
            _view = view;
            _repository = new SupplierRepository();
            _service = new SupplierService(_repository);
            _onSlected = onSelected;
            _view.CloseFormEvent += CloseForm;
            _view.SelectedSupplierEvent += SelectSupplier;
            LoadSupplierList();
            _view.ShowForm();
        }

        private void SelectSupplier(object sender, int e)
        {
            try
            {
                int selectedCode = _view.SupplierCode; // 인터페이스에서 선택된 값 반환하도록
                _onSlected?.Invoke(selectedCode);
                _view.CloseForm();
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void LoadSupplierList()
        {
            try
            {
                var result = _service.LoadSeachList();
                _view.BindingDataTable(result);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }


        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
