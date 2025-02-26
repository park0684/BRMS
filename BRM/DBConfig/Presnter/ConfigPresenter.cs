using common.DTOs;
using DBConfig.Service;
using DBConfig.View;
using System;

namespace DBConfig.Presnter
{
    class ConfigPresenter
    {
        IConfigView _view;
        ConfigService _service;
        ConnectConfigDto _Config;

        public ConfigPresenter(IConfigView view)
        {
            _view = view;
            _service = new ConfigService();
            _Config = new ConnectConfigDto();
            _view.CloseFormEvent += CloseForm;
            _view.SaveEvent += SaveDatabaseConfig;
            _view.ConnectionTestEvent += ConnectionTest;
            _view.PortChangedEvent += checkedPort;
            GetDatabaseInfo();
        }

        private void checkedPort(object sender, EventArgs e)
        {
            if (!_service.ChekedPort(_view.Port))
            {
                _view.ShowMessage("입력 오류 : 숫자만 입력 하세요");
                _view.Port = _Config.Port;
            }
                
        }

        private void ConnectionTest(object sender, EventArgs e)
        {
            try
            {
                var testConfig = SetConfig();
                _service.ConnectionTest(testConfig);
                _view.ShowMessage("데이터 베이스 연결 정상");
            }
            catch(Exception ex)
            {
                _view.ShowMessage($"연결 실패 : {ex.Message}");
            }
        }

        private void SaveDatabaseConfig(object sender, EventArgs e)
        {
            try
            {
                var configInfo = SetConfig();
                _service.SaveDatabaseConfig(configInfo);
                _view.CloseForm();
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void CloseForm(object sender, EventArgs e)
        {
            var before = new ConnectConfigDto
            {
                Address = _Config.Address,
                Port = _Config.Port,
                User = _Config.User,
                Password = _Config.Password,
                Database = _Config.Database
            };
            var after = SetConfig();
            bool check = _service.CheckChanged(before, after);
            if (check)
            {
                string message = "변경 사항이 있습니다.\n저장하지 않고 종료 하시겠습니까?";
                if (_view.ShowConfirmMessage(message))
                    _view.CloseForm();
                else
                    return;
            }
            else
            {
                _view.CloseForm();
            }
            
        }
        
        private void GetDatabaseInfo()
        {
            try
            {
                _Config = _service.LoadConnectionInfo();

                _view.Address = _Config.Address;
                _view.Port = _Config.Port;
                _view.User = _Config.User;
                _view.Password = _Config.Password;
                _view.Database = _Config.Database;
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }
        private ConnectConfigDto SetConfig()
        {
            ConnectConfigDto config = new ConnectConfigDto
            {
                Address = _view.Address,
                Port = _view.Port,
                User = _view.User,
                Password = _view.Password,
                Database = _view.Database
            };

            return config;
        }

    }
}
