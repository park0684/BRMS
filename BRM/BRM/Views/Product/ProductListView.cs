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
using common.Helpers;
using GridControl;

namespace BRM.Views
{
    public partial class ProductListView : UserControl,IProductListView
    {
        BrsDataGridView dgvProductList = new BrsDataGridView();
        
        public ProductListView()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeComboBxo();
            SetDatePickerStatus();
            ViewEvevnt();
        }

        public string Category
        {
            set { lblCategory.Text = value; }
        }
        public string SerchWord
        {
            get { return txtSearchWord.Text; }
        }
        public int Status
        {
            get 
            {
                var resutl = (KeyValuePair<int,string>)cBoxStatus.SelectedItem;
                return resutl.Key;
            }
            set { cBoxStatus.SelectedIndex = value; }
        }
        public DateTime Type1FromDate
        {
            get { return dtpType1From.Value; }
            set { dtpType1From.Value = value; }
        }
        public DateTime Type1ToDate
        {
            get { return dtpType1To.Value; }
            set { dtpType1To.Value = value; }
        }
        public DateTime Type2FromDate
        {
            get { return dtpType2From.Value; }
            set { dtpType2From.Value = value; }
        }
        public DateTime Type2ToDate
        {
            get { return dtpType2To.Value; }
            set { dtpType2To.Value = value; }
        }

        public int DateType1
        {
            get { return cBoxDateType1.SelectedIndex; }
            set { cBoxDateType1.SelectedIndex = value; }
        }
        public int DateType2
        {
            get { return cBoxDateType2.SelectedIndex; }
            set { cBoxDateType2.SelectedIndex = value; }
        }

        public event EventHandler SelectCategoryEvent;
        public event EventHandler AddProcutEvent;
        public event EventHandler SearchEvent;
        public event EventHandler<int> SelectProcutEvent;

        public void BindingDataTable(DataTable result)
        {
            dgvProductList.DataSource = result;
        }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        public Control AsControl() => this;

        private void ViewEvevnt()
        {
            btnCategory.Click += (s, e) => SelectCategoryEvent?.Invoke(this, EventArgs.Empty);
            btnProductAdd.Click += (s, e) => AddProcutEvent?.Invoke(this, EventArgs.Empty);
            btnSearch.Click += (s, e) => SearchEvent?.Invoke(this, EventArgs.Empty);
            cBoxDateType1.SelectedIndexChanged += (s,e) => SetDatePickerStatus();
            cBoxDateType2.SelectedIndexChanged += (s, e) => SetDatePickerStatus();
            dgvProductList.CellDoubleClick += dgvProductList_CellDoubleClick;
        }
        private void dgvProductList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                int code = Convert.ToInt32(dgvProductList.Rows[e.RowIndex].Cells["pdtCode"].Value);
                SelectProcutEvent?.Invoke(this, code);
            }
        }
        private void InitializeDataGridView()
        {
            panelDataGrid.Controls.Add(dgvProductList);
            dgvProductList.Dock = DockStyle.Fill;

            dgvProductList.AutoGenerateColumns = false;
            dgvProductList.Columns.Add("pdtCode", "부품코드");
            dgvProductList.Columns.Add("pdtStatus", "상태");
            dgvProductList.Columns.Add("pdtNumber", "제품번호");
            dgvProductList.Columns.Add("pdtNamekr", "제품명(한글)");
            dgvProductList.Columns.Add("pdtNameEn", "제품명(영문)");
            dgvProductList.Columns.Add("pdtBprice", "매입단가");
            dgvProductList.Columns.Add("pdtPriceKrw", "판매단가(￦)");
            dgvProductList.Columns.Add("pdtPriceUsd", "판매단가($)");
            dgvProductList.Columns.Add("pdtTopName", "대분류");
            dgvProductList.Columns.Add("pdtMidName", "중분류");
            dgvProductList.Columns.Add("pdtBotName", "소분류");
            dgvProductList.Columns.Add("pdtIdate", "등록일");
            dgvProductList.Columns.Add("pdtUdate", "수정일");
            dgvProductList.ReadOnly = true;
            foreach (DataGridViewColumn column in dgvProductList.Columns)
            {
                column.DataPropertyName = column.Name;
            }
            dgvProductList.SetAlwaysHiddenColumns("pdtCode");
            dgvProductList.SetMinViewColumns("No", "pdtNumber", "pdtNamekr", "pdtBprice", "pdtPriceKrw");


        }
        private void InitializeComboBxo()
        {
            var items = StatusHelper.GetMap(StatusHelper.Keys.ProductStatus).Select(kvp => new KeyValuePair<int, string>(kvp.Key, kvp.Value)).ToList();

            items.Insert(0, new KeyValuePair<int, string>(-1, "전체"));

            cBoxStatus.DataSource = items;
            cBoxStatus.DisplayMember = "Value";
            cBoxStatus.ValueMember = "Key";
            cBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            List<string> dateType1List = new List<string> { "등록/수정 선택", "등록일", "수정일" };
            cBoxDateType1.DataSource = dateType1List;
            cBoxDateType1.DropDownStyle = ComboBoxStyle.DropDownList;

            List<string> dataType2List = new List<string>
            { "판매/매입 선택",
                "판매 O", "매입 O",
                "판매&매입 O",
                "판매 X",
                "매입 X,",
                "판매&매입 X",
                "판매 O, 매입 X",
                "판매 X, 매입 O"
            };
            cBoxDateType2.DataSource = dataType2List;
            cBoxDateType2.DropDownStyle = ComboBoxStyle.DropDownList;
            List<string> statusList = new List<string> { "전체", "판매가능", "품절", "단종", "취급 외" };
        }

        private void SetDatePickerStatus()
        {
            if(cBoxDateType1.SelectedIndex == 0)
            {
                dtpType1From.Enabled = false;
                dtpType1To.Enabled = false;
            }
            else
            {
                dtpType1From.Enabled = true;
                dtpType1To.Enabled = true;
            }

            if (cBoxDateType2.SelectedIndex == 0)
            {
                dtpType2From.Enabled = false;
                dtpType2To.Enabled = false;
            }
            else
            {
                dtpType2From.Enabled = true;
                dtpType2To.Enabled = true;
            }
        }
    }
}
