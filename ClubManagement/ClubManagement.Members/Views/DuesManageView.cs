using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Common.Controls;
using ClubManagement.Common.Hlepers;

namespace ClubManagement.Members.Views
{
    public partial class DuesManageView : Form,IDuesManageView
    {
        CustomDataGridViewControl dgvMemberList;
        CustomDataGridViewControl dgvStatementList;
        public DuesManageView()
        {
            InitializeComponent();
            InitializeDataGridView();
            ViewEvevnt();
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        private void ViewEvevnt()
        {
            btnSearchMember.Click += (s, e) => MemberSearchEvent?.Invoke(this, EventArgs.Empty);
            btnAdd.Click += (s, e) => StatementAddEvent?.Invoke(this, EventArgs.Empty);
            btnPost.Click += MovePostMounth;
            btnPre.Click += MovePreMonth;
            dgvMemberList.dgv.CellDoubleClick += (s, e) => StatementSearchEvent?.Invoke(this, EventArgs.Empty);
            dgvStatementList.dgv.CellDoubleClick += (s, e) => StatementEditEvent?.Invoke(this, EventArgs.Empty);
            dtpFromDate.ValueChanged += (s, e) => StatementSearchEvent?.Invoke(this, EventArgs.Empty);
            dtpToDate.ValueChanged += (s, e) => StatementSearchEvent?.Invoke(this, EventArgs.Empty);
        }

        private void InitializeDataGridView()
        {
            dgvMemberList = new CustomDataGridViewControl();
            dgvStatementList = new CustomDataGridViewControl();
            var dgvMember = dgvMemberList.dgv;
            var dgvState = dgvStatementList.dgv;
            pnlMember.Controls.Add(dgvMember);
            dgvMember.Dock = DockStyle.Fill;
            pnlStatemet.Controls.Add(dgvState);
            dgvState.Dock = DockStyle.Fill;

            dgvMember.Columns.Add("code", "코드");
            dgvMember.Columns.Add("name", "회원명");
            dgvMember.Columns.Add("totlaDues", "대상");
            dgvMember.Columns.Add("payment", "납부");
            dgvMember.Columns.Add("nonPayment", "미납");
            dgvMember.Columns.Add("free", "면제");
            dgvMember.ReadOnly = true;
            dgvMember.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (DataGridViewColumn column in dgvMember.Columns)
            {
                column.DataPropertyName = column.Name;
            }
            dgvMember.AutoGenerateColumns = false;
            dgvMember.Columns["Code"].Visible = false;
            dgvMemberList.ApplyDefaultColumnSettings();
            dgvMemberList.FormatAsStringCenter("mem_birht");
            dgvMemberList.FormatAsStringLeft("name");
            dgvMemberList.FormatAsInt("totalDues", "payment", "nonPayment", "fee");

            dgvState.Columns.Add("date", "날짜");
            dgvState.Columns.Add("type", "항목");
            dgvState.Columns.Add("statement", "내용");
            dgvState.Columns.Add("applay", "적용");
            dgvState.Columns.Add("deposit", "입금액");
            dgvState.Columns.Add("withdrawal", "출금액");
            dgvState.Columns.Add("balance", "잔액");
            dgvState.Columns.Add("code", "코드");
            dgvState.ReadOnly = true;
            dgvState.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (DataGridViewColumn column in dgvState.Columns)
            {
                column.DataPropertyName = column.Name;
            }
            dgvState.AutoGenerateColumns = false;
            dgvStatementList.ApplyDefaultColumnSettings();
            dgvStatementList.FormatAsInt("deposit", "withdrawal", "balance");
            dgvStatementList.FormatAsStringCenter("No", "code", "type");
            dgvStatementList.FormatAsStringLeft("memo");
            dgvStatementList.FormatAsDate("date");
        }

        /// <summary>
        /// ▷ 버튼 클릭시 실행 메소드
        /// 날짜를 한달 뒤로 이동 시킴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MovePostMounth(object sender, EventArgs e)
        {
            dtpFromDate.Value = dtpFromDate.Value.AddMonths(1);
            //dtpToDate.Value = dtpToDate.Value.AddMonths(1);
            AdjustToDate();
            StatementSearchEvent?.Invoke(this, EventArgs.Empty);

        }

        /// <summary>
        /// ◁ 버튼 클릭시 실행 메소드
        /// 날짜를 한달전으로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovePreMonth(object sender, EventArgs e)
        {
            dtpFromDate.Value = dtpFromDate.Value.AddMonths(-1);
            //dtpToDate.Value = dtpToDate.Value.AddMonths(-1);
            AdjustToDate();
            StatementSearchEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// DateTimePicker 변경 버튼 클릭시 종료일자를 마지막 날짜로 자동 지정
        /// </summary>
        private void AdjustToDate()
        {
            DateTime fromDate = dtpFromDate.Value;
            DateTime today = DateTime.Now;
            int lastDayOfMonth = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);

            // 현재 월이면 오늘 날짜, 아니면 해당 월의 마지막 날
            if (fromDate.Year == today.Year && fromDate.Month == today.Month)
            {
                dtpToDate.Value = today;
            }
            else
            {
                dtpToDate.Value = new DateTime(fromDate.Year, fromDate.Month, lastDayOfMonth);
            }
        }

        public void SetMemberListBinding(DataTable members)
        {
            dgvMemberList.dgv.DataSource = members;
        }
        public void SetStateListBinding(DataTable states)
        {
            dgvStatementList.dgv.DataSource = states;
        }
        public string SearchWord
        {
            get { return txtSearchWord.Text; }
            set { txtSearchWord.Text = value; }
        }

        public DateTime FromDate
        {
            get { return dtpFromDate.Value; }
            set { dtpFromDate.Value = value; }
        }

        public DateTime ToDate
        {
            get { return dtpToDate.Value; }
            set { dtpToDate.Value = value; }
        }

        public bool SecessInclude
        {
            get { return chkSeceInculde.Checked; }
            set { chkSeceInculde.Checked = value; }
        }
        public int? GetMemberCode
        {
            get
            {
                if (dgvMemberList.dgv.SelectedRows.Count > 0)
                {
                    return Convert.ToInt32(dgvMemberList.dgv.CurrentRow.Cells["Code"].Value);
                }
                return null;
            }
        }

        public int? GetStateMentCode
        {
            get
            {
                if (dgvStatementList.dgv.SelectedRows.Count > 0)
                {
                    return Convert.ToInt32(dgvStatementList.dgv.CurrentRow.Cells["code"].Value);
                }
                return null;
            }
        }
        public event EventHandler MemberSearchEvent;
        public event EventHandler StatementSearchEvent;
        public event EventHandler StatementAddEvent;
        public event EventHandler StatementEditEvent;
    }
}
