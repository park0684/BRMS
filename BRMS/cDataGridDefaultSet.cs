using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Data;
using System.IO;

namespace BRMS
{
    class cDataGridDefaultSet
    {
        public DataGridView Dgv { get; }
        public event EventHandler<DataGridViewCellEventArgs> CellDoubleClick;

        private ContextMenuStrip contextMenu;
        public cDataGridDefaultSet()
        {
            Dgv = new DataGridView();
            SetDefaultSetting();
            Dgv.CellDoubleClick += DataGridView_CellDoubleClick;
            contextMenu = new ContextMenuStrip();

        }
        /// <summary>
        /// 데이터 그리드 생성 기본 설정
        /// </summary>
        private void SetDefaultSetting()
        {
            Dgv.BackgroundColor = Color.White;
            Dgv.EnableHeadersVisualStyles = false;
            Dgv.AllowUserToAddRows = false;
            Dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            Dgv.BackgroundColor = Color.White;
            Dgv.AllowUserToResizeRows = false;
            Dgv.RowHeadersVisible = false;
            Dgv.ColumnHeadersHeight = 35;
            Dgv.RowTemplate.Height = 25;
            Dgv.ColumnHeadersDefaultCellStyle.BackColor = cUIManager.Color.BrightGray;
            Dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(234, 238, 244);
            Dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(128, 212, 255);
            Dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            Dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            Dgv.AllowUserToResizeRows = false;
            Dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            Dgv.DefaultCellStyle.Font = new Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            Dgv.Columns.Add("No", "No");
            Dgv.Columns["No"].Width = 50;
            Dgv.Columns["No"].ReadOnly = true;
            Dgv.Columns["No"].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            Dgv.Columns["No"].DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow;
            FormatAsStringCenter("No");
        }
        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 이벤트를 불러온 클래스에서 이벤트 핸들러를 구현할 수 있도록 호출
            CellDoubleClick?.Invoke(sender, e);
        }
        /// <summary>
        /// 칼럼 색상 설정
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="foregroundColor"></param>
        /// <param name="columnNames"></param>
        public void SetColumnCellColors(DataGridView dataGridView, System.Drawing.Color backgroundColor, System.Drawing.Color foregroundColor, params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dataGridView.Columns.Contains(columnName))
                {
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        // 셀이 null인지 확인 (비어있는 행 대비)
                        var cell = row.Cells[columnName];
                        if (cell != null)
                        {
                            cell.Style.BackColor = backgroundColor; // 배경색 설정
                            cell.Style.ForeColor = foregroundColor; // 글자색 설정
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 지정된 row 셀 색상 설정
        /// </summary>
        /// <param name="dataGridView"></param> // 데이터 그리드 이름
        /// <param name="backgroundColor"></param> // 배경색 
        /// <param name="foregroundColor"></param> // 글자색
        /// <param name="rowIndex"></param> // 로우 인덱스
        public void SetRowCellColors(DataGridView dataGridView, System.Drawing.Color backgroundColor, System.Drawing.Color foregroundColor, int rowIndex)
        {
            if (rowIndex > 0 && rowIndex <= dataGridView.Rows.Count)
            {
                // 지정된 행 가져오기
                var row = dataGridView.Rows[rowIndex];

                // 각 셀에 색상 적용
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = backgroundColor;
                    cell.Style.ForeColor = foregroundColor;
                }
            }

            
        }
        /// <summary>
        /// 마지막 행만 제외하고 소트
        /// 소트 진행시 마지막행을 삭제하고 소트 진행 후 다시 마지막 행을 삽입
        /// </summary>
        /// <param name="e"></param>
        public void ExcludeLastRowSort(DataGridViewCellMouseEventArgs e)
        {
            // 행이 없으면 바로 리턴
            if (Dgv.Rows.Count == 0)
            {
                return;
            }
            // sort 작동 불가 설정
            foreach (DataGridViewColumn column in Dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            int lastRowIndex = Dgv.Rows.Count - 1;
            DataGridViewRow lastRow = Dgv.Rows[lastRowIndex];
            Dgv.Rows.RemoveAt(lastRowIndex); // 마지막 행 제거

            SortColumn(e.ColumnIndex); // 열 정렬 수행

            Dgv.Rows.Add(lastRow); // 마지막 행 다시 추가
        }
        /// <summary>
        /// 소트 시 정열 상태 확인 후 반대로 소트 진행
        /// </summary>
        /// <param name="columnIndex"></param>
        private void SortColumn(int columnIndex)
        {
            var clickedColumn = Dgv.Columns[columnIndex];
            DataGridViewColumn previousSortedColumn = Dgv.SortedColumn;
            SortOrder previousSortOrder = Dgv.SortOrder;

            if (previousSortedColumn == clickedColumn)
            {
                ListSortDirection newDirection = (previousSortOrder == SortOrder.Ascending)
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
                Dgv.Sort(clickedColumn, newDirection);
            }
            else
            {
                Dgv.Sort(clickedColumn, ListSortDirection.Descending);
            }
        }
        /// <summary>
        /// 컬럼 사이즈 조절 함수
        /// </summary>
        public void ApplyDefaultColumnSettings()
        {
            foreach (DataGridViewColumn column in Dgv.Columns)
            {
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False; // 줄바꿈 방지
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;  // 셀 크기 자동 조정
            }

            // 그리드 전체 크기 자동 조정 모드 설정
            Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells; 
        }

        public void ExportToExcel()
        {
            DataTable exportData = new DataTable();
            cExport export = new cExport();
            // 1. DataGridView의 컬럼 헤더를 DataTable의 컬럼으로 추가
            foreach (DataGridViewColumn column in Dgv.Columns)
            {
                if (column.Visible)  // 숨겨진 컬럼은 제외
                {
                    exportData.Columns.Add(column.HeaderText);
                }
            }
            DataRow headerRow = exportData.NewRow();
            foreach (DataGridViewColumn column in Dgv.Columns)
            {
                if (column.Visible)  // 숨겨진 컬럼은 제외
                {
                    headerRow[column.HeaderText] = column.HeaderText; // 첫 번째 행에 컬럼명 추가
                }
            }
            exportData.Rows.Add(headerRow);
            // 2. DataGridView의 각 행 데이터를 DataTable에 추가
            foreach (DataGridViewRow row in Dgv.Rows)
            {
                DataRow dataRow = exportData.NewRow();

                foreach (DataGridViewColumn column in Dgv.Columns)
                {
                    if (column.Visible)  // 숨겨진 컬럼은 제외
                    {
                        dataRow[column.HeaderText] = row.Cells[column.Index].Value ?? DBNull.Value;
                    }
                }

                exportData.Rows.Add(dataRow);
            }

            // 3. DataTable을 Excel로 내보내기
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                Form parentForm = Dgv.FindForm();
                string formText = parentForm.Text;
                saveFileDialog.Filter = "Execl Files (*.xlsx)|*.xlsx|Allfils(*.*)|*.*";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName =$"{DateTime.Now.ToString("yyyyMMddHHmm")}({formText})";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    export.ExportDataToExcelNoneColumn(exportData, filePath);

                }
            }

        }
        
