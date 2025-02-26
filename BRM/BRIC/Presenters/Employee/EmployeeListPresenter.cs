using BRIC.Services;
using common.Helpers;
using common.Interface;
using common.Model;
using System;
using System.Collections.Generic;

namespace BRIC.Presenters
{
    public class EmployeeListPresenter
    {
        IEmployeeListView _view;
        Action<string, int> _openPopup;
        EmployeeService _service;
        int userCode = UserSession.CurrentUser.EmployeeCode;

        public EmployeeListPresenter(IEmployeeListView view, Action<string, int> onPopup)
        {
            _view = view;
            _openPopup = onPopup;
            _service = new EmployeeService(UserSession.CurrentUser.EmployeeCode);
            _view.AddEmployeeEvent += AddEmployee;
            _view.SearchEvent += SearchEmployee;
            _view.SelectEmployeeEvent += CallbackEmployeeInfo;
            SetComboBoxItems();
            SetDataGridViewColumns();
        }

        private void CallbackEmployeeInfo(object sender, int e)
        {
            try
            {
                _openPopup?.Invoke("EmployeeDetail", e);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void SearchEmployee(object sender, EventArgs e)
        {
            try
            {
                var result = _service.LoadEmployeeList(_view.Status, userCode);
                _view.EmpListBinding(result);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void AddEmployee(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 상태 콤보 박스 설정
        /// </summary>
        private void SetComboBoxItems()
        {
            try
            {
                Dictionary<int, string> items = StatusHelper.GetMap(StatusHelper.Keys.EmployeeStatus); // 직원 상태 정보 

                _view.SetComboBoxItems(items);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 직원관리 데이터그리드뷰 칼럼 설정
        /// </summary>
        private void SetDataGridViewColumns()
        {
            try
            {
                var columns = new Dictionary<string, string>
                {
                    {"empCode", "직원코드"},
                    {"empName", "직원명"},
                    {"empLevel", "직책"},
                    {"empCell", "연락처"},
                    {"empEmail", "이메일"},
                    {"empAddr", "주소"},
                    {"empStatus", "상태"},
                    {"empIdate", "등록일"},
                    {"empUdate", "수정일"},
                    {"emptMemo", "메모"}
                };
                _view.SetDataGridColumns(columns);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }
    }
}
