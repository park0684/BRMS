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
    public partial class CustomerLog : Form
    {
        int accessedEmp = cUserSession.AccessedEmp;
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet dgrLog = new cDataGridDefaultSet();
        static Dictionary<string, (int typeCode, string typeString)> parameter = new Dictionary<string, (int, string)>();
        public CustomerLog()
        {
            InitializeComponent();
            pnlDataGrid.Controls.Add(dgrLog.Dgv);
            dgrLog.Dgv.Dock = DockStyle.Fill;
            InitializeCheckBox();
            cLog.GetFilteredParameters(700, 800);//로그 조회를 위한 파라미터 범위 설정
            DataGridFrom();
        }
        /// <summary>
        /// 데이터 그리드 설정
        /// </summary>
        private void DataGridFrom()
        {
            dgrLog.Dgv.Columns.Add("logCustName", "회원명");            
            dgrLog.Dgv.Columns.Add("logParam", "코드");
            dgrLog.Dgv.Columns.Add("logType", "작업내역");
            dgrLog.Dgv.Columns.Add("logBefore", "변경전");
            dgrLog.Dgv.Columns.Add("logAfter", "변경후");
            dgrLog.Dgv.Columns.Add("logEmpName", "작업자명");
            dgrLog.Dgv.Columns.Add("logEmp", "직원코드");
            dgrLog.Dgv.Columns.Add("logDate", "변경일");
            dgrLog.Dgv.ReadOnly = true;
            dgrLog.ApplyDefaultColumnSettings();
            dgrLog.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgrLog.FormatAsDateTime("logDate");
            dgrLog.FormatAsStringLeft("logCustName", "logType", "logBefore", "logAfter");
            dgrLog.FormatAsStringCenter("logEmpName", "logEmp", "logParam");

        }
        /// <summary>
        /// 체크 박스 초기화
        /// 변경 항목에 대한 정보 등록
        /// </summary>
        private void InitializeCheckBox()
        {
            cmBoxWorkType.Items.Add("전체");
            foreach (var entry in parameter)
            {
                cmBoxWorkType.Items.Add(new KeyValuePair<int, string>(entry.Value.typeCode, entry.Value.typeString));
            }

            cmBoxWorkType.DisplayMember = "Value"; // 사용자에게 보여질 값
            cmBoxWorkType.ValueMember = "Key";    // 내부적으로 사용할 값
            cmBoxWorkType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmBoxWorkType.SelectedIndex = 0;
        }
        /// <summary>
        /// 데이터 그리드에 데이더 등록
        /// </summary>
        /// <param name="dataTable"></param>
        private void FillGrid(DataTable dataTable)
        {
            dgrLog.Dgv.Rows.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                DataTable readData = new DataTable();
                object resultObj = new object();
                int custCode = Convert.ToInt32(row["custlog_param"]);
                string query = $"SELECT cust_name FROM customer WHERE cust_code = {custCode} ";
                dbconn.sqlScalaQuery(query, out resultObj);
                
                string custName = resultObj.ToString();
                
                query = $"SELECT emp_name FROM employee WHERE emp_code = {row["custlog_emp"]}";
                dbconn.sqlScalaQuery(query, out resultObj);
                string empName = resultObj.ToString();

                int addRow = dgrLog.Dgv.Rows.Add();
                // 로그 데이터 설정
                string before = row["custlog_before"].ToString();
                string after = row["custlog_after"].ToString();
                switch (Convert.ToInt32(row["custlog_type"]))
                {
                    case 706://국가
                        query = $"SELECT ctry_name FROM country WHERE ctry_code = {before}";
                        dbconn.sqlScalaQuery(query, out resultObj);
                        before = resultObj.ToString();
                        query = $"SELECT ctry_name FROM country WHERE ctry_code = {after}";
                        dbconn.sqlScalaQuery(query, out resultObj);
                        after = resultObj.ToString();
                        break;
                    case 707://회원상태
                        before = cStatusCode.GetCustomerStatus(Convert.ToInt32(before));
                        after = cStatusCode.GetCustomerStatus(Convert.ToInt32(after));
                        break;
                }

                string empCode = row["custlog_emp"].ToString();
                string logDate = Convert.ToDateTime(row["custlog_date"]).ToString("yyyy-MM-dd HH:mm");
                string logType = "";

                var typeInfo = parameter.Values.FirstOrDefault(x => x.typeCode == (int)row["custlog_type"]);
                if (typeInfo.typeString != null)
                {
                    logType = typeInfo.typeString;  // 해당 typeString 값을 사용
                }

                // 해당 셀에 값 설정
                dgrLog.Dgv.Rows[addRow].Cells["No"].Value = addRow + 1;
                dgrLog.Dgv.Rows[addRow].Cells["logCustName"].Value = custName;;
                dgrLog.Dgv.Rows[addRow].Cells["logParam"].Value = custCode;
                dgrLog.Dgv.Rows[addRow].Cells["logType"].Value = logType;  // 여기서 값을 설정
                dgrLog.Dgv.Rows[addRow].Cells["logBefore"].Value = before;
                dgrLog.Dgv.Rows[addRow].Cells["logAfter"].Value = after;
                dgrLog.Dgv.Rows[addRow].Cells["logEmpName"].Value = empName;
                dgrLog.Dgv.Rows[addRow].Cells["logEmp"].Value = empCode;
                dgrLog.Dgv.Rows[addRow].Cells["logDate"].Value = logDate;
            }
        }
        /// <summary>
        /// 로그내역 조회
        /// </summary>
        private void QuerySetting()
        {
            string fromDate = dtpDateFrom.Value.ToString("yyyy-MM-dd");
            string toDate = dtpDateTo.Value.AddDays(1).ToString("yyyy-MM-dd");
            DataTable resultData = new DataTable();
            string query = $"SELECT custlog_type, custlog_before, custlog_after, custlog_param, custlog_emp, custlog_date FROM customerlog WHERE custlog_date > '{fromDate}' AND custlog_date < '{toDate}'";
            if(cmBoxWorkType.SelectedItem is KeyValuePair<int, string> selectedItem)
            {
                query += $" AND custlog_type = {selectedItem.Key}";
            }

            if (!string.IsNullOrEmpty(tBoxSearch.Text))
            {
                string subQuery = $"SELECT distinct(cust_code) FROM customer WHERE cust_name LIKE '%{tBoxSearch.Text}%'";
                dbconn.SqlDataAdapterQuery(subQuery, resultData);
                string resultString = "";
                foreach (DataRow subRow in resultData.Rows)
                {
                    if (string.IsNullOrEmpty(resultString))
                    {
                        resultString = subRow[0].ToString();
                    }
                    resultString += ", " + subRow[0].ToString();
                }
                query += $"AND custlog_param IN ({resultString})";
            }
            query += "ORDER BY custlog_date";
            resultData.Rows.Clear();
            dbconn.SqlDataAdapterQuery(query, resultData);
            FillGrid(resultData);
        }
        /// <summary>
        /// 조회 실행
        /// 조회 성공 후 로그 조회 내역도 직원접속로그에 기록
        /// </summary>
        public void RunQuery()
        {
            try
            {
                QuerySetting();
                cLog.InsertEmpAccessLogNotConnect("@customerLogSearch", accessedEmp, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
