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
    public partial class ProductSearchBox : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet DgrPdtSearch = new cDataGridDefaultSet();
        DataTable resultData = new DataTable();
        public event Action<int> ProductForword;
        public ProductSearchBox()
        {
            InitializeComponent();
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            panelDataGrid.Controls.Add(DgrPdtSearch.Dgv);
            GridForm();
            DgrPdtSearch.Dgv.Dock = DockStyle.Fill;
            DgrPdtSearch.CellDoubleClick += DgrDgrPdtSearch_CellDoubleClick;
            tBoxSearch.KeyUp += tBoxSearch_KeyUpEnter;
            DgrPdtSearch.Dgv.KeyDown += DgrPdtSearch_KeyDown;
            
        }

        private void GridForm()
        {
            DgrPdtSearch.Dgv.AutoGenerateColumns = false;

            DgrPdtSearch.Dgv.Columns.Add("pdtCode", "제품코드");
            DgrPdtSearch.Dgv.Columns.Add("pdtNumber", "제품번호");
            DgrPdtSearch.Dgv.Columns.Add("pdtNameKr", "제품명(한글)");
            DgrPdtSearch.Dgv.Columns.Add("pdtNameEn", "제품명(영문)");
            DgrPdtSearch.Dgv.Columns.Add("pdtBprice", "매입가");
            DgrPdtSearch.Dgv.Columns.Add("pdtSpriceKrw", "판매가");
            DgrPdtSearch.Dgv.Columns["pdtCode"].Visible = false;
            DgrPdtSearch.ApplyDefaultColumnSettings();
            DgrPdtSearch.FormatAsStringLeft("pdtNumber", "pdtNameKr", "pdtNameen");
            DgrPdtSearch.FormatAsDecimal("매입가");
            DgrPdtSearch.FormatAsInt("판매가");
            foreach(DataGridViewColumn column in DgrPdtSearch.Dgv.Columns)
            {
                column.DataPropertyName = column.Name;
            }
            DgrPdtSearch.Dgv.ReadOnly = true;
            DgrPdtSearch.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        }
        ///// <summary>
        ///// 데이터 그리드 결과 등록
        ///// 속도 문제로 DataSource 방식으로 변경
        ///// </summary>
        ///// <param name="dataTable"></param>
        //private void FillGrid(DataTable dataTable)
        //{
            
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {

        //        int newIndex = DgrPdtSearch.Dgv.Rows.Add();
        //        var rowIndex = DgrPdtSearch.Dgv.Rows[newIndex];
        //        rowIndex.Cells["no"].Value = newIndex + 1;
        //        rowIndex.Cells["pdtCode"].Value = dataRow["pdtCode"];
        //        rowIndex.Cells["pdtNumber"].Value = dataRow["pdtNumber"];
        //        rowIndex.Cells["pdtNameKr"].Value = dataRow["pdtNameKr"];
        //        rowIndex.Cells["pdtNameEn"].Value = dataRow["pdtNameEn"];
        //        rowIndex.Cells["pdtBprice"].Value = dataRow["pdtBprice"];
        //        rowIndex.Cells["pdtSpriceKrw"].Value = dataRow["pdtSpriceKrw"];

        //    }

        //}
        
        /// <summary>
        /// 데이터 그리드 등록
        /// </summary>
        private void InsertDataGridView()
        {
            int i= 1;
            resultData.Columns.Add("No");
            foreach(DataRow row in resultData.Rows)
            {
                row["No"] = i;
                i++;
            }
            DgrPdtSearch.Dgv.DataSource = resultData;
        }

        /// <summary>
        /// DataGridView에 데이터 바인딩
        /// </summary>
        /// <param name = "dataTable" ></ param >
        //private void BindDataToGrid(DataTable dataTable)
        //{
        //    DgrPdtSearch.Dgv.RowCount = 0;
        //    resultData = dataTable;

        //    // 가상 모드 설정
        //    DgrPdtSearch.Dgv.VirtualMode = true;

        //    DgrPdtSearch.Dgv.RowCount = resultData.Rows.Count;

        //    // CellValueNeeded 이벤트 연결
        //    DgrPdtSearch.Dgv.CellValueNeeded -= DgrPdtSearch_CellValueNeeded;
        //    DgrPdtSearch.Dgv.CellValueNeeded += DgrPdtSearch_CellValueNeeded;

        //    // 열 크기 조절
        //    DgrPdtSearch.Dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        //}

        /// <summary>
        /// CellValueNeeded 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void DgrPdtSearch_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.RowIndex < resultData.Rows.Count)
        //    {
        //        DataRow row = resultData.Rows[e.RowIndex];
        //        e.Value = e.RowIndex + 1;
        //        switch (DgrPdtSearch.Dgv.Columns[e.ColumnIndex].Name)
        //        {
        //            case "pdtCode":
        //                e.Value = row["pdtCode"];
        //                break;
        //            case "pdtNumber":
        //                e.Value = row["pdtNumber"];
        //                break;
        //            case "pdtNameKr":
        //                e.Value = row["pdtNameKr"];
        //                break;
        //            case "pdtNameEn":
        //                e.Value = row["pdtNameEn"];
        //                break;
        //            case "pdtBprice":
        //                e.Value = row["pdtBprice"];
        //                break;
        //            case "pdtSpriceKrw":
        //                e.Value = row["pdtSpriceKrw"];
        //                break;
        //        }
        //    }
        //}
        private void SearchQuery()
        {
            if (ErrorCheck())
            {
                return;
            }
            //string query = string.Format("SELECT pdt_code,pdt_number,pdt_name_kr,pdt_name_en,pdt_bprice,pdt_sprice_krw FROM product WHERE pdt_number like '%{0}%' \n union\n" +
            //    "SELECT pdt_code,pdt_number,pdt_name_kr,pdt_name_en,pdt_bprice,pdt_sprice_krw  FROM product WHERE pdt_name_kr LIKE '%{0}%'\n UNION \n" +
            //    "SELECT pdt_code,pdt_number,pdt_name_kr,pdt_name_en,pdt_bprice,pdt_sprice_krw  FROM product WHERE pdt_name_en LIKE'%{0}%'", tBoxSearch.Text);
            string query = $"SELECT pdt_code pdtCode,pdt_number pdtNumber,pdt_name_kr pdtNameKr,pdt_name_en pdtNameEn,pdt_bprice pdtBprice,pdt_sprice_krw pdtSpriceKrw FROM product WHERE pdt_number like '%{tBoxSearch.Text}%' \n union\n" +
                $"SELECT pdt_code ,pdt_number ,pdt_name_kr ,pdt_name_en , pdt_bprice ,pdt_sprice_krw  FROM product WHERE pdt_name_kr LIKE '%{tBoxSearch.Text}%'\n UNION \n" +
                $"SELECT pdt_code ,pdt_number ,pdt_name_kr ,pdt_name_en , pdt_bprice ,pdt_sprice_krw  FROM product WHERE pdt_name_en LIKE'%{tBoxSearch.Text}%'";
            DataTable dataTable = new DataTable();
            dbconn = new cDatabaseConnect();
            dbconn.SqlDataAdapterQuery(query, dataTable);
            resultData = dataTable;
            //FillGrid(dataTable);
            InsertDataGridView();
            //DgrPdtSearch.Dgv.DataSource = dataTable;

            DgrPdtSearch.Dgv.Focus();
        }
        private bool ErrorCheck()
        {
            if (string.IsNullOrEmpty(tBoxSearch.Text))
            {
                cUIManager.ShowMessageBox("검색어를 입력 하세요", "알림", MessageBoxButtons.OK);
                return true;
            }
            if(tBoxSearch.Text.Contains("%%"))
            {
                cUIManager.ShowMessageBox(" '%%'는 입력 할 수 없는 문자입니다", "알림", MessageBoxButtons.OK);
                return true;
            }
            if (tBoxSearch.Text.Contains("'"))
            {
                cUIManager.ShowMessageBox("작은 따옴표(') 는 입력 할 수 없는 문자입니다", "알림", MessageBoxButtons.OK);
                return true;
            }

            return false;
        }
        private void tBoxSearch_KeyUpEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchQuery();
            }
        }
        private void DgrPdtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectProduct();
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                // 방향키로 셀 선택 시, 현재 선택된 셀을 커서로 이동시킵니다.
                DgrPdtSearch.Dgv.BeginEdit(true); // 편집을 시작하여 커서를 이동시킵니다.
            }
        }
        private void SelectProduct()
        {
            try
            {
                if(DgrPdtSearch.Dgv.CurrentRow != null)
                {
                    int currentIndex = DgrPdtSearch.Dgv.CurrentRow.Index;
                    int pdtCode = DgrPdtSearch.ConvertToInt(DgrPdtSearch.Dgv.Rows[currentIndex].Cells["pdtCode"].Value);
                    ProductForword?.DynamicInvoke(pdtCode);
                    Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DgrDgrPdtSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectProduct();
        }

        private void bntSearch_Click(object sender, EventArgs e)
        {
            SearchQuery();
        }

        private void bntOk_Click(object sender, EventArgs e)
        {
            SelectProduct();
        }

        private void bntClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
