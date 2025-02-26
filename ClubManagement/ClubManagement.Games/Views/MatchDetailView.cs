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

namespace ClubManagement.Games.Views
{
    public partial class MatchDetailView : Form,IMatchDetailView
    {
        public MatchDetailView()
        {
            InitializeComponent();
            InitializeComboBox();
            ViewEvent();
        }
        private void ViewEvent()
        {
            cmbGameType.SelectedIndexChanged += HostTextBoxEvent;
            btnClose.Click += (s, e) => CloseEvenvt?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
        }
        private void InitializeComboBox()
        {
            foreach (var item in GameHelper.GaemType)
            {
                cmbGameType.Items.Add(new KeyValuePair<int, string>(item.Key, item.Value));
            }
            cmbGameType.DisplayMember = "Value";
            cmbGameType.ValueMember = "Key";
            cmbGameType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGameType.SelectedIndex = 0;
        }
        private void InitializeControlBox()
        {
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }
        private void HostTextBoxEvent(object sender, EventArgs e)
        {
            int typeIndex = ((KeyValuePair<int, string>)cmbGameType.SelectedItem).Key;
            if (((KeyValuePair<int, string>)cmbGameType.SelectedItem).Key == 1)
            {
                txtHost.Text = "정기전";
            }
        }

        public void CloseForm()
        {
            this.Close();
        }

        public void ShowForm()
        {
            Form form = (Form)this;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        public string GameTitle
        {
            get { return txtTitle.Text; }
            set { txtTitle.Text = value; }
        }

        public string GameHost
        {
            get { return txtHost.Text; }
            set { txtHost.Text = value; }
        }

        public string GameMemo
        {
            get { return txtMemo.Text; }
            set { txtMemo.Text = value; }
        }

        public DateTime GameDate
        {
            get { return dtpGameDate.Value; }
            set { dtpGameDate.Value = value; }
        }

        public int GameType
        {
            get { return ((KeyValuePair<int, string>)cmbGameType.SelectedItem).Key; }
            set
            {
                int index = cmbGameType.Items.Cast<KeyValuePair<int, string>>().ToList().FindIndex(item => item.Key == value);
                if (index >= 0)
                {
                    cmbGameType.SelectedIndex = index;
                }
            }
        }

        public event EventHandler SaveEvent;
        public event EventHandler CloseEvenvt;
    }
}
