
namespace ClubManagement.Games.Views
{
    partial class MatchListView
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
            this.pnlGameDataGrid = new System.Windows.Forms.Panel();
            this.btnAddGame = new System.Windows.Forms.Button();
            this.btnGameListEdit = new System.Windows.Forms.Button();
            this.pnlPlayerDataGrid = new System.Windows.Forms.Panel();
            this.btnPalyerListEdit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkExclude = new System.Windows.Forms.CheckBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.btnPost = new System.Windows.Forms.Button();
            this.btnPre = new System.Windows.Forms.Button();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblGameTitle = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlGameDataGrid
            // 
            this.pnlGameDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGameDataGrid.Location = new System.Drawing.Point(6, 52);
            this.pnlGameDataGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlGameDataGrid.Name = "pnlGameDataGrid";
            this.pnlGameDataGrid.Size = new System.Drawing.Size(690, 127);
            this.pnlGameDataGrid.TabIndex = 0;
            // 
            // btnAddGame
            // 
            this.btnAddGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddGame.BackColor = System.Drawing.Color.Black;
            this.btnAddGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddGame.ForeColor = System.Drawing.Color.White;
            this.btnAddGame.Location = new System.Drawing.Point(621, 14);
            this.btnAddGame.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddGame.Name = "btnAddGame";
            this.btnAddGame.Size = new System.Drawing.Size(75, 30);
            this.btnAddGame.TabIndex = 1;
            this.btnAddGame.Text = "모임 등록";
            this.btnAddGame.UseVisualStyleBackColor = false;
            // 
            // btnGameListEdit
            // 
            this.btnGameListEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGameListEdit.BackColor = System.Drawing.Color.Black;
            this.btnGameListEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGameListEdit.ForeColor = System.Drawing.Color.White;
            this.btnGameListEdit.Location = new System.Drawing.Point(540, 14);
            this.btnGameListEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGameListEdit.Name = "btnGameListEdit";
            this.btnGameListEdit.Size = new System.Drawing.Size(75, 30);
            this.btnGameListEdit.TabIndex = 1;
            this.btnGameListEdit.Text = "수정";
            this.btnGameListEdit.UseVisualStyleBackColor = false;
            // 
            // pnlPlayerDataGrid
            // 
            this.pnlPlayerDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPlayerDataGrid.Location = new System.Drawing.Point(6, 52);
            this.pnlPlayerDataGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlPlayerDataGrid.Name = "pnlPlayerDataGrid";
            this.pnlPlayerDataGrid.Size = new System.Drawing.Size(690, 249);
            this.pnlPlayerDataGrid.TabIndex = 0;
            // 
            // btnPalyerListEdit
            // 
            this.btnPalyerListEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPalyerListEdit.BackColor = System.Drawing.Color.Black;
            this.btnPalyerListEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPalyerListEdit.ForeColor = System.Drawing.Color.White;
            this.btnPalyerListEdit.Location = new System.Drawing.Point(621, 14);
            this.btnPalyerListEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPalyerListEdit.Name = "btnPalyerListEdit";
            this.btnPalyerListEdit.Size = new System.Drawing.Size(75, 30);
            this.btnPalyerListEdit.TabIndex = 1;
            this.btnPalyerListEdit.Text = "수정";
            this.btnPalyerListEdit.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkExclude);
            this.groupBox1.Controls.Add(this.cmbType);
            this.groupBox1.Controls.Add(this.btnPost);
            this.groupBox1.Controls.Add(this.btnPre);
            this.groupBox1.Controls.Add(this.dtpToDate);
            this.groupBox1.Controls.Add(this.dtpFromDate);
            this.groupBox1.Controls.Add(this.btnGameListEdit);
            this.groupBox1.Controls.Add(this.btnAddGame);
            this.groupBox1.Controls.Add(this.pnlGameDataGrid);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(702, 186);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // chkExclude
            // 
            this.chkExclude.AutoSize = true;
            this.chkExclude.Location = new System.Drawing.Point(441, 19);
            this.chkExclude.Name = "chkExclude";
            this.chkExclude.Size = new System.Drawing.Size(50, 19);
            this.chkExclude.TabIndex = 5;
            this.chkExclude.Text = "제외";
            this.chkExclude.UseVisualStyleBackColor = true;
            // 
            // cmbType
            // 
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(314, 17);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(121, 23);
            this.cmbType.TabIndex = 4;
            // 
            // btnPost
            // 
            this.btnPost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPost.Location = new System.Drawing.Point(256, 14);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(24, 30);
            this.btnPost.TabIndex = 3;
            this.btnPost.Text = "▷";
            this.btnPost.UseVisualStyleBackColor = true;
            // 
            // btnPre
            // 
            this.btnPre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPre.Location = new System.Drawing.Point(6, 14);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(24, 30);
            this.btnPre.TabIndex = 3;
            this.btnPre.Text = "◁";
            this.btnPre.UseVisualStyleBackColor = true;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(151, 17);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(99, 23);
            this.dtpToDate.TabIndex = 2;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(36, 17);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(99, 23);
            this.dtpFromDate.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblGameTitle);
            this.groupBox2.Controls.Add(this.btnPalyerListEdit);
            this.groupBox2.Controls.Add(this.pnlPlayerDataGrid);
            this.groupBox2.Location = new System.Drawing.Point(12, 207);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(702, 308);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // lblGameTitle
            // 
            this.lblGameTitle.AutoSize = true;
            this.lblGameTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblGameTitle.Location = new System.Drawing.Point(6, 17);
            this.lblGameTitle.Name = "lblGameTitle";
            this.lblGameTitle.Size = new System.Drawing.Size(74, 21);
            this.lblGameTitle.TabIndex = 2;
            this.lblGameTitle.Text = "모임이름";
            // 
            // GameListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(718, 527);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "GameListView";
            this.Text = "GameList";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlGameDataGrid;
        private System.Windows.Forms.Button btnAddGame;
        private System.Windows.Forms.Button btnGameListEdit;
        private System.Windows.Forms.Panel pnlPlayerDataGrid;
        private System.Windows.Forms.Button btnPalyerListEdit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblGameTitle;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.CheckBox chkExclude;
        private System.Windows.Forms.ComboBox cmbType;
    }
}