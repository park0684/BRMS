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
    public partial class SupplierLog : Form
    {
        int accessedEmp = cUserSession.AccessedEmp;
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet dgrLog = new cDataGridDefaultSet();
        static Dictionary<string, (int typeCode, string typeString)> parameter = new Dictionary<string, (int, string)>();
        public SupplierLog()
        {
            InitializeComponent();
            pnlDataGrid.Controls.Add(dgrLog.Dgv);
            dgrLog.Dgv.Dock = DockStyle.Fill;
            ParameterSet();
            checkBoxSetting();
            DataGridFrom();
        }
        private void DataGridFrom()
        {
            dgrLog.Dgv.Columns.Add("logSupname", "공급사명");
            dgrLog.Dgv.Columns.Add("logSupCode", "코드");
            dgrLog.Dgv.Columns.Add("logParam", "파라메터");
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
            dgrLog.FormatAsStringLeft("logSupname", "logType", "logBefore", "logAfter");
            dgrLog.FormatAsStringCenter("logEmpName", "logSupCode", "logEmp");
            dgrLog.Dgv.Columns["logParam"].Visible = false;
        }

        private void ParameterSet()
        {
            foreach (var entry in cLog.logParameter)
            {
                if (entry.Value.typeCode >= 200 && entry.Value.typeCode < 300)
                {
                    parameter[entry.Key] = entry.Value;
                }
            }
        }
        private void checkBoxSetting()
        {
            
            foreach (var entry in parameter)
            {
                cmBoxWorkType.Items.Add(new KeyValuePair<int, string>(entry.Value.typeCode, entry.Value.typeString));
            }

            cmBoxWorkType.DisplayMember = "Value"; // 사용자에게 보여질 값
            cmBoxWorkType.ValueMember = "Key";    // 내부적으로 사용할 값
            cmBoxWorkType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmBoxWorkType.SelectedValue = 0;
        }
        private void FillGrid(DataTable dataTable)
        {
            dgrLog.Dgv.Rows.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                DataTable readData = new DataTable();
                object resultObj = new object();

                string query = $"SELECT sup_name FROM supplier WHERE sup_code = {row["suplog_param"]}";
                dbconn.sqlScalaQuery(query, out resultObj);
                string supName = resultObj.ToString();

                query = $"SELECT emp_name FROM employee WHERE emp_code = {row["suplog_emp"]}";
                dbconn.sqlScalaQuery(query, out resultObj);
                string empName = resultObj.ToString();

                int addRow = dgrLog.Dgv.Rows.Add();
                // 로그 데이터 설정
                string before = row["suplog_before"].ToString();
                string after = row["suplog_after"].ToString();
                switch (Convert.ToInt32(row["suplog_type"]))
                {
                    case 215:
                        before = cStatusCode.GetSupplierStatus(Convert.ToInt32(before));
                        after = cStatusCode.GetSupplierStatus(Convert.ToInt32(after));
                        break;
                    case 217:
                        before = cStatusCode.GetSupplierPayment(Convert.ToInt32(before));
                        after = cStatusCode.GetTaxStatus(Convert.ToInt32(after));
                        break;
                }
                
                string empCode = row["suplog_emp"].ToString();
                string logDate = Convert.ToDateTime(row["suplog_date"]).ToString("yyyy-MM-dd HH:mm");
                string logType = "";

                var typeInfo = parameter.Values.FirstOrDefault(x => x.typeCode == (int)row["suplog_type"]);
                if (typeInfo.typeString != null)
                {
                    logType = typeInfo.typeString;  // 해당 typeString 값을 사용
                }

                // 해당 셀에 값 설정
                dgrLog.Dgv.Rows[addRow].Cells["No"].Value = addRow + 1;
                dgrLog.Dgv.Rows[addRow].Cells["logSupname"].Value = supName;
                dgrLog.Dgv.Rows[addRow].Cells["logSupCode"].Value = row["suplog_param"];
                dgrLog.Dgv.Rows[addRow].Cells["logType"].Value = logType;  // 여기서 값을 설정
                dgrLog.Dgv.Rows[addRow].Cells["logBefore"].Value = before;
                dgrLog.Dgv.Rows[addRow].Cells["logAfter"].Value = after;
                dgrLog.Dgv.Rows[addRow].Cells["logEmpName"].Value = empName;
                dgrLog.Dgv.Rows[addRow].Cells["logEmp"].Value = empCode;
                dgrLog.Dgv.Rows[addRow].Cells["logDate"].Value = logDate;
            }
        }
        private void QuerySetting()
        {
            string fromDate = dtpDateFrom.Value.ToString("yyyy-MM-dd");
            string toDate = dtpDateTo.Value.AddDays(1).ToString("yyyy-MM-dd");
            DataTable resultData = new DataTable();
            string query = $"SELECT suplog_type, suplog_before, suplog_after, suplog_param, suplog_emp, suplog_date FROM supplierlog WHERE suplog_date > '{fromDate}' AND suplog_date < '{toDate}'";
            if (cmBoxWorkType.SelectedItem is KeyValuePair<int, string> selectedItem)
            {
                query += $" AND suplog_type = {selectedItem.Key}";
            }
            if (!string.IsNullOrEmpty(tBoxSearch.Text))
            {
                string supQuery = $"SELECT distinct(sup_code) FROM supplier WHERE sup_name LIKE '%{tBoxSearch.Text}%'";
                dbconn.SqlDataAdapterQuery(supQuery, resultData);
                string resultString = "";
                foreach (DataRow supRow in resultData.Rows)
                {
                    if (string.IsNullOrEmpty(resultString))
                    {
                        resultString = supRow[0].ToString();
                    }
                    resultString += ", " + supRow[0].ToString();
                }
                query += $" AND suplog_param IN ({resultString})";
            }
            query += " ORDER BY suplog_date";
            resultData.Rows.Clear();
            dbconn.SqlDataAdapterQuery(query, resultData);
            cLog.InsertEmpAccessLogNotConnect("@supplierLogSearch", accessedEmp, 0);
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
