using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace BRMS
{
    public partial class EmployeeDetail : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet DgrLog = new cDataGridDefaultSet();

        Dictionary<string, object> originalValues = new Dictionary<string, object>();
        Dictionary<int, int> employeePermission = new Dictionary<int, int>();

        DataTable permissionData = new DataTable();//조회 직원 권한 
        static Dictionary<string, (int typeCode, string typeString)> parameter = new Dictionary<string, (int, string)>();

        int accessedEmp = cUserSession.AccessedEmp;

        int empCode = 0;
        string empPassword = "";
        bool isNewEntry = false;
        bool errorCheck = false;

        public EmployeeDetail()
        {
            InitializeComponent();
            cUIManager.ApplyPopupFormStyleㅡ(this);
            InitializeCombox();
            InitializeTabControl();
        }
        /// <summary>
        /// 등록/수정 권한 여부 확인 후 버튼 비활성화 여부 결정
        /// </summary>
        private void CheckModifyPermission()
        {
            if (!cUserSession.HasPermission(502))
            {
                btnPassWord.Enabled = false;
                btnSetGrade.Enabled = false;
                bntSave.Enabled = false;
            }
            if (accessedEmp == empCode)// 로그인 본인의 정보 수정은 권한 제외 모두 수정 가능
            {
                btnPassWord.Enabled = true;
                bntSave.Enabled = true;
            }
        }


        private void InitializeCombox()
        {
            foreach (var status in cStatusCode.EmployeeStatus)
            {
                cmBoxStatus.Items.Add(new KeyValuePair<int, string>(status.Key, status.Value));
            }
            cmBoxStatus.DisplayMember = "Value"; // 사용자에게 보여질 값
            cmBoxStatus.ValueMember = "Key";    // 내부적으로 사용할 값
            cmBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmBoxStatus.SelectedIndex = 1;
        }
        /// <summary>
        /// 탭페이지 설정
        /// </summary>
        private void InitializeTabControl()
        {
            // TabControl에 접근하여 페이지 이름 설정
            tabCtrlEmployee.TabPages[0].Text = "직원정보";
            tabCtrlEmployee.TabPages[1].Text = "변경로그";
        }
        /// <summary>
        /// 직원조회메뉴에서 직원 더블클릭 또는 새 직원 등록 버튼을 눌러 호출
        /// </summary>
        /// <param name="emp"></param>새 직원을 눌럭을 때는 직원코드는 0으로, 더블 클릭시 선택한 직원의 코드로 적용
        public void CallEmployee(int emp)
        {
            if (emp == 0)
            {
                AddEmoloyee();
            }
            else
            {
                LoadEmployeeInfo(emp);
            }

        }
        /// <summary>
        /// 직원 조회시 정보를 가지고옴
        /// </summary>
        /// <param name="emp"></param>
        private void LoadEmployeeInfo(int emp)
        {
            DataTable resultTable = new DataTable();
            string query = $"SELECT emp_name, emp_password, emp_cell, emp_level, emp_email, emp_addr, emp_status, emp_idate, emp_udate, emp_memo FROM employee WHERE emp_code = {emp}";
            dbconn.SqlReaderQuery(query, resultTable);
            DataRow row = resultTable.Rows[0];
            empCode = emp;
            tBoxEmpName.Text = row["emp_name"].ToString();
            tBoxLevel.Text = row["emp_level"].ToString();
            tBoxCell.Text = row["emp_cell"].ToString();
            tBoxAddress.Text = row["emp_addr"].ToString();
            tBoxEmail.Text = row["emp_email"].ToString();
            tBoxMemo.Text = row["emp_memo"].ToString();
            lblEmpCode.Text = empCode.ToString();
            lblRegDate.Text = Convert.ToDateTime(row["emp_idate"]).ToString("yyyy-MM-dd");
            lblUpdate.Text = Convert.ToDateTime(row["emp_udate"]).ToString("yyyy-MM-dd");
            empPassword = row["emp_password"].ToString();
            LoadEmployeePermission(emp);
            if (empPassword != "")
            {
                DecryptPassword();
            }
            RegisterOriginalData();
            DataGridFrom();
            parameter = cLog.GetFilteredParameters(800, 900);// 로그 파라미터 받아오기
            CheckModifyPermission();
            //ParameterSet();
        }
        /// <summary>
        /// 새 직원 등록시 신규직원 여부 확인하는 isNewEntry는 true로 전환하고
        /// 직원로그를 확인하는 탭페이지2 는 삭제
        /// </summary>
        private void AddEmoloyee()
        {
            isNewEntry = true;
            tabCtrlEmployee.TabPages.Remove(tabPage2);
            lblEmpCode.Visible = false;
            lblRegDate.Visible = false;
            lblUpdate.Visible = false;
        }
        /// <summary>
        /// 변경 로그 데이터 그리드 폼
        /// </summary>
        private void DataGridFrom()
        {
            pnlDataGrid.Controls.Add(DgrLog.Dgv);
            DgrLog.Dgv.Dock = DockStyle.Fill;
            DgrLog.Dgv.Columns.Add("logType", "작업내역");
            DgrLog.Dgv.Columns.Add("logBefore", "변경전");
            DgrLog.Dgv.Columns.Add("logAfter", "변경후");
            DgrLog.Dgv.Columns.Add("logEmpName", "작업자명");
            DgrLog.Dgv.Columns.Add("logEmp", "직원코드");
            DgrLog.Dgv.Columns.Add("logDate", "변경일");
            DgrLog.Dgv.ReadOnly = true;
            DgrLog.ApplyDefaultColumnSettings();
            DgrLog.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgrLog.FormatAsDateTime("logDate");
            DgrLog.FormatAsStringLeft("logType", "logBefore", "logAfter");
            DgrLog.FormatAsStringCenter("logEmpName", "logEmp");
        }

        /// <summary>
        /// 변경 로그 조회 쿼리 설정
        /// </summary>
        private void QuerySetting()
        {
            string fromDate = dtpFrom.Value.ToString("yyyy-MM-dd");
            string toDate = dtpTo.Value.AddDays(1).ToString("yyyy-MM-dd");
            DataTable resultData = new DataTable();
            string query = $"SELECT emplog_type, emplog_before, emplog_after, emplog_param2, emplog_emp, emplog_date FROM emplog WHERE emplog_param = {empCode} AND emplog_date > '{fromDate}' AND emplog_date < '{toDate}' ORDER BY emplog_date"; ;
            dbconn.SqlDataAdapterQuery(query, resultData);
            FillGrid(resultData);
        }
        /// <summary>
        /// 변경 로그 데이터 그리드 내용 삽입
        /// </summary>
        /// <param name="dataTable"></param>
        private void FillGrid(DataTable dataTable)
        {
            if (dataTable.Rows.Count < 1)
            {
                return;
            }
            DgrLog.Dgv.Rows.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                DataTable readData = new DataTable();
                object resultObj = new object();
                int empCode = Convert.ToInt32(row["emplog_emp"]);
                string param2 = "";

                //작업자 이름 조회
                string query = $"SELECT emp_name FROM employee WHERE emp_code = {empCode}";
                dbconn.sqlScalaQuery(query, out resultObj);

                string empName = resultObj.ToString();

                // 로그 데이터 설정
                string before = row["emplog_before"].ToString();
                string after = row["emplog_after"].ToString();

                switch (Convert.ToInt32(row["emplog_type"]))
                {
                    case 809://직원 상태 변경
                        before = cStatusCode.GetEmployeeStatus(Convert.ToInt32(before));
                        after = cStatusCode.GetEmployeeStatus(Convert.ToInt32(after));
                        break;
                    case 807://직원 권한 변경
                        param2 = cStatusCode.GetEmployeePermission(Convert.ToInt32(row["emplog_param2"]));
                        if (before != "")
                        {
                            before = Convert.ToInt32(row["emplog_before"]) == 1 ? "○" : "Ｘ";
                        }
                        after = Convert.ToInt32(row["emplog_after"]) == 1 ? "○" : "Ｘ";

                        before = param2 + $" ({before})";
                        after = param2 + $" ({after})";
                        break;
                }
                string logDate = Convert.ToDateTime(row["emplog_date"]).ToString("yyyy-MM-dd HH:mm");
                string logType = "";

                var typeInfo = parameter.Values.FirstOrDefault(x => x.typeCode == (int)row["emplog_type"]);
                if (typeInfo.typeString != null)
                {
                    logType = typeInfo.typeString;  // 해당 typeString 값을 사용
                }
                // 해당 셀에 값 설정
                int addRow = DgrLog.Dgv.Rows.Add();
                DgrLog.Dgv.Rows[addRow].Cells["No"].Value = addRow + 1;
                DgrLog.Dgv.Rows[addRow].Cells["logType"].Value = logType;  // 여기서 값을 설정
                DgrLog.Dgv.Rows[addRow].Cells["logBefore"].Value = before;
                DgrLog.Dgv.Rows[addRow].Cells["logAfter"].Value = after;
                DgrLog.Dgv.Rows[addRow].Cells["logEmpName"].Value = empName;
                DgrLog.Dgv.Rows[addRow].Cells["logEmp"].Value = empCode;
                DgrLog.Dgv.Rows[addRow].Cells["logDate"].Value = logDate;
            }
        }
        /// <summary>
        /// 직원 권한을 받아와 permissionData 테이블에 정보 저장
        /// </summary>
        /// <param name="empCode"></param>
        private void LoadEmployeePermission(int empCode)
        {
            string qeury = $"SELECT acper_permission, acper_status FROM accpermission WHERE acper_emp ={empCode}";

            dbconn.SqlDataAdapterQuery(qeury, permissionData);

            foreach (DataRow row in permissionData.Rows)
            {
                int permission = Convert.ToInt32(row["acper_permission"]);
                int status = Convert.ToInt32(row["acper_status"]);
                employeePermission[permission] = status;
            }
        }
        /// <summary>
        /// 조회된 원본 데이터 originalValues 딕셔너리에 등록
        /// 수정시 원본과 수정본을 비교하여 로그 생성시 before 데이터로 사용
        /// </summary>
        private void RegisterOriginalData()
        {
            originalValues["@empName"] = tBoxEmpName.Text;
            originalValues["@empLevel"] = tBoxLevel.Text;
            originalValues["@empCell"] = tBoxCell.Text;
            originalValues["@empAddr"] = tBoxAddress.Text;
            originalValues["@empEmail"] = tBoxEmail.Text;
            originalValues["@empMemo"] = tBoxMemo.Text;
            originalValues["@empPassword"] = empPassword;
        }
        /// <summary>
        /// 직원 비밀번호 복호화
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
        /// 직원 비밀번호 암호화
        /// </summary>
        private void EncryptPassword()
        {
            var cryptor = new cCryptor("shared-passphrase");

            if (!string.IsNullOrEmpty(empPassword))
            {
                empPassword = cryptor.Encrypt(empPassword);
            }
        }
        /// <summary>
        /// 설정된 직원 권한 데이터 베이스 등록
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void InsertPermisstionToData(SqlConnection connection, SqlTransaction transaction)
        {
            foreach (var kvp in employeePermission)
            {
                int permissionKey = kvp.Key;
                int status = kvp.Value;

                // Check if the key exists in permissionData
                DataRow existingRow = permissionData.AsEnumerable()
                    .FirstOrDefault(row => Convert.ToInt32(row["acper_permission"]) == permissionKey);

                string before = existingRow != null ? existingRow["acper_status"].ToString() : "";

                if (existingRow == null)//기존 정보가 없다면 새로 등록
                {
                    // If the key does not exist, INSERT new record
                    string insertQuery = $"INSERT INTO accpermission (acper_emp,acper_permission,acper_status,acper_idate,acper_udate)" +
                    "VALUES(@emp, @param, @status, GETDATE(),GETDATE())";
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@emp", SqlDbType.Int){Value = empCode },
                        new SqlParameter("@param",SqlDbType.Int){Value =permissionKey},
                        new SqlParameter("@status",SqlDbType.Int){Value = status}
                    };
                    dbconn.ExecuteNonQuery(insertQuery, connection, transaction, parameters);
                }
                else//기존 정보가 있다면 업데이트로 수정
                {
                    // If the key exists, UPDATE the record
                    string updateQuery = "UPDATE accpermission SET acper_status = @status, acper_udate = GETDATE() WHERE acper_emp = @emp AND acper_permission = @param";
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@emp", SqlDbType.Int){Value = empCode },
                        new SqlParameter("@param",SqlDbType.Int){Value =permissionKey},
                        new SqlParameter("@status",SqlDbType.Int){Value = status}
                    };
                    dbconn.ExecuteNonQuery(updateQuery, connection, transaction, parameters);

                }
                if (status.ToString() != before)
                {
                    cLog.InsertDetailedLogToDatabase("@empPemission", before, status.ToString(), empCode, permissionKey, 0, accessedEmp, connection, transaction);
                }
            }
        }
        /// <summary>
        /// 신규직원 저장
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void InsertEmployeeToDatabase(SqlConnection connection, SqlTransaction transaction)
        {
            object resutObj = new object();
            string query = "SELECT ISNULL(MAX(emp_code),0) +1 FROM employee";
            dbconn.sqlScalaQuery(query, out resutObj);
            query = "INSERT INTO employee(emp_code, emp_name, emp_password, emp_level, emp_cell, emp_email,emp_addr,  emp_status, emp_idate, emp_udate, emp_memo)" +
                "VALUES(@empCode, @empName, @empPassword, @empLevel, @empCell, @empEmail, @empAddr, @empStatus, GETDATE(), GETDATE(), @empMemo)";
            empCode = Convert.ToInt32(resutObj);
            SqlParameter[] parameters =
            {
                new SqlParameter("@empCode",SqlDbType.Int){Value = empCode},
                new SqlParameter("@empName",SqlDbType.VarChar){Value = tBoxEmpName.Text},
                new SqlParameter("@empPassword",SqlDbType.VarChar){Value = empPassword},
                new SqlParameter("@empLevel",SqlDbType.VarChar){Value = tBoxLevel.Text},
                new SqlParameter("@empCell",SqlDbType.VarChar){Value = tBoxCell.Text},
                new SqlParameter("@empEmail",SqlDbType.VarChar){Value = tBoxEmail.Text},
                new SqlParameter("@empAddr",SqlDbType.VarChar){Value = tBoxAddress.Text},
                new SqlParameter("@empStatus",SqlDbType.Int){Value = cmBoxStatus.SelectedIndex},
                new SqlParameter("@empMemo",SqlDbType.VarChar){Value = tBoxMemo.Text}
            };

            dbconn.ExecuteNonQuery(query, connection, transaction, parameters);

        }
        /// <summary>
        /// 직원정보 수정시 저장 메소드
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void ModifyEmployeeTodDatabse(SqlConnection connection, SqlTransaction transaction)
        {
            string query = $"UPDATE employee SET emp_name = @empname, emp_password = @empPassword, emp_level = @empLevel, emp_cell = @empCell, emp_email = @empEmail, " +
                $"emp_addr = @empAddr, emp_status = @empStatus, emp_memo = @empMemo, emp_udate = GETDATE() WHERE emp_code =@empCode";
            SqlParameter[] parameters =
            {
                new SqlParameter("@empCode",SqlDbType.Int){Value = empCode},
                new SqlParameter("@empName",SqlDbType.VarChar){Value = tBoxEmpName.Text},
                new SqlParameter("@empPassword",SqlDbType.VarChar){Value = empPassword},
                new SqlParameter("@empLevel",SqlDbType.VarChar){Value = tBoxLevel.Text},
                new SqlParameter("@empCell",SqlDbType.VarChar){Value = tBoxCell.Text},
                new SqlParameter("@empEmail",SqlDbType.VarChar){Value = tBoxEmail.Text},
                new SqlParameter("@empAddr",SqlDbType.VarChar){Value = tBoxAddress.Text},
                new SqlParameter("@empStatus",SqlDbType.Int){Value = cmBoxStatus.SelectedIndex},
                new SqlParameter("@empMemo",SqlDbType.VarChar){Value = tBoxMemo.Text}
            };

            Dictionary<string, string> modifiedValues = new Dictionary<string, string>();
            foreach (var param in parameters)
            {
                modifiedValues[param.ParameterName] = param.Value.ToString();
            }
            EncryptPassword();
            parameters[2].Value = empPassword;
            dbconn.ExecuteNonQuery(query, connection, transaction, parameters);

            cLog.IsModified(originalValues, modifiedValues, empCode, accessedEmp, connection, transaction);
        }
        private void ErrorCheck()
        {
            if (string.IsNullOrEmpty(empPassword))
            {
                cUIManager.ShowMessageBox("비밀번호를 설정하지 않았습니다.", "알림", MessageBoxButtons.OK);
                errorCheck = true;
                return;
            }
        }
        /// <summary>
        /// 저장 버튼 클릭 시 신규 직원이라면 InsertEmployeeToData를 실행하고, 수정이라면 ModifyEmployee 실행
        /// 신규 저장 여부는 isNewEntry이 true인지 false인지로 구분
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntSave_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("저장 하시겠습니까?", "알림", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = dbconn.Opensql())
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        ErrorCheck();
                        if (errorCheck == true)
                        {
                            return;
                        }
                        if (isNewEntry == true)
                        {
                            InsertEmployeeToDatabase(connection, transaction);
                            cLog.InsertEmpAccessLogConnect("@employeeRegisted", accessedEmp, empCode, connection, transaction);
                            InsertPermisstionToData(connection, transaction);
                        }
                        else
                        {
                            ModifyEmployeeTodDatabse(connection, transaction);
                            cLog.InsertEmpAccessLogConnect("@employeeModify", accessedEmp, empCode, connection, transaction);
                            InsertPermisstionToData(connection, transaction);
                        }
                        transaction.Commit();
                        Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }

        private void bntCancle_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 권한 설정 버튼 클릭시 기존의 조회된 정보를 기준으로 EmployeePermission에 데이터를 저장 후 폼호출
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetPermission_Click(object sender, EventArgs e)
        {
            EmployeePermission permission = new EmployeePermission();
            permission.StartPosition = FormStartPosition.CenterParent;
            permission.GetPermission(employeePermission);
            permission.PermissionsUpdated += (updatedPermissions) =>
            {
                employeePermission.Clear();
                foreach (var permissionEntry in updatedPermissions)
                {
                    employeePermission.Add(permissionEntry.Key, permissionEntry.Value);  // 새로운 값을 추가
                }
            };
            permission.ShowDialog();
        }

        private void btnPassWord_Click(object sender, EventArgs e)
        {
            EmployeePasswordSet employeePasswordSet = new EmployeePasswordSet();
            employeePasswordSet.StartPosition = FormStartPosition.CenterParent;
            employeePasswordSet.GetPassword(empPassword);
            employeePasswordSet.passwordSet += (newPassword) => { empPassword = newPassword; };
            employeePasswordSet.ShowDialog();
        }

        private void bntLogSearch_Click(object sender, EventArgs e)
        {
            QuerySetting();
        }
    }
}
