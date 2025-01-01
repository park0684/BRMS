using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRMS
{
    public partial class DailyReportViewByProduct : Form
    {
        int accessedEmp = cUserSession.AccessedEmp;
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet dgvReport = new cDataGridDefaultSet();
        bool categoryToggle = false;
        int pdtCatTop = 0;
        int pdtCatMid = 0;
        int pdtCatBot = 0;
        public DailyReportViewByProduct()
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
            dgvReport.Dgv.Columns.Add("dayrDate", "일자");
            dgvReport.Dgv.Columns.Add("dayrPdtName", "제품명");
            dgvReport.Dgv.Columns.Add("dayrPdtNumber", "제품번호");
            dgvReport.Dgv.Columns.Add("dayrBeginStock", "기초재고");
            dgvReport.Dgv.Columns.Add("dayrBStockAmount", "기초재고액");
            dgvReport.Dgv.Columns.Add("dayrEndStock", "기말재고");
            dgvReport.Dgv.Columns.Add("dayrEStockAmount", "기말재고액");
            dgvReport.Dgv.Columns.Add("dayrPurQty", "매입량");
            dgvReport.Dgv.Columns.Add("dayrPurAmount", "매입액");
            dgvReport.Dgv.Columns.Add("dayrSaleQty", "매출량");
            dgvReport.Dgv.Columns.Add("dayrSaleAmount", "매출액");
            dgvReport.Dgv.Columns.Add("dayrDifference", "매출입차");
            dgvReport.Dgv.Columns.Add("dayrCost", "매출원가");
            dgvReport.Dgv.Columns.Add("dayrProfitRate", "이익율");
            dgvReport.Dgv.Columns.Add("dayrProfitAmount", "이익액");
            dgvReport.Dgv.Columns.Add("dayrLedgerStock", "장부상재고");
            dgvReport.Dgv.Columns.Add("dayrLedgerStockAmount", "장부상재고액");
            dgvReport.Dgv.Columns.Add("dayrLostStock", "재고로스");
            dgvReport.Dgv.Columns.Add("dayrLostStockAmount", "재로로스액");
            dgvReport.Dgv.Columns.Add("dayrTaxable", "과세");
            dgvReport.Dgv.Columns.Add("dayrTaxFree", "면세");
            dgvReport.Dgv.Columns.Add("dayrPayCash", "현금");
            dgvReport.Dgv.Columns.Add("dayrPayCard", "카드");
            dgvReport.Dgv.Columns.Add("dayrPayAccount", "계좌이체");
            dgvReport.Dgv.Columns.Add("dayrPayPoint", "포인트");
            dgvReport.ApplyDefaultColumnSettings();
            dgvReport.FormatAsInteger("dayrBStockAmount", "dayrBeginStock", "dayrEStockAmount", "dayrEndStock", "dayrPurAmount", "dayrSaleAmount", "dayrDifference", "dayrCost", "dayrProfitAmount", "dayrLedgerStock", "dayrLedgerStockAmount", "dayrLostStock", "dayrLostStockAmount", "dayrTaxable", "dayrTaxFree", "dayrPayCash", "dayrPayCard", "dayrPayAccount", "dayrPayPoint");
            dgvReport.FormatAsStringLeft("dayrpdtName", "dayrpdtNumber");
            dgvReport.FormatAsDecimal("dayrProfitRate");
            dgvReport.Dgv.ReadOnly = true;
            dgvReport.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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
            int dataTableCount = resultData.Rows.Count;
            DataTable totalData = new DataTable();
            foreach (DataRow totalRow in resultData.Rows)
            {

                totalpurcasehAmount += cDataHandler.ConvertToInt(totalRow["dayr_purAmount"]);
                totalSaleAmount += cDataHandler.ConvertToInt(totalRow["dayr_saleAmount"]);
                totalTaxable += cDataHandler.ConvertToInt(totalRow["dayr_taxable"]);
                totalTaxFree += cDataHandler.ConvertToInt(totalRow["dayr_taxfree"]);
                totalPayCash += cDataHandler.ConvertToDecimal(totalRow["dayr_paycash"]);
                totalPayCard += cDataHandler.ConvertToDecimal(totalRow["dayr_paycard"]);
                totalPayAccount += cDataHandler.ConvertToDecimal(totalRow["dayr_payaccount"]);
                totalPayPoint += cDataHandler.ConvertToDecimal(totalRow["dayr_paypoint"]);
                totalBeginStockAmount += cDataHandler.ConvertToInt(totalRow["dayr_bstockamount"]);
                totalEndStockAmount += cDataHandler.ConvertToInt(totalRow["dayr_estockamount"]);
                totalLedgerStockAmount += cDataHandler.ConvertToInt(totalRow["dayr_ledgerstockamount"]);
                totalLostStockAmount += cDataHandler.ConvertToInt(totalRow["dayr_loststockamount"]);
            }
            
            foreach (DataRow row in resultData.Rows)
            {
                int rowIndex = dgvReport.Dgv.Rows.Add();
                string dayrdate = "";
                int beginStock = cDataHandler.ConvertToInt(row["dayr_beginstock"]);
                int beginStockAmount = cDataHandler.ConvertToInt(row["dayr_bstockamount"]);
                int endStock = cDataHandler.ConvertToInt(row["dayr_endstock"]);
                int endStockAmount = cDataHandler.ConvertToInt(row["dayr_estockamount"]);
                int purchaseQty = cDataHandler.ConvertToInt(row["dayr_purqty"]);
                int purchaseAmount = cDataHandler.ConvertToInt(row["dayr_purAmount"]);
                int saleQty = cDataHandler.ConvertToInt(row["dayr_saleqty"]);
                int saleAmount = cDataHandler.ConvertToInt(row["dayr_saleAmount"]);
                int cost = beginStockAmount + purchaseAmount - endStockAmount;
                int profitAmount = saleAmount== 0 ? 0 : saleAmount - cost;
                decimal profitRate = cDataHandler.ConvertProfitRate(saleAmount,cost);
                int ledgerStock = cDataHandler.ConvertToInt(row["dayr_ledgerstock"]);
                int ledgerStockAmount = cDataHandler.ConvertToInt(row["dayr_ledgerstockamount"]);
                int lostStock = cDataHandler.ConvertToInt(row["dayr_loststock"]);
                int lostStockAmount = cDataHandler.ConvertToInt(row["dayr_loststockamount"]);
                int taxAble = cDataHandler.ConvertToInt(row["dayr_taxable"]);
                int tabFree = cDataHandler.ConvertToInt(row["dayr_taxfree"]);
                int payCash = cDataHandler.ConvertToInt(row["dayr_paycash"]);
                int payCard = cDataHandler.ConvertToInt(row["dayr_paycard"]);
                int payAccount = cDataHandler.ConvertToInt(row["dayr_payaccount"]);
                int apyPoint = cDataHandler.ConvertToInt(row["dayr_paypoint"]);
                string pdtName = row["pdt_name_kr"].ToString();
                string pdtNumber = row["pdt_number"].ToString(); ;

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

                if (chkDayli.Checked)//일자별 체크시 조회 기간으로 표시
                {
                    dayrdate = dtpDateFrom.Value.ToString("d") + '~' + dtpDateTo.Value.ToString("d");
                }
                else
                {
                    dayrdate = row["dayr_date"].ToString();
                }

                //string query = $"SELECT pdt_name_kr, pdt_number FROM product WHERE pdt_code = {pdtCode}";
                //DataTable pdtData = new DataTable();
                //dbconn.SqlReaderQuery(query, pdtData);
                //DataRow pdtRow = pdtData.Rows[0];
                //pdtName = pdtRow["pdt_name_kr"].ToString().Trim();
                //pdtNumber = pdtRow["pdt_number"].ToString().Trim();

                if (!profitAmount.Equals(0) && saleAmount.Equals(0))
                {
                    if (saleAmount.Equals(0))
                    {
                        profitRate = -100;
                    }
                    else
                    {
                        profitRate = cDataHandler.ConvertToDecimal(profitAmount / saleAmount * 100);
                    }

                }
                dgvReport.Dgv.Rows[rowIndex].Cells["No"].Value = rowIndex + 1;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrDate"].Value = dayrdate;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPdtName"].Value = pdtName;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPdtNumber"].Value = pdtNumber;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrBeginStock"].Value = beginStock;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrBStockAmount"].Value = beginStockAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrEndStock"].Value = endStock;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrEStockAmount"].Value = endStockAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPurQty"].Value = purchaseQty;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPurAmount"].Value = purchaseAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrSaleQty"].Value = saleQty;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrSaleAmount"].Value = saleAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrDifference"].Value = saleAmount - purchaseAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrCost"].Value = cost;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrProfitRate"].Value = profitRate;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrProfitAmount"].Value = profitAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrLedgerStock"].Value = ledgerStock;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrLedgerStockAmount"].Value = ledgerStockAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrLostStock"].Value = lostStock;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrLostStockAmount"].Value = lostStockAmount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrTaxable"].Value = taxAble;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrTaxFree"].Value = tabFree;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPayCash"].Value = payCash;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPayCard"].Value = payCard;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPayAccount"].Value = payAccount;
                dgvReport.Dgv.Rows[rowIndex].Cells["dayrPayPoint"].Value = apyPoint;
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
            
            ///칼럼 색상 설정
            string[] column1 = new string[] { "dayrBeginStock", "dayrBStockAmount", "dayrEndStock", "dayrEStockAmount", "dayrLedgerStock", "dayrLedgerStockAmount", "dayrLostStock", "dayrLostStockAmount" };
            string[] column2 = new string[] { "dayrPurQty", "dayrPurAmount", "dayrSaleQty", "dayrSaleAmount", "dayrDifference" };
            dgvReport.SetColumnCellColors(dgvReport.Dgv, cUIManager.Color.BrightGray, Color.Black, "dayrDate", "dayrCatName");
            dgvReport.SetColumnCellColors(dgvReport.Dgv, cUIManager.Color.PastelLightBlue, cUIManager.Color.Black, column1);
            dgvReport.SetColumnCellColors(dgvReport.Dgv, cUIManager.Color.PastelGreen, cUIManager.Color.Black, column2);
            dgvReport.SetRowCellColors(dgvReport.Dgv, cUIManager.Color.PastelRose, cUIManager.Color.Black, newRow);



        }
        /// <summary>
        /// 조회 쿼리 설정
        /// </summary>
        private void SearchQuery()
        {
            DataTable resultData = new DataTable();
            string fromDate = dtpDateFrom.Value.ToString("yyyyMMdd");
            string toDate = dtpDateTo.Value.AddDays(1).ToString("yyyyMMdd");
            string searchWord = tBoxSearchWord.Text.Trim();

            string query = $"SELECT dayr_date, pdt_number, pdt_name_kr, dayr_pdt,dayr_bprice,dayr_sprice,dayr_beginstock,dayr_endstock, dayr_purqty, dayr_purAmount, dayr_saleqty,dayr_saleAmount, " +
                $"dayr_bstockamount, dayr_estockamount ,dayr_ledgerstock,(dayr_ledgerstock * dayr_bprice) dayr_ledgerstockamount,dayr_loststock, " +
                $"(dayr_loststock * dayr_bprice) dayr_loststockamount,dayr_taxable,dayr_taxfree,dayr_paycash,dayr_paycard,dayr_payaccount,dayr_paypoint " +
                $"FROM dailyreport,product WHERE  dayr_date >= {fromDate} AND dayr_date < {toDate} AND pdt_code = dayr_pdt ";
            if(chkDayli.Checked)//일자별 합계 체크시 쿼리문 수정
            {
                query = $"SELECT MIN(dayr_date) ,MAX(dayr_date) FROM dailyreport WHERE dayr_date >= {fromDate} AND dayr_date <= {toDate}";
                DataTable dateData = new DataTable();
                dbconn.SqlReaderQuery(query, dateData);
                DataRow dateRow = dateData.Rows[0];
                fromDate = dateRow[0].ToString();
                toDate = dateRow[1].ToString();

                query = "SELECT A.dayr_pdt, pdt_number, pdt_name_kr,dayr_purqty,dayr_purAmount,dayr_saleqty,dayr_saleAmount,dayr_taxable,dayr_taxfree,dayr_paycash,dayr_paycard,dayr_payaccount, " +
                    "dayr_paypoint,dayr_beginstock,dayr_bstockamount,dayr_endstock,dayr_estockamount,dayr_ledgerstock,dayr_ledgerstockamount,dayr_loststock,dayr_loststockamount " +
                    $"FROM(SELECT dayr_pdt, SUM(dayr_purqty) dayr_purqty, SUM(dayr_purAmount) dayr_purAmount, SUM(dayr_saleqty) dayr_saleqty, SUM(dayr_saleAmount) dayr_saleAmount, " +
                    $"SUM(dayr_taxable) dayr_taxable, SUM(dayr_taxfree) dayr_taxfree, SUM(dayr_paycash) dayr_paycash, SUM(dayr_paycard) dayr_paycard, SUM(dayr_payaccount) dayr_payaccount, " +
                    $"SUM(dayr_paypoint) dayr_paypoint FROM dailyreport WHERE dayr_date >= {fromDate} AND dayr_date <= {toDate} GROUP BY dayr_pdt) as A, " +
                    $"(SELECT dayr_pdt, dayr_beginstock, dayr_bstockamount FROM dailyreport WHERE dayr_date = {fromDate}) as B, " +
                    $"(SELECT dayr_pdt, dayr_endstock, dayr_estockamount, dayr_ledgerstock, ISNULL(dayr_ledgerstock * dayr_bprice, 0) dayr_ledgerstockamount ,dayr_loststock, " +
                    $"ISNULL(dayr_loststock * dayr_bprice, 0) dayr_loststockamount FROM dailyreport WHERE dayr_date = {dtpDateTo.Value.ToString("yyyyMMdd")}) as C " +
                    $",product " +
                    $"WHERE A.dayr_pdt = B.dayr_pdt AND A.dayr_pdt = C.dayr_pdt AND pdt_code = A.dayr_pdt ";

            }
            if(!string.IsNullOrEmpty(searchWord))
            {
                query += $"AND (pdt_number LIKE '%{searchWord}%' OR pdt_name_kr LIKE '%{searchWord}%' OR pdt_name_en LIKE '%{searchWord}%') ";
            }
            if (!pdtCatTop.Equals(0))
            {
                query += $"AND dayr_top ={pdtCatTop} ";
            }
            if (!pdtCatMid.Equals(0))
            {
                query += $"AND dayr_mid = {pdtCatMid} ";
            }
            if (!pdtCatBot.Equals(0))
            {
                query += $"AND dayr_bot = {pdtCatBot} ";
            }
            if (!chkDayli.Checked)
            {query += "ORDER BY dayr_date, dayr_pdt"; } 

            dbconn.SqlDataAdapterQuery(query, resultData);
            FillGrid(resultData);
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
