using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Common.Hlepers;
using ClubManagement.Common.Controls;

namespace ClubManagement.Members.Views
{
    public partial class MemberListView : Form, IMemberListView
    {
        CustomDataGridViewControl dgvMemberList;
        public MemberListView()
        {
            InitializeComponent();
            InitializDateTimePicker();
            InitializeComboBox();
            InitializeDataGridView();
            ViewEvent();
        }

        private void InitializeComboBox()
        {
            cmbStatus.Items.Add("전체");
            foreach (var item in MemberHelper.MemStatus)
            {
                cmbStatus.Items.Add(new KeyValuePair<int, string>(item.Key, item.Value));
            }
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "key";
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.SelectedIndex = 1;
        }
        private void InitializDateTimePicker()
        {
            dtpAccFromDate.Enabled = false;
            dtpAccToDate.Enabled = false;
            dtpSecFromDate.Enabled = false;
            dtpSecToDate.Enabled = false;
            dtpGameFromDate.Enabled = false;
            dtpGameToDate.Enabled = false;
            chkExculde.Enabled = false;
        }

        private void InitializeDataGridView()
        {
            dgvMemberList = new CustomDataGridViewControl();
            var dgv = dgvMemberList.dgv;
            pnlDataGrid.Controls.Add(dgv);
            dgv.Dock = DockStyle.Fill;
            // 그리드 칼럼 추가
            dgv.Columns.Add("code", "code");
            dgv.Columns.Add("name", "이름");
            dgv.Columns.Add("birth", "생년");
            dgv.Columns.Add("gender", "성별");
            dgv.Columns.Add("status", "상태");
            dgv.Columns.Add("position", "직책");
            dgv.Columns.Add("regular", "정기전");
            dgv.Columns.Add("regularRate", "참석율");
            dgv.Columns.Add("irregular", "비정기전");
            dgv.Columns.Add("event", "이벤트전");
            dgv.Columns.Add("payment", "납부");
            dgv.Columns.Add("nonPayent", "미납");
            dgv.Columns.Add("accDate", "가입일");
            dgv.Columns.Add("secDate", "탈퇴일");
            dgv.Columns.Add("gameDate", "참가일");
            dgv.Columns.Add("memo", "메모");
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly = true;
            // 바인딩을 위한 프로퍼티 네임 설정
            dgv.Columns["No"].DataPropertyName = "No";
            dgv.Columns["code"].DataPropertyName = "mem_code";
            dgv.Columns["name"].DataPropertyName = "mem_name";
            dgv.Columns["birth"].DataPropertyName = "mem_birth";
            dgv.Columns["gender"].DataPropertyName = "gender";
            dgv.Columns["status"].DataPropertyName = "status";
            dgv.Columns["position"].DataPropertyName = "position";
            dgv.Columns["regular"].DataPropertyName = "reglar_count";
            dgv.Columns["regularRate"].DataPropertyName = "regularRate";
            dgv.Columns["irregular"].DataPropertyName = "irregular_count";
            dgv.Columns["event"].DataPropertyName = "event_count";
            dgv.Columns["payment"].DataPropertyName = "payment";
            dgv.Columns["nonPayent"].DataPropertyName = "nonPayment";
            dgv.Columns["accDate"].DataPropertyName = "mem_access";
            dgv.Columns["secDate"].DataPropertyName = "mem_secess";
            dgv.Columns["gameDate"].DataPropertyName = "game_last";
            dgv.Columns["memo"].DataPropertyName = "mem_memo";

            //코드 칼럼 및 칼럼 자동생성 비화성
            dgv.Columns["code"].Visible = false;
            dgv.AutoGenerateColumns = false;

            //칼럼 포멧 설정
            dgvMemberList.ApplyDefaultColumnSettings();
            dgvMemberList.FormatAsInt("irregular", "regular", "event", "payment", "nonPayent");
            dgvMemberList.FormatAsStringCenter("name", "birth", "gender", "status", "position");

        }
        private void ViewEvent()
        {
            btnSearch.Click += (s, e) => SearchEvent?.Invoke(this, EventArgs.Empty);
            txtSearchWord.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    SearchEvent?.Invoke(this, EventArgs.Empty);
            };
            btnAddMember.Click += (s, e) => AddMemberEvent?.Invoke(this, EventArgs.Empty);
            dgvMemberList.dgv.CellDoubleClick += (s, e) => EidtMemberEvent?.Invoke(this, EventArgs.Empty);
            chkAccDate.CheckedChanged += SetAccDatePickersEnabled;
            chkSecDate.CheckedChanged += SetSecDatePickersEnabled;
            chkGameDate.CheckedChanged += SetGameDatePickersEnabled;
            cmbStatus.SelectedIndexChanged += SetExchuldeCheckBoxSet;
            chkExRegularGeme.CheckedChanged += SetCheckBoxEnable;
            chkExIrregularGame.CheckedChanged += SetCheckBoxEnable;
            chkExEventGame.CheckedChanged += SetCheckBoxEnable;
        }

