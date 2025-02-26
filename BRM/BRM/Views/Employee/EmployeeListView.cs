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
    public partial class EmployeeListView : UserControl,IEmployeeListView
    {
        BrsDataGridView dgvEmpList;
        public EmployeeListView()
        {
            InitializeComponent();
            InitializeDataGridView();
            ViewEvent();
        }

        public int Status
        {
            get 
            {
                var result = (KeyValuePair<int, string>)cmbStatus.SelectedItem;
                return result.Key;
            }
        }
            

        public event EventHandler SearchEvent;
        public event EventHandler AddEmployeeEvent;
        public event EventHandler<int> SelectEmployeeEvent;

        public Control AsControl()
        {
            return this;
        }

        /// <summary>
        /// 직원 리스트 데이터 바인딩
        /// </summary>
        /// <param name="resutl"></param>
        public void EmpListBinding(DataTable resutl)
        {
            dgvEmpList.DataSource = resutl;
        }

        /// <summary>
        /// 상태 콤보박스 기본 설정
        /// </summary>
        /// <param name="items"></param>
        public void SetComboBoxItems(Dictionary<int, string> items)
        {
            //콤보박스 데이터소스 딕셔너리 생성 및 전체 항목 등록
            var comboSource = new Dictionary<int, string>
            {
                {-1, "전체" }
            };
            //전달 받은 딕셔너리 정보 데이터소스 딕셔너리에 등록
            foreach(var item in items)
            {
                comboSource.Add(item.Key, item.Value);
            }

            cmbStatus.DataSource = comboSource.ToList();
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "Key";
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// 데이터그리드뷰 컬럼 생성
        /// </summary>
        /// <param name="columns"></param>
        public void SetDataGridColumns(Dictionary<string, string> columns)
        {
            foreach(var column in columns)
            {
                dgvEmpList.Columns.Add( new DataGridViewTextBoxColumn
                { 
                    Name = column.Key,
                    DataPropertyName = column.Key,
                    HeaderText = column.Value
                });
            }
            foreach (DataGridViewColumn column in dgvEmpList.Columns)
            {
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False; //칼럼 헤더 줄바꿈 방지
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // 셀 크기 자동 조절
            }
            dgvEmpList.AllowUserToResizeColumns = true;
        }

        /// <summary>
        /// 메시지 박스 생성 메소드
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// 이벤트 등록
        /// </summary>
        private void ViewEvent()
        {
            btnSearch.Click += (s, e) => SearchEvent?.Invoke(this, EventArgs.Empty);
            btnAddEmployee.Click += (s, e) => AddEmployeeEvent?.Invoke(this, EventArgs.Empty);
            dgvEmpList.CellDoubleClick += dgvEmpList_CellDoubleClick;
        }

        /// <summary>
        /// 데이터그리드뷰 초기화 
        /// </summary>
        private void InitializeDataGridView()
        {
            dgvEmpList = new BrsDataGridView();
            pnlDataGrid.Controls.Add(dgvEmpList);
            dgvEmpList.Dock = DockStyle.Fill;
            dgvEmpList.ReadOnly = true;
            dgvEmpList.AutoGenerateColumns = false;
        }

        /// <summary>
        /// 데이터그리드뷰 더블 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvEmpList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                int code = Convert.ToInt32(dgvEmpList.Rows[e.RowIndex].Cells["empCode"].Value);
                SelectEmployeeEvent?.Invoke(this, code);
            }
        }
    }
}
