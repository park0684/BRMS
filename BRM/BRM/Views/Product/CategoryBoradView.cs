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
    public partial class CategoryBoradView : Form, ICategoryBoradView,IView
    {
        BrsDataGridView dgvCategoryTop;
        BrsDataGridView dgvCategoryMid;
        BrsDataGridView dgvCategoryBot;

        public bool CategoryStatus 
        { 
            get => chkStatus.Checked; 
        }

        public CategoryBoradView()
        {
            InitializeComponent();
            InitializeDataGridView();
            ViewEvent();
        }

        public event EventHandler SaveEvent;
        public event EventHandler CloseFormEvent;
        public event EventHandler ShowFormEvent;
        public event EventHandler CategoryTopEditEvent;
        public event EventHandler CategoryTopAddEvent;
        public event EventHandler CategoryMidEditEvetn;
        public event EventHandler CategoryMidAddEvent;
        public event EventHandler CategoryBotEditEvent;
        public event EventHandler CategoryBotAddEvent;
        public event EventHandler SelectCategoryTopEvent;
        public event EventHandler SelectCategoryMidEvent;


        public Control AsControl()
        {
            return this;
        }
        public void CategoryBotBindign(DataTable result)
        {
            dgvCategoryBot.DataSource = result;
        }

        public void CategoryMidBindign(DataTable result)
        {
            dgvCategoryMid.DataSource = result;
        }

        public void CategoryTopBindign(DataTable result)
        {
            dgvCategoryTop.DataSource = result;
        }

        public void CloseForm()
        {
            this.Close();
        }

        public int GetCurrentBot()
        {
            return Convert.ToInt32(dgvCategoryBot.CurrentRow.Cells["catCode"].Value);
        }

        public int GetCurrentMid()
        {
            return Convert.ToInt32(dgvCategoryMid.CurrentRow.Cells["catCode"].Value);
        }

        public int GetCurrentTop()
        {
            return Convert.ToInt32(dgvCategoryTop.CurrentRow.Cells["catCode"].Value);
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

        public void SetIsEidtMod(int mode)
        {

            switch (mode)
            {
                case 0:// 분류 등록/수정 모드
                    this.Text = "분류 등록/수정";
                    btnOk.Visible = false;
                    break;
                case 1:// 분류 선택 모드
                    this.Text = "분류 선택";
                    btnTopCategoryEidt.Visible = false;
                    btnMidCategoryEidt.Visible = false;
                    btnBotCategoryEidt.Visible = false;
                    btnTopCategoryAdd.Visible = false;
                    btnMidCategoryAdd.Visible = false;
                    btnBotCategoryAdd.Visible = false;
                    break;
            }
        }



        private void ViewEvent()
        {
            btnTopCategoryEidt.Click += (s, e) => CategoryTopEditEvent?.Invoke(this, EventArgs.Empty);
            btnTopCategoryAdd.Click += (s, e) => CategoryTopAddEvent?.Invoke(this, EventArgs.Empty);
            btnMidCategoryEidt.Click += (s, e) => CategoryMidEditEvetn?.Invoke(this, EventArgs.Empty);
            btnMidCategoryAdd.Click += (s, e) => CategoryMidAddEvent?.Invoke(this, EventArgs.Empty);
            btnBotCategoryEidt.Click += (s, e) => CategoryBotEditEvent?.Invoke(this, EventArgs.Empty);
            btnBotCategoryAdd.Click += (s, e) => CategoryBotAddEvent?.Invoke(this, EventArgs.Empty);
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);
            btnOk.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
            dgvCategoryTop.CellClick += (s, e) => SelectCategoryTopEvent?.Invoke(this, EventArgs.Empty);
            dgvCategoryMid.CellClick += (s, e) => SelectCategoryMidEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 데이터 그리드 초기화
        /// </summary>
        private void InitializeDataGridView()
        {
            dgvCategoryTop = new BrsDataGridView();
            dgvCategoryMid = new BrsDataGridView();
            dgvCategoryBot = new BrsDataGridView();

            pnlCategoryTop.Controls.Add(dgvCategoryTop);
            dgvCategoryTop.Dock = DockStyle.Fill;
            pnlCategoryMid.Controls.Add(dgvCategoryMid);
            dgvCategoryMid.Dock = DockStyle.Fill;
            pnlCategoryBot.Controls.Add(dgvCategoryBot);
            dgvCategoryBot.Dock = DockStyle.Fill;

            dgvCategoryTop.Columns.Add("catCode", "");
            dgvCategoryTop.Columns.Add("catNameKr", "분류명");
            dgvCategoryTop.Columns.Add("catNameEn", "분류명");

            dgvCategoryMid.Columns.Add("catCode", "");
            dgvCategoryMid.Columns.Add("catNameKr", "분류명");
            dgvCategoryMid.Columns.Add("catNameEn", "분류명");

            dgvCategoryBot.Columns.Add("catCode", "");
            dgvCategoryBot.Columns.Add("catNameKr", "분류명");
            dgvCategoryBot.Columns.Add("catNameEn", "분류명");

            dgvCategoryTop.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategoryMid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategoryBot.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvCategoryTop.MultiSelect = false;
            dgvCategoryMid.MultiSelect = false;
            dgvCategoryBot.MultiSelect = false;

            dgvCategoryTop.ReadOnly = true;
            dgvCategoryMid.ReadOnly = true;
            dgvCategoryBot.ReadOnly = true;

            dgvCategoryTop.Columns["No"].Visible = false;
            dgvCategoryMid.Columns["No"].Visible = false;
            dgvCategoryBot.Columns["No"].Visible = false;

            dgvCategoryTop.Columns["catCode"].Width = 30;
            dgvCategoryMid.Columns["catCode"].Width = 30;
            dgvCategoryBot.Columns["catCode"].Width = 30;

            foreach(DataGridViewColumn column in dgvCategoryTop.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DataPropertyName = column.Name;
            }
            foreach (DataGridViewColumn column in dgvCategoryMid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DataPropertyName = column.Name;
            }
            foreach (DataGridViewColumn column in dgvCategoryBot.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DataPropertyName = column.Name;
            }
        }

    }
}
