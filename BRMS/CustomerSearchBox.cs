using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRMS
{
    public partial class CustomerSearchBox : Form
    {

        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet DgrSearchBox = new cDataGridDefaultSet();
        public event Action<int> GetCustomerCode;
        public CustomerSearchBox()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            panelDataGrid.Controls.Add(DgrSearchBox.Dgv);
            GridForm();
            DgrSearchBox.Dgv.Dock = DockStyle.Fill;
            tBoxSearch.KeyUp += tBoxSearch_KeyUp;
            DgrSearchBox.Dgv.KeyDown += DgrSearchBox_KeyDown;
            DgrSearchBox.CellDoubleClick += DgrSearchBox_CellDoubleClick;
        }

        private void GridForm()
        {
            DgrSearchBox.Dgv.Columns.Add("custCode", "회원코드");
            DgrSearchBox.Dgv.Columns.Add("custName", "회원명");
            DgrSearchBox.Dgv.Columns.Add("custCountry", "국가");
            DgrSearchBox.Dgv.Columns.Add("custAddress", "주소");
            DgrSearchBox.Dgv.Columns.Add("custCell", "휴대폰");
            DgrSearchBox.Dgv.Columns.Add("custTell", "일반전화");
            DgrSearchBox.Dgv.Columns.Add("custEmail", "E메일");
            DgrSearchBox.Dgv.Columns["custCode"].Visible = false;

            DgrSearchBox.FormatAsStringLeft("custName", "custEmail", "custCountry", "custAddress");
            DgrSearchBox.FormatAsStringCenter("custCell", "custTell");
            DgrSearchBox.Dgv.ReadOnly = true;
            DgrSearchBox.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgrSearchBox.ApplyDefaultColumnSettings();
        }
        private void FillGrid(DataTable dataTable)
        {
            DgrSearchBox.Dgv.Rows.Clear();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int i = dataTable.Rows.IndexOf(dataRow);
                DgrSearchBox.Dgv.Rows.Add();
                DgrSearchBox.Dgv.Rows[i].Cells["no"].Value = i + 1;
                DgrSearchBox.Dgv.Rows[i].Cells["custCode"].Value = dataRow["cust_code"].ToString();
                DgrSearchBox.Dgv.Rows[i].Cells["custName"].Value = dataRow["cust_name"].ToString();
                DgrSearchBox.Dgv.Rows[i].Cells["custCountry"].Value = dataRow["ctry_name"].ToString();
                DgrSearchBox.Dgv.Rows[i].Cells["custCell"].Value = dataRow["cust_cell"].ToString();
                DgrSearchBox.Dgv.Rows[i].Cells["custTell"].Value = dataRow["cust_tell"].ToString();
                DgrSearchBox.Dgv.Rows[i].Cells["custEmail"].Value = dataRow["cust_email"].ToString();
                
            }
        }
        private void SearchQuery()
        {
            string query = string.Format("SELECT cust_code, cust_name, (SELECT ctry_name FROM country WHERE ctry_code = cust_country) ctry_name, cust_cell, cust_tell, cust_email FROM customer WHERE cust_name like '%{0}%' \n union\n " +
                "SELECT cust_code, cust_name, ctry_name, cust_cell, cust_tell, cust_email FROM customer, country WHERE ctry_name LIKE '%{0}%' AND cust_country = ctry_code \n UNION \n" +
                "SELECT cust_code, cust_name, (SELECT ctry_name FROM country WHERE ctry_code = cust_country) ctry_name, cust_cell, cust_tell, cust_email FROM customer WHERE cust_email LIKE'%{0}%'", tBoxSearch.Text);
            DataTable dataTable = new DataTable(); 
            dbconn = new cDatabaseConnect();        ;
            dbconn.SqlDataAdapterQuery(query, dataTable);
            FillGrid(dataTable);
            DgrSearchBox.Dgv.Focus();
        }

        private void SelectedCustomer()
        {
            try
            {
                if (DgrSearchBox.Dgv.CurrentRow != null)
                {
                    int currentIndex = DgrSearchBox.Dgv.CurrentRow.Index;
                    int custCode = DgrSearchBox.ConvertToInt(DgrSearchBox.Dgv.Rows[currentIndex].Cells["custCode"].Value) ;
                    string custName = DgrSearchBox.Dgv.Rows[currentIndex].Cells["custName"].Value.ToString();
                    GetCustomerCode?.DynamicInvoke(custCode);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DgrSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectedCustomer();
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                // 방향키로 셀 선택 시, 현재 선택된 셀을 커서로 이동시킵니다.
                DgrSearchBox.Dgv.BeginEdit(true); // 편집을 시작하여 커서를 이동시킵니다.
            }
        }
        private void tBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchQuery();
            }
        }
        private void DgrSearchBox_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedCustomer();
        }

        private void bntOk_Click(object sender, EventArgs e)
        {
            SelectedCustomer();
        }

        private void bntSearch_Click(object sender, EventArgs e)
        {
            SearchQuery();
        }

        private void bntClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
