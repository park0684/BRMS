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
    public partial class CustomerList : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet custList = new cDataGridDefaultSet();
        private cCryptor cryptor = new cCryptor("YourPassphrase");
        
        Dictionary<int, string> accessPermission = new Dictionary<int, string>();
        int accessedEmp = cUserSession.AccessedEmp;
        public CustomerList()
        {
            InitializeComponent();
            panelDataGrid.Controls.Add(custList.Dgv);
            custList.Dgv.Dock = DockStyle.Fill;
            InitializeComBox();
            this.Load += CustomerList_Load;
            checkBoxSaveDate.CheckedChanged += new EventHandler(checkBoxSaveDate_checked);
            checkBoxSaleDate.CheckedChanged += new EventHandler(checkBoxSaleDate_checked);
            tBoxSearchWord.KeyDown += tBoxSearchWord_KeyDown;
            custList.CellDoubleClick += custList_DoubleClick;
            GridForm();
            CheckModifyPermission();
        }
        /// <summary>
        /// 등록 권한 여부 확인 후 새 회원 등록 버튼 비활성화 여부 결정
        /// </summary>
        private void CheckModifyPermission()
        {
            if (!cUserSession.HasPermission(402))
            {
                bntCustomerAdd.Enabled = false;
            }
        }
        private void CustomerList_Load(object sender, EventArgs e)
        {
            tBoxSearchWord.Focus();  // 폼이 로드된 후 tBoxSearchWord에 포커스 설정
            checkBoxSaveDate.Checked = false;
            checkBoxSaleDate.Checked = false;
            checkBoxSaveDate_checked(checkBoxSaveDate, EventArgs.Empty);
            checkBoxSaleDate_checked(checkBoxSaleDate, EventArgs.Empty);
        }
        /// <summary>
        /// 데이터 그리드 설정
        /// </summary>
        private void GridForm()
        {
            custList.Dgv.Columns.Add("custCode", "회원코드");
            custList.Dgv.Columns.Add("custStatus", "상태");
            custList.Dgv.Columns.Add("custName", "회원명");
            custList.Dgv.Columns.Add("custCountry", "국가코드");
            custList.Dgv.Columns.Add("custCountryName", "국가");
            custList.Dgv.Columns.Add("custPoint", "포인트");
            custList.Dgv.Columns.Add("custTel", "전화");
            custList.Dgv.Columns.Add("custCell", "휴대폰");
            custList.Dgv.Columns.Add("custEmail", "이메일");
            custList.Dgv.Columns.Add("custAddress", "주소");
            custList.Dgv.Columns.Add("custRegDate", "등록일");
            custList.Dgv.Columns.Add("custSaleDate", "최종거래일");
            custList.Dgv.Columns.Add("custMemo", "메모");

            custList.FormatAsStringCenter("custStatus", "custCountry", "custTel", "custCell");
            custList.FormatAsStringLeft("custName", "custEmail", "custAddress", "custMemo");
            custList.FormatAsDate("custRegDate", "custSaleDate");
            custList.FormatAsInt("custPoint");
            custList.ApplyDefaultColumnSettings();
            custList.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            custList.Dgv.Columns["custCode"].Visible = false;
            custList.Dgv.Columns["custCountry"].Visible = false;
            custList.Dgv.ReadOnly = true;
        }
        /// <summary>
        /// 콤보박스 초기 설정
        /// </summary>
        private void InitializeComBox()
        {
            cmBoxSaveDate.DropDownStyle = ComboBoxStyle.DropDownList;
            cmBoxSaveDate.Items.AddRange(new string[] { "등록일", "수정일" });
            cmBoxSaveDate.SelectedIndex = 0;

            cmBoxSaleDate.DropDownStyle = ComboBoxStyle.DropDownList;
            cmBoxSaleDate.Items.AddRange(new string[] { "기간내 거래", "최종 거래일" });
            cmBoxSaleDate.SelectedIndex = 0;

            cmBoxStatus.Items.Add("전체");
            foreach (var status in cStatusCode.CustomerStatus)
            {
                cmBoxStatus.Items.Add(new KeyValuePair<int, string>(status.Key, status.Value));
            }
            cmBoxStatus.DisplayMember = "Value"; // 사용자에게 보여질 값
            cmBoxStatus.ValueMember = "Key";    // 내부적으로 사용할 값
            cmBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmBoxStatus.SelectedIndex = 0;            
        }
        /// <summary>
        /// 등록 또는 수정일 지정을 위한 체크박스 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSaveDate_checked(object sender, EventArgs e)
        {
            if(checkBoxSaveDate.Checked)
            {
                cmBoxSaveDate.Enabled = true;
                dtpSaveDateFrom.Enabled = true;
                dtpSaveDateTo.Enabled = true;
            }
            else
            {
                cmBoxSaveDate.Enabled = false;
                dtpSaveDateFrom.Enabled = false;
                dtpSaveDateTo.Enabled = false;
            }
        }
        /// <summary>
        /// 판매일정 지정을 위한 체크박스 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSaleDate_checked(object sender, EventArgs e)
        {
            if (checkBoxSaleDate.Checked)
            {
                cmBoxSaleDate.Enabled = true;
                dtpSaleDateFrom.Enabled = true;
                dtpSaleDateTo.Enabled = true;
            }
            else
            {
                cmBoxSaleDate.Enabled = false;
                dtpSaleDateFrom.Enabled = false;
                dtpSaleDateTo.Enabled = false;
            }
        }
        /// <summary>
        /// 조회 쿼리 설정
        /// </summary>
        private void QuerySetting()
        {
            DataTable resultData = new DataTable();
            string queryBase = "SELECT cust_code, cust_name, cust_country,ctry_name, cust_point, " +
                "cust_tell, cust_cell, cust_email, cust_addr, cust_idate, cust_lastsaledate, cust_status, cust_memo FROM customer, country WHERE cust_country = ctry_code ";
            List<string> queries = new List<string>();  // UNION으로 결합할 쿼리 리스트
            List<string> conditions = new List<string>();

            string word = tBoxSearchWord.Text;
            string fromDate = dtpSaveDateFrom.Value.ToString("d");
            string toDate = dtpSaveDateTo.Value.AddDays(1).ToString("d");
            // 검색어에 따른 쿼리 생성 (UNION 유지)
            if (!string.IsNullOrEmpty(word))
            {
                string searchWordCondition = $"LIKE '%{word}%'";
                queries.Add(queryBase + $" AND cust_name {searchWordCondition}");
                queries.Add(queryBase + $" AND cust_email {searchWordCondition}");
                queries.Add(queryBase + $" AND cust_addr {searchWordCondition}");
                queries.Add(queryBase + $" AND cust_country IN (SELECT ctry_code FROM country WHERE ctry_name {searchWordCondition})");
            }
            else
            {
                queries.Add(queryBase);  // 검색어가 없으면 기본 쿼리만 사용
            }

            // 상태 필터링
            if (cmBoxStatus.SelectedIndex != 0)
            {
                var selectedItem = (KeyValuePair<int, string>)cmBoxStatus.SelectedItem;
                string statusCondition = $"cust_status = {selectedItem.Key}";
                conditions.Add(statusCondition);
            }

            // 등록일 또는 수정일 필터링
            if (checkBoxSaveDate.Checked)
            {
                string saveDateCondition = cmBoxSaveDate.SelectedIndex == 0 ? 
                    $"cust_idate BETWEEN '{fromDate}' AND '{toDate}'" : $"cust_udate BETWEEN '{fromDate}' AND '{toDate}'";
                conditions.Add(saveDateCondition);
            }

            // 거래일 필터링
            if (checkBoxSaleDate.Checked)
            {
                string saleDateCondition = cmBoxSaleDate.SelectedIndex == 0
                    ? $"cust_code IN (SELECT sale_cust FROM sales WHERE sale_date BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}')"
                    : $"cust_lastsaledate BETWEEN '{fromDate}' AND '{toDate}'";
                conditions.Add(saleDateCondition);
            }

            // 각 쿼리에 조건을 적용 (조건을 WHERE가 있으면 AND, 없으면 WHERE로 처리)
            for (int i = 0; i < queries.Count && conditions.Count>0 ; i++)
            {
                queries[i] += " AND " + string.Join(" AND ", conditions);
            }
            // 최종 쿼리 결합 (UNION으로 검색어 관련 쿼리 결합 후, 조건 추가)
            string finalQuery = string.Join("\n UNION \n", queries);

            // 결과 데이터베이스 조회 및 그리드 업데이트
            dbconn.SqlDataAdapterQuery(finalQuery, resultData);
            GridFill(resultData);
            cLog.InsertEmpAccessLogNotConnect("@customerSearch", accessedEmp, 0);
        }
        /// <summary>
        /// 조회 데이터 등록
        /// </summary>
        /// <param name="dataTable"></param>
        private void GridFill(DataTable dataTable)
        {
            //int index = 0;
            custList.Dgv.Rows.Clear();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var rowIndex = custList.Dgv.Rows.Add();
                var row = custList.Dgv.Rows[rowIndex];            
                
                row.Cells["No"].Value = custList.Dgv.RowCount;
                row.Cells["custCode"].Value = dataRow["cust_code"];
                row.Cells["custStatus"].Value = cStatusCode.GetCustomerStatus(cDataHandler.ConvertToInt(dataRow["cust_status"]));
                row.Cells["custName"].Value = dataRow["cust_name"];
                row.Cells["custCountry"].Value = dataRow["cust_country"];
                row.Cells["custCountryName"].Value = dataRow["ctry_name"];
                row.Cells["custPoint"].Value = dataRow["cust_point"];
                row.Cells["custTel"].Value = dataRow["cust_tell"].ToString();
                row.Cells["custCell"].Value = dataRow["cust_cell"].ToString();
                row.Cells["custEmail"].Value = dataRow["cust_email"];
                row.Cells["custAddress"].Value = dataRow["cust_addr"];
                row.Cells["custRegDate"].Value = dataRow["cust_idate"];
                row.Cells["custSaleDate"].Value = dataRow["cust_lastsaledate"];
                row.Cells["custMemo"].Value = dataRow["cust_memo"];

            }
        }
        public void RunQuery()
        {
            try
            {
                QuerySetting();
                cLog.InsertEmpAccessLogNotConnect("@customerSearch", accessedEmp, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 엑셀 출력 기능
        /// </summary>
        public void ExportExcel()
        {
            try
            {
                custList.ExportToExcel();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 회원조회 텍스트 박스 엔터 입력시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBoxSearchWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RunQuery();
            }
        }
        /// <summary>
        /// 조회 회원 더블 클릭
        /// 선택된 회원 상세 정보 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void custList_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int custCode = custList.ConvertToInt(custList.Dgv.CurrentRow.Cells["custCode"].Value);
            CustomerDetail customerDetail = new CustomerDetail();
            customerDetail.SearchCustomer(custCode);
            cLog.InsertEmpAccessLogNotConnect("@customerSearch", accessedEmp, custCode);
            customerDetail.StartPosition = FormStartPosition.CenterParent;
            customerDetail.ShowDialog();
        }

        /// <summary>
        /// 회원 추가 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntCustomerAdd_Click(object sender, EventArgs e)
        {
            CustomerDetail customerDetail = new CustomerDetail();
            customerDetail.CustAdd();
            customerDetail.StartPosition = FormStartPosition.CenterParent;
            customerDetail.ShowDialog();
            
        }
    }
}
