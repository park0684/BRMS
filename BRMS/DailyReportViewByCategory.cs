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
    public partial class DailyReportViewByCategory : Form
    {
        int accessedEmp = cUserSession.AccessedEmp;
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet dgvReport = new cDataGridDefaultSet();
        bool categoryToggle = false;
        int pdtCatTop = 0;
        int pdtCatMid = 0;
        int pdtCatBot = 0;
        public DailyReportViewByCategory()
        {
            InitializeComponent();
            DataGridForm();
            lblCategory.Text = "전체";
            dgvReport.Dgv.ColumnHeaderMouseClick += dgrReport_ColumnHeaderMouseClick;
        }
        /// <summary>
        /// 데이터 그리드 설정
        /// </summary>
        private void DataGridForm()
        {
            pnlDatagrid.Controls.Add(dgvReport.Dgv);
            dgvReport.Dgv.Dock = DockStyle.Fill;
            dgvReport.Dgv.Columns["No"].Visible = false;
            dgvReport.Dgv.Columns.Add("dayrCatName", "분류명");
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
            dgvReport.FormatAsInteger("dayrBStockAmount", "dayrEStockAmount", "dayrPurAmount", "dayrSaleAmount", "dayrDifference", "dayrCost",  "dayrProfitAmount", "dayrLedgerStockAmount", "dayrLostStockAmount", "dayrTaxable", "dayrTaxFree");
            dgvReport.FormatAsDecimal("dayrProfitRate", "dayrPayCash", "dayrPayCard", "dayrPayAccount", "dayrPayPoint");
            dgvReport.FormatAsStringLeft("dayrCatName");

            
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
                query = $"SELECT cat_name_kr, cat_name_en FROM category WHERE cat_top = {top} AND cat_mid = {mid} AND  cat_bot = 0";
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
        /// 데이터 그리드에 조회 내용 표시
        /// </summary>
        /// <param name="resultData"></param>
        private void FillGrid(DataTable resultData)
        {
            dgvReport.Dgv.Rows.Clear();
            //총합용 변수 선언
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

            foreach (DataRow row in resultData.Rows)
            {
                int rowIndex = dgvReport.Dgv.Rows.Add();
                int beginStockAmount = cDataHandler.ConvertToInt(row["dayr_bstockamount"]);
                int endStockAmount = cDataHandler.ConvertToInt(row["dayr_estockamount"]);
                int purchaseAmount = cDataHandler.ConvertToInt(row["dayr_purAmount"]);
                int saleAmount = cDataHandler.ConvertToInt(row["dayr_saleAmount"]);
                int difference = saleAmount - saleAmount;
                int cost = beginStockAmount + purchaseAmount - endStockAmount;
                int profitAmount = saleAmount - cost;
                decimal profitRate = cDataHandler.ConvertProfitRate(saleAmount,cost);
                int ledgerStockAmount = cDataHandler.ConvertToInt(row["dayr_ledgerstockamount"]);
                int lostStockAmount = cDataHandler.ConvertToInt(row["dayr_loststockamount"]);
                int taxAble = cDataHandler.ConvertToInt(row["dayr_taxable"]);
                int tabFree = cDataHandler.ConvertToInt(row["dayr_taxfree"]);
                decimal payCash = cDataHandler.ConvertToDecimal(row["dayr_paycash"]);
                decimal payCard = cDataHandler.ConvertToDecimal(row["dayr_paycard"]);
                decimal payAccount = cDataHandler.ConvertToDecimal(row["dayr_payaccount"]);
                decimal apyPoint = cDataHandler.ConvertToDecimal(row["dayr_paypoint"]);
                int top = cDataHandler.ConvertToInt(row["dayr_top"]);
                int mid = cDataHandler.ConvertToInt(row["dayr_mid"]);
                int bot = cDataHandler.ConvertToInt(row["dayr_bot"]);
                string query = $"SELECT cat_name_kr FROM category WHERE cat_top ={top} AND cat_mid = {mid} AND cat_bot ={bot}";
                object resultObj;
                dbconn.sqlScalaQuery(query, out resultObj);
                string catName = resultObj.ToString();
                
                //함계 변수에 데이터 추가
                totalBeginStockAmount += beginStockAmount;
                totalEndStockAmount += endStockAmount;
                totalpurcasehAmount += purchaseAmount;
                totalSaleAmount += saleAmount;
                totalTaxable += taxAble;
                totalTaxFree += tabFree;
                totalPayCash += payCash;
                totalPayCard += payCard;
                totalPayAccount += payAccount;
                totalPayPoint += apyPoint;
                totalLedgerStockAmount += ledgerStockAmount;
                totalLostStockAmount += lostStockAmount;

                dgvReport.Dgv.Rows[rowIndex].Cells["dayrCatName"].Value = catName;
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
            }
            //합계 변수 계산
            totalDifference = totalSaleAmount - totalpurcasehAmount;
            totalcost = totalBeginStockAmount + totalpurcasehAmount - totalEndStockAmount;
            totalProfitAmount = totalSaleAmount - totalcost;
            totalProfitRate = cDataHandler.ConvertProfitRate(totalSaleAmount, totalcost);
            // 총합 로우 생성 및 데이터 등록
            int newRow = dgvReport.Dgv.Rows.Add();
            dgvReport.Dgv.Rows[newRow].Cells["dayrCatName"].Value = "합계";
            dgvReport.Dgv.Rows[newRow].Cells["dayrBStockAmount"].Value = totalBeginStockAmount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrEStockAmount"].Value = totalEndStockAmount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrPurAmount"].Value = totalpurcasehAmount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrSaleAmount"].Value = totalSaleAmount;
            dgvReport.Dgv.Rows[newRow].Cells["dayrDifference"].Value = totalDifference;
            dgvReport.Dgv.Rows[newRow].Cells["dayrCost"].Value = totalcost ;
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
            //칼럼 및 로우 색상 설정

            string[] column1 = new string[] { "dayrBStockAmount", "dayrEStockAmount", "dayrLedgerStockAmount", "dayrLostStockAmount" };
            string[] column2 = new string[] { "dayrPurAmount", "dayrSaleAmount", "dayrDifference" };
            dgvReport.SetColumnCellColors(dgvReport.Dgv, cUIManager.Color.BrightGray, Color.Black, "dayrDate", "dayrCatName");
            dgvReport.SetColumnCellColors(dgvReport.Dgv, cUIManager.Color.PastelLightBlue, cUIManager.Color.Black, column1);
            dgvReport.SetColumnCellColors(dgvReport.Dgv, cUIManager.Color.PastelGreen, cUIManager.Color.Black, column2);
            dgvReport.SetRowCellColors(dgvReport.Dgv, cUIManager.Color.PastelRose, cUIManager.Color.Black, newRow);
            
        }
        /// <summary>
        /// 조회용 쿼리 설정
        /// </summary>
        private void SearchQuery()
        {
            DataTable resultData = new DataTable();
            string fromDate = dtpDateFrom.Value.ToString("yyyyMMdd");
            string toDate = dtpDateTo.Value.AddDays(1).ToString("yyyyMMdd");
            string select = "dayr_top, 0 dayr_mid, 0 dayr_bot,";
            string groupby = "dayr_top ";
            string orderby = "dayr_top ";
            // 기본 쿼리문
            string query = $"SUM(dayr_purqty) dayr_purqty, SUM(dayr_purAmount) dayr_purAmount, SUM(dayr_saleqty) dayr_saleqty, " +
                $"SUM(dayr_saleAmount) dayr_saleAmount,SUM(dayr_taxable) dayr_taxable, SUM(dayr_taxfree) dayr_taxfree, SUM(dayr_paycash) dayr_paycash, " +
                $"SUM(dayr_paycard) dayr_paycard, SUM(dayr_payaccount) dayr_payaccount, SUM(dayr_paypoint) dayr_paypoint, SUM(dayr_bstockamount) dayr_bstockamount, " +
                $"SUM(dayr_estockamount) dayr_estockamount, SUM(ISNULL(dayr_ledgerstock * dayr_bprice, 0)) dayr_ledgerstockamount ,SUM(ISNULL(dayr_loststock * dayr_bprice, 0)) dayr_loststockamount " +
                $"FROM dailyreport WHERE dayr_date >= {fromDate} AND dayr_date < {toDate} ";
            //대분류 지정 시 쿼리문 추가
            if (!pdtCatTop.Equals(0))
            {
                query += $"AND dayr_top ={pdtCatTop} ";
                select = "dayr_top, dayr_mid, 0 dayr_bot,";
                groupby += ",dayr_mid ";
                orderby += ",dayr_mid ";
            }
            //중분류 지정 시 쿼리문 추가
            if (!pdtCatMid.Equals(0))
            {
                query += $"AND dayr_mid = {pdtCatMid} ";
                select = "dayr_top, dayr_mid, dayr_bot, ";
                groupby += ",dayr_bot";
                orderby += ",dayr_bot ";
            }
            //소분류 지정 시 쿼리문 추가
            if (!pdtCatBot.Equals(0))
            {
                query += $"AND dayr_bot = {pdtCatBot} ";
            }
            //최종 쿼리문 설정
            query = $"SELECT {select}{query} GROUP BY {groupby} ORDER BY {orderby}";
            dbconn.SqlDataAdapterQuery(query, resultData);
            FillGrid(resultData);
        }
        /// <summary>
        /// 컬럼 헤드 클릭 시 소트 설정
        /// 마지막 합계 행은 제외 한다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgrReport_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvReport.ExcludeLastRowSort(e);

        }
        /// <summary>
        /// main폼 조회 클릭 실행
        /// </summary>
        public void RunQuery()
        {
            try
            {
                SearchQuery();
                cLog.InsertEmpAccessLogNotConnect("@dailyReportSearch", accessedEmp, 2);
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
