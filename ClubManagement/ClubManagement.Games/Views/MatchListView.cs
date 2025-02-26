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

namespace ClubManagement.Games.Views
{
    public partial class MatchListView : Form,IMatchListView
    {
        CustomDataGridViewControl dgvGameList;
        CustomDataGridViewControl dgvPlayerList;
        public MatchListView()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeComboBox();
            InitializeDateTimePicker();
            ViewEvent();
        }

        private void ViewEvent()
        {

            btnAddGame.Click += (s, e) => AddMatchEvent?.Invoke(this, EventArgs.Empty);
            btnGameListEdit.Click += (s, e) => EditMatchEvent?.Invoke(this, EventArgs.Empty);
            btnPalyerListEdit.Click += (s, e) => EditPlayerEvent?.Invoke(this, EventArgs.Empty);
            dgvGameList.dgv.CellDoubleClick += GameSelectedEvent;

            dtpFromDate.ValueChanged += (s, e) => SearchMatchEvent?.Invoke(this, EventArgs.Empty);
            dtpToDate.ValueChanged += (s, e) => SearchMatchEvent?.Invoke(this, EventArgs.Empty);
            chkExclude.CheckedChanged += (s, e) => SearchMatchEvent?.Invoke(this, EventArgs.Empty);

            // 내부 컨트롤러
            btnPre.Click += MovePreMonth;
            btnPost.Click += MovePostMounth;
            cmbType.SelectedIndexChanged += ComboBoxChaingedEvent;

        }
        private void CheckBoxEnanbleSet()
        {
            if (cmbType.SelectedIndex != 0)
            {
                chkExclude.Enabled = true;
            }
            else
            {
                chkExclude.Enabled = false;
            }
        }
        private void GameSelectedEvent(object sender, EventArgs e)
        {
            lblGameTitle.Text = MatchTile;
            SearchPlayerEvent?.Invoke(this, EventArgs.Empty);
        }
        private void InitializeDateTimePicker()
        {
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        /// <summary>
        /// 콤보 박스 변경 이벤트에 실행될 메소드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxChaingedEvent(object sender, EventArgs e)
        {
            CheckBoxEnanbleSet();
            SearchMatchEvent?.Invoke(this, EventArgs.Empty);
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
            AdjustToDate();
            SearchMatchEvent?.Invoke(this, EventArgs.Empty);

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
            AdjustToDate();
            SearchMatchEvent?.Invoke(this, EventArgs.Empty);
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
        /// <summary>
        /// 오류 발생시 종료 메시지
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }

        /// <summary>
        /// 콤보 박스 기본 설정
        /// </summary>
        private void InitializeComboBox()
        {
            cmbType.Items.Add("전체");
            foreach (var item in GameHelper.GaemType)
            {
                cmbType.Items.Add(new KeyValuePair<int, string>(item.Key, item.Value));
            }
            cmbType.DisplayMember = "Value";
            cmbType.ValueMember = "Key";
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.SelectedIndex = 0;
            CheckBoxEnanbleSet();
        }

        /// <summary>
        /// 데이터 그리드 뷰 설정
        /// </summary>
        private void InitializeDataGridView()
        {
            dgvGameList = new CustomDataGridViewControl();
            dgvPlayerList = new CustomDataGridViewControl();

            pnlGameDataGrid.Controls.Add(dgvGameList.dgv);
            dgvGameList.dgv.Dock = DockStyle.Fill;
            pnlPlayerDataGrid.Controls.Add(dgvPlayerList.dgv);
            dgvPlayerList.dgv.Dock = DockStyle.Fill;

            //모임 리스트 컬럼 설정
            dgvGameList.dgv.Columns.Add("code", "code");
            dgvGameList.dgv.Columns.Add("title", "모임명");
            dgvGameList.dgv.Columns.Add("date", "날짜");
            dgvGameList.dgv.Columns.Add("host", "주최자");
            dgvGameList.dgv.Columns.Add("totalPlayer", "참석인원");
            dgvGameList.dgv.Columns.Add("member", "회원");
            dgvGameList.dgv.Columns.Add("nonMember", "비회원");
            dgvGameList.ApplyDefaultColumnSettings();
            dgvGameList.FormatAsDate("date");
            dgvGameList.FormatAsStringCenter("host", "member", "nonMember");
            dgvGameList.FormatAsStringLeft("title");
            dgvGameList.dgv.Columns["code"].Visible = false;
            dgvGameList.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGameList.dgv.ReadOnly = true;

            dgvGameList.dgv.Columns["code"].DataPropertyName = "game_code";
            dgvGameList.dgv.Columns["title"].DataPropertyName = "game_title";
            dgvGameList.dgv.Columns["date"].DataPropertyName = "game_date";
            dgvGameList.dgv.Columns["host"].DataPropertyName = "game_host";
            dgvGameList.dgv.Columns["totalPlayer"].DataPropertyName = "totalPlayer";
            dgvGameList.dgv.Columns["member"].DataPropertyName = "member";
            dgvGameList.dgv.Columns["nonMember"].DataPropertyName = "guest";
            dgvGameList.dgv.AutoGenerateColumns = false;

            //참가자 리스트 컬럼 설정
            dgvPlayerList.dgv.Columns.Add("code", "memberCode");
            dgvPlayerList.dgv.Columns.Add("player", "이름");
            dgvPlayerList.dgv.Columns.Add("birht", "");
            dgvPlayerList.dgv.Columns.Add("gender", "성별");
            dgvPlayerList.dgv.Columns.Add("memberType", "회원여부");
            dgvPlayerList.ApplyDefaultColumnSettings();
            dgvPlayerList.FormatAsStringCenter("player", "birht", "gender", "memberType");
            dgvPlayerList.dgv.Columns["code"].Visible = false;
            dgvPlayerList.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlayerList.dgv.ReadOnly = true;

            dgvPlayerList.dgv.Columns["No"].DataPropertyName = "No";
            dgvPlayerList.dgv.Columns["code"].DataPropertyName = "mem_code";
            dgvPlayerList.dgv.Columns["player"].DataPropertyName = "att_name";
            dgvPlayerList.dgv.Columns["birht"].DataPropertyName = "mem_birth";
            dgvPlayerList.dgv.Columns["gender"].DataPropertyName = "gender";
            dgvPlayerList.dgv.Columns["memberType"].DataPropertyName = "memberType";

            dgvPlayerList.dgv.AutoGenerateColumns = false;

        }
        public DateTime MatchFromDate
        {
            get { return dtpFromDate.Value; }
            set { dtpFromDate.Value = value; }
        }

        public DateTime MatchToDate
        {
            get { return dtpToDate.Value; }
            set { dtpToDate.Value = value; }
        }

        public int? MatchType
        {
            get
            {
                if (cmbType.SelectedItem is KeyValuePair<int, string> selectedItem)
                {
                    return selectedItem.Key == 0 ? (int?)null : selectedItem.Key;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    int index = cmbType.Items.Cast<KeyValuePair<int, string>>().ToList().FindIndex(item => item.Key == value.Value);
                    if (index >= 0)
                    {
                        cmbType.SelectedIndex = index;
                    }
                }
            }

        }

        public bool ExcludeType
        {
            get { return chkExclude.Checked; }
            set { chkExclude.Checked = value; }
        }

        public int? GetMatchCode
        {
            get
            {
                if (dgvGameList.dgv.SelectedRows.Count > 0)
                {
                    return Convert.ToInt32(dgvGameList.dgv.CurrentRow.Cells["code"].Value);
                }
                return null;
            }
        }

        public string MatchTile
        {
            get
            {
                return dgvGameList.dgv.CurrentRow.Cells["title"].Value.ToString();
            }

        }

        public event EventHandler SearchMatchEvent;
        public event EventHandler AddMatchEvent;
        public event EventHandler SearchPlayerEvent;
        public event EventHandler EditPlayerEvent;
        public event EventHandler EditMatchEvent;

        public void SetGameListBinding(DataTable source)
        {
            dgvGameList.dgv.DataSource = source;
        }

        public void SetPlayerListBinding(DataTable source)
        {
            dgvPlayerList.dgv.DataSource = source;
        }
    }
}

