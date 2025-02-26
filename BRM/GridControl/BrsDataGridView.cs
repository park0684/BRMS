using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace GridControl
{
    public class BrsDataGridView : DataGridView
    {
        /// <summary>
        /// ContextMenuStrip 선택 이벤트
        /// </summary>
        public Action<string> GridMenuActionClicked;

        /// <summary>
        /// 출여보기 대상 Column
        /// </summary> 
        public List<string> _minViewCloumns = new List<string>();
        
        // DataGridView에 표시 하지 않을 Column
        public List<string> _allwaysHiddenColumns= new List<string>();

        //ContextMenuStrip 메뉴 사용 여부 설정
        public bool EnableContextMenu { get; set; } = true;

        //DataGridView 형식으로 엑셀 출력 기능 설정
        public bool EnableExport { get; set; } = true;

        //DataGridView 형식으로 엑셀 출력
        public bool EnablePrint { get; set; } = true;

        public BrsDataGridView()
        {
            InitailzeDataGridView();
            this.MouseClick += OnRightClick;
        }
        private void InitailzeDataGridView()
        {
            EnableHeadersVisualStyles = false;
            
            BackgroundColor = Color.White;
            
            AllowUserToAddRows = false;
            AllowUserToResizeRows = false;
            RowHeadersVisible = false;
            ColumnHeadersHeight = 35;
            RowTemplate.Height = 25;
            
            ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 9F);
            ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(234, 238, 244);
            ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(234, 238, 244);
            DefaultCellStyle.Font = new Font("맑은 고딕", 9F);
            DefaultCellStyle.SelectionBackColor = Color.FromArgb(128, 212, 255);
            DefaultCellStyle.SelectionForeColor = Color.Black;
            SelectionMode = DataGridViewSelectionMode.CellSelect;

            Columns.Add("No", "No");
            Columns["No"].Width = 50;
            Columns["No"].ReadOnly = true;
            Columns["No"].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            Columns["No"].DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow;
            Columns["No"].ValueType = typeof(int);
            Columns["No"].DataPropertyName = "No";
            Columns["No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void OnRightClick(object sender, MouseEventArgs e)
        {
            if (EnableContextMenu && e.Button == MouseButtons.Right)
            {
                var menu = ContextMenuStripManager.GetStandardContextMenu(this, action =>
                {
                    GridMenuActionClicked?.Invoke(action);
                });
                menu.Show(Cursor.Position);
            }
        }
        public void SetMinViewColumns(params string[] columnNames)
        {
            _minViewCloumns.Clear();
            _minViewCloumns.AddRange(columnNames);
        }

        public void SetAlwaysHiddenColumns(params string[] columnNames)
        {
            _allwaysHiddenColumns.Clear();
            _allwaysHiddenColumns.AddRange(columnNames);
            foreach (DataGridViewColumn column in this.Columns)
            {
                if (_allwaysHiddenColumns.Contains(column.Name))
                    column.Visible = false;
            }
        }
        /// <summary>
        /// 펼처보기시 포함 될 칼럼 추가. AllwaysHiddenColumns에서 예외로 등록한 칼럼들만 제외 
        /// </summary>
        public void ShowExpendedColumns()
        {
            foreach(DataGridViewColumn column in this.Columns)
            {
                column.Visible = !_allwaysHiddenColumns.Contains(column.Name);
            }
        }

        /// <summary>
        /// 줄여보기시 표시될 칼럼 추가. MinViewCloumns에서 등록한 칼럼
        /// </summary>
        public void ShowMinimalColumns()
        {
            foreach(DataGridViewColumn column in this.Columns)
            {
                column.Visible = _minViewCloumns.Contains(column.Name);
            }
        }



    }
}
