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
    public partial class GuestAddView : Form, IGuestAddView
    {
        public GuestAddView()
        {
            InitializeComponent();
            ViewEvent();
            flpGuestList.AutoScroll = true;
        }

        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseFormEvevnt?.Invoke(this, EventArgs.Empty);
            btnAddGuest.Click += (s, e) => AddGuestEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveGuestEvent?.Invoke(this, EventArgs.Empty);
            txtGuestName.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    AddGuestEvent?.Invoke(this, EventArgs.Empty);
            };
        }

        public string GuestName
        {
            get { return txtGuestName.Text; }
            set { txtGuestName.Text = value; }
        }
        public List<PlayerInfoDto> GusetDate()
        {
            var guestList = new List<PlayerInfoDto>();

            foreach (Panel pnl in flpGuestList.Controls)
            {
                string guestName = pnl.Controls[0].Text;
                bool isSelect = true;
                bool gender = false;
                if (pnl.Controls[1].Tag == "1")
                    gender = true;
                int handi = Convert.ToInt32(pnl.Controls[4].Text);
                bool isPro = false;
                if (pnl.Controls[2].Tag == "1")
                    isPro = true;
                PlayerInfoDto guest = new PlayerInfoDto
                {
                    PalyerName = guestName,
                    MemberCode = 0,
                    IsSelected = true,
                    Gender = gender,
                    Handycap = handi,
                    IsPro = isPro
                };
                guestList.Add(guest);
            }
            return guestList;
        }
        public event EventHandler AddGuestEvent;
        public event EventHandler SaveGuestEvent;
        public event EventHandler CloseFormEvevnt;

        public void CloseForm()
        {
            this.Close();
        }
        public void AddGuestPanel()
        {
            var pnl = new Panel
            {
                Size = new Size(380, 60),
                Text = ""
            };
            var txtName = new TextBox
            {
                Text = string.IsNullOrEmpty(txtGuestName.Text) ? "게스트" : txtGuestName.Text,
                Size = new Size(100, 40),
                Font = new Font("맑은 고딕", 12, FontStyle.Bold),
                Location = new Point(10, 20),
                TextAlign = HorizontalAlignment.Center
            };
            var btnGender = new Button
            {
                Text = "여",
                Tag = "0",
                Location = new Point(119, 14),
                Size = new Size(40, 40),
            };

            var btnPro = new Button
            {
                Text = "프로",
                Tag = "0",
                Location = new Point(168, 14),
                Size = new Size(40, 40),
            };
            var lblHandi = new Label
            {
                Text = "핸디",
                Size = new Size(40, 35),
                Font = new Font("맑은 고딕", 10),
                Location = new Point(217, 24),
                BackColor = Color.Transparent
            };
            var txtHandi = new TextBox
            {
                Text = "0",
                Font = new Font("맑은 고딕", 12, FontStyle.Bold),
                Size = new Size(60, 40),
                Location = new Point(258, 20),
                TextAlign = HorizontalAlignment.Center
            };
            var btnDelete = new Button
            {
                Text = "X",
                Location = new Point(328, 14),
                Size = new Size(40, 40)
            };
            btnDelete.Click += (sender, e) =>
            {
                flpGuestList.Controls.Remove(pnl);
            };
            pnl.Controls.Add(txtName);
            pnl.Controls.Add(btnGender);
            pnl.Controls.Add(btnPro);
            pnl.Controls.Add(lblHandi);
            pnl.Controls.Add(txtHandi);
            pnl.Controls.Add(btnDelete);
            flpGuestList.Controls.Add(pnl);
            btnGender.Click += (sender, e) =>
            {

                btnGender.Tag = btnGender.Tag == "0" ? "1" : "0";
                btnGender.BackColor = btnGender.Tag == "1" ? Color.AliceBlue : Color.Transparent;
                int i = Convert.ToInt32(txtHandi.Text);
                if (btnGender.Tag == "1")
                {
                    txtHandi.Text = (i + 15).ToString();
                }
                else
                {
                    txtHandi.Text = (i - 15).ToString();
                }
            };
            btnPro.Click += (sender, e) =>
            {

                btnPro.Tag = btnPro.Tag == "0" ? "1" : "0";
                btnPro.BackColor = btnPro.Tag == "1" ? Color.AliceBlue : Color.Transparent;
                int i = Convert.ToInt32(txtHandi.Text);
                if (btnPro.Tag == "1")
                {
                    txtHandi.Text = (i - 5).ToString();
                }
                else
                {
                    txtHandi.Text = (i + 5).ToString();
                }
            };
        }
        public void ShowForm()
        {
            Form form = (Form)this;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }
        public void TextBoxClear()
        {
            txtGuestName.Text = "";
        }
    }
}
