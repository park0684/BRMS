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
    public partial class ConfigPoint : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        Dictionary<string, object> originalValues = new Dictionary<string, object>(); // 원본 딕셔너리
        Dictionary<string, object> modifyRewordValues = new Dictionary<string, object>(); // 수정 딕셔너리
        static Dictionary<string, (int typeCode, string typeString)> logParameter = new Dictionary<string, (int, string)>();//로그 파라미터
        int accessedEmp = cUserSession.AccessedEmp;
        public ConfigPoint()
        {
            InitializeComponent();
            LoadPointSet();
            logParameter = cLog.GetFilteredParameters(900, 1000);
        }

        private void LoadPointSet()
        {
            DataTable resultData = new DataTable();
            string query = "SELECT cf_value FROM config WHERE cf_code >= 3 AND cf_code <= 6";
            dbconn.SqlDataAdapterQuery(query, resultData);
            decimal cashRate = cDataHandler.ConvertToDecimal(resultData.Rows[0]["cf_value"]) / 10;
            decimal cardRate = cDataHandler.ConvertToDecimal(resultData.Rows[1]["cf_value"]) / 10;
            decimal accountRate = cDataHandler.ConvertToDecimal(resultData.Rows[2]["cf_value"]) / 10;
            decimal poinRate = cDataHandler.ConvertToDecimal(resultData.Rows[3]["cf_value"]) / 10;

            tBoxCash.Text = cashRate.ToString("#,##0.#");
            tBoxCard.Text = cardRate.ToString("#,##0.#");
            tBoxAccount.Text = accountRate.ToString("#,##0.#");
            tboxPoint.Text = poinRate.ToString("#,##0.#");

            RegisterOriginalData();
        }
        private void RegisterOriginalData()
        {
            originalValues["@cashRewadRate"] = cDataHandler.ConvertToDecimal(tBoxCash.Text);
            originalValues["@cardRewadRate"] = cDataHandler.ConvertToDecimal(tBoxCard.Text);
            originalValues["@accountRewadRate"] = cDataHandler.ConvertToDecimal(tBoxAccount.Text);
            originalValues["@pointRewadRate"] = cDataHandler.ConvertToDecimal(tboxPoint.Text);
        }
        /// <summary>
        /// 수정값 딕셔너리 저장
        /// 오리지널 딕셔너리와 비교하여 변경된 값만 수정하는 목적
        /// </summary>
        private void InsertModifyValueDictionary()
        {
            modifyRewordValues["@cashRewadRate"] = cDataHandler.ConvertToDecimal(tBoxCash.Text);
            modifyRewordValues["@cardRewadRate"] = cDataHandler.ConvertToDecimal(tBoxCard.Text);
            modifyRewordValues["@accountRewadRate"] = cDataHandler.ConvertToDecimal(tBoxAccount.Text);
            modifyRewordValues["@pointRewadRate"] = cDataHandler.ConvertToDecimal(tboxPoint.Text);
        }
        /// <summary>
        /// 수정 쿼리 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void ModifyQuerySet(SqlConnection connection, SqlTransaction transaction)
        {
            
            string query;
            
            //로그 기록용 데이터테이블
            DataTable configLogData = new DataTable(); 
            List<string> columns = new List<string>{"key","param","before","after","accessedEmp","Date" };
            foreach(string columnName in columns)
            {
                configLogData.Columns.Add(columnName);
            }

            // 포인트 적립율 수정
            int cfCode = 3;
            foreach(var modifyValue in modifyRewordValues)
            {
                decimal before = cDataHandler.ConvertToDecimal(originalValues[modifyValue.Key]);
                decimal after = cDataHandler.ConvertToDecimal(modifyValue.Value);
                int value = cDataHandler.ConvertToInt(after * 10);
                if (originalValues.ContainsKey(modifyValue.Key) && before != after)
                {
                    query = $"UPDATE config SET cf_value = @value WHERE cf_code = {cfCode}";
                    SqlParameter parameter = new SqlParameter("@value", SqlDbType.Int) { Value = value };
                    dbconn.ExecuteNonQuery(query, connection, transaction, parameter);
                    cLog.InsertEmpAccessLogConnect("@RewadRate", accessedEmp, 0,connection, transaction);
                    //cLog.InsertConfigLog(modifyValue.Key, before, after, accessedEmp, connection, transaction);
                    configLogData.Rows.Add("@RewadRate", cfCode, before, after, accessedEmp, DateTime.Now.ToString());
                }
                cfCode++;
            }
            //config 변경 로그 기록
            cLog.InsertConfigLog(configLogData, connection, transaction);
        }
        public void RunQuery()
        {
            using (SqlConnection connection = dbconn.Opensql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    InsertModifyValueDictionary();
                    ModifyQuerySet(connection, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
