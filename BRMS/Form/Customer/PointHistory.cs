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
    public partial class PointHistory : Form
    {
        int accessedEmp = cUserSession.AccessedEmp;
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet dgvPointHistory = new cDataGridDefaultSet();
        static Dictionary<string, (int typeCode, string typeString)> parameter = new Dictionary<string, (int, string)>();
        int customerCode = 0;
        bool custCheck = false;

        public PointHistory()
        {
            InitializeComponent();
            InitializeComboBxo();
            GridForm();
            dgvPointHistory.CellDoubleClick += dgvPointHistory_CellDoubleClick;
            dgvPointHistory.Dgv.MouseClick += dgvPointHistory_MouseRightClick;
        }
        /// <summary>
        /// 적립/사용 내역 콤보박스 설정
        /// cSatatus 클래스에서 정보 수신
        /// </summary>
        private void InitializeComboBxo()
        {
            cmBoxWorkType.Items.Add("전체");
            foreach (var entry in cStatusCode.PointHistory)
            {
                cmBoxWorkType.Items.Add(new KeyValuePair<int, string>(entry.Key, entry.Value));
            }

            cmBoxWorkType.DisplayMember = "Value"; // 사용자에게 보여질 값
            cmBoxWorkType.ValueMember = "Key";    // 내부적으로 사용할 값
            cmBoxWorkType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmBoxWorkType.SelectedIndex = 0;
        }
        private void GridForm()
        {
            pnlDataGrid.Controls.Add(dgvPointHistory.Dgv);
            dgvPointHistory.Dgv.Dock = DockStyle.Fill;
            dgvPointHistory.Dgv.Columns.Add("phDate", "날짜");
            dgvPointHistory.Dgv.Columns.Add("phCustName", "회원명");
            dgvPointHistory.Dgv.Columns.Add("phCust", "회원코드");
            dgvPointHistory.Dgv.Columns.Add("phType", "구분");
            dgvPointHistory.Dgv.Columns.Add("phParam", "파라메터");
            dgvPointHistory.Dgv.Columns.Add("phAddPoint", "적립 포인트");
            dgvPointHistory.Dgv.Columns.Add("phDedPoint", "차감 포인트");
            dgvPointHistory.Dgv.Columns.Add("phPrevious", "직전 포인트");
            dgvPointHistory.Dgv.ReadOnly = true;
            dgvPointHistory.Dgv.Columns["phCust"].Visible = false;
            dgvPointHistory.Dgv.Columns["phParam"].Visible = false;
            dgvPointHistory.ApplyDefaultColumnSettings();
            dgvPointHistory.FormatAsDateTime("phDate");
            dgvPointHistory.FormatAsStringLeft("phCustName");
            dgvPointHistory.FormatAsStringCenter("phType");
            dgvPointHistory.FormatAsInt("phAddPoint", "phDedPoint", "phPrevious");

        }

        private void FillGrid(DataTable dataTable)
        {
            dgvPointHistory.Dgv.Rows.Clear();
            
            foreach(DataRow row in dataTable.Rows)
            {
                int newRow = dgvPointHistory.Dgv.Rows.Add();
                var gridViewRow = dgvPointHistory.Dgv.Rows[newRow];
                int custCode = cDataHandler.ConvertToInt(row["ph_cust"]);
                string custName = row["cust_name"].ToString();
                string type = cStatusCode.GetPointHistory(cDataHandler.ConvertToInt(row["ph_type"]));
                int param = cDataHandler.ConvertToInt(row["ph_param"]);
                int Point = cDataHandler.ConvertToInt(row["ph_point"]);
                int addPoint = Point > 0 ? Point : 0;
                int DedPoint = Point < 0 ? Point : 0;
                int prePoint = cDataHandler.ConvertToInt(row["ph_previous"]);
                string getDate = Convert.ToDateTime(row["ph_date"]).ToString("yyyy-MM-dd HH:mm");
               gridViewRow.Cells["No"].Value = newRow + 1;
               gridViewRow.Cells["phDate"].Value = getDate;
               gridViewRow.Cells["phCustName"].Value = custName;
               gridViewRow.Cells ["phParam"].Value = 
               gridViewRow.Cells["phCust"].Value = custCode;
               gridViewRow.Cells["phType"].Value = type;
               gridViewRow.Cells["phAddPoint"].Value = addPoint;
               gridViewRow.Cells["phDedPoint"].Value = DedPoint;
                gridViewRow.Cells["phPrevious"].Value = prePoint;
            }
        }
        /// <summary>
        /// 회원 버튼 토글
        /// </summary>
        /// <param name="check"></param>
        private void CustomerToggle(bool check)
        {
            tBoxSearch.Enabled = false;
            if (check == true)
            {
                custCheck = true;
                btnCustomer.Text = "회원취소";
            }
            else
            {
                custCheck = false;
                btnCustomer.Text = "회원";
            }
        }
        /// <summary>
        /// 회원 검색창 호출
        /// </summary>
        private void CallCustoerSearchBox()
        {
            if(custCheck == false)
            {
                CustomerSearchBox customerSearchBox = new CustomerSearchBox();
                customerSearchBox.StartPosition = FormStartPosition.CenterParent;
                customerSearchBox.GetCustomerCode += (custCode) => { customerCode = (custCode); };
                customerSearchBox.ShowDialog();
                LoadCustmerInfo();
            }
        }
        /// <summary>
        /// 회원 검색창 초기화 
        /// </summary>
        private void ClearCustomerSearchBox()
        {
            CustomerToggle(false);
            tBoxSearch.Text = "";
            customerCode = 0;
        }
        /// <summary>
        /// 회원정보 수신 및 회원명 표시
        /// </summary>
        private void LoadCustmerInfo()
        {
            string qeury = $"SELECT cust_name FROM customer WHERE cust_code = {customerCode}";
            object resultObj = new object();
            dbconn.sqlScalaQuery(qeury, out resultObj);
            string custName = resultObj.ToString().Trim();
            tBoxSearch.Text = custName;
            CustomerToggle(true);
        }
        private void QuerySetting()
        {
            DataTable resultData = new DataTable();
            string fromDate = dtpDateFrom.Value.ToString("d");
            string toDate = dtpDateTo.Value.AddDays(1).ToString("d");
            string query = $"SELECT ph_type, ph_cust, ph_param, ph_point, ph_previous, ph_date, cust_name FROM pointhistory, customer WHERE ph_date > '{fromDate}' AND ph_date < '{toDate}' AND cust_code = ph_cust ";
            if(cmBoxWorkType.SelectedIndex != 0)
            {
                int typeCode = cStatusCode.GetPointHistoryCode(cmBoxWorkType.SelectedItem.ToString());
                query += $"AND ph_type = {typeCode} ";
            }
            if(!string.IsNullOrEmpty(tBoxSearch.Text))
            {
                query += $"AND ph_cust = {customerCode} ";
            }
            query += "ORDER BY ph_date, ph_seq";

            dbconn.SqlDataAdapterQuery(query, resultData);
            FillGrid(resultData);
        }
        private void LoadCustomerDetail()
        {
            int custCode = cDataHandler.ConvertToInt(dgvPointHistory.Dgv.CurrentRow.Cells["phCust"].Value);
            CustomerDetail customerDetail = new CustomerDetail();
            customerDetail.SearchCustomer(custCode);
            cLog.InsertEmpAccessLogNotConnect("@customerSearch", accessedEmp, custCode);
            customerDetail.StartPosition = FormStartPosition.CenterParent;
            customerDetail.ShowDialog();
        }
        private void LoadSaleDetail()
        {
            int saleCode = cDataHandler.ConvertToInt(dgvPointHistory.Dgv.CurrentRow.Cells["phParam"].Value);
            SalesRegist salesRegist = new SalesRegist();
            salesRegist.GetSaleCode(saleCode);
            cLog.InsertEmpAccessLogNotConnect("@salesSearch", accessedEmp, saleCode);
            salesRegist.StartPosition = FormStartPosition.CenterParent;
            salesRegist.ShowDialog();
        }
        private void dgvPointHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadCustomerDetail();
        }
        private void dgvPointHistory_MouseRightClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                ContextMenuStrip contextMenu = new ContextMenuStrip();
                ToolStripSeparator separator = new ToolStripSeparator();
                contextMenu.Items.Add("회원 조회");
                contextMenu.Items.Add("거래내역 조회");
                contextMenu.Items.Add(separator);
                // Grid 칼럼 이름을 메뉴 항목으로 추가
                foreach (DataGridViewColumn column in dgvPointHistory.Dgv.Columns)
                {
                    //제외 컬럼 외
                    if (column.Name != "No" && column.Name != "phCust" && column.Name != "phParam")
                    {
                        ToolStripMenuItem columnMenuItem = new ToolStripMenuItem(column.HeaderText)
                        {
                            CheckOnClick = true,
                            Checked = column.Visible
                        };

                        // 칼럼의 Name을 Tag로 저장
                        columnMenuItem.Tag = column.Name;

                        contextMenu.Items.Add(columnMenuItem);
                    }
                }
                contextMenu.Show(pnlDataGrid, e.Location);
                contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(Menu_Click);
            }

        }
        private void Menu_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            string clickedItemText = e.ClickedItem.Text;

            switch (clickedItemText)
            {
                case "회원 조회":
                    LoadCustomerDetail();
                    break;
                case "거래내역 조회":
                    LoadSaleDetail();
                    break;
                default:
                    // 칼럼 이름으로 메뉴 항목 클릭 시 가시성 토글
                    //var menuItem = (ToolStripMenuItem)e.ClickedItem;
                    string columnName = e.ClickedItem.Tag.ToString();

                    // 칼럼 이름이 데이터 그리드에 존재하는지 확인
                    if (dgvPointHistory.Dgv.Columns.Contains(columnName))
                    {
                        var column = dgvPointHistory.Dgv.Columns[columnName];
                        // Grid 표시 상태 반전으로 표시 또는 해제
                        column.Visible = !column.Visible;
                    }
                    break;
            }
        }
        /// <summary>
        /// 회원버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            if (!custCheck)
            {
                CallCustoerSearchBox();
            }
            else
            {
                ClearCustomerSearchBox();
            }
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
