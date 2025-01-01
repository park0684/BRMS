using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace BRMS
{
    class DailyRepoft
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
        public void RunQeury()
        {
            using (SqlConnection connection = dbconn.Opensql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    total(connection,transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
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
        private void CreateProgressBar()
        {
            Panel pnlProgress = new Panel();
            pnlProgress.Size = new System.Drawing.Size(80,450);
            pnlProgress.BackColor = cUIManager.Color.White;
            
            Label lblCounter = new Label();
            lblCounter.Font = new System.Drawing.Font("맑은 고딕", 9f);
            lblCounter.Dock = DockStyle.Right;
           
            ProgressBar pbReport = new ProgressBar();
            pbReport.Size = new System.Drawing.Size(70, 400);
            pbReport.Dock = DockStyle.Left;
            pnlProgress.Controls.Add(pbReport);
            pnlProgress.Controls.Add(lblCounter);
        }
        private void total(SqlConnection connection, SqlTransaction transaction)
        {
            DataSet reportDataSet = new DataSet();
            Int32ToDataetime();
            string startDate = _dailyDate.ToString("yyyy-MM-dd");
            string endDate = _dailyDate.AddDays(1).ToString("yyyy-MM-dd");
            DataTable endStock = EndStock();
            DataTable beginStock = BeginStock();
            DataTable purchaseQty = PuchaseQuer(startDate, endDate);
            DataTable salesQty = SaleQuery(startDate, endDate);
            int recodes = endStock.Rows.Count;
            int count = 0;
            string progressValue = $"{count}/{recodes}";
            string query;
            InsertWorktable(connection, transaction);
            DailyReportCheck(connection, transaction);
            CreateProgressBar();
            

            foreach (DataRow row in endStock.Rows)
            {
                
                int pdtCode = cDataHandler.ConvertToInt(row["pdt_code"]);               //제품코드
                int pdtTax = cDataHandler.ConvertToInt(row["pdt_tax"]);                 //과면세 여부
                decimal pdtBprice = cDataHandler.ConvertToDecimal(row["pdt_bprice"]);   //매입단가
                int pdtSprice = cDataHandler.ConvertToInt(row["pdt_sprice_krw"]);       //판매단가
                int pdtStock = cDataHandler.ConvertToInt(row["pdt_stock"]);             //재고량(기말)
                int pdtTop = cDataHandler.ConvertToInt(row["pdt_top"]); ;
                int pdtMid = cDataHandler.ConvertToInt(row["pdt_mid"]);
                int pdtBot = cDataHandler.ConvertToInt(row["pdt_bot"]);
                int pdtSup = cDataHandler.ConvertToInt(row["pdt_sup"]);
                int purQty = 0;                                                         //매입량
                int saledQty = 0;                                                       //판매량
                int pdtBeginStock = 0;                                                  //기초재고
                decimal purAmount = 0;                                                  //매입액
                int saledAmount = 0;                                                    //판매액
                int beginStockAmount = 0;                                               //기초재고액
                int endStockAmount = Convert.ToInt32(pdtBprice) * pdtStock;            //기말재고액

                foreach (DataRow purRow in purchaseQty.Rows)
                {
                    if (pdtCode.Equals(Convert.ToInt32(purRow["purd_pdt"])))
                    {
                        purQty = Convert.ToInt32(purRow["purd_qty"]);
                        purAmount = Convert.ToDecimal(purRow["pur_amount"]);
                    }
                }
                foreach(DataRow saleRow in salesQty.Rows)
                {
                    if(pdtCode.Equals(Convert.ToInt32(saleRow["saled_pdt"])))
                    {
                        saledQty = Convert.ToInt32(saleRow["saled_qty"]);
                        saledAmount = Convert.ToInt32(saleRow["saled_amount"]);
                    }
                }
                foreach(DataRow beginRow in beginStock.Rows)
                {
                    if (pdtCode.Equals(Convert.ToInt32(beginRow[0])))
                    {
                        pdtBeginStock = Convert.ToInt32(beginRow[1]);
                        beginStockAmount = Convert.ToInt32(pdtBprice) * pdtBeginStock;
                    }
                }
                query = "INSERT INTO dailyreport(dayr_date, dayr_pdt, dayr_sprice, dayr_bprice, dayr_beginstock, dayr_bsockamount, dayr_endstock, dayr_estockamount, dayr_purqty, dayr_puramount, dayr_saleqty, dayr_saleamount,dayr_top,dayr_mid,dayr_bot, dayr_sup)" +
                    "VALUES(@date,@pdt,@spirice,@brpcie,@beginStoock,@beginStoockAmount,@endStock,@endStockAmount,@purQty, @purAmount, @saleQty, @saleAmount, @pdtTop, @pdtMid, @pdtBot, @pdtSup)";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@date",SqlDbType.Int){Value = dailyDate},
                    new SqlParameter("@pdt",SqlDbType.Int){Value = pdtCode},
                    new SqlParameter("@spirice",SqlDbType.Int){Value = pdtSprice},
                    new SqlParameter("@brpcie",SqlDbType.Float){Value = pdtBprice},
                    new SqlParameter("@beginStoock",SqlDbType.Int){Value = pdtBeginStock},
                    new SqlParameter("@beginStoockAmount",SqlDbType.Int){Value = beginStockAmount},
                    new SqlParameter("@endStock",SqlDbType.Int){Value = pdtStock},
                    new SqlParameter("@endStockAmount",SqlDbType.Int){Value = endStockAmount},
                    new SqlParameter("@purQty",SqlDbType.Int){Value = purQty},
                    new SqlParameter("@purAmount",SqlDbType.Float){Value = purAmount},
                    new SqlParameter("@saleQty",SqlDbType.Int){Value = saledQty},
                    new SqlParameter("@pdtTop",SqlDbType.Int){Value = pdtTop},
                    new SqlParameter("@pdtMid",SqlDbType.Int){Value = pdtMid},
                    new SqlParameter("@pdtBot",SqlDbType.Int){Value = pdtBot},
                    new SqlParameter("@pdtSup",SqlDbType.Int){Value = pdtSup},
                    new SqlParameter("@saleAmount",SqlDbType.Int){Value = saledAmount}
                };
                dbconn.ExecuteNonQuery(query, connection, transaction, parameters);
                count++;
            }
            cLog.InsertEmpAccessLogConnect("@dailyReportWrite", accessedEmp, Convert.ToInt32(dailyDate), connection, transaction);
            query = $"UPDATE worktable SET work_exeend = GETDATE() WHERE work_type = 101 AND work_date = @date";
            SqlParameter parameter = new SqlParameter("@date", SqlDbType.Int) { Value = dailyDate };
            dbconn.ExecuteNonQuery(query, connection, transaction, parameter);
        }
        private DataTable EndStock()
        {
            string SearchDate = _dailyDate.ToString("yyyy-MM-dd");
            DataTable pdtStockData = new DataTable();

            string query = $"SELECT pdt_code, pdt_tax, pdt_bprice, pdt_sprice_krw, pdt_stock" +
                $" - ISNULL((SELECT sum(purd_qty) FROM purdetail, purchase WHERE pur_code = purd_code AND purd_pdt = pdt_code AND pur_date > {SearchDate}),0) " +
                $" - ISNULL((SELECT SUM(saled_qty) FROM saledetail, sales WHERE sale_code = saled_code AND saled_pdt = pdt_code AND sale_date > {SearchDate}),0) pdt_stock,pdt_top,pdt_mid, pdt_bot, pdt_sup " +
                "FROM product ORDER BY pdt_code";
            dbconn.SqlDataAdapterQuery(query, pdtStockData);
            return pdtStockData;
        }

        private DataTable BeginStock()
        {
            object resultObj = new object();
            string yesterDay = _dailyDate.AddDays(-1).ToString("yyyyMMdd");
            string query = $"SELECT count(*) FROM worktable WHERE work_type = 101 AND work_date = {yesterDay}";
            DataTable beginStcok = new DataTable();
            dbconn.sqlScalaQuery(query, out resultObj);
            int count = Convert.ToInt32(resultObj);
            if(count ==0 )
            {
                query = "SELECT pdt_code, ISNULL(pdt_stock, 0) -ISNULL((SELECT sum(purd_qty) FROM purdetail, purchase WHERE pur_code = purd_code AND purd_pdt = pdt_code AND pur_date < GETDATE()),0) -ISNULL((SELECT SUM(saled_qty) FROM saledetail, sales WHERE sale_code = saled_code AND saled_pdt = pdt_code AND sale_date < GETDATE()),0) beginstock FROM product ORDER BY pdt_code";
                dbconn.SqlDataAdapterQuery(query, beginStcok);
            }
            else
            {
                query = $"SELECT dayr_pdt, dayr_endstock FROM  dailyreport WHERE dayr_date = {yesterDay}";
            }
            dbconn.SqlDataAdapterQuery(query, beginStcok);
            return beginStcok;
        }
        private DataTable PuchaseQuer(string startDate, string endDAte)
        {
            DataTable purDate = new DataTable();

            string query = $"SELECT purd_pdt,SUM(purd_qty) purd_qty, SUM(pur_amount) pur_amount FROM purchase,purdetail WHERE pur_code = purd_code AND pur_date  > {startDate} AND pur_date < {endDAte} GROUP BY purd_pdt";
            dbconn.SqlDataAdapterQuery(query, purDate);
            return purDate;
        }
        private DataTable SaleQuery(string startDate, string endDAte)
        {
            DataTable saleData = new DataTable();
            string query = $"SELECT saled_pdt,SUM(saled_qty) saled_qty, SUM(saled_amount_krw) saled_amount FROM sales,saledetail WHERE sale_code = saled_code AND sale_date >  {startDate} AND sale_date < {endDAte} GROUP BY saled_pdt";
            dbconn.SqlDataAdapterQuery(query, saleData);
            return saleData;
        }

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
