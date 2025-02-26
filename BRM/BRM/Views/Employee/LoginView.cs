using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using common.Interface;

namespace BRM.Views
{
    public partial class LoginView : Form,ILoginView
    {
        public LoginView()
        {
            InitializeComponent();
            ViewEvent();
            ControlBox = false;
            txtEmpName.Enabled = false;
        }

        public string EmployeeName { set => txtEmpName.Text = value; }

        public int EmployeeCode =>  Convert.ToInt32(txtEmpCode.Text);

        public string Passwordk => txtPassword.Text;

        public event EventHandler LoginEvent;
        public event EventHandler CloseFormEvent;
        public event EventHandler GetEmployeeNameEvent;

        public void CloseForm()
        {
            this.Close();
        }

        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowDialog(); 
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        public void FocusMove()
        {
            txtPassword.Focus();
        }

        public void GetEmpInof()
        {
            throw new NotImplementedException();
        }
        private void ViewEvent()
        {
            btnCancle.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);
            txtEmpCode.KeyDown += txtEmpCode_KeyDown;
            btnOk.Click += (s, e) => LoginEvent?.Invoke(this, EventArgs.Empty);
            txtPassword.KeyDown += txtPassword_keyDown;
        }
        private void txtEmpCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetEmployeeNameEvent?.Invoke(this, e);
        }
        private void txtPassword_keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoginEvent?.Invoke(this, e);
        }


    }
}
