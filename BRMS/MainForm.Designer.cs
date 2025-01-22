
namespace BRMS
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelHead = new System.Windows.Forms.Panel();
            this.btnDatabaseConnect = new System.Windows.Forms.Button();
            this.panelSideMenu = new System.Windows.Forms.Panel();
            this.panelMenuTitle = new System.Windows.Forms.Panel();
            this.lblMenuTitle = new System.Windows.Forms.Label();
            this.panelControlMenu = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.bntSearch = new System.Windows.Forms.Button();
            this.panelViewer = new System.Windows.Forms.Panel();
            this.btnConfig = new System.Windows.Forms.Button();
            this.panelHead.SuspendLayout();
            this.panelSideMenu.SuspendLayout();
            this.panelMenuTitle.SuspendLayout();
            this.panelControlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHead
            // 
            this.panelHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(58)))));
            this.panelHead.Controls.Add(this.btnConfig);
            this.panelHead.Controls.Add(this.btnDatabaseConnect);
            this.panelHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHead.Location = new System.Drawing.Point(0, 0);
            this.panelHead.Name = "panelHead";
            this.panelHead.Size = new System.Drawing.Size(1008, 60);
            this.panelHead.TabIndex = 0;
            // 
            // btnDatabaseConnect
            // 
            this.btnDatabaseConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDatabaseConnect.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnDatabaseConnect.Location = new System.Drawing.Point(932, 4);
            this.btnDatabaseConnect.Name = "btnDatabaseConnect";
            this.btnDatabaseConnect.Size = new System.Drawing.Size(64, 50);
            this.btnDatabaseConnect.TabIndex = 0;
            this.btnDatabaseConnect.Text = "연결설정";
            this.btnDatabaseConnect.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDatabaseConnect.UseVisualStyleBackColor = true;
            this.btnDatabaseConnect.Click += new System.EventHandler(this.btnDatabaseConnect_Click);
            // 
            // panelSideMenu
            // 
            this.panelSideMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(58)))));
            this.panelSideMenu.Controls.Add(this.panelMenuTitle);
            this.panelSideMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSideMenu.Location = new System.Drawing.Point(0, 60);
            this.panelSideMenu.Name = "panelSideMenu";
            this.panelSideMenu.Size = new System.Drawing.Size(150, 669);
            this.panelSideMenu.TabIndex = 1;
            // 
            // panelMenuTitle
            // 
            this.panelMenuTitle.BackColor = System.Drawing.Color.White;
            this.panelMenuTitle.Controls.Add(this.lblMenuTitle);
            this.panelMenuTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMenuTitle.Location = new System.Drawing.Point(0, 0);
            this.panelMenuTitle.Name = "panelMenuTitle";
            this.panelMenuTitle.Size = new System.Drawing.Size(150, 40);
            this.panelMenuTitle.TabIndex = 0;
            // 
            // lblMenuTitle
            // 
            this.lblMenuTitle.AutoSize = true;
            this.lblMenuTitle.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.lblMenuTitle.Location = new System.Drawing.Point(3, 7);
            this.lblMenuTitle.Name = "lblMenuTitle";
            this.lblMenuTitle.Size = new System.Drawing.Size(42, 21);
            this.lblMenuTitle.TabIndex = 0;
            this.lblMenuTitle.Text = "메뉴";
            this.lblMenuTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelControlMenu
            // 
            this.panelControlMenu.BackColor = System.Drawing.Color.White;
            this.panelControlMenu.Controls.Add(this.btnPrint);
            this.panelControlMenu.Controls.Add(this.btnExportExcel);
            this.panelControlMenu.Controls.Add(this.bntSearch);
            this.panelControlMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlMenu.Location = new System.Drawing.Point(150, 60);
            this.panelControlMenu.Name = "panelControlMenu";
            this.panelControlMenu.Size = new System.Drawing.Size(858, 40);
            this.panelControlMenu.TabIndex = 2;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(772, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(34, 34);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportExcel.Location = new System.Drawing.Point(812, 3);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(34, 34);
            this.btnExportExcel.TabIndex = 1;
            this.btnExportExcel.Text = "엑셀";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // bntSearch
            // 
            this.bntSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.bntSearch.Image = ((System.Drawing.Image)(resources.GetObject("bntSearch.Image")));
            this.bntSearch.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bntSearch.Location = new System.Drawing.Point(356, 0);
            this.bntSearch.Name = "bntSearch";
            this.bntSearch.Size = new System.Drawing.Size(59, 40);
            this.bntSearch.TabIndex = 0;
            this.bntSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.bntSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bntSearch.UseVisualStyleBackColor = true;
            this.bntSearch.Click += new System.EventHandler(this.bntSearch_Click);
            // 
            // panelViewer
            // 
            this.panelViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelViewer.Location = new System.Drawing.Point(150, 100);
            this.panelViewer.Name = "panelViewer";
            this.panelViewer.Size = new System.Drawing.Size(858, 629);
            this.panelViewer.TabIndex = 3;
            // 
            // btnConfig
            // 
            this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfig.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnConfig.Location = new System.Drawing.Point(862, 4);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(64, 50);
            this.btnConfig.TabIndex = 0;
            this.btnConfig.Text = "환경설정";
            this.btnConfig.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.panelViewer);
            this.Controls.Add(this.panelControlMenu);
            this.Controls.Add(this.panelSideMenu);
            this.Controls.Add(this.panelHead);
            this.Name = "MainForm";
            this.Text = "BRMS";
            this.panelHead.ResumeLayout(false);
            this.panelSideMenu.ResumeLayout(false);
            this.panelMenuTitle.ResumeLayout(false);
            this.panelMenuTitle.PerformLayout();
            this.panelControlMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHead;
        private System.Windows.Forms.Panel panelSideMenu;
        private System.Windows.Forms.Panel panelMenuTitle;
        private System.Windows.Forms.Panel panelControlMenu;
        private System.Windows.Forms.Panel panelViewer;
        private System.Windows.Forms.Button bntSearch;
        private System.Windows.Forms.Button btnDatabaseConnect;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblMenuTitle;
        private System.Windows.Forms.Button btnConfig;
    }
}

