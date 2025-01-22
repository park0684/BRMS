using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRMS
{
    public partial class ConfigBasic : Form
    {
        int accessedEmp = cUserSession.AccessedEmp;
        Dictionary<string, object> originalValues = new Dictionary<string, object>(); // 원본 딕셔너리
        Dictionary<string, object> modifyValues = new Dictionary<string, object>(); // 수정 딕셔너리
        cDatabaseConnect dbconn = new cDatabaseConnect();
        public ConfigBasic()
        {
            InitializeComponent();
            InitializeTextBox();
            LoadBasicInfo();
        }
        /// <summary>
        /// 텍스트박스 읽기전용 설정
        /// 환율 설정 텍스트 박스만 제외
        /// </summary>
        private void InitializeTextBox()
        {
            tBoxStoreCode.ReadOnly = true;
            tBoxBusinessName.ReadOnly = true;
            tBoxBusinessNumber.ReadOnly = true;
            tBoxCeoName.ReadOnly = true;
            tBoxBusinessType.ReadOnly = true;
            tBoxIndustry.ReadOnly = true;
            tBoxTel.ReadOnly = true;
            tBoxAddr.ReadOnly = true;
            tBoxSearchExchange.ReadOnly = true;
        }
        /// <summary>
        /// 기본설정 정보 조회
        /// </summary>
        private void LoadBasicInfo()
        {
            DataTable resultData = new DataTable();
            string query = "SELECT cf_strvalue FROM config WHERE cf_code >= 7 AND cf_code <=14";
            dbconn.SqlDataAdapterQuery(query, resultData);
            string code = resultData.Rows[0]["cf_strvalue"].ToString().Trim();
            string businessName = resultData.Rows[1]["cf_strvalue"].ToString().Trim();
            string businessNumber = resultData.Rows[2]["cf_strvalue"].ToString().Trim();
            string ceoName = resultData.Rows[3]["cf_strvalue"].ToString().Trim();
            string businessType = resultData.Rows[4]["cf_strvalue"].ToString().Trim();
            string industry = resultData.Rows[5]["cf_strvalue"].ToString().Trim();
            string telephon = resultData.Rows[6]["cf_strvalue"].ToString().Trim();
            string addr = resultData.Rows[7]["cf_strvalue"].ToString().Trim();

            tBoxStoreCode.Text = code;
            tBoxBusinessName.Text = businessName;
            tBoxBusinessNumber.Text = businessNumber;
            tBoxCeoName.Text = ceoName;
            tBoxBusinessType.Text = businessType;
            tBoxIndustry.Text = industry;
            tBoxTel.Text = telephon;
            tBoxAddr.Text = addr;
            query = "SELECT cf_value FROM config WHERE cf_code =  1";
            object resultObj = new object();
            dbconn.sqlScalaQuery(query, out resultObj);
            int exchange = cDataHandler.ConvertToInt(resultObj);
            tBoxExchange.Text = exchange.ToString("#,##0");

            tBoxStoreCode.ReadOnly = true;
            tBoxBusinessName.ReadOnly = true;
            tBoxBusinessNumber.ReadOnly = true;
            tBoxCeoName.ReadOnly = true;
            tBoxBusinessType.ReadOnly = true;
            tBoxIndustry.ReadOnly = true;
            tBoxTel.ReadOnly = true;
            tBoxAddr.ReadOnly = true;

            RegisterOriginalData();
        }
        /// <summary>
        /// 조회된 원본 데이터 originalValues 딕셔너리에 등록
        /// 수정시 원본과 수정본을 비교하여 로그 생성시 before 데이터로 사용
        /// </summary>
        private void RegisterOriginalData()
        {
            originalValues["@exchange"] = cDataHandler.ConvertToInt(tBoxExchange.Text);
        }
        /// <summary>
        /// 수정값 딕셔너리 저장
        /// 오리지널 딕셔너리와 비교하여 변경된 값만 수정하는 목적
        /// </summary>
        private void InsertModifyValueDictionary()
        {
            modifyValues["@exchange"] = cDataHandler.ConvertToInt(tBoxExchange.Text);
        }
        /// <summary>
        /// 환율정보 가지고 오기
        /// 웹스크래핑을 통해 네이버에서 환율 정보 취득
        /// </summary>
        private void GetExchangeRateAsync()
        {
            string url = "https://finance.naver.com/marketindex/";

            try
            {
                using (WebClient client = new WebClient())
                {
                    string html = client.DownloadString(url);

                    Match match = Regex.Match(html, @"<span class=""value"">([\d,\.]+)</span>");

                    if (match.Success)
                    {
                        decimal exchangeRate = cDataHandler.ConvertToDecimal(match.Groups[1].Value);
                        tBoxSearchExchange.Text = Math.Round(exchangeRate).ToString("#,##0");
                    }
                    else
                    {
                        cUIManager.ShowMessageBox("환율 정보를 찾을 수 없습니다.","알림",MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}");
            }
        }
        private void ModifyQuerySet(SqlConnection connection, SqlTransaction transaction)
        {

            string query;
            SqlParameter parameter = new SqlParameter("@value", SqlDbType.Int);
            //로그 기록용 데이터테이블
            DataTable configLogData = new DataTable();
            List<string> columns = new List<string> { "key", "param", "before", "after", "accessedEmp", "Date" };
            foreach (string columnName in columns)
            {
                configLogData.Columns.Add(columnName);
            }

            //환율 정보 수정
            int cfCode = 1;
            var exchangeBefore = originalValues["@exchange"].ToString();
            var exchangeAfter = modifyValues["@exchange"].ToString();
            if (exchangeBefore  != exchangeAfter)
            {
                query = $"UPDATE config SET cf_value = @value WHERE cf_code = {cfCode}";
                parameter.Value = modifyValues["@exchange"].ToString();
                dbconn.ExecuteNonQuery(query, connection, transaction, parameter);
                cLog.InsertEmpAccessLogConnect("@exchange", accessedEmp, 0, connection, transaction);
                //cLog.InsertConfigLog("@exchange", exchangeBefore, exchangeAfter, accessedEmp, connection, transaction);
                configLogData.Rows.Add("@exchange", cfCode, exchangeBefore, exchangeAfter, accessedEmp, DateTime.Now.ToString());
            }

            //로그 기록 실행
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
        private void btnSearchExchange_Click(object sender, EventArgs e)
        {
            GetExchangeRateAsync();
        }
    }
}
