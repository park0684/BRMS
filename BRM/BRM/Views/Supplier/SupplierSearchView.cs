using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GridControl;
using common.Interface;

namespace BRM.Views
{
    public partial class SupplierSearchView : Form,ISupplierSearchView,IView
    {
        BrsDataGridView dgvSupplier;

        public int SupplierCode 
        { get => Convert.ToInt32(dgvSupplier.CurrentRow.Cells["supCode"].Value); }

        public SupplierSearchView()
        {
            InitializeComponent();
            InitailizeDataGridView();
            ViewEvent();
            ControlBox = false;

        }

        public event EventHandler CloseFormEvent;
        public event EventHandler<int> SelectedSupplierEvent;
        public Control AsControl()
        {
            return this;
        }

        public void CloseForm()
        {
            this.Close();
        }


        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            ShowDialog();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void BindingDataTable(DataTable result)
        {
            dgvSupplier.DataSource = result;
        }
        private void ViewEvent()
        {
            //btnOk.Click += SupplierSelectEvent;
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);
            dgvSupplier.CellDoubleClick +=  SupplierSelectEvent;
            btnOk.Click += (s, e) => SelectedSupplierEvent?.Invoke(this, (int)(dgvSupplier.CurrentRow.Cells["supCode"].Value));
        }
        private void InitailizeDataGridView()
        {
            dgvSupplier = new BrsDataGridView();
            pnllDataGrid.Controls.Add(dgvSupplier);
            dgvSupplier.Dock = DockStyle.Fill;

            dgvSupplier.Columns["No"].Visible = false;
            dgvSupplier.Columns.Add("supCode", "코드");
            dgvSupplier.Columns.Add("supName", "공급사명");
            dgvSupplier.Columns["supCode"].Width = 90;
            dgvSupplier.Columns["supName"].Width = 180;
            dgvSupplier.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSupplier.ReadOnly = true;
            foreach(DataGridViewColumn column in dgvSupplier.Columns )
            {
                column.DataPropertyName = column.Name;
            }
        }
        private void SupplierSelectEvent(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                int code = Convert.ToInt32(dgvSupplier.CurrentRow.Cells["supCode"].Value);
                SelectedSupplierEvent?.Invoke(this, code);
            }
        }

    }
}
