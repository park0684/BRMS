using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BRMS
{
    class cUIManager
    {
        public static DialogResult ShowMessageBox(string message, string caption, MessageBoxButtons buttons)
        {
            return CreatMessageBox(message, caption, buttons);
        }
        /// <summary>
        /// 메시지 박스 생성 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        private static DialogResult CreatMessageBox(string message, string caption, MessageBoxButtons buttons)
        {
            // 사용자 정의 메시지 박스 생성
            using (Form messageBox = new Form())
            {
                messageBox.Text = caption;
                messageBox.StartPosition = FormStartPosition.CenterParent; // 중앙 위치 설정
                messageBox.ClientSize = new System.Drawing.Size(300, 150);
                messageBox.ControlBox = false;
                messageBox.BackColor = System.Drawing.Color.White;

                System.Drawing.SizeF textSize = TextRenderer.MeasureText(message, new System.Drawing.Font("맑은 고딕", 9F));

                // 최소 크기와 글자 크기를 고려한 폼 크기 계산
                int width = Math.Max(300, (int)textSize.Width + 40); // 최소 너비: 300
                int height = Math.Max(150, (int)textSize.Height + 100); // 최소 높이: 150 (텍스트 높이 + 여백)

                messageBox.ClientSize = new System.Drawing.Size(width, height);
                // 메시지 레이블 설정
                Label lblMessage = new Label
                {
                    AutoSize = false,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter, // 중앙 정렬 설정
                    Dock = DockStyle.Fill, // DockFill로 설정하여 공간을 모두 차지하게 함
                    MaximumSize = new System.Drawing.Size(width - 20, 0), // 폼 너비에 맞게 최대 크기 설정
                    Text = message,
                    Font = new System.Drawing.Font("맑은 고딕", 9F)
                };
                messageBox.Controls.Add(lblMessage);

                // 버튼 패널 설정
                Panel pnlButton = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 50
                };
                messageBox.Controls.Add(pnlButton);

                // 버튼 생성
                CreateMessgeButtons(pnlButton, buttons, messageBox);

                return messageBox.ShowDialog(); // 대화 상자를 모달로 표시
            }
        }

        /// <summary>
        /// 메시지 버튼 생성 메서드
        /// </summary>
        /// <param name="pnlButton"></param>
        /// <param name="buttons"></param>
        /// <param name="messageBox"></param>
        private static void CreateMessgeButtons(Panel pnlButton, MessageBoxButtons buttons, Form messageBox)
        {
            // 버튼 패널 초기화
            pnlButton.Controls.Clear();
            // OK 버튼 생성
            Button btnOK = new Button
            {
                Text = "확인",
                DialogResult = DialogResult.OK
            };
            btnOK.Click += (sender, e) => { messageBox.Close(); };
            // Cancel 버튼 생성
            Button btnCancel = new Button
            {
                Text = "취소",
                DialogResult = DialogResult.Cancel
            };
            btnCancel.Click += (sender, e) => { messageBox.Close(); };
            // Yes 버튼 생성
            Button btnYes = new Button
            {
                Text = "예",
                DialogResult = DialogResult.Yes
            };
            btnYes.Click += (sender, e) => { messageBox.Close(); };
            // No 버튼 생성
            Button btnNo = new Button
            {
                Text = "아니요",
                DialogResult = DialogResult.No
            };
            btnNo.Click += (sender, e) => { messageBox.Close(); };
            // 버튼 배치 로직
            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10),
                AutoSize = true
            };
            // MessageBoxButtons에 따른 버튼 추가
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    panel.Controls.Add(btnOK);
                    break;
                case MessageBoxButtons.OKCancel:
                    panel.Controls.Add(btnCancel);
                    panel.Controls.Add(btnOK);
                    break;
                case MessageBoxButtons.YesNo:
                    panel.Controls.Add(btnNo);
                    panel.Controls.Add(btnYes);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    panel.Controls.Add(btnCancel);
                    panel.Controls.Add(btnNo);
                    panel.Controls.Add(btnYes);
                    break;
            }
            pnlButton.Controls.Add(panel);
        }
        /// <summary>
        /// 지정 색상
        /// </summary>
        public static class Color
        {
            public static readonly System.Drawing.Color Blue = System.Drawing.Color.FromArgb(81, 161, 243);
            public static readonly System.Drawing.Color Green = System.Drawing.Color.FromArgb(73, 173, 8);
            public static readonly System.Drawing.Color Orange = System.Drawing.Color.FromArgb(240, 144, 1);
            public static readonly System.Drawing.Color Red = System.Drawing.Color.FromArgb(239, 61, 86);
            public static readonly System.Drawing.Color LightGreen = System.Drawing.Color.FromArgb(181, 230, 162);
            public static readonly System.Drawing.Color LightYellow = System.Drawing.Color.FromArgb(255, 235, 156);//255, 235, 156
            public static readonly System.Drawing.Color Gray60 = System.Drawing.Color.FromArgb(153, 153, 153);
            public static readonly System.Drawing.Color DarkNavy = System.Drawing.Color.FromArgb(21, 96, 130);
            public static readonly System.Drawing.Color GreenGray = System.Drawing.Color.FromArgb(192, 200, 192);
            public static readonly System.Drawing.Color White = System.Drawing.Color.FromArgb(255, 255, 255);
            public static readonly System.Drawing.Color Black = System.Drawing.Color.FromArgb(0, 0, 0); 
            public static readonly System.Drawing.Color Purple = System.Drawing.Color.FromArgb(104, 46, 155);
            public static readonly System.Drawing.Color BrightGray = System.Drawing.Color.FromArgb(234, 238, 244);
            public static readonly System.Drawing.Color PastelLightBlue = System.Drawing.Color.FromArgb(206, 229, 237);
            public static readonly System.Drawing.Color PastelGreen = System.Drawing.Color.FromArgb(179, 207, 153);
            public static readonly System.Drawing.Color PastelRose = System.Drawing.Color.FromArgb(246, 184, 208);
        }
        /// <summary>
        /// 상세내역 과 같이 메인폼이 아닌 팝업 형식의 폼에 적용
        /// </summary>
        /// <param name="form"></param>
        public static void ApplyPopupFormStyleㅡ(Form form)
        {
            if (form == null) return;

            // 폼 속성 설정
            form.ControlBox = false;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.BackColor = System.Drawing.Color.White;
            form.MaximizeBox = false;
            form.ControlBox = false;
        }

        public static UserControl CalenderUI(string date, int day, bool check, int status, int week)
        {
            // CustomCalendarCell 생성
            var cell = new UserControl
            {
                Size = new Size(60, 60),
                BackColor = Color.White,
                Name = date.ToString(),
                Tag = status
            };
            switch(status)
            {
                case 0:
                    cell.BackColor = Color.Orange;
                    break;
                case 1:
                    cell.BackColor = Color.GreenGray;
                    break;
                case 2:
                    cell.BackColor = Color.Purple;
                    break;
            }
            // 체크박스 추가
            var checkBox = new CheckBox
            {
                
                Location = new Point(5, 5),
                AutoSize = true
            };
            if (status == -1)
            {
                checkBox.Visible = false;
            }
            checkBox.Checked = check;
            cell.Controls.Add(checkBox);

            // 날짜 입력 필드 추가
            var lblDay = new Label
            {
                Location = new Point(25, 5),
                Width = 50
            };
            lblDay.Text = day.ToString();
            switch(week)
            {
                case 0:
                    lblDay.ForeColor = Color.Red;
                    break;
                case 6:
                    lblDay.ForeColor = Color.Blue;
                    break;
                default:
                    lblDay.ForeColor = Color.Black;
                    break;
            }
            
            cell.Controls.Add(lblDay);

            return cell;
        }
        
    }
}
