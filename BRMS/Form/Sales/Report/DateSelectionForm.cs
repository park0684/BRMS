using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BRMS
{
    public partial class DateSelectionForm : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        int accessedEmp = cUserSession.AccessedEmp;
        DataTable resultData = new DataTable();
        TableLayoutPanel tableLayoutCalendar = new TableLayoutPanel();
        DateTime monthDate = DateTime.Now;
        public DateSelectionForm()
        {
            InitializeComponent();
            //DataGridFrom();
            LabelDateSet();
            ObjectUISet();

        }       
        private void ObjectUISet()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button button) // 버튼인 경우만 처리
                {
                    button.BackColor = cUIManager.Color.Blue; // 버튼 배경색
                    button.ForeColor = Color.White;  // 버튼 텍스트 색상
                }
            }
        }
        private void LabelDateSet()
        {
            string yearString = monthDate.ToString("yyyy");
            string monthString = monthDate.ToString("MM");
            lblMonthDate.Text = $"{yearString}년{monthString}월";
            SeachWorkTableQuery();
            CreateCalendar();
            
        }
        private void SeachWorkTableQuery()
        {
            string workStart = monthDate.ToString("yyyyMM");
            string workEnd = monthDate.AddMonths(1).ToString("yyyyMM");
            string query = $"SELECT work_date, work_saleupdate, work_purupdate FROM worktable WHERE work_date >= {workStart}01 AND work_date < {workEnd}01 AND work_type = 101";
            dbconn.SqlDataAdapterQuery(query, resultData);
        }
        
        private void CreateCalendar()
        {
            
            pnlDataGrid.Controls.Clear();
            tableLayoutCalendar.Controls.Clear();
            tableLayoutCalendar.Dock = DockStyle.Fill;
            tableLayoutCalendar.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutCalendar.BackColor = Color.White;

            // 첫 번째 행에 일요일부터 토요일까지 설정
            string[] weekdays = { "일요일", "월요일", "화요일", "수요일", "목요일", "금요일", "토요일" };

            tableLayoutCalendar.RowCount = 1;  // 첫 번째 행
            tableLayoutCalendar.ColumnCount = 7;  // 7열 (일요일부터 토요일까지)

            // 첫 번째 행에 요일 라벨 추가
            for (int i = 0; i < weekdays.Length; i++)
            {
                Label lblDay = new Label();
                lblDay.Text = weekdays[i];
                lblDay.TextAlign = ContentAlignment.MiddleCenter;
                lblDay.Dock = DockStyle.Fill;  // 각 셀이 꽉 차도록 설정
                lblDay.Margin = new Padding(0);
                lblDay.BackColor = cUIManager.Color.LightYellow;
                tableLayoutCalendar.Controls.Add(lblDay, i, 0);  // 첫 번째 행에 요일 이름 추가
            }

            // 첫 번째 행의 높이를 35로 고정
            tableLayoutCalendar.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));

            // 2. 날짜를 추가
            DateTime firstDayOfMonth = new DateTime(monthDate.Year, monthDate.Month, 1);
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;  // 첫 번째 날의 요일 (일요일: 0, 월요일: 1, ...)
            int daysInMonth = DateTime.DaysInMonth(monthDate.Year, monthDate.Month);

            // 빈 셀 처리 (시작하는 날짜가 월요일이 아닐 경우 빈 셀을 추가)
            for (int i = 0; i < startDayOfWeek; i++)
            {
                tableLayoutCalendar.Controls.Add(new Label(), i, 1);  // 빈 라벨 추가
            }

            // 날짜 셀 추가 (날짜 체크박스와 라벨을 함께 추가)
            int totalCells = startDayOfWeek + daysInMonth;
            int rowCount = (int)Math.Ceiling((double)totalCells / 7);  // 행 수 계산

            // 이후의 행에 대해 높이를 고정
            int remainingHeight = pnlDataGrid.Height - 35;  // 첫 번째 행을 제외한 나머지 패널 높이
            int rowHeight = remainingHeight / rowCount;  // 나머지 공간을 균등하게 나누기

            for (int i = 1; i <= rowCount; i++)
            {
                tableLayoutCalendar.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
            }
            // 각 행의 높이를 균등하게 나누기
            for (int i = 1; i <= rowCount; i++)
            {
                tableLayoutCalendar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowCount));  // 각 행의 높이를 균등하게 나누기
            }

            // 각 열의 너비를 균등하게 나누기
            for (int i = 0; i < 7; i++)
            {
                tableLayoutCalendar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / 7));  // 각 열의 너비를 균등하게 나누기
            }
            // 날짜 체크박스와 라벨 추가
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDate = new DateTime(monthDate.Year, monthDate.Month, day);
                string curretDay = currentDate.ToString("yyyyMMdd");
                // 날짜를 표시할 셀을 만들기 위해 Panel 사용
                Panel dateCell = new Panel();
                int workStstus = 0;

                DataRow[] matchedRows = resultData.Select($"work_date = '{curretDay}'");
                // work_date가 없는 경우
                if (matchedRows.Length == 0)
                {
                    workStstus = 0;
                }
                foreach (DataRow row in matchedRows)
                {
                    int saleUpdate = row.Field<int>("work_saleupdate");
                    int purUpdate = row.Field<int>("work_purupdate");
                    workStstus = 1;
                    if (saleUpdate == 1 || purUpdate == 1)
                    {
                        workStstus = 2; // 하나라도 1이면 2 반환
                    }
                }

                if (currentDate >= DateTime.Today)
                {
                    workStstus = -1;
                }

                // 체크박스 추가
                CheckBox chkDate = new CheckBox();
                chkDate.Name = "chkDate_" + currentDate.ToString("yyyyMMdd");
                chkDate.Checked = false;  // 기본 상태는 체크되지 않음
                chkDate.Dock = DockStyle.Bottom;  // 위에 배치
                chkDate.Tag = workStstus;
                dateCell.Controls.Add(chkDate);
                // 라벨 추가 (날짜의 상세 정보)
                Label lblDate = new Label();
                lblDate.Text = currentDate.Day.ToString();
                lblDate.TextAlign = ContentAlignment.TopRight;
                lblDate.Dock = DockStyle.Top;  // 셀 안에 꽉 차도록
                dateCell.Controls.Add(lblDate);

                switch ((startDayOfWeek + day - 1) % 7)
                {
                    case 0:
                        lblDate.ForeColor = Color.Red;
                        break;
                    case 6:
                        lblDate.ForeColor = Color.Blue;
                        break;
                }

                switch (workStstus)
                {
                    case -1:
                        chkDate.Visible = false;
                        dateCell.BackColor = Color.White;
                        break;
                    case 0:
                        chkDate.Visible = true;
                        chkDate.Checked = true;
                        dateCell.BackColor = Color.Ivory;
                        break;
                    case 1:
                        chkDate.Visible = true;
                        chkDate.Checked = false;
                        dateCell.BackColor = Color.DarkSeaGreen;
                        break;
                    case 2:
                        chkDate.Visible = true;
                        chkDate.Checked = true;
                        dateCell.BackColor = cUIManager.Color.Purple;
                        break;
                }
                // 날짜 셀을 TableLayoutPanel에 추가
                tableLayoutCalendar.Controls.Add(dateCell, (startDayOfWeek + day - 1) % 7, (startDayOfWeek + day - 1) / 7 + 1);
            }

            // 3. pnlDataGrid에 TableLayoutPanel 추가
            pnlDataGrid.Controls.Add(tableLayoutCalendar);

        }

        private void CheckBoxControl(bool status)
        {
            foreach (Control control in tableLayoutCalendar.Controls)
            {
                // 각 Control이 Panel인지 확인
                if (control is Panel dateCell)
                {
                    // Panel 내부의 CheckBox를 확인
                    foreach (Control panelControl in dateCell.Controls)
                    {
                        if (panelControl is CheckBox checkBox)
                        {
                            checkBox.Checked = status;  // CheckBox를 체크 상태로 변경
                        }
                    }
                }
            }
        }

        private void RunReportWirte()
        {
            List<string> workDate = new List<string>();
            bool registed = false;
            foreach (Control control in tableLayoutCalendar.Controls)
            {
                // 각 Control이 Panel인지 확인
                if (control is Panel dateCell)
                {
                    // Panel 내부의 CheckBox를 확인
                    foreach (Control panelControl in dateCell.Controls)
                    {
                        if (panelControl is CheckBox checkBox)
                        {
                            if (checkBox.Checked == true)
                            {
                                string checkDate = checkBox.Name.Replace("chkDate_", "");
                                //태그의 값이 2라면 재실행 대상으로 결산내역이 있음을 알리는 목적으로 사용
                                if(checkBox.Tag.Equals(2))
                                {
                                    registed = true;
                                }
                                workDate.Add(checkDate);
                            }
                        }
                    }
                }
            }
            if (registed == true)
            {
                DialogResult result = cUIManager.ShowMessageBox("이미 결산이 완료되었습니다.\n삭제 후 생성 하시겠습니까?", "알림", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            try
            {
                foreach (string date in workDate)
                {
                    ReportProgress.GetDailyDate = date;
                    ReportProgress repoft = new ReportProgress();
                    repoft.StartPosition = FormStartPosition.CenterScreen;

                    repoft.Show();
                    repoft.RunQeury();

                }
                cUIManager.ShowMessageBox("레포트 생성 작업 완료", "알림", MessageBoxButtons.OK);
                foreach (Control control in this.Controls)
                {
                    if (control is Button button) // 버튼인 경우만 처리
                    {
                        button.Enabled = false;
                        button.BackColor = cUIManager.Color.Gray60;
                    }
                }
                btnCancle.Enabled = true;
                btnCancle.BackColor = cUIManager.Color.Blue; // 버튼 배경색
                btnCancle.ForeColor = Color.White;  // 버튼 텍스트 색상
            }
            catch(Exception ex)
            {
                cUIManager.ShowMessageBox(ex.Message, "알림", MessageBoxButtons.OK);
                return;
            }
        }

        /// <summary>
        /// 선택된 날짜 작업 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcute_Click(object sender, EventArgs e)
        {
            try
            {
                RunReportWirte();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 실행 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 전체 날짜 체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            CheckBoxControl(true);
        }
        /// <summary>
        /// 모든 날짜 체크 해제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckCancle_Click(object sender, EventArgs e)
        {
            CheckBoxControl(false);
        }
        /// <summary>
        /// 실행되지 않은 날짜만 체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnExeCheck_Click(object sender, EventArgs e)
        {
            CheckBoxControl(false);
            foreach (Control control in tableLayoutCalendar.Controls)
            {
                // 각 Control이 Panel인지 확인
                if (control is Panel dateCell)
                {
                    // Panel 내부의 CheckBox를 확인
                    foreach (Control panelControl in dateCell.Controls)
                    {
                        if (panelControl is CheckBox checkBox)
                        {
                            // Tag 값이 "0" 또는 "2"인 경우에만 체크
                            if (checkBox.Tag != null && (checkBox.Tag.ToString() == "0" || checkBox.Tag.ToString() == "2"))
                            {
                                checkBox.Checked = true;  // CheckBox를 체크 상태로 변경
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 현재 선택된 날짜에서 1개월 이전으로 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDateLeft_Click(object sender, EventArgs e)
        {
            monthDate = monthDate.AddMonths(-1);
            LabelDateSet();
        }
        /// <summary>
        /// 햔제 선택된 날짜에서 1개월 이후로 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDateRight_Click(object sender, EventArgs e)
        {
            monthDate = monthDate.AddMonths(1);
            LabelDateSet();
        }
    }
}
