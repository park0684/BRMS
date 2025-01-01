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
    public partial class DailyReportViewByDay : Form
    {
        int accessedEmp = cUserSession.AccessedEmp;
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet dgvReport = new cDataGridDefaultSet();
        int pdtCatTop = 0;
        int pdtCatMid = 0;
        int pdtCatBot = 0;
        bool categoryToggle = false;
        public DailyReportViewByDay()
        {
            InitializeComponent();
            DataGridForm();
            lblCategory.Text = "전체";
        }
        /// <summary>
        /// 데이터 그리드 설정
        /// </summary>
        private void DataGridForm()
        {
            pnlDatagrid.Controls.Add(dgvReport.Dgv);
            dgvReport.Dgv.Dock = DockStyle.Fill;
            dgvReport.Dgv.Columns["No"].Visible = false;
            dgvReport.Dgv.Columns.Add("dayrDate", "일자");
            dgvReport.Dgv.Columns.Add("dayrBStockAmount", "기초재고액");
            dgvReport.Dgv.Columns.Add("dayrEStockAmount", "기말재고액");
            dgvReport.Dgv.Columns.Add("dayrPurAmount", "매입액");
            dgvReport.Dgv.Columns.Add("dayrSaleAmount", "매출액");
            dgvReport.Dgv.Columns.Add("dayrDifference", "매출입차");
            dgvReport.Dgv.Columns.Add("dayrCost", "매출원가");
            dgvReport.Dgv.Columns.Add("dayrProfitRate", "이익율");
            dgvReport.Dgv.Columns.Add("dayrProfitAmount", "이익액");
            dgvReport.Dgv.Columns.Add("dayrLedgerStockAmount", "장부상재고액");
            dgvReport.Dgv.Columns.Add("dayrLostStockAmount", "재로로스액");
            dgvReport.Dgv.Columns.Add("dayrTaxable", "과세");
            dgvReport.Dgv.Columns.Add("dayrTaxFree", "면세");
            dgvReport.Dgv.Columns.Add("dayrPayCash", "현금");
            dgvReport.Dgv.Columns.Add("dayrPayCard", "카드");
            dgvReport.Dgv.Columns.Add("dayrPayAccount", "계좌이체");
            dgvReport.Dgv.Columns.Add("dayrPayPoint", "포인트");
            dgvReport.ApplyDefaultColumnSettings();
            dgvReport.FormatAsInteger("dayrBStockAmount", "dayrEStockAmount", "dayrPurAmount", "dayrSaleAmount", "dayrDifference", "dayrCost", "dayrProfitAmount", "dayrLedgerStock", "dayrLostStock", "dayrTaxable", "dayrDutyFree", "dayrPayCash", "dayrPayCard", "dayrPayAacount", "dayrPayPoint");
            dgvReport.FormatAsDecimal("dayrRate", "dayrProfitRate");
            dgvReport.FormatAsStringCenter("dayrDate");                                                                                                                                                                                      
        }
        /// <summary>
        /// 분류 버튼 클릭 후 받아온 분류 정보로 라벨에 표시
        /// </summary>
        /// <param name="top"></param>
        /// <param name="mid"></param>
        /// <param name="bot"></param>
        private void GetCategoryInfo(int top, int mid, int bot)
        {
            DataTable dataTable = new DataTable();
            DataRow dataRow;
            string query;
            string categoryText;

            query = $"SELECT cat_name_kr,cat_name_en FROM category WHERE cat_top = {top} AND cat_mid = 0 AND  cat_bot = 0";
            dataTable.Reset();
            dbconn.SqlReaderQuery(query, dataTable);
            dataRow = dataTable.Rows[0];
            categoryText = string.Format("{0}({1})", dataRow["cat_name_kr"].ToString(), dataRow["cat_name_en"].ToString());
            if (mid != 0)
            {
                query = $"SELECT cat_name_kr,cat_name_en FROM category WHERE cat_top = {top} AND cat_mid = {mid} AND  cat_bot = 0";
                dataTable.Reset();
                dbconn.SqlReaderQuery(query, dataTable);
                dataRow = dataTable.Rows[0];
                categoryText = categoryText + string.Format(" ▶ {0}({1})", dataRow["cat_name_kr"].ToString(), dataRow["cat_name_en"].ToString());
            }

            if (mid != 0 && bot != 0)
            {
                query = $"SELECT cat_name_kr,cat_name_en FROM category WHERE cat_top = {top} AND cat_mid = {mid} AND  cat_bot = {bot}";
                dataTable.Reset();
                dbconn.SqlReaderQuery(query, dataTable);
                dataRow = dataTable.Rows[0];
                categoryText = categoryText + string.Format(" ▶ {0}({1})", dataRow["cat_name_kr"].ToString(), dataRow["cat_name_en"].ToString());
            }
            lblCategory.Text = categoryText;
            pdtCatTop = top;
            pdtCatMid = mid;
            pdtCatBot = bot;
            categoryToggle = true;
        }
        /// <summary>
        /// 조회 쿼리 설정
        /// </summary>
        private void SearchQuery()
        {
            DataTable resultData = new DataTable();
            string fromDate = dtpDateFrom.Value.ToString("yyyyMMdd");
            string toDate = dtpDateTo.Value.AddDays(1).ToString("yyyyMMdd");

            string query = $"SELECT dayr_date, SUM(dayr_purAmount) dayr_purAmount, SUM(dayr_saleAmount) dayr_saleAmount, SUM(dayr_bstockamount) dayr_bstockamount, SUM(dayr_estockamount) dayr_estockamount ,SUM(dayr_ledgerstock * dayr_bprice) dayr_ledgerstock,SUM(dayr_loststock * dayr_bprice) dayr_loststock,SUM(dayr_taxable) dayr_taxable,SUM(dayr_taxfree) dayr_taxfree,SUM(dayr_paycash) dayr_paycash,SUM(dayr_paycard) dayr_paycard,SUM(dayr_payaccount) dayr_payaccount,SUM(dayr_paypoint) dayr_paypoint " +
                $"FROM dailyreport WHERE dayr_date >= {fromDate} AND dayr_date < {toDate} ";
            if(!pdtCatTop.Equals(0))
            {
                query += $"AND dayr_top ={pdtCatTop} ";
            }
            if(!pdtCatMid.Equals(0))
            {
                query += $"AND dayr_mid = {pdtCatMid} ";
            }
            if(!pdtCatBot.Equals(0))
            {
                query += $"AND dayr_bot = {pdtCatBot} ";
            }
            query += "GROUP BY dayr_date ORDER BY dayr_date";

            dbconn.SqlDataAdapterQuery(query, resultData);
            FillGrid(resultData);
            
        }
        //dayr_date,SUM(dayr_beginstock), SUM(dayr_endstock),  SUM(dayr_purAmount), SUM(dayr_saleqty), SUM(dayr_saleAmount), SUM(dayr_bsockamount), SUM(dayr_estockamount)
        private void FillGrid(DataTable resultDate)
        {
            dgvReport.Dgv.Rows.Clear();
            int totalAmount = 0;
            // 총합용 변수 선언
            int totalBeginStockAmount = 0;
            int totalEndStockAmount = 0;
            int totalpurcasehAmount = 0;
            int totalSaleAmount = 0;
            int totalDifference = 0;
            int totalcost = 0;
            int totalProfitAmount = 0;
            decimal totalProfitRate = 0;
            int totalLedgerStockAmount = 0;
            int totalLostStockAmount = 0;
            int totalTaxable = 0;
            int totalTaxFree = 0;
            decimal totalPayCash = 0;
            decimal totalPayCard = 0;
            decimal totalPayAccount = 0;
            decimal totalPayPoint = 0;

            foreach (DataRow row in resultDate.Rows)
            {
                int rowIndex = dgvReport.Dgv.Rows.Add();
                DateTime reportDate = cDataHandler.ConvertIntToDate(row["dayr_date"]);
                int beginStockAmount = cDataHandler.ConvertToInt(row["dayr_bstockamount"]);
                int endStockAmount = cDataHandler.ConvertToInt(row["dayr_estockamount"]);
                int purchaseAmount = cDataHandler.ConvertToInt(row["dayr_purAmount"]);
                int saleAmount = cDataHandler.ConvertToInt(row["dayr_saleAmount"]);
                int cost = beginStockAmount + purchaseAmount - endStockAmount;
                int profitAmount = saleAmount - cost;
                decimal profitRate = cDataHandler.ConvertProfitRate(saleAmount, cost);
                int ledgerStockAmount = cDataHandler.ConvertToInt(row["dayr_ledgerstock"]);
                int lostStockAmount = cDataHandler.ConvertToInt(row["dayr_loststock"]);
                int taxAble = cDataHandler.ConvertToInt(row["dayr_taxable"]);
                int tabFree = cDataHandler.ConvertToInt(row["dayr_taxfree"]);
                decimal payCash = cDataHandler.ConvertToDecimal(row["dayr_paycash"]);
                decimal payCard = cDataHandler.ConvertToDecimal(row["dayr_paycard"]);
                decimal payAccount = cDataHandler.ConvertToDecimal(row["dayr_payaccount"]);
                decimal apyPoint = cDataHandler.ConvertToDecimal(row["dayr_paypoint"]);

                totalpurcasehAmount += purchaseAmount;
                totalSaleAmount += saleAmount;
                totalTaxable += taxAble;
                totalTaxFree += tabFree;
                totalPayCash += payCash;
                totalPayCard += payCard;
                totalPayAccount += payAccount;
                totalPayPoint += apyPoint;
                totalBeginStockAmount += beginStockAmount;
                totalEndStockAmount += endStockAmount;
                totalLedgerStockAmount += ledgerStockAmount;
                totalLostStockAmount += lostStockAmount;

                dgvReport.Dgv.Rows[rowIndex].Cells["dayrDate"].Value = reportDate.ToString("d");
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrBStockAmount"].Value = beginStockAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrEStockAmount"].Value = endStockAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPurAmount"].Value = purchaseAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrSaleAmount"].Value = saleAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrDifference"].Value = saleAmount - purchaseAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrCost"].Value = cost;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrProfitRate"].Value = profitRate;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrProfitAmount"].Value = profitAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrLedgerStockAmount"].Value = ledgerStockAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrLostStockAmount"].Value = lostStockAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrTaxable"].Value = taxAble;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrTaxFree"].Value = tabFree;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPayCash"].Value = payCash;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPayCard"].Value = payCard;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPayAccount"].Value = payAccount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPayPoint"].Value = apyPoint;
                totalAmount += saleAmount;
            }
            totalDifference = totalSaleAmount - totalpurcasehAmount;
            totalcost = totalBeginStockAmount + totalpurcasehAmount - totalEndStockAmount;
            totalProfitAmount = totalSaleAmount - totalcost;
            totalProfitRate = cDataHandler.ConvertProfitRate(totalSaleAmount, totalcost);
            //합계 로우 생성 및 등록
            int newRow = dgvReport.Dgv.Rows.Add();
            dgvReport.Dgv.Rows[newRow].Cells["dayrDate"].Value = "합계";
            dgvReport.Dgv.Rows[newRow].Cells["dayrBStockAmount"].Value = totalBeginStockAmount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrEStockAmount"].Value = totalEndStockAmount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrPurAmount"].Value = totalpurcasehAmount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrSaleAmount"].Value = totalSaleAmount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrDifference"].Value = totalDifference;
            dgvReport.Dgv.Rows[newRow].Cells["dayrCost"].Value = totalcost;
            dgvReport.Dgv.Rows[newRow].Cells["dayrProfitRate"].Value = totalProfitRate;
            dgvReport.Dgv.Rows[newRow].Cells["dayrProfitAmount"].Value = totalProfitAmount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrLedgerStockAmount"].Value = totalLedgerStockAmount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrLostStockAmount"].Value = totalLostStockAmount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrTaxable"].Value = totalTaxable;
            dgvReport.Dgv.Rows[newRow].Cells["dayrTaxFree"].Value = totalTaxFree;
            dgvReport.Dgv.Rows[newRow].Cells["dayrPayCash"].Value = totalPayCash;
            dgvReport.Dgv.Rows[newRow].Cells["dayrPayCard"].Value = totalPayCard;
            dgvReport.Dgv.Rows[newRow].Cells["dayrPayAccount"].Value = totalPayAccount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrPayPoint"].Value = totalPayPoint;
            string[] column1 = new string[] { "dayrBStockAmount", "dayrEStockAmount", "dayrLedgerStockAmount", "dayrLostStockAmount" };
            string[] column2 = new string[] { "dayrPurAmount", "dayrSaleAmount", "dayrDifference" };
            dgvReport.SetColumnCellColors(dgvReport.Dgv, cUIManager.Color.BrightGray, Color.Black, "dayrDate");
            dgvReport.SetColumnCellColors(dgvReport.Dgv, cUIManager.Color.PastelLightBlue, cUIManager.Color.Black, column1);
            dgvReport.SetColumnCellColors(dgvReport.Dgv, cUIManager.Color.PastelGreen, cUIManager.Color.Black, column2);
            dgvReport.SetRowCellColors(dgvReport.Dgv, cUIManager.Color.PastelRose, cUIManager.Color.Black, newRow);
        }
        /// <summary>
        /// main폼 조회 클릭 실행
        /// </summary>
        public void RunQuery()
        {
            try
            {
                SearchQuery();
                cLog.InsertEmpAccessLogNotConnect("@dailyReportSearch", accessedEmp, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 분류 버튼 클릭시 분류 선택
        /// 이미 분류가 선택되었다면 전체 분류로 변경 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCategory_Click(object sender, EventArgs e)
        {
            if (categoryToggle == true)
            {
                categoryToggle = false;
                lblCategory.Text = "전체";
                pdtCatTop = 0;
                pdtCatMid = 0;
                pdtCatBot = 0;
            }
            else
            {
                CategoryBoard categoryBoard = new CategoryBoard();
                categoryBoard.CategorySelected += (top, mid, bot) => { GetCategoryInfo(top, mid, bot); };
                categoryBoard.SearchMode();
                categoryBoard.ShowDialog();
                if (pdtCatTop != 0) { categoryToggle = true; }
            }
        }
    }
}
