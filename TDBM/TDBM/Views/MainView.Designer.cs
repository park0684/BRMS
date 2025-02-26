
namespace TDBM.Views
{
    partial class MainView
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlTopmenu = new System.Windows.Forms.Panel();
            this.pnlSidemenu = new System.Windows.Forms.Panel();
            this.pnlView = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlTopmenu
            // 
            this.pnlTopmenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopmenu.Location = new System.Drawing.Point(0, 0);
            this.pnlTopmenu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTopmenu.Name = "pnlTopmenu";
            this.pnlTopmenu.Size = new System.Drawing.Size(1008, 70);
            this.pnlTopmenu.TabIndex = 0;
            // 
            // pnlSidemenu
            // 
            this.pnlSidemenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidemenu.Location = new System.Drawing.Point(0, 70);
            this.pnlSidemenu.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlSidemenu.Name = "pnlSidemenu";
            this.pnlSidemenu.Size = new System.Drawing.Size(150, 659);
            this.pnlSidemenu.TabIndex = 1;
            // 
            // pnlView
            // 
            this.pnlView.BackColor = System.Drawing.Color.Transparent;
            this.pnlView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlView.Location = new System.Drawing.Point(150, 70);
            this.pnlView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlView.Name = "pnlView";
            this.pnlView.Size = new System.Drawing.Size(858, 659);
            this.pnlView.TabIndex = 2;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.pnlView);
            this.Controls.Add(this.pnlSidemenu);
            this.Controls.Add(this.pnlTopmenu);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopmenu;
        private System.Windows.Forms.Panel pnlSidemenu;
        private System.Windows.Forms.Panel pnlView;
    }
}

