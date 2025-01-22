using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;


namespace BRMS
{
    class cDatabaseConnect
    {
        //데이터 베이스 연결 설정 ini 파일 불러오기
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        private string str_sqlconnection;
        private cCryptor cryptor = new cCryptor("YourPassphrase");
        private SqlConnection connection;
        private SqlTransaction transaction;

        StringBuilder db_addr = new StringBuilder();
        StringBuilder db_port = new StringBuilder();
        StringBuilder db_id = new StringBuilder();
        StringBuilder db_pw = new StringBuilder(255);
        StringBuilder db_name = new StringBuilder();

        /// <summary>
        ///INI 파일 존재 여부 확인 메서드 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool IsIniFileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 설정 폼 열기 메서드
        /// </summary>
        /// <param name="iniFilePath"></param>
        private void OpenDatabaseConfigForm(string iniFilePath)
        {
            MessageBox.Show("데이터베이스 연결 설정 파일이 없습니다. 설정 화면을 열겠습니다.", "설정 파일 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            // 설정 폼 열기
            cDataBaseConfig configForm = new cDataBaseConfig();
            configForm.StartPosition = FormStartPosition.CenterScreen;
            configForm.ShowDialog();
        }
        // <summary>
        /// 데이터베이스 연결 테스트를 수행하고 실패 시 설정 창을 띄웁니다.
        /// </summary>
        public void TestDatabaseConnection()
        {
            string iniFilePath = Directory.GetCurrentDirectory() + "\\dbconn.ini";
            // INI 파일 존재 여부 확인
            if (!IsIniFileExists(iniFilePath))
            {
                OpenDatabaseConfigForm(iniFilePath);

                // 설정 폼이 닫힌 후 INI 파일 재확인
                if (!IsIniFileExists(iniFilePath))
                {
                    throw new FileNotFoundException("데이터베이스 연결 설정 파일이 없습니다. 프로그램을 종료합니다.");
                }
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(str_sqlconnection))
                {
                    connection.Open(); // 연결 시도
                    connection.Close(); // 테스트 완료 후 연결 종료
                }
            }
            catch (Exception ex)
            {
                // 연결 실패 시 설정 창 열기
                DialogResult result = MessageBox.Show(
                    "데이터베이스 연결에 실패했습니다. 설정 화면을 열어 수정하시겠습니까?",
                    "연결 실패",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    
                    OpenDatabaseConfigForm(iniFilePath);

                    // 설정이 수정된 후 INI 파일에서 재설정 읽기
                    LoadConnectionSettings(iniFilePath);
                    // 설정 재로드 후 연결 테스트 재시도
                    TestDatabaseConnection();
                }
                else
                {
                    throw new InvalidOperationException("데이터베이스 연결 실패로 인해 프로그램이 종료됩니다.", ex);
                }
            }
        }
        public cDatabaseConnect()
        {
            string iniFilePath = Directory.GetCurrentDirectory() + "\\dbconn.ini";

            
            LoadConnectionSettings(iniFilePath);

        }
        private void LoadConnectionSettings(string iniFilePath)
        {
            GetPrivateProfileString("dbconn", "Address", "", db_addr, db_addr.Capacity, Directory.GetCurrentDirectory() + "\\dbconn.ini");
            GetPrivateProfileString("dbconn", "port", "", db_port, db_port.Capacity, Directory.GetCurrentDirectory() + "\\dbconn.ini");
            GetPrivateProfileString("dbconn", "id", "", db_id, db_id.Capacity, Directory.GetCurrentDirectory() + "\\dbconn.ini");
            GetPrivateProfileString("dbconn", "pw", "", db_pw, db_pw.Capacity, Directory.GetCurrentDirectory() + "\\dbconn.ini");
            GetPrivateProfileString("dbconn", "database", "", db_name, db_name.Capacity, Directory.GetCurrentDirectory() + "\\dbconn.ini");
            string password = db_pw.ToString();
            password = cryptor.Decrypt(password);  // 암호화된 비밀번호 복호화
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = db_addr.ToString() + ',' + db_port.ToString();
            builder.UserID = db_id.ToString();
            builder.Password = password;
            builder.InitialCatalog = db_name.ToString();
            str_sqlconnection = builder.ConnectionString;
        }


        public SqlConnection Opensql()
        {
            SqlConnection connectdb = new SqlConnection(str_sqlconnection);
            connectdb.Open();

            return connectdb;

        }

        public DataRow ReaderQuery(string query)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(query, Opensql()))
                {

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Clear();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dataTable.Columns.Add(reader.GetName(i));
                    }
                    if (reader.Read())
                    {
                        DataRow row = dataTable.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i].ToString();
                        }
                        dataTable.Rows.Add(row);
                        reader.Close();
                        return row;
                    }
                    else
                    {
                        reader.Close();
                        return null; // 데이터가 없으면 null을 반환합니다.
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("연결 오류 :" + ex.Message);
                return null;
            }
        }

        //단일 row 결과물 조회 쿼리
        public void SqlReaderQuery(string query, DataTable dataTable)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(query, Opensql()) )
                {
                    
                    SqlDataReader reader = command.ExecuteReader();
                    dataTable.Clear();
                    dataTable.Columns.Clear();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dataTable.Columns.Add(reader.GetName(i));
                    }
                    if (reader.Read())
                    {
                        DataRow row = dataTable.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader[i].ToString();
                        }
                        dataTable.Rows.Add(row);
                    }
                    reader.Close();
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("연결 오류 :" + ex.Message);
            }

        }
        public object ScalaResult(string query, SqlConnection connection, SqlTransaction transaction)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
command.Transaction = transaction;
                return command.ExecuteScalar();
            }
        }

        //단일 값 조회
        public void sqlScalaQuery(string query, out object objResult)
        {
            objResult = null;
            try
            {
                using (SqlConnection connection = Opensql())
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    objResult = command.ExecuteScalar();

                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("조회 오류 :" + ex.Message);
            }
        }

        public void ExecuteNonQuery(string query,SqlConnection connection, SqlTransaction transaction = null, params SqlParameter[] parameters)
        {
            
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (transaction != null)
                {
                    command.Transaction = transaction;
                }

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                command.ExecuteNonQuery();
            }
        }
        public void NonQuery(string query, SqlConnection connection, SqlTransaction transaction = null)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Assign the transaction to the command if provided
                if (transaction != null)
                {
                    command.Transaction = transaction;
                }

                command.ExecuteNonQuery();
            }

        }

        //다량의 row 일괄 조회
        public void SqlDataAdapterQuery(string query, DataTable dataTable)
        {
            try
            {
                using (SqlConnection connection = Opensql())
                {
                    //DataSet dataSet = new DataSet();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(query, connection);
                    dataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 조회 오류 : " + ex.Message);
            }
        }

        public void BeginTransaction()
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                Opensql();
            }
            transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }
        }
    }
}