        public void SetMemberListBindingSource(BindingSource source)
        {
            dgvMemberList.dgv.DataSource = source;
        }
        private void SetExchuldeCheckBoxSet(object sender, EventArgs e)
        {
            if (cmbStatus.SelectedIndex == 0)
            {
                chkExculde.Checked = false;
                chkExculde.Enabled = false;
            }
            else
            {
                chkExculde.Enabled = true;
            }
        }
        private void SetCheckBoxEnable(object sender, EventArgs e)
        {
            if (chkExRegularGeme.Checked && chkExIrregularGame.Checked && chkExEventGame.Checked)
            {
                MessageBox.Show("최소 한개 이상의 게임은 선택해야 합니다", "알림");
                CheckBox lastChecked = sender as CheckBox;
                if (lastChecked != null)
                {
                    lastChecked.Checked = false;
                }
            }
        }

        public void SetAccDatePickersEnabled(object sender, EventArgs e)
        {
            bool isEnabled = chkAccDate.Checked;
            dtpAccFromDate.Enabled = isEnabled;
            dtpAccToDate.Enabled = isEnabled;
        }
        public void SetSecDatePickersEnabled(object sender, EventArgs e)
        {
            bool isEnabled = chkSecDate.Checked;
            dtpSecFromDate.Enabled = isEnabled;
            dtpSecToDate.Enabled = isEnabled;
        }
        public void SetGameDatePickersEnabled(object sender, EventArgs e)
        {
            bool isEnabled = chkGameDate.Checked;
            dtpGameFromDate.Enabled = isEnabled;
            dtpGameToDate.Enabled = isEnabled;
        }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }

        public void MemberListBinding(DataTable members)
        {
          
            dgvMemberList.dgv.DataSource = members;

        }

        public void ClearDataGridView()
        {
            dgvMemberList.dgv.Rows.Clear();
        }

        public string SearchWord
        {
            get { return txtSearchWord.Text; }
            set { txtSearchWord.Text = value; }
        }
        public int? Status
        {
            get
            {
                if (cmbStatus.SelectedIndex == 0)
                    return 0;
                return ((KeyValuePair<int, string>)cmbStatus.SelectedItem).Key;
            }
            set { cmbStatus.SelectedIndex = (int)value; }
        }
        public DateTime? AccFromDate
        {
            get { return dtpAccFromDate.Value; }
            set { dtpAccFromDate.Value = (DateTime)value; }
        }
        public DateTime? AccToDate
        {
            get { return dtpAccToDate.Value; }
            set { dtpAccToDate.Value = (DateTime)value; }
        }
        public DateTime? SecFromDate
        {
            get { return dtpSecFromDate.Value; }
            set { dtpSecFromDate.Value = (DateTime)value; }
        }
        public DateTime? SecToDate
        {
            get { return dtpSecToDate.Value; }
            set { dtpSecToDate.Value = (DateTime)value; }
        }
        public DateTime? GameFromDate
        {
            get { return dtpGameFromDate.Value; }
            set { dtpGameFromDate.Value = (DateTime)value; }
        }
        public DateTime? GameToDate
        {
            get { return dtpGameToDate.Value; }
            set { dtpGameToDate.Value = (DateTime)value; }
        }
        public bool ExcludeRegular
        {
            get { return chkExRegularGeme.Checked; }
            set { chkExRegularGeme.Checked = value; }
        }
        public bool ExcludeIrregular
        {
            get { return chkExIrregularGame.Checked; }
            set { chkExRegularGeme.Checked = value; }
        }
        public bool ExcludeEvent
        {
            get { return chkExEventGame.Checked; }
            set { chkExEventGame.Checked = value; }
        }
        public bool ExcludeMember
        {
            get { return chkExculde.Checked; }
            set { chkExculde.Checked = value; }
        }
        public bool AccessCheck
        {
            get { return chkAccDate.Checked; }
            set { chkAccDate.Enabled = value; }
        }
        public bool SecessCheck
        {
            get { return chkSecDate.Checked; }
            set { chkSecDate.Checked = value; }
        }
        public bool GameCheck
        {
            get { return chkGameDate.Checked; }
            set { chkGameDate.Checked = value; }
        }

        public int? SelectedCode
        {
            get
            {
                if (dgvMemberList.dgv.SelectedRows.Count > 0)
                {
                    return Convert.ToInt32(dgvMemberList.dgv.CurrentRow.Cells["code"].Value);
                }
                return null;
            }
        }

        public event EventHandler SearchEvent;
        public event EventHandler AddMemberEvent;
        public event EventHandler EidtMemberEvent;
    }
}
