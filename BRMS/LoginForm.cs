using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRMS
{
    public partial class LoginForm : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        public Dictionary<int, string> accessPermission { get; private set; } = new Dictionary<int, string>();
        public int accessedEmp { get; private set; }
        string empPassword;
        bool errorCheck = false;
        public bool login { get; private set; } = false;
        public LoginForm()
        {
            InitializeComponent();
            cUIManager.ApplyPopupFormStyle(this);
            tBoxEmpName.Enabled = false;
            tBoxEmpCode.KeyUp += tBoxEmpcode_KeyUp;
            tBoxPassword.KeyDown += tBoxPassword_KeyDown;
            tBoxPassword.PasswordChar = '*';
        }
        /// <summary>
        /// 직원코드 입력 텍스트 박스 키이벤트 생성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBoxEmpcode_KeyUp(object sender, KeyEventArgs e)
        {
            cDataHandler.AllowOnlyInteger(sender, e, tBoxEmpCode);
            if (e.KeyCode == Keys.Enter)
            {
                GetEmployeeInfo();
                tBoxPassword.Focus();
            }
        }
        
        /// <summary>
        /// 패스워드 입력 텍스트 박스 키이벤트 생성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }
        /// <summary>
        /// 직원코드 입력 후 직원이름 텍스트박스에 표시
        /// </summary>
        private void GetEmployeeInfo()
        {
            DataTable resultData = new DataTable();
            if(tBoxEmpCode.Text !="")
                accessedEmp = Convert.ToInt32(tBoxEmpCode.Text);
            string query = $"SELECT emp_name, emp_password FROM employee WHERE emp_code = {accessedEmp} AND emp_status = 1";
            dbconn.SqlReaderQuery(query, resultData);
            if(resultData.Rows.Count == 0)
            {
                cUIManager.ShowMessageBox("직원코드를 확인해 주세요", "알림", MessageBoxButtons.OK);
                return;
            }
            DataRow empRow = resultData.Rows[0];
            tBoxEmpName.Text = empRow["emp_name"].ToString().Trim();
            empPassword = empRow["emp_password"].ToString();
            DecryptPassword();
        }
        /// <summary>
        /// 패스워드 복호화
        /// </summary>
        private void DecryptPassword()
        {
            var cryptor = new cCryptor("shared-passphrase");
            if (!string.IsNullOrEmpty(empPassword))
            {
                empPassword = cryptor.Decrypt(empPassword);
            }
        }
        /// <summary>
        /// 오류 여부 확인
        /// </summary>
        private void ErrorCheck()
        {
            if(tBoxEmpName.Text == "")
            {
                cUIManager.ShowMessageBox("직원코드를 입력해 주세요", "알림", MessageBoxButtons.OK);
                errorCheck = true;
                return;
            }
            if(tBoxPassword.Text != empPassword)
            {
                cUIManager.ShowMessageBox("비밀번호가 일치 하지 않습니다", "경고", MessageBoxButtons.OK);
                errorCheck = true;
            }
        }
        /// <summary>
        /// 로그인한 직원 권한 조회
        /// </summary>
        private void GetEmployeePermission()
        {
            DataTable permissoinData = new DataTable();
            string query = $"SELECT acper_permission FROM accpermission WHERE acper_emp = {accessedEmp} AND acper_status = 1";
            dbconn.SqlDataAdapterQuery(query, permissoinData);
            foreach(DataRow permissoin in permissoinData.Rows)
            {
                int permissionCode = Convert.ToInt32(permissoin["acper_permission"]);
                if (!accessPermission.ContainsKey(permissionCode))
                {
                    accessPermission.Add(permissionCode, "GRANTED");
                }
            }
        }
        /// <summary>
        /// 로그인 실행
        /// </summary>
        private void Login()
        {
            errorCheck = false;
            ErrorCheck();
            if(errorCheck != true)
            {
                GetEmployeePermission();
                cUserSession.AccessedEmp = accessedEmp;
                cUserSession.AccessPermission = accessPermission;
                login = true;
                Close();
            }

        }

        private void bntOk_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void bntCancle_Click(object sender, EventArgs e)
        {
            Close();           
        }
    }
}
