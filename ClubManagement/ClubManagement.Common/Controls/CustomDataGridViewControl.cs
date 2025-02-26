using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClubManagement.Common.Controls
{
    public class CustomDataGridViewControl
    {
        public DataGridView dgv { get; }

        public CustomDataGridViewControl()
        {
            dgv = new DataGridView();
            SetDataGridDefaultSet();
        }

        private void SetDataGridDefaultSet()
        {
            dgv.BackgroundColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AllowUserToAddRows = false;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dgv.BackgroundColor = Color.White;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersHeight = 35;
            dgv.RowTemplate.Height = 25;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(234, 238, 244);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(234, 238, 244);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(128, 212, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.DefaultCellStyle.Font = new Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dgv.Columns.Add("No", "No");
            dgv.Columns["No"].Width = 10;
            dgv.Columns["No"].ReadOnly = true;
            dgv.Columns["No"].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            dgv.Columns["No"].DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow;
            dgv.Columns["No"].ValueType = typeof(int);
            dgv.Columns["No"].DataPropertyName = "No";
            FormatAsStringCenter("No");
        }

        /// <summary>
        /// 컬럼 사이즈 조절 함수
        /// </summary>
        public void ApplyDefaultColumnSettings()
        {
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False; // 줄바꿈 방지
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;  // 셀 크기 자동 조정
            }

            // 그리드 전체 크기 자동 조정 모드 설정
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        public void FormatAsInt(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dgv.Columns.Contains(columnName))
                {
                    dgv.Columns[columnName].DefaultCellStyle.Format = "#,##0;-#,##0;0";
                    dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv.Columns[columnName].MinimumWidth = 10;
                    dgv.Columns[columnName].ValueType = typeof(int);
                }
            }
        }
        /// <summary>
        /// int 포멧으로 지정
        /// </summary>
        /// <param name="columnNames"></param>
        public void FormatAsFloat(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dgv.Columns.Contains(columnName))
                {
                    dgv.Columns[columnName].DefaultCellStyle.Format = "#,##0;-#,##0;0";
                    dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv.Columns[columnName].MinimumWidth = 10;
                    dgv.Columns[columnName].ValueType = typeof(float);
                }
            }
        }
        /// <summary>
        /// Dicimal 포멧으로 지정
        /// </summary>
        /// <param name="columnNames"></param>
        public void FormatAsDecimal(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dgv.Columns.Contains(columnName))
                {
                    dgv.Columns[columnName].DefaultCellStyle.Format = "#,##0.##;-#,##0.##;0";
                    dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv.Columns[columnName].MinimumWidth = 10;
                    dgv.Columns[columnName].ValueType = typeof(decimal);
                }
            }
        }
        /// <summary>
        /// 좌측 정렬 string 포멧으로 지정
        /// </summary>
        /// <param name="columnNames"></param>
        public void FormatAsStringLeft(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dgv.Columns.Contains(columnName))
                {
                    dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgv.Columns[columnName].ValueType = typeof(string);
                }
            }
        }
        /// <summary>
        /// 우측 정렬 string 포멧으로 지정
        /// </summary>
        /// <param name="columnNames"></param>
        public void FormatAsStringRight(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dgv.Columns.Contains(columnName))
                {
                    dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv.Columns[columnName].ValueType = typeof(string);
                }
            }

        }
        /// <summary>
        /// 중앙 정렬 string 포멧으로 지정
        /// </summary>
        /// <param name="columnNames"></param>
        public void FormatAsStringCenter(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dgv.Columns.Contains(columnName))
                {
                    dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgv.Columns[columnName].ValueType = typeof(string);
                }
            }

        }
        /// <summary>
        /// Date 포멧으로 지정
        /// </summary>
        /// <param name="columnNames"></param>
        public void FormatAsDate(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dgv.Columns.Contains(columnName))
                {
                    dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgv.Columns[columnName].MinimumWidth = 120;
                    dgv.Columns[columnName].DefaultCellStyle.Format = "yyyy-MM-dd";
                    dgv.Columns[columnName].ValueType = typeof(DateTime);
                }
            }
        }
        /// <summary>
        /// DataTime 포멧으로 지정
        /// </summary>
        /// <param name="columnNames"></param>
        public void FormatAsDateTime(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dgv.Columns.Contains(columnName))
                {
                    dgv.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgv.Columns[columnName].MinimumWidth = 120;
                    dgv.Columns[columnName].DefaultCellStyle.Format = "yyyy년MM월dd일 hh시mm분";
                    dgv.Columns[columnName].ValueType = typeof(DateTime);
                }
            }
        }
    }
}
