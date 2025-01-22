using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRMS
{
    class cUserSession
    {
        private static int _accessedEmp;
        private static Dictionary<int, string> _accessPermission;

        // 직원 코드 설정/가져오기
        public static int AccessedEmp
        {
            get { return _accessedEmp; }
            set { _accessedEmp = value; }
        }

        // 권한 설정/가져오기
        public static Dictionary<int, string> AccessPermission
        {
            get { return _accessPermission; }
            set { _accessPermission = value; }
        }

        public static bool HasPermission(int requiredPermission)
        {
            return _accessPermission.ContainsKey(requiredPermission);
        }
    }
}
