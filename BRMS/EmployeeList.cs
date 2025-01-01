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
    public partial class EmployeeList : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet dgrEmp = new cDataGridDefaultSet();
        int accessedEmp = cUserSession.AccessedEmp;
        public EmployeeList()
        {
            InitializeComponent();
            cmBoxStatus.Items.Clear();
            InitializeComboBox();
            panelDataGrid.Controls.Add(dgrEmp.Dgv);
            dgrEmp.Dgv.Dock = DockStyle.Fill;
            FormGrid();
            dgrEmp.CellDoubleClick += dgrEmp_CellDoubleClick;
        }
        /// <summary>
        /// 콤보박스 초기 설정
        /// </summary>
        private void InitializeComboBox()
        {
           
            foreach (var status in cStatusCode.EmployeeStatus)
            {
                cmBoxStatus.Items.Add(new KeyValuePair<int, string>(status.Key, status.Value));
            }
            cmBoxStatus.Items.Add("전체");
           
            cmBoxStatus.DisplayMember = "Value"; // 사용자에게 보여질 값
            cmBoxStatus.ValueMember = "Key";    // 내부적으로 사용할 값
            cmBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmBoxStatus.SelectedIndex = 1;
        }
        /// <summary>
        /// 데이터 그리드 설정
        /// </summary>
        private void FormGrid()
        {
            dgrEmp.Dgv.Columns.Add("empCode", "직원코드");
            dgrEmp.Dgv.Columns.Add("empName", "직원명");
            dgrEmp.Dgv.Columns.Add("empLevel", "직책");
            dgrEmp.Dgv.Columns.Add("empCell", "연락처");
            dgrEmp.Dgv.Columns.Add("empEmail", "이메일");
            dgrEmp.Dgv.Columns.Add("empAddr", "주소");
            dgrEmp.Dgv.Columns.Add("empStatus", "상태");
            dgrEmp.Dgv.Columns.Add("empIdate", "등록일");
            dgrEmp.Dgv.Columns.Add("empUdate", "수정일");
            dgrEmp.Dgv.Columns.Add("emptMemo", "메모");
            dgrEmp.Dgv.ReadOnly = true;
            dgrEmp.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgrEmp.FormatAsDateTime("empIdate", "empUdate");
            dgrEmp.FormatAsStringCenter("empName", "empLevel", "empCell", "empEmail", "empStatus");
            dgrEmp.FormatAsStringLeft("empAddr", "emptMemo");
            dgrEmp.Dgv.Columns["empCode"].Visible = false;
            dgrEmp.ApplyDefaultColumnSettings();

        }
 
        public void RunQuery()
        {
            dgrEmp.Dgv.Rows.Clear();
            string query = "SELECT emp_code,emp_name, emp_level, emp_cell, emp_email, emp_addr, emp_status,emp_idate, emp_udate, emp_memo FROM employee";
            if(cmBoxStatus.SelectedIndex != cmBoxStatus.Items.Count - 1)
            {
                query += $" WHERE emp_status = {cmBoxStatus.SelectedIndex}";
            }
            DataTable resultData = new DataTable();
            dbconn.SqlDataAdapterQuery(query, resultData);
            GridFill(resultData);
            cLog.InsertEmpAccessLogNotConnect("@employeeSearch", accessedEmp, 0);
        }
        /// <summary>
        /// 직원 데이터 데이터그리드에 등록
        /// </summary>
        /// <param name="dataTable"></param>
        private void GridFill(DataTable dataTable)
        {
            foreach(DataRow row in dataTable.Rows)
            {
                int newRow = dgrEmp.Dgv.Rows.Add();
                dgrEmp.Dgv.Rows[newRow].Cells["No"].Value = newRow + 1;
                dgrEmp.Dgv.Rows[newRow].Cells["empCode"].Value = row["emp_code"];
                dgrEmp.Dgv.Rows[newRow].Cells["empName"].Value = row["emp_name"];
                dgrEmp.Dgv.Rows[newRow].Cells["empLevel"].Value = row["emp_level"];
                dgrEmp.Dgv.Rows[newRow].Cells["empCell"].Value = row["emp_cell"];
                dgrEmp.Dgv.Rows[newRow].Cells["empEmail"].Value = row["emp_email"];
                dgrEmp.Dgv.Rows[newRow].Cells["empAddr"].Value = row["emp_addr"];
                dgrEmp.Dgv.Rows[newRow].Cells["empStatus"].Value = cStatusCode.GetEmployeeStatus(Convert.ToInt32(row["emp_status"]));
                dgrEmp.Dgv.Rows[newRow].Cells["empIdate"].Value = row["emp_idate"];
                dgrEmp.Dgv.Rows[newRow].Cells["empUdate"].Value = row["emp_udate"];
                dgrEmp.Dgv.Rows[newRow].Cells["emptMemo"].Value = row["emp_memo"];
            }

        }
        /// <summary>
        /// 직원등록정보창 호출
        /// 직원코드 0으로 등록 시 신규로 사용
        /// </summary>
        /// <param name="empCode"></param>
        private void CallEmployeeDetail(int empCode)
        {
            EmployeeDetail employeeDetail = new EmployeeDetail();
            employeeDetail.StartPosition = FormStartPosition.CenterParent;
            employeeDetail.CallEmployee(empCode);           
            //productDetail.refresh += (refreshCode) => refresh = refreshCode;
            employeeDetail.ShowDialog();
        }
        /// <summary>
        /// 조회된 직원 더블클릭으로 정보조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgrEmp_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRowIndex = e.RowIndex;
            if (currentRowIndex >= 0)
            {
                DataGridViewRow currentRow = dgrEmp.Dgv.Rows[currentRowIndex];
                int empCode = dgrEmp.ConvertToInt(currentRow.Cells["empCode"].Value);
                CallEmployeeDetail(empCode);                
            }
        }
        private void bntAddEmployee_Click(object sender, EventArgs e)
        {
            CallEmployeeDetail(0);
        }
    }
}
