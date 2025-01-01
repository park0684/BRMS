using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BRMS
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]

        static void Main()
        {
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // 데이터베이스 연결 테스트
            try
            {
                cDatabaseConnect dbConnect = new cDatabaseConnect();
                dbConnect.TestDatabaseConnection(); // 연결 테스트
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "데이터베이스 연결에 실패했습니다. 프로그램을 종료합니다.\n\n" + ex.Message,
                    "연결 실패",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return; // 프로그램 종료
            }
            //로그인 실행
            LoginForm loginForm = new LoginForm();
            loginForm.StartPosition = FormStartPosition.CenterParent;
            loginForm.ShowDialog();
            if (loginForm.login== true)
            {

                MainForm mainForm = new MainForm();
                Application.Run(mainForm);
            }
            else
            {
                Application.Exit();
            }

        }
    }
}
