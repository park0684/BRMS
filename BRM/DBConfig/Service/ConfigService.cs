using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.DTOs;
using common.Helpers;
using System.Data.SqlClient;

namespace DBConfig.Service
{
    public class ConfigService
    {
        IniFileHelper _ini;
        public ConfigService()
        {
            _ini = new IniFileHelper();
        }
        public ConnectConfigDto LoadConnectionInfo()
        {
            string port = "1433";
            if(!string.IsNullOrEmpty(_ini.Read("Database Config", "Port")))
            {
                port = _ini.Read("Database Config", "Port");
            }
            var result = new ConnectConfigDto
            {
                Address = _ini.Read("Database Config", "Address"),
                Port = port,
                User = _ini.Read("Database Config", "User"),
                Password = _ini.Read("Database Config", "Password"),
                Database = _ini.Read("Database Config", "Database")
            };

            return result;
        }
        public void ConnectionTest(ConnectConfigDto config)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = config.Address + ',' + config.Port.ToString(),
                InitialCatalog = config.Database,
                UserID = config.User,
                Password = config.Password,
                ConnectTimeout = 5
            };

            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                conn.Open(); // 연결 실패 시 예외 발생
            }
        }

        public void SaveDatabaseConfig(ConnectConfigDto config)
        {
            _ini.Write("Database Config", "Address", config.Address);
            _ini.Write("Database Config", "Port", config.Port.ToString());
            _ini.Write("Database Config", "User", config.User);
            _ini.Write("Database Config", "Password", config.Password);
            _ini.Write("Database Config", "Database", config.Database);
        }

        public bool CheckChanged(ConnectConfigDto before, ConnectConfigDto after)
        {
            bool result = false;
            foreach(var porp in typeof(ConnectConfigDto).GetProperties())
            {
                object beforeValue = porp.GetValue(before);
                object afterValue = porp.GetValue(after);
                if((beforeValue== null && afterValue != null) || (beforeValue != null && !beforeValue.Equals(afterValue)))
                {
                    result = true;
                    return result;
                }
            }
            return result;
        }
        public bool ChekedPort(object port)
        {
            if (int.TryParse(port.ToString(), out int result))
            {
                return result >= 0; // 0이상의 숫자인 경우 true로 반환
            }
            return false;
        }
    }
}
