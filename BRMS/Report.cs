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
    public partial class Report : Form
    {
        private static string dailyDate;
        private static int accessedEmp = cUserSession.AccessedEmp;
        private static DateTime _dailyDate;
        cDatabaseConnect dbconn = new cDatabaseConnect();
        public static string GetDailyDate
        {
            get { return dailyDate; }
            set { dailyDate = value; }
        }
        private static DateTime Int32ToDataetime()
        {
            string date = $"{dailyDate.Substring(0, 4)}-{dailyDate.Substring(4, 2)}-{dailyDate.Substring(6, 2)}";
            _dailyDate = Convert.ToDateTime(date);
            return _dailyDate;
        }
        public Report()
        {
            InitializeComponent();
            cUIManager.ApplyPopupFormStyle(this);
            //RunQeury();
        }
        /// <summary>
        /// 조회 실행 메소드
        /// </summary>
        public void RunQeury()
        {
            using (SqlConnection connection = dbconn.Opensql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ReportWrite(connection, transaction);
                    transaction.Commit();
                    Close();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 레포트 기록
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void ReportWrite(SqlConnection connection, SqlTransaction transaction)
        {
            DataSet reportDataSet = new DataSet();
            Int32ToDataetime();
            string fromDate = _dailyDate.ToString("d");
            string toDate = _dailyDate.AddDays(1).ToString("d");
            DataTable prodcutData = GetProduct();
            DataTable endStock = EndStock();
            DataTable beginStock = BeginStock();
            DataTable purchaseQty = PuchaseQuer(fromDate, toDate);
            DataTable salesQty = SaleQuery(fromDate, toDate);
            string query;
                      
            InsertWorktable(connection, transaction);
            DailyReportCheck(connection, transaction);
            int recodes = endStock.Rows.Count;
            lblTotal.Text = " / " + recodes.ToString();
            lblProgress.Text = "0";
            pbReport.Maximum = recodes;  // 총 작업 수를 최대값으로 설정
            pbReport.Value = 0;  // 프로그래스바 초기화
            int currentProgress = 0;

            foreach (DataRow row in prodcutData.Rows)
            {

                int pdtCode = cDataHandler.ConvertToInt(row["pdt_code"]);               //제품코드
                int pdtTax = cDataHandler.ConvertToInt(row["pdt_tax"]);                 //과면세 여부
                decimal pdtBprice = cDataHandler.ConvertToDecimal(row["pdt_bprice"]);   //매입단가
                int pdtSprice = cDataHandler.ConvertToInt(row["pdt_sprice_krw"]);       //판매단가
                int pdtTop = cDataHandler.ConvertToInt(row["pdt_top"]); ;
                int pdtMid = cDataHandler.ConvertToInt(row["pdt_mid"]);
                int pdtBot = cDataHandler.ConvertToInt(row["pdt_bot"]);
                int pdtSup = cDataHandler.ConvertToInt(row["pdt_sup"]);
                int purQty = 0;                                                         //매입량
                int saledQty = 0;                                                       //판매량
                int pdtBeginStock = 0;                                                  //기초재고
                int pdtEndStock = 0;                                                    //재고량(기말)
                decimal purAmount = 0;                                                  //매입액
                int saledAmount = 0;                                                    //판매액
                int beginStockAmount = 0;                                               //기초재고액
                int endStockAmount = 0;                                                 //기말재고액
                int ledgerStock = 0;
                int lostStock = 0;
                decimal payCash = 0;
                decimal payCard = 0;
                decimal payAccount = 0;
                decimal payPoint = 0;
                int taxAble = 0;
                int taxFree = 0;
                //기말재고 자료 적용
                foreach (DataRow endStockRow in endStock.Rows)
                {
                    if(pdtCode.Equals(Convert.ToInt32(endStockRow["pdt_code"])))
                    {
                        pdtEndStock = Convert.ToInt32(endStockRow["pdt_stock"]);
                        endStockAmount = Convert.ToInt32(pdtBprice) * pdtEndStock;
                    }
                }
                //매입자료 적용
                foreach (DataRow purRow in purchaseQty.Rows)
                {
                    if (pdtCode.Equals(Convert.ToInt32(purRow["purd_pdt"])))
                    {
                        purQty = Convert.ToInt32(purRow["purd_qty"]);
                        purAmount = Convert.ToDecimal(purRow["pur_amount"]);
                    }
                }
                //판매자료 적용
                foreach (DataRow saleRow in salesQty.Rows)
                {
                    if (pdtCode.Equals(Convert.ToInt32(saleRow["saled_pdt"])))
                    {
                        decimal cash = cDataHandler.ConvertToDecimal(saleRow["spay_cash_krw"]);
                        decimal card = cDataHandler.ConvertToDecimal(saleRow["spay_credit_krw"]);
                        decimal account = cDataHandler.ConvertToDecimal(saleRow["spay_account_krw"]);
                        decimal point = cDataHandler.ConvertToDecimal(saleRow["spay_point_krw"]);
                        decimal amount = cash + card + account + point;
                        decimal _saleAmount = cDataHandler.ConvertToDecimal(saleRow["saled_amount_krw"]);

                        saledQty += Convert.ToInt32(saleRow["saled_qty"]);
                        saledAmount += Convert.ToInt32(saleRow["saled_amount_krw"]);
                        payCash += cash / amount * _saleAmount;
                        payCard += card / amount * _saleAmount;
                        payAccount += account / amount * _saleAmount;
                        payPoint += point / amount * _saleAmount;
                        taxAble += Convert.ToInt32(saleRow["saled_taxable"]);
                        taxFree += Convert.ToInt32(saleRow["saled_taxfree"]);
                    }
                }
                //기초자료 적용
                foreach (DataRow beginRow in beginStock.Rows)
                {
                    if (pdtCode.Equals(Convert.ToInt32(beginRow[0])))
                    {
                        pdtBeginStock = Convert.ToInt32(beginRow[1]);
                        beginStockAmount = Convert.ToInt32(pdtBprice) * pdtBeginStock;
                    }
                }
                //장부상 재고와 로스재고 설정
                ledgerStock = pdtBeginStock + purQty - saledQty;
                lostStock = pdtEndStock - ledgerStock; 

                query = "INSERT INTO dailyreport(dayr_date, dayr_pdt, dayr_sprice, dayr_bprice, dayr_beginstock, dayr_bstockamount, dayr_endstock, dayr_estockamount, dayr_purqty, dayr_puramount, dayr_saleqty, dayr_saleamount,dayr_top,dayr_mid,dayr_bot, dayr_sup,dayr_ledgerstock, dayr_loststock, dayr_paycash, dayr_paycard, dayr_payaccount, dayr_paypoint, dayr_taxable, dayr_taxfree)" +
                    "VALUES(@date,@pdt,@spirice,@brpcie,@beginStoock,@beginStoockAmount,@endStock,@endStockAmount,@purQty, @purAmount, @saleQty, @saleAmount, @pdtTop, @pdtMid, @pdtBot, @pdtSup, @ledgerStock, @lostStock, @payCash, @payCard, @payAccount, @payPoint, @taxAble, @taxFree)";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@date",SqlDbType.Int){Value = dailyDate},
                    new SqlParameter("@pdt",SqlDbType.Int){Value = pdtCode},
                    new SqlParameter("@spirice",SqlDbType.Int){Value = pdtSprice},
                    new SqlParameter("@brpcie",SqlDbType.Float){Value = pdtBprice},
                    new SqlParameter("@beginStoock",SqlDbType.Int){Value = pdtBeginStock},
                    new SqlParameter("@beginStoockAmount",SqlDbType.Int){Value = beginStockAmount},
                    new SqlParameter("@endStock",SqlDbType.Int){Value = pdtEndStock},
                    new SqlParameter("@endStockAmount",SqlDbType.Int){Value = endStockAmount},
                    new SqlParameter("@purQty",SqlDbType.Int){Value = purQty},
                    new SqlParameter("@purAmount",SqlDbType.Float){Value = purAmount},
                    new SqlParameter("@saleQty",SqlDbType.Int){Value = saledQty},
                    new SqlParameter("@pdtTop",SqlDbType.Int){Value = pdtTop},
                    new SqlParameter("@pdtMid",SqlDbType.Int){Value = pdtMid},
                    new SqlParameter("@pdtBot",SqlDbType.Int){Value = pdtBot},
                    new SqlParameter("@pdtSup",SqlDbType.Int){Value = pdtSup},
                    new SqlParameter("@saleAmount",SqlDbType.Int){Value = saledAmount},
                    new SqlParameter("@ledgerStock",SqlDbType.Int){Value = ledgerStock },
                    new SqlParameter("@lostStock",SqlDbType.Int){Value = lostStock},
                    new SqlParameter("@payCash",SqlDbType.Float){Value = payCash},
                    new SqlParameter("@payCard",SqlDbType.Float){Value = payCard},
                    new SqlParameter("@payAccount",SqlDbType.Float){Value = payAccount},
                    new SqlParameter("@payPoint",SqlDbType.Float){Value = payPoint},
                    new SqlParameter("@taxAble",SqlDbType.Int){Value = taxAble},
                    new SqlParameter("@taxFree",SqlDbType.Int){Value = taxFree}

                };
                dbconn.ExecuteNonQuery(query, connection, transaction, parameters);
                currentProgress++;
                pbReport.Value = currentProgress;  // 프로그래스바 값 업데이트
                pbReport.Refresh();  // UI 갱신
                lblProgress.Text = currentProgress.ToString();
            }
            cLog.InsertEmpAccessLogConnect("@dailyReportWrite", accessedEmp, Convert.ToInt32(dailyDate), connection, transaction);
            query = $"UPDATE worktable SET work_exeend = GETDATE() WHERE work_type = 101 AND work_date = @date";
            SqlParameter parameter = new SqlParameter("@date", SqlDbType.Int) { Value = dailyDate };
            dbconn.ExecuteNonQuery(query, connection, transaction, parameter);
        }

        /// <summary>
        /// 기록 대상 제품 정보 조회
        /// </summary>
        /// <returns></returns>
        private DataTable GetProduct()
        {
            DataTable productData = new DataTable();
            string fromDate = _dailyDate.ToString("yyyy-MM-dd");
            string toDate = _dailyDate.AddDays(1).ToString("yyyy-MM-dd");
            string query = "SELECT pdt_code, pdt_stock, pdt_top, pdt_mid, pdt_bot, pdt_bprice, pdt_sprice_krw, pdt_tax, pdt_sup FROM product \n " +
                $"WHERE pdt_code IN (SELECT purd_pdt FROM purchase, purdetail WHERE pur_code = purd_code AND pur_date > {fromDate} AND pur_date < {toDate}) OR " +
                $"pdt_code IN (SELECT saled_pdt FROM sales, saledetail WHERE sale_code = saled_code AND sale_date > {fromDate} AND sale_date <{toDate}) OR pdt_stock <>0";

            dbconn.SqlDataAdapterQuery(query, productData);
            return productData;
        }
        /// <summary>
        /// 기말재고 데이터 조회
        /// </summary>
        /// <returns></returns>
        private DataTable EndStock()
        {
            string SearchDate = _dailyDate.AddDays(1).ToString("d");
            DataTable EndStockData = new DataTable();

            string query = $"SELECT pdt_code, pdt_stock" +
                $" - ISNULL((SELECT sum(purd_qty) FROM purdetail, purchase WHERE pur_code = purd_code AND purd_pdt = pdt_code AND pur_date > '{SearchDate}'),0) " +
                $" + ISNULL((SELECT SUM(saled_qty) FROM saledetail, sales WHERE sale_code = saled_code AND saled_pdt = pdt_code AND sale_date > '{SearchDate}'),0) pdt_stock " +
                "FROM product ORDER BY pdt_code";
            dbconn.SqlDataAdapterQuery(query, EndStockData);
            return EndStockData;
        }
        /// <summary>
        /// 기초재고 데이터 조회
        /// 기본적으로 이전 날짜 기말재고를 가지고 옴
        /// 이전 기말재고가 없을 경우 현재재고와 매입,판매를 역산으로 기초재고를 생성
        /// </summary>
        /// <returns></returns>
        private DataTable BeginStock()
        {
            object resultObj = new object();
            string yesterDay = _dailyDate.AddDays(-1).ToString("yyyyMMdd");
            string query = $"SELECT count(*) FROM worktable WHERE work_type = 101 AND work_date = {yesterDay}";
            DataTable beginStcok = new DataTable();
            dbconn.sqlScalaQuery(query, out resultObj);
            int count = Convert.ToInt32(resultObj);
            if (count == 0)
            {
                query = "SELECT pdt_code, ISNULL(pdt_stock, 0) -ISNULL((SELECT sum(purd_qty) FROM purdetail, purchase WHERE pur_code = purd_code AND purd_pdt = pdt_code AND pur_date < GETDATE()),0) +ISNULL((SELECT SUM(saled_qty) FROM saledetail, sales WHERE sale_code = saled_code AND saled_pdt = pdt_code AND sale_date < GETDATE()),0) beginstock FROM product ORDER BY pdt_code";
                dbconn.SqlDataAdapterQuery(query, beginStcok);
            }
            else
            {
                query = $"SELECT dayr_pdt, dayr_endstock FROM  dailyreport WHERE dayr_date = {yesterDay}";
            }
            dbconn.SqlDataAdapterQuery(query, beginStcok);
            return beginStcok;
        }
        /// <summary>
        /// 매입 데이터 조회
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private DataTable PuchaseQuer(string fromDate, string toDate)
        {
            DataTable purDate = new DataTable();

            string query = $"SELECT purd_pdt,SUM(purd_qty) purd_qty, SUM(purd_amount) pur_amount FROM purchase,purdetail WHERE pur_code = purd_code AND pur_date  > '{fromDate}' AND pur_date < '{toDate}' GROUP BY purd_pdt";
            dbconn.SqlDataAdapterQuery(query, purDate);
            return purDate;
        }
        /// <summary>
        /// 매출 데이터 조회
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private DataTable SaleQuery(string fromDate, string toDate)
        {
            DataTable saleData = new DataTable();
            string query = $"SELECT saled_pdt,saled_qty saled_qty, saled_amount_krw, spay_cash_krw, spay_credit_krw,spay_account_krw, spay_point_krw, CASE WHEN saled_tax = 1 THEN saled_amount_krw ELSE 0 END saled_taxable, CASE WHEN saled_tax = 0 THEN saled_amount_krw ELSE 0 END saled_taxfree " +
                $"FROM (SELECT sale_code FROM sales WHERE sale_date >  '{fromDate}' AND sale_date < '{toDate}' ) as A, saledetail ,salepay  WHERE sale_code = saled_code AND sale_code = spay_code ORDER BY saled_pdt";
            dbconn.SqlDataAdapterQuery(query, saleData);
            return saleData;
        }
        /// <summary>
        /// DB의 worktable에 작업 내역 기록
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void InsertWorktable(SqlConnection connection, SqlTransaction transaction)
        {
            object resultObj = new object();


            string query = $"SELECT count(*) FROM worktable WHERE work_date = {dailyDate} AND work_type = 101";
            dbconn.sqlScalaQuery(query, out resultObj);

            if (Convert.ToInt32(resultObj) > 0)
            {
                query = $"DELETE FROM worktable WHERE work_date = @date AND work_type = 101";
                SqlParameter[] delParameters =
                {
                    new SqlParameter("@date",SqlDbType.Int){Value = dailyDate},
                    new SqlParameter("@emp",SqlDbType.Int){Value = accessedEmp}
                };
                dbconn.ExecuteNonQuery(query, connection, transaction, delParameters);
            }
            query = "INSERT INTO worktable(work_date, work_type, work_exestart, work_emp, work_saleupdate, work_purupdate)" +
                "VALUES(@date, 101,GETDATE(),@emp,0,0)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@date",SqlDbType.Int){Value = dailyDate},
                new SqlParameter("@emp",SqlDbType.Int){Value = accessedEmp}
            };
            dbconn.ExecuteNonQuery(query, connection, transaction, parameters);

        }
        /// <summary>
        /// 일결산 삭제
        /// 일결산 실행 시 기존 자료가 있다면 삭제 진행
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void DailyReportCheck(SqlConnection connection, SqlTransaction transaction)
        {
            object resultObj = new object();
            SqlParameter[] parameters =
            {
                new SqlParameter("@date",SqlDbType.Int){Value = dailyDate},
                new SqlParameter("@emp",SqlDbType.Int){Value = accessedEmp}
            };
            string query = $"SELECT count(*) FROM dailyreport WHERE dayr_date = {dailyDate}";
            dbconn.sqlScalaQuery(query, out resultObj);
            if (Convert.ToInt32(resultObj) > 0)
            {
                query = $"DELETE FROM dailyreport WHERE dayr_date = @date";

                dbconn.ExecuteNonQuery(query, connection, transaction, parameters);
            }
        }
    }
}