        /// <summary>
        /// 데이터 그리드 구조 정수형
        /// </summary>
        /// <param name="columnNames"></param>
        public void FormatAsInteger(params  string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (Dgv.Columns.Contains(columnName))
                {
                    Dgv.Columns[columnName].DefaultCellStyle.Format = "#,##0;#,##0;0";
                    Dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Dgv.Columns[columnName].MinimumWidth = 80;
                }
            }
            
        }
        public void FormatAsDecimal(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (Dgv.Columns.Contains(columnName))
                {
                    Dgv.Columns[columnName].DefaultCellStyle.Format = "#,##0.##;#,##0.##;0";
                    Dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    Dgv.Columns[columnName].MinimumWidth = 80;
                }
            }
        }

        public void FormatAsStringLeft(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (Dgv.Columns.Contains(columnName))
                {
                    Dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
            }
        }
        public void FormatAsStringRight(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (Dgv.Columns.Contains(columnName))
                {
                    Dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

        }
        public void FormatAsStringCenter(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (Dgv.Columns.Contains(columnName))
                {
                    Dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            
        }
        public void FormatAsDate(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (Dgv.Columns.Contains(columnName))
                {
                    Dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    Dgv.Columns[columnName].MinimumWidth = 120;
                    Dgv.Columns[columnName].DefaultCellStyle.Format = "yyyy년MM월dd일";
                }
            }
        }
        public void FormatAsDateTime(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (Dgv.Columns.Contains(columnName))
                {
                    Dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    Dgv.Columns[columnName].MinimumWidth = 120;
                    Dgv.Columns[columnName].DefaultCellStyle.Format = "yyyy년MM월dd일 hh시mm분";
                }
            }
        }
        public decimal ConvertToDecimal(object value)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                // Convert the value to a string, remove any commas, and then convert to int
                string valueAsString = value.ToString().Replace(",", ""); // 콤마 제거
                if (decimal.TryParse(valueAsString, out decimal number))
                {
                    return number;
                }
            }
            return 0;
        }

        public int ConvertToInt(object value)
        {
            if(value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                // Convert the value to a string, remove any commas, and then convert to int
                string valueAsString = value.ToString().Replace(",", ""); // 콤마 제거
                if (decimal.TryParse(valueAsString, out decimal number))
                {
                    return (int)number;
                }
            }
            return 0;
        }
    }
}
