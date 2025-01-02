using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BRMS
{
    public partial class CategoryBoard : Form
    {
        private cDataGridDefaultSet DgvTopCategory;
        private cDataGridDefaultSet DgvMidCategory;
        private cDataGridDefaultSet DgvBotCategory;
        cDatabaseConnect dbconn = new cDatabaseConnect();
        CategoryEdit categoryEdit = new CategoryEdit();
        public event Action<int, int, int> CategorySelected;
        int WorkTye = 0; // 0== 분류 지정 | 1 == 분류설정 | 2 == 분류 조회 조건 설정
        int pdtTop = 0;
        int pdtMid = 0;
        int pdtBot = 0;
        int accessedEmp = cUserSession.AccessedEmp;
        bool isEditMode = false;

        public CategoryBoard()
        {
            InitializeComponent();
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            DataGridViewForm();
            GetTopCategoryInfo();
            DgvTopCategory.Dgv.CellClick += CurrentDatagridviewTop;
            DgvMidCategory.Dgv.CellClick += CurrentDatagridviewMid;
            DgvBotCategory.Dgv.CellClick += CurrentDatagridviewBot;
            chkStatus.CheckedChanged += chkStatus_ChagedChcked;
            CheckModifyPermission();
        }
        /// <summary>
        /// 등록/수정 권한 확인 후 버튼 비활성화 여부 결정
        /// </summary>
        private void CheckModifyPermission()
        {
            if (!cUserSession.HasPermission(122))
            {
                bntBotCategoryAdd.Enabled = false;
                bntBotCategoryModify.Enabled = false;
                bntMidCategoryAdd.Enabled = false;
                bntMidCategoryModify.Enabled = false;
                bntTopCategoryAdd.Enabled = false;
                bntTopCategoryModify.Enabled = false;
            }
        }
        /// <summary>
        /// 설정모드로 Form 실행
        /// </summary>
        /// <returns></returns>
        public bool EditeMode()
        {
            isEditMode = true;
            ButtonStatusSet();
            return isEditMode;
        }
        /// <summary>
        /// 조회모드로 Form 실행
        /// </summary>
        /// <returns></returns>
        public bool SearchMode()
        {
            isEditMode = false;
            ButtonStatusSet();
            return isEditMode;
        }
        /// <summary>
        /// 실행 모드에 따라 버튼 활성화 상태 설정
        /// </summary>
        public void ButtonStatusSet()
        {
           
            if(isEditMode == true)
            {
                bntOk.Visible = false;
            }
            else
            {
                bntBotCategoryAdd.Visible = false;
                bntMidCategoryAdd.Visible = false;
                bntTopCategoryAdd.Visible = false;
                bntBotCategoryModify.Visible = false;
                bntMidCategoryModify.Visible = false;
                bntTopCategoryModify.Visible = false;
            }

        }
        /// <summary>
        /// 데이트 그리드 설정
        /// </summary>
        private void DataGridViewForm()
        {
            DgvTopCategory = new cDataGridDefaultSet();
            DgvMidCategory = new cDataGridDefaultSet();
            DgvBotCategory = new cDataGridDefaultSet();
            panelCategoryTop.Controls.Add(DgvTopCategory.Dgv);
            DgvTopCategory.Dgv.Dock = DockStyle.Fill;
            panelCategoryMid.Controls.Add(DgvMidCategory.Dgv);
            DgvMidCategory.Dgv.Dock = DockStyle.Fill;
            panelCategoryBot.Controls.Add(DgvBotCategory.Dgv);
            DgvBotCategory.Dgv.Dock = DockStyle.Fill;

            DgvTopCategory.Dgv.Columns.Add("catTop", "");
            DgvTopCategory.Dgv.Columns.Add("CatTopNameKr", "분류명");
            DgvTopCategory.Dgv.Columns.Add("CatTopNameEn", "분류명");
            DgvMidCategory.Dgv.Columns.Add("catMid", "");
            DgvMidCategory.Dgv.Columns.Add("CatMidNameKr", "분류명");
            DgvMidCategory.Dgv.Columns.Add("CatMidNameEn", "분류명");
            DgvBotCategory.Dgv.Columns.Add("CatBot", "");
            DgvBotCategory.Dgv.Columns.Add("CatBotNamekr", "분류명");
            DgvBotCategory.Dgv.Columns.Add("CatBotNameEn", "분류명");
            DgvTopCategory.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvMidCategory.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvBotCategory.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvTopCategory.FormatAsStringCenter("catTop", "CatTopNameKr", "CatTopNameEn","catMid", "CatMidNameKr", "CatMidNameEn", "CatBot", "CatBotNamekr", "CatBotNameEn");
            DgvTopCategory.Dgv.ReadOnly = true;
            DgvMidCategory.Dgv.ReadOnly = true;
            DgvBotCategory.Dgv.ReadOnly = true;
            DgvTopCategory.Dgv.Columns["catTop"].Width = 30;
            DgvMidCategory.Dgv.Columns["catMid"].Width = 30;
            DgvBotCategory.Dgv.Columns["CatBot"].Width = 30;
            DgvTopCategory.Dgv.Columns["no"].Visible = false;
            DgvMidCategory.Dgv.Columns["no"].Visible = false;
            DgvBotCategory.Dgv.Columns["no"].Visible = false;


            //그리드 정렬 기능 제외
            // Top DataGridView
            foreach (DataGridViewColumn column in DgvTopCategory.Dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Mid DataGridView
            foreach (DataGridViewColumn column in DgvMidCategory.Dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Bot DataGridView
            foreach (DataGridViewColumn column in DgvBotCategory.Dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        /// <summary>
        /// 대분류 카테고리 정보 조회
        /// </summary>
        private void GetTopCategoryInfo()
        {
            DgvTopCategory.Dgv.Rows.Clear();
            DataTable dataTable = new DataTable();
            string query = "SELECT cat_top, cat_name_kr,cat_name_en, cat_status FROm category WHERE cat_top != 0 AND cat_mid = 0 AND cat_bot = 0";
            query += chkStatus.Checked == false ? " AND cat_status = 1":"";
            dbconn.SqlDataAdapterQuery(query, dataTable);
            int rowIndex = 0;
            foreach(DataRow dataRow in dataTable.Rows)
            {
                DgvTopCategory.Dgv.Rows.Add();
                DgvTopCategory.Dgv.Rows[rowIndex].Cells[1].Value = dataRow[0];
                DgvTopCategory.Dgv.Rows[rowIndex].Cells[2].Value = dataRow[1];
                DgvTopCategory.Dgv.Rows[rowIndex].Cells[3].Value = dataRow[2];
                if(cDataHandler.ConvertToInt(dataRow["cat_status"]).Equals(0) )
                {
                    DgvTopCategory.Dgv.Rows[rowIndex].DefaultCellStyle.BackColor = cUIManager.Color.PastelRose;
                }
                rowIndex++;
            }
        }
        /// <summary>
        /// 중분류 카테고리 정보 조회
        /// </summary>
        private void GetMidCategoryInfo()
        {
            DgvMidCategory.Dgv.Rows.Clear();
            DataTable dataTable = new DataTable();
            int topCode = cDataHandler.ConvertToInt(DgvTopCategory.Dgv.CurrentRow.Cells[1].Value);
            string query = $"SELECT cat_mid, cat_name_kr, cat_name_en, cat_status FROM category WHERE cat_top ={topCode} AND cat_mid != 0 AND cat_bot = 0";
            query += chkStatus.Checked == false ? " AND cat_status = 1" : "";
            dbconn.SqlDataAdapterQuery(query, dataTable);
            int rowIndex = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DgvMidCategory.Dgv.Rows.Add();
                DgvMidCategory.Dgv.Rows[rowIndex].Cells[1].Value = dataRow[0];
                DgvMidCategory.Dgv.Rows[rowIndex].Cells[2].Value = dataRow[1];
                DgvMidCategory.Dgv.Rows[rowIndex].Cells[3].Value = dataRow[2];
                if (cDataHandler.ConvertToInt(dataRow["cat_status"]).Equals(0))
                {
                    DgvMidCategory.Dgv.Rows[rowIndex].DefaultCellStyle.BackColor = cUIManager.Color.PastelRose;
                }
                rowIndex++;
            }
            if(WorkTye == 2)
            {
                DgvMidCategory.Dgv.ClearSelection();
            }
        }
        /// <summary>
        /// 소분류 카테고리 정보 조회
        /// </summary>
        private void GetBotCategoryInfo()
        {
            DgvBotCategory.Dgv.Rows.Clear();
            DataTable dataTable = new DataTable();
            int topCode = cDataHandler.ConvertToInt(DgvTopCategory.Dgv.CurrentRow.Cells[1].Value);
            int midCode = cDataHandler.ConvertToInt(DgvMidCategory.Dgv.CurrentRow.Cells[1].Value);
            string query = $"SELECT cat_bot, cat_name_kr, cat_name_en, cat_status FROM category WHERE cat_top ={topCode} AND cat_mid = {midCode} AND cat_bot != 0";
            query += chkStatus.Checked == false ? " AND cat_status = 1" : "";
            dbconn.SqlDataAdapterQuery(query, dataTable);
            int rowIndex = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DgvBotCategory.Dgv.Rows.Add();
                DgvBotCategory.Dgv.Rows[rowIndex].Cells[1].Value = dataRow[0];
                DgvBotCategory.Dgv.Rows[rowIndex].Cells[2].Value = dataRow[1];
                DgvBotCategory.Dgv.Rows[rowIndex].Cells[3].Value = dataRow[2];
                if (cDataHandler.ConvertToInt(dataRow["cat_status"]).Equals(0))
                {
                    DgvBotCategory.Dgv.Rows[rowIndex].DefaultCellStyle.BackColor = cUIManager.Color.PastelRose;
                }
                rowIndex++;
            }
            if (WorkTye == 2)
            {
                DgvBotCategory.Dgv.ClearSelection();
            }
        }
        /// <summary>
        /// 대분류 선택
        /// 선택된 대분류 내 하위 중분류 정보 조회 실행
        /// </summary>
        /// <param name="sener"></param>
        /// <param name="e"></param>
        private void CurrentDatagridviewTop(object sener, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = e.RowIndex;
            if(selectedRowIndex >=0)
            {
                DgvMidCategory.Dgv.Rows.Clear();
                DgvBotCategory.Dgv.Rows.Clear();
                DataGridViewRow selectedRow = DgvTopCategory.Dgv.Rows[selectedRowIndex];
                GetMidCategoryInfo();
                pdtTop = Convert.ToInt32(DgvTopCategory.Dgv.CurrentRow.Cells[1].Value);

            }
        }
        /// <summary>
        /// 중분류 선택
        /// 선택된 중분류 내 하위 소분류 정보 조회
        /// </summary>
        /// <param name="sener"></param>
        /// <param name="e"></param>
        private void CurrentDatagridviewMid(object sener, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = e.RowIndex;
            if (selectedRowIndex >= 0)
            {
                DgvBotCategory.Dgv.Rows.Clear();
                DataGridViewRow selectedRow = DgvMidCategory.Dgv.Rows[selectedRowIndex];
                GetBotCategoryInfo();
                pdtMid = Convert.ToInt32(DgvMidCategory.Dgv.CurrentRow.Cells[1].Value);
            }
        }
        /// <summary>
        /// 소분류 선택
        /// </summary>
        /// <param name="sener"></param>
        /// <param name="e"></param>
        private void CurrentDatagridviewBot(object sener, DataGridViewCellEventArgs e)
        {
            pdtBot = Convert.ToInt32(DgvBotCategory.Dgv.CurrentRow.Cells[1].Value);
        }
        /// <summary>
        /// 분류정보 수정 창 호출
        /// </summary>
        private void CallCategoryEditFrom()
        {
            categoryEdit.StartPosition = FormStartPosition.CenterParent;
            categoryEdit.ShowDialog();
        }

        /// <summary>
        /// 대분류 수정 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntTopCategoryModify_Click(object sender, EventArgs e)
        {
            int row = DgvTopCategory.Dgv.CurrentRow.Index;
            int topCode = cDataHandler.ConvertToInt(DgvTopCategory.Dgv.CurrentRow.Cells["catTop"].Value);
            int midCode = 0;
            int botCode = 0;
            categoryEdit.GetCategoryinfo(topCode, midCode, botCode, false);
            CallCategoryEditFrom();
            GetTopCategoryInfo();
        }
        /// <summary>
        /// 중분류 수정 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntMidCategoryModify_Click(object sender, EventArgs e)
        {
            if (DgvMidCategory.Dgv.RowCount > 0)
            {
                int topCode = cDataHandler.ConvertToInt(DgvTopCategory.Dgv.CurrentRow.Cells["catTop"].Value);
                int midCode = cDataHandler.ConvertToInt(DgvMidCategory.Dgv.CurrentRow.Cells["catMid"].Value);
                int botCode = 0;
                categoryEdit.GetCategoryinfo(topCode, midCode, botCode, false);
                CallCategoryEditFrom();
                GetMidCategoryInfo();
            }
            else
            {
                MessageBox.Show("수정 할 수 있는 중분류가 없습니다");
            }
        }
        /// <summary>
        /// 소분류 수정 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntBotCategoryModify_Click(object sender, EventArgs e)
        {
            if (DgvBotCategory.Dgv.RowCount > 0)
            {
                int topCode = cDataHandler.ConvertToInt(DgvTopCategory.Dgv.CurrentRow.Cells["catTop"].Value);
                int midCode = cDataHandler.ConvertToInt(DgvMidCategory.Dgv.CurrentRow.Cells["catMid"].Value);
                int botCode = cDataHandler.ConvertToInt(DgvBotCategory.Dgv.CurrentRow.Cells["catBot"].Value);
                categoryEdit.GetCategoryinfo(topCode, midCode, botCode, false);
                CallCategoryEditFrom();
                GetBotCategoryInfo();
            }
            else
            {
                MessageBox.Show("수정 할 수 있는 소분류가 없습니다");
            }
        }
        /// <summary>
        /// 대분류 추가 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntTopCategoryAdd_Click(object sender, EventArgs e)
        {
            int topCode = 0;
            int midCode = 0;
            int botCode = 0;
            categoryEdit.GetCategoryinfo(topCode, midCode, botCode, true);
            CallCategoryEditFrom();
            GetTopCategoryInfo();
        }
        /// <summary>
        /// 중분류 추가 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntMidCategoryAdd_Click(object sender, EventArgs e)
        {
            int topCode = cDataHandler.ConvertToInt(DgvTopCategory.Dgv.CurrentRow.Cells["catTop"].Value);
            int midCode = 0;
            int botCode = 0;
            categoryEdit.GetCategoryinfo(topCode, midCode, botCode, true);
            CallCategoryEditFrom();
            GetMidCategoryInfo();
        }
        /// <summary>
        /// 소분류 추가 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntBotCategoryAdd_Click(object sender, EventArgs e)
        {
            int topCode = cDataHandler.ConvertToInt(DgvTopCategory.Dgv.CurrentRow.Cells["catTop"].Value);
            int midCode = cDataHandler.ConvertToInt(DgvMidCategory.Dgv.CurrentRow.Cells["catMid"].Value);
            int botCode = 0;
            if (topCode != 0 || midCode != 0)
            {
                categoryEdit.GetCategoryinfo(topCode, midCode, botCode, true);
                CallCategoryEditFrom();
            }
            GetBotCategoryInfo();
        }
        /// <summary>
        /// 닫기 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 선택 버튼
        /// 선택된 분류 정보가 이 클래스를 호출한 부모클래스에게 데이터 전달
        /// SearchMode 상태일 경우만 활성화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntOk_Click(object sender, EventArgs e)
        {
            if(WorkTye != 2)
            {
                if (DgvMidCategory.Dgv.CurrentRow == null && DgvBotCategory.Dgv.CurrentRow == null)
                {
                    MessageBox.Show("중,소분류를 선택하세요");
                }
                else if (DgvBotCategory.Dgv.CurrentRow == null)
                {
                    MessageBox.Show("소분류를 선택하세요");
                }
                else
                {
                    pdtTop = Convert.ToInt32(DgvTopCategory.Dgv.CurrentRow.Cells[1].Value);
                    pdtMid = Convert.ToInt32(DgvMidCategory.Dgv.CurrentRow.Cells[1].Value);
                    pdtBot = Convert.ToInt32(DgvBotCategory.Dgv.CurrentRow.Cells[1].Value);
                    //productInfo.GetCategory(pdttop, pdtmid, pdtbot);
                    CategorySelected?.Invoke(pdtTop, pdtMid, pdtBot);
                    Close();
                }
            }
            else
            {
                //productInfo.GetCategory(pdttop, pdtmid, pdtbot);
                CategorySelected?.Invoke(pdtTop, pdtMid, pdtBot);
                Close();
            }
        }
        private void chkStatus_ChagedChcked(object sender, EventArgs e)
        {
            GetTopCategoryInfo();
        }
    }
}
