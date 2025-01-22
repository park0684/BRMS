using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BRMS
{
    public partial class CategoryEdit : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        Dictionary<string, object> originalValues = new Dictionary<string, object>();
        int accessedEmp = cUserSession.AccessedEmp;
        int categoryCode = 0;
        string catNameKr = "";
        string catNameEn = "";
        bool isNewEntry = false;
        bool errorCheck = false;
        public CategoryEdit()
        {
            InitializeComponent();
            TextBoxDefault();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ControlBox = false;
        }
        /// <summary>
        /// 텍스트 박스 상태 설정
        /// </summary>
        private void TextBoxDefault()
        {
            tBoxTopCode.Enabled = true;
            tBoxTopKr.Enabled = true;
            tBoxTopEn.Enabled = true;
            tBoxMidCode.Visible = false;
            tBoxMidKr.Visible = false;
            tBoxMidEn.Visible = false;
            tBoxBotCode.Visible = false;
            tBoxBotKr.Visible = false;
            tBoxBotEn.Visible = false;
            tBoxMidCode.Enabled = false;
            tBoxMidKr.Enabled = false;
            tBoxMidEn.Enabled = false;
            tBoxBotCode.Enabled = false;
            tBoxBotKr.Enabled = false;
            tBoxBotEn.Enabled = false;
            tBoxTopCode.Text = "0";
            tBoxTopKr.Text = "";
            tBoxTopEn.Text = "";
            tBoxMidCode.Text = "0";
            tBoxMidKr.Text = "";
            tBoxMidEn.Text = "";
            tBoxBotCode.Text = "0";
            tBoxBotKr.Text = "";
            tBoxBotEn.Text = "";
        }
        /// <summary>
        /// 카테고리 정보 받아오기
        /// </summary>
        /// <param name="topCode"></param>
        /// <param name="midCode"></param>
        /// <param name="botcode"></param>
        /// <param name="newEntery"></param>
        public void GetCategoryinfo(int topCode, int midCode, int botcode, bool newEntery)
        {
            TextBoxDefault();
            isNewEntry = newEntery;
            if (isNewEntry == true) // 추가 등록
            {
                chkStatus.Checked = true;
                object resultObj = new object();
                string query;
                if (topCode == 0)
                {
                    query = "SELECT MAX(cat_top) + 1 FROM category";
                    dbconn.sqlScalaQuery(query, out resultObj);
                    tBoxTopCode.Text = resultObj.ToString();
                    tBoxTopCode.Enabled = false;
                }

                if (topCode != 0 && midCode == 0)
                {
                    TopCategorySearch(topCode);
                    query = string.Format("SELECT ISNULL(MAX(cat_mid) + 1,1) FROM category WHERE cat_top = {0} AND cat_mid != 0", topCode);
                    dbconn.sqlScalaQuery(query, out resultObj);
                    tBoxMidCode.Text = resultObj.ToString();
                    tBoxTopCode.Enabled = false;
                    tBoxTopKr.Enabled = false;
                    tBoxTopEn.Enabled = false;
                    tBoxMidCode.Visible = true;
                    tBoxMidKr.Visible = true;
                    tBoxMidEn.Visible = true;
                    tBoxMidKr.Enabled = true;
                    tBoxMidEn.Enabled = true;
                }
                else if (midCode != 0 && botcode == 0)
                {
                    MidCategorySearch(topCode, midCode);
                    query = string.Format("SELECT ISNULL(MAX(cat_bot) + 1,1) FROM category WHERE cat_top = {0} AND cat_mid = {1} AND cat_bot !=0", topCode, midCode);
                    dbconn.sqlScalaQuery(query, out resultObj);
                    tBoxBotCode.Text = resultObj.ToString();
                    tBoxMidCode.Enabled = false;
                    tBoxMidKr.Enabled = false;
                    tBoxMidEn.Enabled = false;
                    tBoxBotCode.Visible = true;
                    tBoxBotKr.Visible = true;
                    tBoxBotEn.Visible = true;
                    tBoxBotKr.Enabled = true;
                    tBoxBotEn.Enabled = true;
                }

            }
            else if (isNewEntry == false) // 수정
            {
                if (midCode == 0 && botcode == 0)
                {
                    TopCategorySearch(topCode);
                }
                if (midCode != 0 && botcode == 0)
                {
                    MidCategorySearch(topCode, midCode);
                }
                if (midCode != 0 && botcode != 0)
                {
                    BotCategorySearch(topCode, midCode, botcode);
                }
            }

        }
        /// <summary>
        /// 대분류 정보 조회
        /// </summary>
        /// <param name="topCode"></param>
        private void TopCategorySearch(int topCode)
        {
            DataTable resultData = new DataTable();
            string query = string.Format("SELECT cat_name_kr,cat_name_en,cat_code,cat_status FROM category WHERE cat_top ={0} AND cat_mid = 0 AND cat_bot =0", topCode);
            dbconn.SqlReaderQuery(query, resultData);
            DataRow dataRow = resultData.Rows[0];
            categoryCode = cDataHandler.ConvertToInt(dataRow["cat_code"]);
            catNameKr = dataRow["cat_name_kr"].ToString();
            catNameEn = dataRow["cat_name_en"].ToString();
            tBoxTopCode.Text = topCode.ToString();
            tBoxTopKr.Text = catNameKr;
            tBoxTopEn.Text = catNameEn;
            tBoxTopCode.Enabled = false;
            int status = cDataHandler.ConvertToInt(dataRow["cat_status"]);
            chkStatus.Checked = status == 1 ? true : false;
            RegisterOriginalData();
        }
        /// <summary>
        /// 중분류 정보 조회
        /// </summary>
        /// <param name="topCode"></param>
        /// <param name="midCode"></param>
        private void MidCategorySearch(int topCode, int midCode)
        {
            TopCategorySearch(topCode);
            DataTable resultData = new DataTable();
            string query = string.Format("SELECT cat_name_kr,cat_name_en,cat_code, cat_status FROM category WHERE cat_top ={0} AND cat_mid = {1} AND cat_bot =0", topCode, midCode);
            dbconn.SqlReaderQuery(query, resultData);
            DataRow dataRow = resultData.Rows[0];
            categoryCode = cDataHandler.ConvertToInt(dataRow["cat_code"]);
            catNameKr = dataRow["cat_name_kr"].ToString();
            catNameEn = dataRow["cat_name_en"].ToString();
            tBoxMidCode.Text = midCode.ToString();
            tBoxMidKr.Text = catNameKr;
            tBoxMidEn.Text = catNameEn;
            tBoxTopCode.Enabled = false;
            tBoxTopKr.Enabled = false;
            tBoxTopEn.Enabled = false;
            tBoxMidCode.Visible = true;
            tBoxMidKr.Visible = true;
            tBoxMidEn.Visible = true;
            tBoxMidCode.Enabled = true;
            tBoxMidKr.Enabled = true;
            tBoxMidEn.Enabled = true;
            int status = cDataHandler.ConvertToInt(dataRow["cat_status"]);
            chkStatus.Checked = status == 1 ? true : false;
            RegisterOriginalData();
        }
        /// <summary>
        /// 소분류 정보 조회
        /// </summary>
        /// <param name="topCode"></param>
        /// <param name="midCode"></param>
        /// <param name="botCode"></param>
        private void BotCategorySearch(int topCode, int midCode, int botCode)
        {
            TopCategorySearch(topCode);
            MidCategorySearch(topCode, midCode);
            DataTable resultData = new DataTable();
            string query = string.Format("SELECT cat_name_kr,cat_name_en, cat_code, cat_status FROM category WHERE cat_top ={0} AND cat_mid = {1} AND cat_bot ={2}", topCode, midCode, botCode);
            dbconn.SqlReaderQuery(query, resultData);
            DataRow dataRow = resultData.Rows[0];
            categoryCode = cDataHandler.ConvertToInt(dataRow["cat_code"]);
            catNameKr = dataRow["cat_name_kr"].ToString();
            catNameEn = dataRow["cat_name_en"].ToString();
            tBoxBotCode.Text = botCode.ToString();
            tBoxBotKr.Text = catNameKr;
            tBoxBotEn.Text = catNameEn;
            tBoxBotCode.Visible = true;
            tBoxBotKr.Visible = true;
            tBoxBotEn.Visible = true;
            tBoxMidCode.Enabled = false;
            tBoxMidKr.Enabled = false;
            tBoxMidEn.Enabled = false;
            tBoxMidCode.Visible = true;
            tBoxMidKr.Visible = true;
            tBoxMidEn.Visible = true;
            tBoxBotKr.Enabled = true;
            tBoxBotEn.Enabled = true;
            int status = cDataHandler.ConvertToInt(dataRow["cat_status"]);
            chkStatus.Checked = status == 1 ? true : false;
            RegisterOriginalData();
        }
        /// <summary>
        /// 조회된 원본 데이터 originalValues 딕셔너리에 등록
        /// 수정시 원본과 수정본을 비교하여 로그 생성시 before 데이터로 사용
        /// </summary>
        private void RegisterOriginalData()
        {
            originalValues.Clear();
            originalValues["@catNameKr"] = catNameKr;
            originalValues["@catNameEn"] = catNameEn;
        }
        /// <summary>
        /// 분류 등록 및 수정 전 이상여부 확인
        /// </summary>
        private void ErrorCheck()
        {
            string query = "";
            object resultObj = new object();
            //신규 등록시 분류 번호 값이 동일 할 경우
            query = $"SELECT count(cat_code) FROM category WHERE cat_top = {tBoxTopCode.Text} AND cat_mid = {tBoxMidCode.Text} AND cat_bot = {tBoxBotCode.Text}";
            dbconn.sqlScalaQuery(query, out resultObj);
            if(Convert.ToInt32(resultObj) > 0 && isNewEntry == true)
            {
                cUIManager.ShowMessageBox("이미 등록된 분류번호 입니다", "분류번호 중복 지정", MessageBoxButtons.OK);
                errorCheck = true;
                return;
            }
            //분류 번호 수정 시 이미 동일한 분류 번호가 있는지 확인
            //새로 등록 할 분류번호로 분류코드 조회 후 동일 여부 확인
            query = $"SELECT cat_code FROM category WHERE cat_top = {tBoxTopCode.Text} AND cat_mid = {tBoxMidCode.Text} AND cat_bot = {tBoxBotCode.Text}";
            dbconn.sqlScalaQuery(query, out resultObj);
            if (Convert.ToInt32(resultObj) != categoryCode && isNewEntry == false)
            {
                cUIManager.ShowMessageBox("이미 등록된 분류번호 입니다", "분류번호 중복 지정", MessageBoxButtons.OK);
                errorCheck = true;
                return;
            }
            
        }
        /// <summary>
        /// 분류 등록 실행 코드
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void InsertCategory(SqlConnection connection, SqlTransaction transaction)
        {
            string top = "0";
            string mid = "0";
            string bot = "0";
            string name_kr = "";
            string name_en = "";
            string query;
            object resultObj;
            if (!string.IsNullOrEmpty(tBoxTopCode.Text))
            {
                top = tBoxTopCode.Text;
            }
            if (!string.IsNullOrEmpty(tBoxMidCode.Text))
            {
                mid = tBoxMidCode.Text;
            }
            if (!string.IsNullOrEmpty(tBoxBotCode.Text))
            {
                bot = tBoxBotCode.Text;
            }
            query = string.Format("SELECT MAX(cat_code) + 1 FROM category ");
            dbconn.sqlScalaQuery(query, out resultObj);
            int newCategoryCode = Convert.ToInt32(resultObj);
            if (tBoxTopKr.Enabled == true)
            {
                name_kr = tBoxTopKr.Text;
                name_en = tBoxTopEn.Text;
            }
            else if (tBoxMidKr.Enabled == true)
            {
                name_kr = tBoxMidKr.Text;
                name_en = tBoxMidEn.Text;
            }
            else if (tBoxBotKr.Enabled == true)
            {
                name_kr = tBoxBotKr.Text;
                name_en = tBoxBotEn.Text;
            }
            query = "INSERT INTO category (cat_code, cat_top, cat_mid, cat_bot,cat_name_kr, cat_name_en, cat_idate, cat_udate, cat_status)" +
                "VALUES(@catCode,@catTop,@catMid,@catBot,@catNameKr,@catNameEn, GETDATE(), GETDATE(), 1)";
            SqlParameter[] parameter =
            {
                new SqlParameter("@catCode",SqlDbType.Int){Value = newCategoryCode},
                new SqlParameter("@catTop",SqlDbType.Int){Value = top},
                new SqlParameter("@catMid",SqlDbType.Int){Value = mid},
                new SqlParameter("@catBot",SqlDbType.Int){Value = bot},
                new SqlParameter("@catNameKr",SqlDbType.NVarChar){Value = name_kr},
                new SqlParameter("@catNameEn",SqlDbType.NVarChar){Value = name_en},
            };
            dbconn.ExecuteNonQuery(query, connection, transaction,parameter);

        }
        /// <summary>
        /// 분류 변경 실행 코드
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void UpdateCategory(SqlConnection connection, SqlTransaction transaction)
        {

            string top = "0";
            string mid = "0";
            string bot = "0";
            string name_kr = "";
            string name_en = "";
            string query;
            int status = chkStatus.Checked == true ? 1:0;
            if (tBoxTopKr.Enabled == true)
            {
                top = tBoxTopCode.Text;
                name_kr = tBoxTopKr.Text;
                name_en = tBoxTopEn.Text;

            }
            else if (tBoxMidKr.Enabled == true)
            {
                top = tBoxTopCode.Text;
                mid = tBoxMidCode.Text;
                name_kr = tBoxMidKr.Text;
                name_en = tBoxMidEn.Text;
            }
            else if (tBoxBotKr.Enabled == true)
            {
                top = tBoxTopCode.Text;
                mid = tBoxMidCode.Text;
                bot = tBoxBotCode.Text;
                name_kr = tBoxBotKr.Text;
                name_en = tBoxBotEn.Text;
            }
            
            query = "UPDATE category SET cat_top = @catTop, cat_mid = @catMid, cat_bot = @catBot, cat_name_kr =@catNameKr, cat_name_en = @catNameEn, cat_udate = GETDATE(), cat_status = @catStatus WHERE cat_code = @catCode";
            SqlParameter[] parameter =
            {
                new SqlParameter("@catCode",SqlDbType.Int){Value = categoryCode},
                new SqlParameter("@catTop",SqlDbType.Int){Value = top},
                new SqlParameter("@catMid",SqlDbType.Int){Value = mid},
                new SqlParameter("@catBot",SqlDbType.Int){Value = bot},
                new SqlParameter("@catNameKr",SqlDbType.NVarChar){Value = name_kr},
                new SqlParameter("@catNameEn",SqlDbType.NVarChar){Value = name_en},
                new SqlParameter("@catStatus",SqlDbType.Int){Value = status}
            };
            dbconn.ExecuteNonQuery(query, connection, transaction, parameter);
            Dictionary<string, string> modifiedValues = new Dictionary<string, string>();
            foreach (var param in parameter)
            {
                modifiedValues[param.ParameterName] = param.Value.ToString();
            }
            cLog.IsModified(originalValues, modifiedValues, categoryCode, accessedEmp, connection, transaction);
        }
        private void RunQeury()
        {

            using (SqlConnection connection = dbconn.Opensql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ErrorCheck();
                    if (errorCheck == true)//이상여부 확인 시 종료
                    {
                        return;
                    }
                    if (isNewEntry == true) // 신규 분류일 경우 등록 후 로그 기록
                    {
                        InsertCategory(connection, transaction);
                        cLog.InsertEmpAccessLogConnect("@categoryRegist", accessedEmp, categoryCode, connection, transaction);
                    }
                    else // 분류 수정일 경우 수정 후 로그 기록
                    {
                        UpdateCategory(connection, transaction);
                        cLog.InsertEmpAccessLogConnect("@categroyModify", accessedEmp, categoryCode, connection, transaction);
                    }
                    transaction.Commit();
                    Close();
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
            Close();

        }
        private void bntSave_Click(object sender, EventArgs e)
        {
            RunQeury();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
