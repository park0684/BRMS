using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using common.DTOs;
using common.Helpers;
using common.Interface;
using common.Model;
using BRIC.Services;

namespace BRIC.Presenters
{
    public class EmployeeDetailPresenter
    {
        IEmployeeDetailView _view;
        Action<string, int> _openPopup;
        Action<string, int, Delegate> _openPopupWithCallback;
        List<LogDto> _logs;
        EmployeeDto _model;
        EmployeeDto _original;
        EmployeeService _service;
        LogService _logService;
        public EmployeeDetailPresenter(IEmployeeDetailView view, int code, Action<string, int> openPopup, Action<string, int, Delegate> opnePopupWithCallback)
        {
            _view = view;
            _openPopup = openPopup;
            _openPopupWithCallback = opnePopupWithCallback;
            _service = new EmployeeService(UserSession.CurrentUser.EmployeeCode);
            _logService = new LogService(UserSession.CurrentUser.EmployeeCode);
            _view.CloseFormEvent += CloseForm;
            _view.SaveEvent += EmployeeSave;
            _view.SetAuthorityEvent += SetAuthority;
            _view.SetPasswordEvent += SetPassword;
            _view.LogSearchEvent += SearchLog;
            _view.SetComboBoxItems(StatusHelper.GetMap(StatusHelper.Keys.EmployeeStatus));
            CheckIsNew(code);
            LoadEmployeeInfo(code);
            _view.ShowForm();
        }

        private void SearchLog(object sender, EventArgs e)
        {
            try
            {
                LogSearchDto search = new LogSearchDto()
                {
                    FromDate = _view.FromDate,
                    ToDate = _view.ToDate,
                    type = -1,
                    SearchWord =""
                };
                var result = _logService.LoadEmployeeLog(search);
                _view.DataGridViewBinding(result);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }            
        }

        private void LoadEmployeeInfo(int code)
        {
            try
            {
                var result = _service.LoadEmployeeInfo(code);
                _model = result;
                _original = CloneHelper.DeepCopy(result);

                _view.EmployeeCode = _model.EmployeeCode;
                _view.EmployeeName = _model.EmployeeName;
                _view.EmployeePhon = _model.EmployeePhon;
                _view.EmployeeLevel = _model.EmployeeLevel;
                _view.EmployeeStatus = _model.EmployeeStatus;
                _view.Employeememo = _model.Employeememo;
                _view.EmployeeIdate = _model.EmployeeIdate;
                _view.EmployeeUdate = _model.EmployeeUdate;
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void CheckIsNew(int code)
        {
            try
            {
                Dictionary<string, string> logColumns = new Dictionary<string, string>
            {
               {"logType", "작업내역"},
               {"logBefore", "변경전"},
               {"logAfter", "변경후"},
               {"logEmpName", "작업자명"},
               {"logEmp", "직원코드"},
               {"logDate", "변경일"},
            };
                if (code == 0)
                    _view.SetTabpage(true);
                else
                {
                    _view.SetTabpage(false, logColumns);
                }
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
                
        }
        

        private void SetPassword(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void SetAuthority(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void EmployeeSave(object sender, EventArgs e)
        {
            try
            {
                if (_model.EmployeeCode == 0)
                    _service.InsertEmployee(_model,UserSession.CurrentUser.EmployeeCode);
                else
                    _service.UpdateEmployee(_model,_logs);
                _view.CloseForm();
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
