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
using GridControl;

namespace BRM.Views
{
    public partial class EmployeeDetailView : Form, IEmployeeDetailView, IView
    {
        BrsDataGridView dgvLog;
        public EmployeeDetailView()
        {
            InitializeComponent();
            DataGridViewInitialize();
            ViewEvent();
        }

        public int EmployeeCode { set => lblEmpCode.Text = value.ToString() ; }
        public string EmployeeName
        {
            get => txtEmpName.Text; 
            set => txtEmpName.Text = value; 
        }
        public string EmployeePhon 
        {
            get => txtCell.Text;
            set => txtCell.Text = value;
        }
        public string EmployeeLevel 
        {
            get => txtLevel.Text;
            set => txtLevel.Text = value; 
        }
        public int EmployeeStatus
        {
            get => (int)cmbStatus.SelectedValue;
            set => cmbStatus.SelectedValue = value;
        }
        public string EmployeeEmail 
        { 
            get => txtEmail.Text;
            set => txtEmail.Text = value; 
        }
        public string EmployeeAddress 
        { 
            get => txtAddress.Text;
            set => txtAddress.Text = value;
        }
        public string Employeememo 
        { 
            get => txtMemo.Text;
            set => txtMemo.Text = value;
        }
        public DateTime EmployeeIdate { set => lblIdate.Text = value.ToString("yyyy-MM-dd HH:mm"); }
        public DateTime EmployeeUdate { set => lblEdate.Text = value.ToString("yyyy-MM-dd HH:mm"); }

        public DateTime FromDate => dtpFrom.Value;

        public DateTime ToDate => dtpTo.Value;

        public event EventHandler CloseFormEvent;
        public event EventHandler SetAuthorityEvent;
        public event EventHandler SetPasswordEvent;
        public event EventHandler SaveEvent;
        public event EventHandler LogSearchEvent;

        public Control AsControl()
        {
            return this;
        }

        public void CloseForm()
        {
            this.Close();
        }

        public void DataGridViewBinding(DataTable result)
        {
            dgvLog.DataSource = result;
        }

        public void SetComboBoxItems(Dictionary<int, string> items)
        {
            var source = new Dictionary<int, string>();

            cmbStatus.DataSource = items.ToList();
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "Key";
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        public void SetTabpage(bool isNew, Dictionary<string, string> columns = null)
        {
            tabCtrlEmployee.TabPages[0].Text = "직원정보";
            tabCtrlEmployee.TabPages[1].Text = "변경로그";
            if (isNew)
                tabCtrlEmployee.TabPages.Remove(tabPage2);
            else 
            {
                if (columns == null)
                    return;
                
                foreach(var column in columns)
                {
                    dgvLog.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = column.Key,
                        DataPropertyName = column.Key,
                        HeaderText = column.Value
                    });
                }
                foreach (DataGridViewColumn column in dgvLog.Columns)
                {
                    column.HeaderCell.Style.WrapMode = DataGridViewTriState.False; //칼럼 헤더 줄바꿈 방지
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // 셀 크기 자동 조절
                }
                dgvLog.AllowUserToResizeColumns = true;
            }
        }

        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
            btnPassWord.Click += (s, e) => SetPasswordEvent?.Invoke(this, EventArgs.Empty);
            btnSetAuthority.Click += (s, e) => SetAuthorityEvent?.Invoke(this, EventArgs.Empty);
            btnLogSearch.Click += (s, e) => LogSearchEvent?.Invoke(this, EventArgs.Empty);
        }
        private void DataGridViewInitialize()
        {
            dgvLog = new BrsDataGridView();
            pnlDataGrid.Controls.Add(dgvLog);
            dgvLog.Dock = DockStyle.Fill;
            dgvLog.ReadOnly = true;
            dgvLog.AutoGenerateColumns = false;
        }
    }
}
