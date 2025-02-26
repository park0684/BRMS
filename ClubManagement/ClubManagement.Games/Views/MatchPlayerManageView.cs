using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public partial class MatchPlayerManageView : Form,IMatchPlayerManageView
    {
        public MatchPlayerManageView()
        {
            InitializeComponent();
            ViewEvent();
        }
        private void ViewEvent()
        {
            btnSearch.Click += (s, e) => SearchMemberEvent?.Invoke(this, EventArgs.Empty);
            btnClose.Click += (s, e) => CloseEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SavePlayerListEvent?.Invoke(this, EventArgs.Empty);
            btnAddGuest.Click += (s, e) => AddGuestEvent?.Invoke(this, EventArgs.Empty);
            txtSearchWord.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    SearchMemberEvent?.Invoke(this, EventArgs.Empty);
            };
        }
        public string SearchWord
        {
            get { return txtSearchWord.Text; }
            set { txtSearchWord.Text = value; }
        }

        public bool SecessMember
        {
            get { return chkSecessMember.Checked; }
            set { chkSecessMember.Checked = value; }
        }


        public event EventHandler SearchMemberEvent;
        public event EventHandler PlayerUpdateEvent;
        public event EventHandler<PlayerButtonEventArgs> PlayerAddEvent;
        public event EventHandler<PlayerButtonEventArgs> PlayerRemoveEvent;
        public event EventHandler SavePlayerListEvent;
        public event EventHandler CloseEvent;
        public event EventHandler AddGuestEvent;

        public void CloseForm()
        {
            this.Close();
        }
        public void ShowForm()
        {
            Form form = (Form)this;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }
        public void SetMemberList(List<PlayerInfoDto> members)
        {
            flpMemberList.Controls.Clear();
            foreach (var member in members)
            {
                Button btn = new Button
                {
                    Size = new Size(90, 50),
                    FlatStyle = FlatStyle.Flat,

                    Text = member.PalyerName,
                    Tag = member.MemberCode,
                    BackColor = member.IsSelected ? Color.FromArgb(65, 100, 223) : Color.FromArgb(54, 178, 221)
                };
                btn.Click += (sender, e) => PlayerAddEvent?.Invoke(this, new PlayerButtonEventArgs((int)btn.Tag,btn.Text));
                flpMemberList.Controls.Add(btn);
            }

        }


        public void SetPlayerList(List<PlayerInfoDto> players)
        {
            flpPlayerList.Controls.Clear();
            //flpPlayerList.AutoScroll = true;
            foreach (var player in players)
            {
                Button btn = new Button
                {
                    Size = new Size(90, 50),
                    FlatStyle = FlatStyle.Flat,
                    Text = player.PalyerName,
                    Tag = player.MemberCode,
                    BackColor = Color.FromArgb(54, 178, 221)
                };
                btn.Click += (sender, e) => PlayerRemoveEvent?.Invoke(this, new PlayerButtonEventArgs((int)btn.Tag, btn.Text));
                flpPlayerList.Controls.Add(btn);
            }
        }

        public void UpdateButtonColor(int memberCode, bool isAdded)
        {
            foreach (Control control in flpMemberList.Controls)
            {
                if (control is Button btn && (int)btn.Tag == memberCode)
                {
                    btn.BackColor = isAdded ? Color.FromArgb(65, 100, 223) : Color.FromArgb(54, 178, 221);
                    break; // 찾았으면 더 이상 반복할 필요 없음
                }
            }
        }
    }
    /// <summary>
    /// 자동 생성되는 버튼의 이벤트 생성
    /// </summary>
    public class PlayerButtonEventArgs : EventArgs
    {
        public int MemberCode { get; }
        public string MemberName { get; }

        public PlayerButtonEventArgs(int memberCode, string memberName)
        {
            MemberCode = memberCode;
            MemberName = memberName;
        }
    }
}
