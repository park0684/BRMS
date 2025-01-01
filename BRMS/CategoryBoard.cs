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
        private cDataGridDefaultSet DgrTopCategory;
        private cDataGridDefaultSet DgrMidCategory;
        private cDataGridDefaultSet DgrBotCategory;
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
            DgrTopCategory.Dgv.CellClick += CurrentDatagridviewTop;
            DgrMidCategory.Dgv.CellClick += CurrentDatagridviewMid;
            DgrBotCategory.Dgv.CellClick += CurrentDatagridviewBot;
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
            DgrTopCategory = new cDataGridDefaultSet();
            DgrMidCategory = new cDataGridDefaultSet();
            DgrBotCategory = new cDataGridDefaultSet();
            panelCategoryTop.Controls.Add(DgrTopCategory.Dgv);
            DgrTopCategory.Dgv.Dock = DockStyle.Fill;
            panelCategoryMid.Controls.Add(DgrMidCategory.Dgv);
            DgrMidCategory.Dgv.Dock = DockStyle.Fill;
            panelCategoryBot.Controls.Add(DgrBotCategory.Dgv);
            DgrBotCategory.Dgv.Dock = DockStyle.Fill;

            DgrTopCategory.Dgv.Columns.Add("catTop", "");
            DgrTopCategory.Dgv.Columns.Add("CatTopNameKr", "분류명");
            DgrTopCategory.Dgv.Columns.Add("CatTopNameEn", "분류명");
            DgrMidCategory.Dgv.Columns.Add("catMid", "");
            DgrMidCategory.Dgv.Columns.Add("CatMidNameKr", "분류명");
            DgrMidCategory.Dgv.Columns.Add("CatMidNameEn", "분류명");
            DgrBotCategory.Dgv.Columns.Add("CatBot", "");
            DgrBotCategory.Dgv.Columns.Add("CatBotNamekr", "분류명");
            DgrBotCategory.Dgv.Columns.Add("CatBotNameEn", "분류명");
            DgrTopCategory.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgrMidCategory.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgrBotCategory.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgrTopCategory.FormatAsStringCenter("catTop", "CatTopNameKr", "CatTopNameEn","catMid", "CatMidNameKr", "CatMidNameEn", "CatBot", "CatBotNamekr", "CatBotNameEn");
            DgrTopCategory.Dgv.ReadOnly = true;
            DgrMidCategory.Dgv.ReadOnly = true;
            DgrBotCategory.Dgv.ReadOnly = true;
            DgrTopCategory.Dgv.Columns["catTop"].Width = 30;
            DgrMidCategory.Dgv.Columns["catMid"].Width = 30;
            DgrBotCategory.Dgv.Columns["CatBot"].Width = 30;
            DgrTopCategory.Dgv.Columns["no"].Visible = false;
            DgrMidCategory.Dgv.Columns["no"].Visible = false;
            DgrBotCategory.Dgv.Columns["no"].Visible = false;


            //그리드 정렬 기능 제외
            // Top DataGridView
            foreach (DataGridViewColumn column in DgrTopCategory.Dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Mid DataGridView
            foreach (DataGridViewColumn column in DgrMidCategory.Dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Bot DataGridView
            foreach (DataGridViewColumn column in DgrBotCategory.Dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        /// <summary>
        /// 대분류 카테고리 정보 조회
        /// </summary>
        private void GetTopCategoryInfo()
        {
            DgrTopCategory.Dgv.Rows.Clear();
            DataTable dataTable = new DataTable();
            string query = string.Format("SELECT cat_top, cat_name_kr,cat_name_en FROm category WHERE cat_top != 0 AND cat_mid = 0 AND cat_bot = 0");
            dbconn.SqlDataAdapterQuery(query, dataTable);
            int rowIndex = 0;
            foreach(DataRow dataRow in dataTable.Rows)
            {
                DgrTopCategory.Dgv.Rows.Add();
                DgrTopCategory.Dgv.Rows[rowIndex].Cells[1].Value = dataRow[0];
                DgrTopCategory.Dgv.Rows[rowIndex].Cells[2].Value = dataRow[1];
                DgrTopCategory.Dgv.Rows[rowIndex].Cells[3].Value = dataRow[2];
                rowIndex++;
            }
        }
        /// <summary>
        /// 중분류 카테고리 정보 조회
        /// </summary>
        private void GetMidCategoryInfo()
        {
            DgrMidCategory.Dgv.Rows.Clear();
            DataTable dataTable = new DataTable();
            string query = string.Format("SELECT cat_mid, cat_name_kr, cat_name_en FROM category WHERE cat_top ={0} AND cat_mid != 0 AND cat_bot = 0"
                , DgrTopCategory.Dgv.CurrentRow.Cells[1].Value.ToString());
            dbconn.SqlDataAdapterQuery(query, dataTable);
            int rowIndex = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DgrMidCategory.Dgv.Rows.Add();
                DgrMidCategory.Dgv.Rows[rowIndex].Cells[1].Value = dataRow[0];
                DgrMidCategory.Dgv.Rows[rowIndex].Cells[2].Value = dataRow[1];
                DgrMidCategory.Dgv.Rows[rowIndex].Cells[3].Value = dataRow[2];
                rowIndex++;
            }
            if(WorkTye == 2)
            {
                DgrMidCategory.Dgv.ClearSelection();
            }
        }
        /// <summary>
        /// 소분류 카테고리 정보 조회
        /// </summary>
        private void GetBotCategoryInfo()
        {
            DgrBotCategory.Dgv.Rows.Clear();
            DataTable dataTable = new DataTable();
            string query = string.Format("SELECT cat_bot, cat_name_kr, cat_name_en FROM category WHERE cat_top ={0} AND cat_mid = {1} AND cat_bot != 0"
                , DgrTopCategory.Dgv.CurrentRow.Cells[1].Value.ToString(), DgrMidCategory.Dgv.CurrentRow.Cells[1].Value.ToString());
            dbconn.SqlDataAdapterQuery(query, dataTable);
            int rowIndex = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DgrBotCategory.Dgv.Rows.Add();
                DgrBotCategory.Dgv.Rows[rowIndex].Cells[1].Value = dataRow[0];
                DgrBotCategory.Dgv.Rows[rowIndex].Cells[2].Value = dataRow[1];
                DgrBotCategory.Dgv.Rows[rowIndex].Cells[3].Value = dataRow[2];
                rowIndex++;
            }
            if (WorkTye == 2)
            {
                DgrBotCategory.Dgv.ClearSelection();
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
                DgrMidCategory.Dgv.Rows.Clear();
                DgrBotCategory.Dgv.Rows.Clear();
                DataGridViewRow selectedRow = DgrTopCategory.Dgv.Rows[selectedRowIndex];
                GetMidCategoryInfo();
                pdtTop = Convert.ToInt32(DgrTopCategory.Dgv.CurrentRow.Cells[1].Value);

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
                DgrBotCategory.Dgv.Rows.Clear();
                DataGridViewRow selectedRow = DgrMidCategory.Dgv.Rows[selectedRowIndex];
                GetBotCategoryInfo();
                pdtMid = Convert.ToInt32(DgrMidCategory.Dgv.CurrentRow.Cells[1].Value);
            }
        }
        /// <summary>
        /// 소분류 선택
        /// </summary>
        /// <param name="sener"></param>
        /// <param name="e"></param>
        private void CurrentDatagridviewBot(object sener, DataGridViewCellEventArgs e)
        {
            pdtBot = Convert.ToInt32(DgrBotCategory.Dgv.CurrentRow.Cells[1].Value);
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
            int row = DgrTopCategory.Dgv.CurrentRow.Index;
            int topCode = cDataHandler.ConvertToInt(DgrTopCategory.Dgv.CurrentRow.Cells["catTop"].Value);
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
            if (DgrMidCategory.Dgv.RowCount > 0)
            {
                int topCode = cDataHandler.ConvertToInt(DgrTopCategory.Dgv.CurrentRow.Cells["catTop"].Value);
                int midCode = cDataHandler.ConvertToInt(DgrMidCategory.Dgv.CurrentRow.Cells["catMid"].Value);
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
            if (DgrBotCategory.Dgv.RowCount > 0)
            {
                int topCode = cDataHandler.ConvertToInt(DgrTopCategory.Dgv.CurrentRow.Cells["catTop"].Value);
                int midCode = cDataHandler.ConvertToInt(DgrMidCategory.Dgv.CurrentRow.Cells["catMid"].Value);
                int botCode = cDataHandler.ConvertToInt(DgrBotCategory.Dgv.CurrentRow.Cells["catBot"].Value);
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
            int topCode = cDataHandler.ConvertToInt(DgrTopCategory.Dgv.CurrentRow.Cells["catTop"].Value);
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
            int topCode = cDataHandler.ConvertToInt(DgrTopCategory.Dgv.CurrentRow.Cells["catTop"].Value);
            int midCode = cDataHandler.ConvertToInt(DgrMidCategory.Dgv.CurrentRow.Cells["catMid"].Value);
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
                if (DgrMidCategory.Dgv.CurrentRow == null && DgrBotCategory.Dgv.CurrentRow == null)
                {
                    MessageBox.Show("중,소분류를 선택하세요");
                }
                else if (DgrBotCategory.Dgv.CurrentRow == null)
                {
                    MessageBox.Show("소분류를 선택하세요");
                }
                else
                {
                    pdtTop = Convert.ToInt32(DgrTopCategory.Dgv.CurrentRow.Cells[1].Value);
                    pdtMid = Convert.ToInt32(DgrMidCategory.Dgv.CurrentRow.Cells[1].Value);
                    pdtBot = Convert.ToInt32(DgrBotCategory.Dgv.CurrentRow.Cells[1].Value);
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
    }
}
