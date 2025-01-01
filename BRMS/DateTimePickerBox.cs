using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRMS
{
    public partial class DateTimePickerBox : Form
    {
        public event Action<DateTime> DateTiemPick;

        public DateTimePickerBox()
        {
            InitializeComponent();
            ComboBoxSetTime();

        }
        /// <summary>
        /// 콤보박스내 시간 데이터 입력
        /// </summary>
        private void ComboBoxSetTime()
        {
            cBoxHour.DropDownStyle = ComboBoxStyle.DropDownList;
            cBoxMinute.DropDownStyle = ComboBoxStyle.DropDownList;
            
            // cBoxHour에 00시부터 24시까지 추가
            for (int i = 0; i <= 24; i++)
            {
                cBoxHour.Items.Add(i.ToString("D2"));
            }
            
            // cBoxMinit에 00분부터 59분까지 추가
            for (int i = 0; i < 60; i++)
            {
                cBoxMinute.Items.Add(i.ToString("D2"));
            }
        }
        /// <summary>
        /// 시간 입력 여부에 따라 시간 콤보 박스 활성화 여부 설정
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="useTime"></param>
        public void GetDateTime(DateTime dateTime,bool useTime)
        {
            dtpBox.Value = dateTime;
            if(useTime == true)
            {
                cBoxHour.SelectedItem = dateTime.Hour.ToString("D2");
                cBoxMinute.SelectedItem = dateTime.Minute.ToString("D2");
            }
            else
            {
                cBoxHour.Visible = false;
                cBoxMinute.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                cBoxHour.SelectedIndex = 0;
                cBoxMinute.SelectedIndex = 0;
            }
            
        }
        /// <summary>
        /// 확인 버튼 클릭
        /// 이벤트를 통해 설정 시간 전달
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntOk_Click(object sender, EventArgs e)
        {
            DateTime getDatetime = new DateTime( 
                dtpBox.Value.Year, 
                dtpBox.Value.Month, 
                dtpBox.Value.Day, 
                int.Parse(cBoxHour.SelectedItem.ToString()), 
                int.Parse(cBoxMinute.SelectedItem.ToString()),
                0);

            DateTiemPick?.Invoke(getDatetime);
            Close();
        }

        private void bntClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
