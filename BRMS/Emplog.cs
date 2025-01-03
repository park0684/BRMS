﻿using System;
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
    public partial class Emplog : Form
    {
        int accessedEmp = cUserSession.AccessedEmp;
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet dgrLog = new cDataGridDefaultSet();
        static Dictionary<string, (int typeCode, string typeString)> parameter = new Dictionary<string, (int, string)>();
        public Emplog()
        {
            InitializeComponent();
            pnlDataGrid.Controls.Add(dgrLog.Dgv);
            dgrLog.Dgv.Dock = DockStyle.Fill;
            parameter = cLog.GetFilteredParameters(800, 900);// 로그 파라미터 받아오기
            InitializeCombBox();
            DataGridFrom();
        }
        /// <summary>
        /// 데이터 그리드 설정
        /// </summary>
        private void DataGridFrom()
        {
            dgrLog.Dgv.Columns.Add("logName", "직원명");
            dgrLog.Dgv.Columns.Add("logEmpCode", "직원코드");
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
            dgrLog.FormatAsStringLeft("logType", "logBefore", "logAfter");
            dgrLog.FormatAsStringCenter("logName", "logEmpCode", "logEmpName", "logEmp");
        }
        /// <summary>
        /// 콤보박스 초기 설정
        /// </summary>
        private void InitializeCombBox()
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
        /// 조회된 변경 내역 데이터 그리드에 등록
        /// </summary>
        /// <param name="dataTable"></param>
        private void FillGrid(DataTable dataTable)
        {
            if(dataTable.Rows.Count < 1)
            {
                return;
            }
            dgrLog.Dgv.Rows.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                DataTable readData = new DataTable();
                object resultObj = new object();
                int param = Convert.ToInt32(row["emplog_param"]);
                int empCode = Convert.ToInt32(row["emplog_emp"]);
                string param2="";
                //변경 대상 직원이름 조회
                string query = $"SELECT emp_name  FROM employee WHERE emp_code = {param}";
                dbconn.sqlScalaQuery(query, out resultObj);
                string tagetEmpName = resultObj.ToString();

                //작업자 이름 조회
                query = $"SELECT emp_name FROM employee WHERE emp_code = {empCode}";
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
                        if (before!="")
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
                int addRow = dgrLog.Dgv.Rows.Add();
                dgrLog.Dgv.Rows[addRow].Cells["No"].Value = addRow + 1;
                dgrLog.Dgv.Rows[addRow].Cells["logName"].Value = tagetEmpName;
                dgrLog.Dgv.Rows[addRow].Cells["logEmpCode"].Value = param;
                dgrLog.Dgv.Rows[addRow].Cells["logType"].Value = logType;  // 여기서 값을 설정
                dgrLog.Dgv.Rows[addRow].Cells["logBefore"].Value = before;
                dgrLog.Dgv.Rows[addRow].Cells["logAfter"].Value = after;
                dgrLog.Dgv.Rows[addRow].Cells["logEmpName"].Value = empName;
                dgrLog.Dgv.Rows[addRow].Cells["logEmp"].Value = empCode;
                dgrLog.Dgv.Rows[addRow].Cells["logDate"].Value = logDate;
            }
        }
        /// <summary>
        /// 조회 쿼리 설정
        /// </summary>
        private void QuerySetting()
        {
            string fromDate = dtpDateFrom.Value.ToString("yyyy-MM-dd");
            string toDate = dtpDateTo.Value.AddDays(1).ToString("yyyy-MM-dd");
            DataTable resultData = new DataTable();
            string query = $"SELECT emplog_type, emplog_before,emplog_after, emplog_param,emplog_param2, emplog_emp, emplog_date FROM emplog WHERE emplog_date > '{fromDate}' AND emplog_date < '{toDate}'";
            if (cmBoxWorkType.SelectedItem is KeyValuePair<int, string> selectedItem)
            {
                query += $" AND emplog_type = {selectedItem.Key}";
            }
            if (!string.IsNullOrEmpty(tBoxSearch.Text))
            {
                string pdtQuery = $"SELECT distinct(emp_code) FROM employee WHERE pdt_name like '%{tBoxSearch.Text}%'";
                dbconn.SqlDataAdapterQuery(pdtQuery, resultData);
                string resultString = "";
                foreach (DataRow pdtRow in resultData.Rows)
                {
                    if (string.IsNullOrEmpty(resultString))
                    {
                        resultString = pdtRow[0].ToString();
                    }
                    resultString += ", " + pdtRow[0].ToString();
                }
                query += $"AND emplog_param IN ({resultString})";
            }
            query += " ORDER BY emplog_date";
            dbconn.SqlDataAdapterQuery(query, resultData);
            FillGrid(resultData);
        }
        public void RunQuery()
        {
            try
            {
                QuerySetting();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
