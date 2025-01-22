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
    public partial class ConfigLog : Form
    {
        cDataGridDefaultSet dgvLog = new cDataGridDefaultSet();
        cDatabaseConnect dbconn = new cDatabaseConnect();
        static Dictionary<string, (int typeCode, string typeString)> parameter = new Dictionary<string, (int, string)>();
        public ConfigLog()
        {
            InitializeComponent();
            GridForm();
            parameter = LogParameterSet();
            InitializeComboBox();
        }

        private void GridForm()
        {
            pnlDataGrid.Controls.Add(dgvLog.Dgv);
            dgvLog.Dgv.Dock = DockStyle.Fill;
            dgvLog.Dgv.Columns.Add("cflogType", "작업유형");
            dgvLog.Dgv.Columns.Add("cflogEmp", "작업자");
            dgvLog.Dgv.Columns.Add("cflogDetail", "작업내역");
            dgvLog.Dgv.Columns.Add("cflogBefore", "변경 전");
            dgvLog.Dgv.Columns.Add("cflogAfter", "변경 후");
            dgvLog.Dgv.Columns.Add("cflogDate", "일자");
            dgvLog.ApplyDefaultColumnSettings();
            dgvLog.FormatAsDateTime("cflogDate");
            dgvLog.FormatAsStringCenter("No", "cflogEmp", "cflogBefore", "cflogAfter");
            dgvLog.FormatAsStringLeft("cflogType", "cflogDetail");
            dgvLog.Dgv.ReadOnly = true;
        }
        private void FillGrid(DataTable dataTable)
        {
            dgvLog.Dgv.Rows.Clear();
            foreach(DataRow row in dataTable.Rows)
            {
                int empCode = cDataHandler.ConvertToInt(row["cfl_emp"]);
                string empName;
                cLog.GetEmployeeInfo(empCode, out empName);
                string logType="";
                var type = parameter.Values.FirstOrDefault(x => x.typeCode == (int)row["cfl_type"]);
                if (type.typeString != null)
                {
                    logType = type.typeString;  // 해당 typeString 값을 사용
                }
                object resultObj = new object();
                string query = $"SELECT cf_name FROM config WHERE cf_code = {row["cfl_param"]}";
                dbconn.sqlScalaQuery(query, out resultObj);

                int newRow = dgvLog.Dgv.Rows.Add();
                var rowIndex = dgvLog.Dgv.Rows[newRow];
                rowIndex.Cells["No"].Value = newRow + 1;
                rowIndex.Cells["cflogType"].Value = logType;
                rowIndex.Cells["cflogEmp"].Value = empName;
                rowIndex.Cells["cflogDetail"].Value = resultObj.ToString().Trim();
                rowIndex.Cells["cflogBefore"].Value = row["cfl_before"].ToString();
                rowIndex.Cells["cflogAfter"].Value = row["cfl_after"].ToString();
                rowIndex.Cells["cflogDate"].Value = Convert.ToDateTime(row["cfl_date"]).ToString("d");
            }
        }
        /// <summary>
        /// 로그 파라미터 생성
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, (int, string)> LogParameterSet()
        {
            List<int> logTypeParameters = new List<int> { 931, 932, 933, 934 };
            Dictionary<string, (int typeCode, string typeString)> parameter = new Dictionary<string, (int, string)>();  // 파라미터 딕셔너리 선언

            foreach (int workType in logTypeParameters)
            {
                // logParameter 딕셔너리에서 typeCode를 기준으로 typeString을 찾기
                var found = cLog.logParameter.FirstOrDefault(x => x.Value.typeCode == workType);

                if (!EqualityComparer<KeyValuePair<string, (int, string)>>.Default.Equals(found, default(KeyValuePair<string, (int, string)>)))
                {
                    // 해당 typeCode가 있는 경우 해당하는 key와 typeCode, typeString을 딕셔너리에 추가
                    parameter[found.Key] = found.Value;
                }
            }
            return parameter;
        }
        /// <summary>
        /// 콤보박스 설정
        /// 콤보박스 아이템은 로그 파라미터 내용
        /// </summary>
        private void InitializeComboBox()
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
        /// 조회 쿼리 생성 및 데이터 전달
        /// </summary>
        private void SearchQuerySetting()
        {
            DataTable resultData = new DataTable();
            string fromDate = dtpDateFrom.Value.ToString("d");
            string toDate = dtpDateTo.Value.AddDays(1).ToString("d");

            string query = $"SELECT cfl_type, cfl_emp, cfl_before, cfl_after,cfl_date, cfl_param FROM configlog WHERE cfl_date > '{fromDate}' AND cfl_date < '{toDate}'";
            if(cmBoxWorkType.SelectedItem is KeyValuePair<int, string> selectedItem)
            {
                query += $" AND cfl_type = {selectedItem.Key}";
            }
            query += " ORDER BY cfl_date";
            dbconn.SqlDataAdapterQuery(query, resultData);
            FillGrid(resultData);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchQuerySetting();
        }
    }
}
