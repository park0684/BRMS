using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace common.Helpers
{
    public class IniFileHelper
    {
        string _path;

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        public IniFileHelper()
        {
            _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dbconn.ini");
            if(!File.Exists(_path))
            {
                File.Create(_path).Close();
                var ini = new IniFileHelper();
                ini.Write("Database Config", "Address", "");
                ini.Write("Database Config", "Port", "");
                ini.Write("Database Config", "User", "");
                ini.Write("Database Config", "Password", "");
                ini.Write("Database Config", "Database", "");
            }
        }

        public string Read(string section, string key)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", retVal, 255, _path);
            return retVal.ToString();
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, _path);
        }
    }
}
