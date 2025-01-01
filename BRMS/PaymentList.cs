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
    public partial class PaymentList : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet DgrSupList = new cDataGridDefaultSet();
        cDataGridDefaultSet DgrPayList = new cDataGridDefaultSet();
        int selectedSupplier = 0;
        int balance = 0;
        int currentBalane = 0;
        int accessedEmp = cUserSession.AccessedEmp;
        
        public PaymentList()
        {
            InitializeComponent();
            panelSupplierList.Controls.Add(DgrSupList.Dgv);
            panelPaymentList.Controls.Add(DgrPayList.Dgv);
            DgrSupList.Dgv.Dock = DockStyle.Fill;
            DgrPayList.Dgv.Dock = DockStyle.Fill;
            GridForm();
            LoadSupplierList();
            DgrPayList.CellDoubleClick += DgrpayList_CellDoubleClick;
            DgrSupList.CellDoubleClick += DgrSupList_CellDoubleClick;
            DgrPayList.Dgv.MouseClick += DgrpayList_MouseRightClick;
            GridhideColumn();
            supplierLabelSet(0);
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ModifyPermission();
        }
        /// <summary>
        /// 직원권한 여부 확인 후 결제등로 버튼 비활성화 여부 판단
        /// </summary>
        private void ModifyPermission()
        {
            if (!cUserSession.HasPermission(222))
            {
                bntPaymentAdd.Enabled = false;
            }
        }
        /// <summary>
        /// 데이터 그리드 설정
        /// </summary>
        private void GridForm()
        {
            DgrSupList.Dgv.Columns["No"].Visible = false;
            DgrSupList.Dgv.Columns.Add("supCode", "코드");
            DgrSupList.Dgv.Columns.Add("supName", "공급사명");
            DgrSupList.Dgv.ReadOnly = true;
            DgrSupList.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgrSupList.Dgv.Columns["supCode"].Width = 50;
            DgrSupList.Dgv.Columns["supName"].Width = 175;
            foreach (DataGridViewColumn supListColumn in DgrSupList.Dgv.Columns)
            {
                supListColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                supListColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            DgrPayList.Dgv.Columns["No"].Visible = false;
            DgrPayList.Dgv.Columns.Add("payCode","코드");
            DgrPayList.Dgv.Columns.Add("workType","유형");
            DgrPayList.Dgv.Columns.Add("payDate","날짜");
            DgrPayList.Dgv.Columns.Add("purchaseAmount","매입액");
            DgrPayList.Dgv.Columns.Add("paymentAmount","결제액");
            DgrPayList.Dgv.Columns.Add("cahsPayment", "현금결제");
            DgrPayList.Dgv.Columns.Add("bankTransfer", "계좌이체");
            DgrPayList.Dgv.Columns.Add("creditPayment", "카드결제");
            DgrPayList.Dgv.Columns.Add("NotePayment", "어음");
            DgrPayList.Dgv.Columns.Add("Discount", "D/C");
            DgrPayList.Dgv.Columns.Add("Coupon", "쿠폰");
            DgrPayList.Dgv.Columns.Add("Supsiby", "장려금");
            DgrPayList.Dgv.Columns.Add("etc", "기타");
            DgrPayList.Dgv.Columns.Add("balance","잔액");
            DgrPayList.Dgv.Columns.Add("employee", "등록자");
            DgrPayList.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgrPayList.ApplyDefaultColumnSettings();

            //칼럼 포멧 설정
            DgrPayList.FormatAsInteger("purchaseAmount", "paymentAmount", "cahsPayment", "bankTransfer", "creditPayment", "NotePayment", "Discount", "Coupon", "Supsiby", "etc", "balance");
            DgrPayList.FormatAsDateTime("payDate");
            DgrPayList.FormatAsStringCenter("payCode", "workType", "employee");

            foreach (DataGridViewColumn payListColumn in DgrPayList.Dgv.Columns)
            {
                payListColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                payListColumn.MinimumWidth = 50;
            }
            
            DgrPayList.Dgv.ReadOnly = true;
        }
        private void LoadSupplierList()
        {
            string query = "SELECT sup_code,sup_name FROM supplier";
            DataTable supplierList = new DataTable();
            dbconn.SqlDataAdapterQuery(query, supplierList);
            int index = 0;
            foreach(DataRow dataRow in supplierList.Rows)
            {
                DgrSupList.Dgv.Rows.Add();
                DgrSupList.Dgv.Rows[index].Cells["supCode"].Value = dataRow["sup_code"].ToString();
                DgrSupList.Dgv.Rows[index].Cells["supName"].Value = dataRow["sup_name"].ToString();
                index++;
            }
        }
        
        /// <summary>
        /// 우클릭 메뉴 펼처보기
        /// </summary>
        private void GridExpandColumn()
        {
            DgrPayList.Dgv.Columns["payCode"].Visible = true;
            DgrPayList.Dgv.Columns["workType"].Visible = true;
            DgrPayList.Dgv.Columns["payDate"].Visible = true;
            DgrPayList.Dgv.Columns["purchaseAmount"].Visible = true;
            DgrPayList.Dgv.Columns["paymentAmount"].Visible = true;
            DgrPayList.Dgv.Columns["cahsPayment"].Visible = true;
            DgrPayList.Dgv.Columns["bankTransfer"].Visible = true;
            DgrPayList.Dgv.Columns["creditPayment"].Visible = true;
            DgrPayList.Dgv.Columns["NotePayment"].Visible = true;
            DgrPayList.Dgv.Columns["Discount"].Visible = true;
            DgrPayList.Dgv.Columns["Coupon"].Visible = true;
            DgrPayList.Dgv.Columns["Supsiby"].Visible = true;
            DgrPayList.Dgv.Columns["etc"].Visible = true;
            DgrPayList.Dgv.Columns["balance"].Visible = true;
            DgrPayList.Dgv.Columns["employee"].Visible = true;
            //DgrPayList.Dgr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
        }
        /// <summary>
        /// 우클릭 메뉴 줄여보기
        /// 클래스가 최초 호출시에는 줄여보기로 시작
        /// </summary>
        private void GridhideColumn()
        {
            DgrPayList.Dgv.Columns["payCode"].Visible = true;
            DgrPayList.Dgv.Columns["workType"].Visible = true;
            DgrPayList.Dgv.Columns["payDate"].Visible = true;
            DgrPayList.Dgv.Columns["purchaseAmount"].Visible = true;
            DgrPayList.Dgv.Columns["paymentAmount"].Visible = true;
            DgrPayList.Dgv.Columns["cahsPayment"].Visible = false;
            DgrPayList.Dgv.Columns["bankTransfer"].Visible = false;
            DgrPayList.Dgv.Columns["creditPayment"].Visible = false;
            DgrPayList.Dgv.Columns["NotePayment"].Visible = false;
            DgrPayList.Dgv.Columns["Discount"].Visible = false;
            DgrPayList.Dgv.Columns["Coupon"].Visible = false;
            DgrPayList.Dgv.Columns["Supsiby"].Visible = false;
            DgrPayList.Dgv.Columns["etc"].Visible = false;
            DgrPayList.Dgv.Columns["balance"].Visible = true;
            DgrPayList.Dgv.Columns["employee"].Visible = true;
            //DgrPayList.Dgr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; 
        }
        /// <summary>
        /// 선택한 공급사로 변수 및 라벨 값 수정
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private int supplierLabelSet(int code)
        {
            selectedSupplier = Convert.ToInt32(DgrSupList.Dgv.Rows[code].Cells["supCode"].Value.ToString());
            lblSupplier.Text = "공급사 : " + DgrSupList.Dgv.Rows[code].Cells["supName"].Value.ToString();
            DataTable resultData = new DataTable();
            DateTime nowDate = DateTime.Now.AddDays(1);
            DateTime fromDate = DateTime.Now.AddMonths(-5);
            fromDate = new DateTime(fromDate.Year, fromDate.Month, 1, 0, 0, 0);

            object resultObj = new object();
            currentBalane = 0; // 잔액 0원으로 수정 후 조회 및 계산
            string query = string.Format("SELECT cb_balance FROM closingbalance WHERE cb_sup =  {0} AND cb_date = {1}", selectedSupplier, fromDate.AddMonths(-1).ToString("yyyyMM"));
            dbconn.sqlScalaQuery(query, out resultObj);
            currentBalane = Convert.ToInt32(resultObj);
            
            query = string.Format("SELECT ISNULL(SUM(pur_amount),0) FROM purchase WHERE pur_sup = {0} AND pur_date > '{1}' AND pur_date < '{2}'", selectedSupplier, fromDate.ToString("yyyy-MM-01"), nowDate.ToString("yyyy-MM-dd"));
            dbconn.sqlScalaQuery(query, out resultObj);
            currentBalane += Convert.ToInt32(resultObj);
            
            query = string.Format("SELECT ISNULL(SUM(pay_paycash + pay_accounttransfer + pay_paycredit + pay_paynote + pay_DC + pay_coupone + pay_supsiby + pay_etc),0) FROM payment WHERE pay_sup = {0} AND pay_date > '{1}' AND pay_date < '{2}' AND pay_status = 1", selectedSupplier, fromDate.ToString("yyyy-MM-dd"), nowDate.ToString("yyyy-MM-dd"));
            dbconn.sqlScalaQuery(query, out resultObj);
            currentBalane -= Convert.ToInt32(resultObj);
            lblCurrentBalance.Text = Convert.ToString(currentBalane);
            return currentBalane;
        }

        private void GridFill(DataTable dataTable)
        {
            DgrPayList.Dgv.Rows.Clear();
            int rowIndex = 0;
            int cash = 0;
            int credit = 0;
            int note = 0;
            int account = 0;
            int dc = 0;
            int coupon = 0;
            int supsiby = 0;
            int etc = 0;
            int total = 0;
            int purchase = 0;
            int cashTotal = 0, creditTotal = 0, noteTotal = 0, accountTotal = 0;
            int dcTotal = 0, couponTotal = 0, supsibyTotal = 0, etcTotal = 0, totalPayment = 0, purchaseTotal = 0;
            string workType = "";

            string employee = "";
            foreach (DataRow dataRow in dataTable.Rows)
            {
                //DataRow StatusRow = cBoxStatus.SelectedValue.ToString .[int.Parse(dataRow["pdt_status"].ToString())];
                DgrPayList.Dgv.Rows.Add();
                string query = $"SELECT emp_name FROM employee WHERE emp_code = {dataRow["pay_emp"]}";
                object resultObj = new object();
                dbconn.sqlScalaQuery(query, out resultObj);
                employee = resultObj.ToString();
                purchase = Convert.ToInt32(dataRow["pur_amount"].ToString());
                cash = Convert.ToInt32(dataRow["pay_paycash"].ToString());
                credit = Convert.ToInt32(dataRow["pay_paycredit"].ToString());
                note = Convert.ToInt32(dataRow["pay_paynote"].ToString());
                account = Convert.ToInt32(dataRow["pay_accounttransfer"].ToString());
                dc = Convert.ToInt32(dataRow["pay_DC"].ToString());
                coupon = Convert.ToInt32(dataRow["pay_coupone"].ToString());
                supsiby = Convert.ToInt32(dataRow["pay_supsiby"].ToString());
                etc = Convert.ToInt32(dataRow["pay_etc"].ToString());
                total = cash + credit + note + account + dc + coupon + supsiby + etc;
                switch(dataRow["pay_type"].ToString())
                {
                    case "1":
                        workType = "결제";
                        break;
                    case "2":
                        workType = "매입결제";
                        break;
                    case "0":
                        workType = "매입";
                        break;
                }
                DgrPayList.Dgv.Rows[rowIndex].Cells["payCode"].Value = dataRow["pay_code"];
                DgrPayList.Dgv.Rows[rowIndex].Cells["workType"].Value = workType;
                DgrPayList.Dgv.Rows[rowIndex].Cells["payDate"].Value = Convert.ToDateTime(dataRow["pay_date"]);
                DgrPayList.Dgv.Rows[rowIndex].Cells["purchaseAmount"].Value = purchase;
                DgrPayList.Dgv.Rows[rowIndex].Cells["paymentAmount"].Value = total;
                DgrPayList.Dgv.Rows[rowIndex].Cells["cahsPayment"].Value = cash;
                DgrPayList.Dgv.Rows[rowIndex].Cells["bankTransfer"].Value = account;
                DgrPayList.Dgv.Rows[rowIndex].Cells["creditPayment"].Value = credit;
                DgrPayList.Dgv.Rows[rowIndex].Cells["NotePayment"].Value = note;
                DgrPayList.Dgv.Rows[rowIndex].Cells["Discount"].Value = dc;
                DgrPayList.Dgv.Rows[rowIndex].Cells["Coupon"].Value = coupon;
                DgrPayList.Dgv.Rows[rowIndex].Cells["Supsiby"].Value = supsiby;
                DgrPayList.Dgv.Rows[rowIndex].Cells["etc"].Value = etc;
                DgrPayList.Dgv.Rows[rowIndex].Cells["balance"].Value = balance + purchase - total;
                DgrPayList.Dgv.Rows[rowIndex].Cells["employee"].Value = employee;
                balance = DgrPayList.ConvertToInt(DgrPayList.Dgv.Rows[rowIndex].Cells["balance"].Value);
                rowIndex++;
                // Update totals
                purchaseTotal += purchase;
                cashTotal += cash;
                creditTotal += credit;
                noteTotal += note;
                accountTotal += account;
                dcTotal += dc;
                couponTotal += coupon;
                supsibyTotal += supsiby;
                etcTotal += etc;
                totalPayment += total;
            }
            DgrPayList.Dgv.Rows.Add();
            
            DgrPayList.Dgv.Rows[rowIndex].Cells["payCode"].Value = "합계";
            DgrPayList.Dgv.Rows[rowIndex].Cells["purchaseAmount"].Value = purchaseTotal;
            DgrPayList.Dgv.Rows[rowIndex].Cells["paymentAmount"].Value = totalPayment;
            DgrPayList.Dgv.Rows[rowIndex].Cells["cahsPayment"].Value = cashTotal;
            DgrPayList.Dgv.Rows[rowIndex].Cells["bankTransfer"].Value = accountTotal;
            DgrPayList.Dgv.Rows[rowIndex].Cells["creditPayment"].Value = creditTotal;
            DgrPayList.Dgv.Rows[rowIndex].Cells["NotePayment"].Value = noteTotal;
            DgrPayList.Dgv.Rows[rowIndex].Cells["Discount"].Value = dcTotal;
            DgrPayList.Dgv.Rows[rowIndex].Cells["Coupon"].Value = couponTotal;
            DgrPayList.Dgv.Rows[rowIndex].Cells["Supsiby"].Value = supsibyTotal;
            DgrPayList.Dgv.Rows[rowIndex].Cells["etc"].Value = etcTotal;
            DgrPayList.Dgv.Rows[rowIndex].Cells["balance"].Value = balance;
            
        }
        private void QuerySetting()
        {
            DataTable resultData = new DataTable();
            DateTime fromDate = dtpFrom.Value.AddMonths(-5);
            fromDate = new DateTime(fromDate.Year, fromDate.Month, 1, 0, 0, 0);
            DateTime toDate = dtpFrom.Value.AddDays(-1);

            string query = $"SELECT ISNULL(cb_balance,0) cb_balance  FROM closingbalance WHERE cb_sup = {selectedSupplier} AND cb_date = {fromDate.AddMonths(-1).ToString("yyyyMM")}";
            object resultObj = new object();
            dbconn.sqlScalaQuery(query, out resultObj);
            balance = Convert.ToInt32(resultObj);
            
            query = $"SELECT ISNULL(SUM(pur_amount),0) FROM purchase WHERE pur_sup = {selectedSupplier} AND pur_date > '{fromDate.ToString("yyyy-MM-dd")}' AND pur_date < '{toDate.ToString("yyyy-MM-dd")}'";
            dbconn.sqlScalaQuery(query, out resultObj);
            balance += Convert.ToInt32(resultObj);

            query = $"SELECT ISNULL(SUM(pay_paycash + pay_accounttransfer + pay_paycredit + pay_paynote + pay_DC + pay_coupone + pay_supsiby + pay_etc),0) FROM payment WHERE pay_sup = {selectedSupplier} AND pay_date > '{fromDate.ToString("yyyy-MM-dd")}' AND pay_date < '{toDate.ToString("yyyy-MM-dd")}' AND pay_status = 1";
            dbconn.sqlScalaQuery(query, out resultObj);
            balance -= Convert.ToInt32(resultObj);


            string dtpFromDate = dtpFrom.Value.ToString("yyyy-MM-dd");
            string dtpToDate = dtpTo.Value.AddDays(1).ToString("yyyy-MM-dd");

            query = "SELECT pay_code, pay_type, 0 pur_amount ,pay_date, pay_paycash,pay_accounttransfer,pay_paycredit,pay_paynote,pay_DC," +
                "pay_coupone,pay_supsiby,pay_etc,pay_memo,pay_emp " +
                $"FROM payment WHERE pay_sup = {selectedSupplier} AND pay_date >= '{dtpFromDate}' AND pay_date < '{dtpToDate}' AND pay_status = 1 \n UNION ALL " +
                "\nSELECT pur_code, 0 as pay_type, pur_amount, pur_date, 0, 0, 0, 0, 0, 0, 0, 0, '',pur_emp " +
                $"FROM purchase WHERE pur_date >= '{dtpFromDate}' AND  pur_date < '{dtpToDate}' AND pur_sup = {selectedSupplier} ORDER BY pay_date";

            dbconn.SqlDataAdapterQuery(query, resultData);
            GridFill(resultData);
            
        }
        public void RunQuery()
        {
            try
            {

                QuerySetting();
                cLog.InsertEmpAccessLogNotConnect("@paymentSearch", accessedEmp, selectedSupplier);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void DgrpayList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentIndex = e.RowIndex;
            int lastIndex = DgrPayList.Dgv.RowCount - 1;
            if (currentIndex != lastIndex && Convert.ToString(DgrPayList.Dgv.Rows[currentIndex].Cells["workType"].Value).Contains("결제"))
            {
                int payCode = Convert.ToInt32(DgrPayList.Dgv.Rows[currentIndex].Cells["payCode"].Value);
                PaymentDetail paymentDetail = new PaymentDetail();
                cLog.InsertEmpAccessLogNotConnect("@payDetailSearch", accessedEmp, payCode);
                paymentDetail.LoadPaymentDetail(payCode);
                paymentDetail.ShowDialog();
            }            
        }
        /// <summary>
        /// 우클릭 기능
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgrpayList_MouseRightClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) 
            {
                
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                ToolStripSeparator separator = new ToolStripSeparator();
                contextMenu.Items.Add("펼처보기");
                contextMenu.Items.Add("줄여보기");
                contextMenu.Items.Add(separator);
                // Grid 칼럼 이름을 메뉴 항목으로 추가
                foreach (DataGridViewColumn column in DgrPayList.Dgv.Columns)
                {
                    // 첫 번째 칼럼은 제외
                    if (column.Name != "No")
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
                contextMenu.Show(panelPaymentList, e.Location);
                contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(Menu_Click);
            }

        }

        private void Menu_Click(object sender, ToolStripItemClickedEventArgs e )
        {
            string clickedItemText = e.ClickedItem.Text;
            
            switch (clickedItemText)
            {
                case "펼처보기":
                    GridExpandColumn();
                    break;
                case "줄여보기":
                    GridhideColumn();
                    break;
                default:
                    // 칼럼 이름으로 메뉴 항목 클릭 시 가시성 토글
                    //var menuItem = (ToolStripMenuItem)e.ClickedItem;
                    string columnName = e.ClickedItem.Tag.ToString();

                    // 칼럼 이름이 데이터 그리드에 존재하는지 확인
                    if (DgrPayList.Dgv.Columns.Contains(columnName))
                    {
                        var column = DgrPayList.Dgv.Columns[columnName];
                        // Grid 표시 상태 반전으로 표시 또는 해제
                        column.Visible = !column.Visible;  
                    }
                    break;
            }
        }
        private void DgrSupList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentIndex = e.RowIndex;
            
            supplierLabelSet(currentIndex);
            QuerySetting();

            
        }

        private void bntPaymentAdd_Click(object sender, EventArgs e)
        {

            PaymentDetail paymentDetail = new PaymentDetail();
           
            paymentDetail.AddPayment(selectedSupplier,0,currentBalane);
            paymentDetail.ShowDialog();
        }

    }
}
