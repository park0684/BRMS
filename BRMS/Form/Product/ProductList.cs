using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BRMS
{
    public partial class ProductList : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        private cDataGridDefaultSet DgvProductList;
        DataTable PdtListData = new DataTable();
        int pdtCatTop = 0;
        int pdtCatMid = 0;
        int pdtCatBot = 0;
        bool categoryToggle = false;
        int accessedEmp = cUserSession.AccessedEmp;

        public ProductList()
        {
            InitializeComponent();
            DgvProductList = new cDataGridDefaultSet();
            panelDataGrid.Controls.Add(DgvProductList.Dgv);
            DgvProductList.Dgv.Dock = DockStyle.Fill;
            tBoxSearchWord.KeyUp += KeyUpEnter;
            DgvProductList.CellDoubleClick += DgrProductList_CellDoubleClick;
            InitializeDefaultValues();
            GridForm();
            DateTypeSetting();
            ModifyPermission();
        }
        private void ModifyPermission()
        {
            if (!cUserSession.HasPermission(102))
            {
                bntProductAdd.Enabled = false;
            }
        }
        private void GridForm()
        {
            DgvProductList.Dgv.AutoGenerateColumns = false;
            DgvProductList.Dgv.Columns.Add("pdtCode", "부품코드");
            DgvProductList.Dgv.Columns.Add("pdtStatus", "상태");
            DgvProductList.Dgv.Columns.Add("pdtNumber", "제품번호");
            DgvProductList.Dgv.Columns.Add("pdtNamekr", "제품명(한글)");
            DgvProductList.Dgv.Columns.Add("pdtNameEn", "제품명(영문)");
            DgvProductList.Dgv.Columns.Add("pdtBprice", "매입단가");
            DgvProductList.Dgv.Columns.Add("pdtPriceKrw", "판매단가(￦)");
            DgvProductList.Dgv.Columns.Add("pdtPriceUsd", "판매단가($)");
            DgvProductList.Dgv.Columns.Add("pdtTopName", "대분류");
            DgvProductList.Dgv.Columns.Add("pdtMidName", "중분류");
            DgvProductList.Dgv.Columns.Add("pdtBotName", "소분류");
            DgvProductList.Dgv.Columns.Add("pdtIdate", "등록일");
            DgvProductList.Dgv.Columns.Add("pdtUdate", "수정일");
            DgvProductList.Dgv.ReadOnly = true;
            DgvProductList.Dgv.Columns["pdtCode"].Visible = false;
            DgvProductList.ApplyDefaultColumnSettings();
            foreach(DataGridViewColumn column in DgvProductList.Dgv.Columns)
            {
                column.DataPropertyName = column.Name;
            }
            //포멧설정
            DgvProductList.FormatAsInt("pdtPriceKrw");
            DgvProductList.FormatAsDecimal("pdtBprice", "pdtPriceUsd");
            DgvProductList.FormatAsStringLeft("pdtNumber", "pdtNamekr", "pdtNameUs");
            DgvProductList.FormatAsStringCenter("pdtStatus", "pdtTopName", "pdtMidName", "pdtBotName");
            DgvProductList.FormatAsDateTime("pdtIdate", "pdtUdate");

        }

        private void InitializeDefaultValues()
        {
            lblCategory.Text = "전체";
            cBoxStatus.SelectedItem = "판매가능";
            dtpType1From.Enabled = false;
            dtpType1To.Enabled = false;
            dtpType2From.Enabled = false;
            dtpType2To.Enabled = false;
            dtpType1From.Format = DateTimePickerFormat.Short;
            dtpType1To.Format = DateTimePickerFormat.Short;
            dtpType2From.Format = DateTimePickerFormat.Short;
            dtpType2To.Format = DateTimePickerFormat.Short;
        }

        private void DateTypeSetting()
        {
            List<string> dateType1List = new List<string> {"등록/수정 선택","등록일", "수정일" };
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
            cBoxStatus.DataSource = statusList;
            cBoxStatus.SelectedIndex = 1;
            cBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void DatePickerEanble(ComboBox comboBox, DateTimePicker fromDate, DateTimePicker toDate)
        {
            if(comboBox.SelectedIndex != 0)
            {
                fromDate.Enabled = true;
                toDate.Enabled = true;
            }
            else
            {
                fromDate.Enabled = false;
                toDate.Enabled = false;
            }
        }

        private void cBoxDateType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatePickerEanble(cBoxDateType1, dtpType1From, dtpType1To);
        }

        private void cBoxDateType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatePickerEanble(cBoxDateType2, dtpType2From, dtpType2To);
        }

        /// <summary>
        /// 데이터 그리드 데이터 등록
        /// 검색 속도 문제로 DataSource로 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void FillGrid(DataTable dataTable)
        //{
        //    DgvProductList.Dgv.Rows.Clear();
        //    int rowIndex = 0;
            
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
                
        //        //DataRow StatusRow = cBoxStatus.SelectedValue.ToString .[int.Parse(dataRow["pdt_status"].ToString())];
        //        DgvProductList.Dgv.Rows.Add();
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["No"].Value = DgvProductList.Dgv.RowCount;
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtcode"].Value = dataRow["pdtcode"].ToString();
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtStatus"].Value = cBoxStatus.Items[int.Parse(dataRow["pdtStatus"].ToString())].ToString(); ;//StatusRow[1].ToString();
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtNumber"].Value = dataRow["pdtNumber"].ToString();
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtNamekr"].Value = dataRow["pdtNamekr"].ToString();
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtNameEn"].Value = dataRow["pdtNameEn"].ToString();
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtBprice"].Value = Convert.ToDecimal(dataRow["pdtBprice"]);
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtPriceKrw"].Value = Convert.ToDecimal(dataRow["pdtPriceKrw"]);
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtPriceUsd"].Value = Convert.ToDecimal(dataRow["pdtPriceUsd"]);
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtTopName"].Value = dataRow["pdtTopName"].ToString();
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtMidName"].Value = dataRow["pdtMidName"].ToString();
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtBotName"].Value = dataRow["pdtBotName"].ToString();
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtIdate"].Value = Convert.ToDateTime(dataRow["pdtIdate"].ToString()).ToString("yyyy-MM-dd HH:mm"); ;
        //        DgvProductList.Dgv.Rows[rowIndex].Cells["pdtUdate"].Value = Convert.ToDateTime(dataRow["pdtUdate"].ToString()).ToString("yyyy-MM-dd HH:mm");

        //        rowIndex++;

        //    }
        //}
        
        private void bntCategory_Click(object sender, EventArgs e)
        {
            if(categoryToggle == true)
            {
                categoryToggle = false;
                lblCategory.Text = "전체";
                pdtCatTop = 0;
                pdtCatMid = 0;
                pdtCatBot = 0;
            }
            else
            {
                //CategoryTreeView categoryTreeView = new CategoryTreeView();
                //categoryTreeView.CategorySelected += (top, mid, bot) => { GetCategoryInfo(top, mid, bot); };
                //categoryTreeView.ShowDialog();
                CategoryBoard categoryBoard = new CategoryBoard();
                categoryBoard.StartPosition = FormStartPosition.CenterParent;
                categoryBoard.CategorySelected += (top, mid, bot) => { GetCategoryInfo(top, mid, bot); };
                categoryBoard.WorkType = 2;
                categoryBoard.SearchMode();
                categoryBoard.ShowDialog();
            }

        }

        private void GetCategoryInfo(int top, int mid, int bot)
        {
            DataTable dataTable = new DataTable();
            DataRow dataRow;
            string query;
            string categoryText;

            query = string.Format("SELECT cat_name_kr,cat_name_en FROM category WHERE cat_top = {0} AND cat_mid = 0 AND  cat_bot = 0", top);
            dataTable.Reset();
            dbconn.SqlReaderQuery(query, dataTable);
            dataRow = dataTable.Rows[0];
            categoryText = string.Format("{0}({1})", dataRow["cat_name_kr"].ToString(), dataRow["cat_name_en"].ToString());
            if (mid != 0)
            {
                query = string.Format("SELECT cat_name_kr,cat_name_en FROM category WHERE cat_top = {0} AND cat_mid = {1} AND  cat_bot = 0", top, mid);
                dataTable.Reset();
                dbconn.SqlReaderQuery(query, dataTable);
                dataRow = dataTable.Rows[0];
                categoryText = categoryText + string.Format(" ▶ {0}({1})", dataRow["cat_name_kr"].ToString(), dataRow["cat_name_en"].ToString());
            }

            if (mid != 0 && bot != 0)
            {
                query = string.Format("SELECT cat_name_kr,cat_name_en FROM category WHERE cat_top = {0} AND cat_mid = {1} AND  cat_bot = {2}", top, mid, bot);
                dataTable.Reset();
                dbconn.SqlReaderQuery(query, dataTable);
                dataRow = dataTable.Rows[0];
                categoryText = categoryText + string.Format(" ▶ {0}({1})", dataRow["cat_name_kr"].ToString(), dataRow["cat_name_en"].ToString());
            }
            lblCategory.Text = categoryText;
            this.pdtCatTop = top;
            pdtCatMid = mid;
            pdtCatBot = bot;
            categoryToggle = true;
        }

        private void DgrProductList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRowIndex = e.RowIndex;
            if (currentRowIndex >= 0)
            {
                DataGridViewRow currentRow = DgvProductList.Dgv.Rows[currentRowIndex];
                int pdtCode = DgvProductList.ConvertToInt(currentRow.Cells["pdtCode"].Value);

                ProductDetail productDetail = new ProductDetail();
                productDetail.StartPosition = FormStartPosition.CenterParent;
                productDetail.GetProductInfo(pdtCode);
                cLog.InsertEmpAccessLogNotConnect("@pdtSearch", accessedEmp, pdtCode);
                //productDetail.refresh += (refreshCode) => refresh = refreshCode;
                productDetail.ShowDialog();
                //if (refresh == 1)
                //{
                //    DialogResult result = MessageBox.Show("다시 조회 하시겠습니까?", "알림", MessageBoxButtons.YesNo);
                //    if (result == DialogResult.Yes)
                //    {
                //        RunQuery();
                //        refresh = 0;
                //    }
                //}

            }
        }

        private void QuerySetting()
        {
            DataTable reusltdata = new DataTable();

            string query;
            string querySelect= "SELECT pdt_code AS pdtCode ,pdt_status , RTRIM(pdt_number) AS pdtNumber, RTRIM(pdt_name_kr) AS pdtNamekr, RTRIM(pdt_name_en)  AS pdtNameEn ," +
                "pdt_bprice AS pdtBprice, pdt_sprice_krw AS pdtPriceKrw, pdt_sprice_usd AS pdtPriceUsd,\n" +
                "(SELECT cat_name_kr FROM category WHERE cat_top = pdt_top AND cat_mid = 0 AND cat_bot = 0) AS pdtTopName,\n" +
                "(SELECT cat_name_kr FROM category WHERE cat_top = pdt_top AND cat_mid = pdt_mid AND cat_bot = 0 ) AS pdtMidName,\n" +
                "(SELECT cat_name_kr FROM category WHERE cat_top = pdt_top AND cat_mid = pdt_mid AND cat_bot = pdt_bot) AS pdtBotName,pdt_idate pdtIdate, pdt_udate pdtUdate FROM product";
            string queryWhere = null;

            //if(string.IsNullOrEmpty(tBoxSearchWord.Text) && categoryToggle== false && cBoxDateType1.SelectedIndex == 0 && cBoxDateType2.SelectedIndex==0)
            //{
            //    if(MessageBox.Show("검색 조건 없이 상품 검색시 시간이 소요 될 수 있습니다.\n계속 하시겠습니까?","알림",MessageBoxButtons.YesNo)==DialogResult.Yes)
                
            //}
            
           
           
           
            if(categoryToggle==true)
            {
                if(!string.IsNullOrEmpty(tBoxSearchWord.Text))
                {
                    queryWhere = string.Format(queryWhere + " AND pdt_top = {0}",pdtCatTop);
                }
                else
                {
                    queryWhere = string.Format(" pdt_top = {0}", pdtCatTop);
                }
                if (pdtCatMid != 0)
                {
                    queryWhere = string.Format(queryWhere + " AND pdt_mid = {0}", pdtCatMid);
                }
                if(pdtCatBot != 0)
                {
                    queryWhere = string.Format(queryWhere + " AND pdt_bot = {0}", pdtCatBot);
                }
            }
            if(cBoxDateType1.SelectedIndex != 0)
            {
                string DateType1 = null;
                string dtpFrom = dtpType1From.Text;
                string dtpTo = dtpType1To.Value.AddDays(1).ToString("yyyy-MM-dd");

            
                switch (cBoxDateType1.SelectedIndex)
                {
                    case 1:
                        DateType1 = $"pdt_idate >= '{dtpFrom}' AND pdt_idate <= '{dtpTo}'";
                        break;
                    case 2:
                        DateType1 = $"pdt_udate >= '{dtpFrom}' AND pdt_udate <= '{dtpTo}'";
                        break;
                }
                if (!string.IsNullOrEmpty(tBoxSearchWord.Text) || !string.IsNullOrEmpty(queryWhere))
                {
                    queryWhere = string.Format(queryWhere + " AND " + DateType1);
                }
                else
                {
                    queryWhere = DateType1;
                }
            }
            if(cBoxDateType2.SelectedIndex != 0)
            {
                 string dtpFrom = dtpType2From.Value.ToString("yyyy-MM-dd");
                 string dtpTo = dtpType2To.Value.AddDays(1).ToString("yyyy-MM-dd");
                   
                 string DateType2 = null;
                 switch (cBoxDateType2.SelectedIndex)
                 {
                     case 1:
                         
                         DateType2 = $" pdt_code IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{dtpFrom}' AND sale_date <='{dtpTo}' AND sale_code =  saled_code)";
                         break;
                     case 2:
                         DateType2 = $" pdt_code IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{dtpFrom}' AND pur_date <='{dtpTo}' AND pur_code = purd_code)";
                         break;
                     case 3:
                         DateType2 = $" pdt_code IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{dtpFrom}' AND sale_date <='{dtpTo}' AND sale_code =  saled_code) AND" +
                             $" pdt_code IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{dtpFrom}' AND pur_date <='{dtpTo}' AND pur_code = purd_code)";
                         break;
                     case 4:
                         DateType2 = $" pdt_code NOT IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{dtpFrom}' AND sale_date <='{dtpTo}' AND sale_code =  saled_code)";
                         break;
                     case 5:
                         DateType2 = $" pdt_code NOT IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{dtpFrom}' AND pur_date <='{dtpTo}' AND pur_code = purd_code)";
                         break;
                     case 6:
                         DateType2 = $" pdt_code NOT IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{dtpFrom}' AND sale_date <='{dtpTo}' AND sale_code =  saled_code) AND" +
                             $" pdt_code NOT IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{dtpFrom}' AND pur_date <='{dtpTo}' AND pur_code = purd_code)";
                         break;
                     case 7:
                         DateType2 = $" pdt_code IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{dtpFrom}' AND sale_date <='{dtpTo}' AND sale_code =  saled_code) AND" +
                             $" pdt_code NOT IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{dtpFrom}' AND pur_date <='{dtpTo}' AND pur_code = purd_code)";
                         break;
                     case 8:
                         DateType2 = $" pdt_code NOT IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{dtpFrom}' AND sale_date <='{dtpTo}' AND sale_code =  saled_code) AND" +
                             $" pdt_code IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{dtpFrom}' AND pur_date <='{dtpTo}' AND pur_code = purd_code)";
                         break;
                 }
                 if (!string.IsNullOrEmpty(tBoxSearchWord.Text) || !string.IsNullOrEmpty(queryWhere))
                 {
                     queryWhere = queryWhere + " AND " + DateType2;
                 }
                 else
                 {
                     queryWhere =  DateType2;
                 }
            }
            if(cBoxStatus.SelectedIndex != 0)
            {
                string Status = string.Format(" pdt_status ={0} ", cBoxStatus.SelectedIndex.ToString());
                if(!string.IsNullOrEmpty(tBoxSearchWord.Text) || !string.IsNullOrEmpty(queryWhere))
                {
                    queryWhere = queryWhere + " AND " + Status;
                }
                else
                {
                    queryWhere =  Status;
                }
            }
            if (string.IsNullOrEmpty(tBoxSearchWord.Text))
            {
                query = string.Format("{0} WHERE {1} ORDER BY pdtNumber", querySelect, queryWhere);
            }
            else
            {
                query = string.Format("{0} WHERE pdt_name_kr LIKE '%{1}%' {2} \nunion \n {0} WHERE pdt_name_en LIKE '%{1}%' {2} \nunion \n {0} WHERE pdt_number LIKE '%{1}%' {2} ORDER BY pdtNumber", querySelect, tBoxSearchWord.Text, queryWhere);
            }
            
            //MessageBox.Show(query);
            dbconn.SqlDataAdapterQuery(query, reusltdata);
            PdtListData = reusltdata;
            InsertToDataGridView();
            //FillGrid(reusltdata);
            cLog.InsertEmpAccessLogNotConnect("@pdtSearch", accessedEmp, 0);
        }
        private void InsertToDataGridView()
        {
            PdtListData.Columns.Add("No");
            PdtListData.Columns.Add("pdtStatus");
            int i = 1;
            foreach(DataRow row in PdtListData.Rows)
            {

                row["No"] = i;
                string status = cStatusCode.GetProductStatus(cDataHandler.ConvertToInt(row["pdt_status"]));
                row["pdtStatus"] = status;
                i++;
            }
            DgvProductList.Dgv.DataSource = PdtListData;
        }
        

        public void RunQuery()
        {
            if(tBoxSearchWord.Text.Contains("'"))
            {
                MessageBox.Show("검색어에 작은 따옴표( ' )가 표함된 경우 조회가 불가능 합니다", "알림");
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(tBoxSearchWord.Text) && categoryToggle == false && cBoxDateType1.SelectedIndex == 0 && cBoxDateType2.SelectedIndex == 0)
                    {
                        if (MessageBox.Show("검색 조건 없이 상품 검색시 시간이 소요 될 수 있습니다.\n계속 하시겠습니까?", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            QuerySetting();
                        }
                    }
                    else
                    {
                        QuerySetting();
                    }
                }   
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void KeyUpEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RunQuery();
            }
        }

        private void bntProductAdd_Click(object sender, EventArgs e)
        {
            ProductDetail productDetail = new ProductDetail();
            productDetail.StartPosition = FormStartPosition.CenterParent;
            productDetail.UnregisteredProduct("", "0", "0");
            productDetail.ShowDialog();
        }
    }
}
