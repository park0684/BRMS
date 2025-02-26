
namespace BRM.Views
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHead = new System.Windows.Forms.Panel();
            this.btnConfig = new System.Windows.Forms.Button();
            this.btnDatabaseConfig = new System.Windows.Forms.Button();
            this.pnlSideMenu = new System.Windows.Forms.Panel();
            this.pnllMenuTitle = new System.Windows.Forms.Panel();
            this.lblMenuTitle = new System.Windows.Forms.Label();
            this.pnllViewer = new System.Windows.Forms.Panel();
            this.pnlHead.SuspendLayout();
            this.pnlSideMenu.SuspendLayout();
            this.pnllMenuTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(58)))));
            this.pnlHead.Controls.Add(this.btnConfig);
            this.pnlHead.Controls.Add(this.btnDatabaseConfig);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(1008, 60);
            this.pnlHead.TabIndex = 1;
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
            // 
            // btnDatabaseConfig
            // 
            this.btnDatabaseConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDatabaseConfig.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnDatabaseConfig.Location = new System.Drawing.Point(932, 4);
            this.btnDatabaseConfig.Name = "btnDatabaseConfig";
            this.btnDatabaseConfig.Size = new System.Drawing.Size(64, 50);
            this.btnDatabaseConfig.TabIndex = 0;
            this.btnDatabaseConfig.Text = "연결설정";
            this.btnDatabaseConfig.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDatabaseConfig.UseVisualStyleBackColor = true;
            // 
            // pnlSideMenu
            // 
            this.pnlSideMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(58)))));
            this.pnlSideMenu.Controls.Add(this.pnllMenuTitle);
            this.pnlSideMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSideMenu.Location = new System.Drawing.Point(0, 60);
            this.pnlSideMenu.Name = "pnlSideMenu";
            this.pnlSideMenu.Size = new System.Drawing.Size(150, 669);
            this.pnlSideMenu.TabIndex = 2;
            // 
            // pnllMenuTitle
            // 
            this.pnllMenuTitle.BackColor = System.Drawing.Color.White;
            this.pnllMenuTitle.Controls.Add(this.lblMenuTitle);
            this.pnllMenuTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnllMenuTitle.Location = new System.Drawing.Point(0, 0);
            this.pnllMenuTitle.Name = "pnllMenuTitle";
            this.pnllMenuTitle.Size = new System.Drawing.Size(150, 40);
            this.pnllMenuTitle.TabIndex = 0;
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
            // pnllViewer
            // 
            this.pnllViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnllViewer.Location = new System.Drawing.Point(150, 60);
            this.pnllViewer.Name = "pnllViewer";
            this.pnllViewer.Size = new System.Drawing.Size(858, 669);
            this.pnllViewer.TabIndex = 4;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.pnllViewer);
            this.Controls.Add(this.pnlSideMenu);
            this.Controls.Add(this.pnlHead);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainView";
            this.Text = "MainView";
            this.pnlHead.ResumeLayout(false);
            this.pnlSideMenu.ResumeLayout(false);
            this.pnllMenuTitle.ResumeLayout(false);
            this.pnllMenuTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Button btnDatabaseConfig;
        private System.Windows.Forms.Panel pnlSideMenu;
        private System.Windows.Forms.Panel pnllMenuTitle;
        private System.Windows.Forms.Label lblMenuTitle;
        private System.Windows.Forms.Panel pnllViewer;
    }
}