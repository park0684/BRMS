using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBConfig.View
{
    public partial class ConfigView : Form, IConfigView
    {
        public ConfigView()
        {
            InitializeComponent();
            ControlBox = false;
            this.Text = "데이터 베이스 연결 설정";
            txtPassword.PasswordChar = '*';
            ViewEvent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        public string Address
        {
            get { return txtAddresse.Text; }
            set { txtAddresse.Text = value; }
        }
        public string Port
        {
            get { return txtPort.Text; }
            set { txtPort.Text = value; }
        }
        public string User
        {
            get { return txtUser.Text; }
            set { txtUser.Text = value; }
        }
        public string Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }
        public string Database
        {
            get { return txtDatabase.Text; }
            set { txtDatabase.Text = value; }
        }

        public event EventHandler ConnectionTestEvent;
        public event EventHandler CloseFormEvent;
        public event EventHandler SaveEvent;
        public event EventHandler PortChangedEvent;

        public void CloseForm()
        {
            this.Close();
        }

        public bool ShowConfirmMessage(string message)
        {
            return MessageBox.Show(message, "확인", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        public void ShowFrom()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            ShowDialog();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// 포트 텍스트박스에 숫자외 입력 제한 설정 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPort_keyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
            btnTest.Click += (s, e) => ConnectionTestEvent?.Invoke(this, EventArgs.Empty);
            txtPort.TextChanged += (s, e) => PortChangedEvent?.Invoke(this, EventArgs.Empty);
            //내부 이벤트
            txtPort.KeyPress += txtPort_keyPress;
        }
    }
}
