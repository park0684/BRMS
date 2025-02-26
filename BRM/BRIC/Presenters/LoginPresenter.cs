using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.Interface;
using BRIC.Repositories;
using common.DTOs;
using BRIC.Services;
using common.Model;

namespace BRIC.Presenters
{
    public class LoginPresenter
    {
        ILoginView _view;
        LoginService _service;
        UserDto _model;

        public LoginPresenter(ILoginView view)
        {
            _view = view;
            _service = new LoginService();
            _model = new UserDto();
            _view.GetEmployeeNameEvent += LoadEmployeeInfo;
            _view.LoginEvent += Loging;
            _view.CloseFormEvent += CloseForm;
            _view.ShowForm();
        }

        private void Loging(object sender, EventArgs e)
        {
            try
            {
                if(_model == null)
                {
                    _view.ShowMessage("직원코드를 먼저 입력해주세요");
                    return;
                }
                string password = _model.EmployeePassword;
                if( _view.Passwordk != password)
                {
                    _view.ShowMessage("로그인 실패 : 비밀번호 불일치");
                    return;
                }
                _model.PermissionList = _service.LoadPermission(_model.EmployeeCode);
                UserSession.CurrentUser = _model;
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessage("로그인 실패 : " + ex.Message);
            }
        }

        private void LoadEmployeeInfo(object sender, EventArgs e)
        {
            try
            {
                var result = _service.LoadEmployeeName(_view.EmployeeCode);
                if (result == null)
                {
                    _view.ShowMessage("로그인 실패 : 직원코드를 확인해 주세요");
                    return;
                }
                _model.EmployeePassword = result["emp_password"].ToString();
                _model.EmployeeName = result["emp_name"].ToString();
                _model.EmployeeCode = _view.EmployeeCode;
                _view.EmployeeName = _model.EmployeeName;
                _view.FocusMove();
            }
            catch(Exception ex)
            {
                _view.ShowMessage("로그인 실패 : " + ex.Message);
            }
            
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
