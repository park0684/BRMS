
namespace BRMS
{
    partial class DateSelectionForm
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
            this.pnlDataGrid = new System.Windows.Forms.Panel();
            this.btnExcute = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnAllCheck = new System.Windows.Forms.Button();
            this.btnCheckCancle = new System.Windows.Forms.Button();
            this.btnUnExeCheck = new System.Windows.Forms.Button();
            this.btnDateLeft = new System.Windows.Forms.Button();
            this.btnDateLight = new System.Windows.Forms.Button();
            this.lblMonthDate = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDataGrid
            // 
            this.pnlDataGrid.Location = new System.Drawing.Point(12, 45);
            this.pnlDataGrid.Name = "pnlDataGrid";
            this.pnlDataGrid.Size = new System.Drawing.Size(560, 340);
            this.pnlDataGrid.TabIndex = 0;
            // 
            // btnExcute
            // 
            this.btnExcute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcute.FlatAppearance.BorderSize = 0;
            this.btnExcute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcute.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnExcute.Location = new System.Drawing.Point(409, 404);
            this.btnExcute.Margin = new System.Windows.Forms.Padding(0);
            this.btnExcute.Name = "btnExcute";
            this.btnExcute.Size = new System.Drawing.Size(80, 25);
            this.btnExcute.TabIndex = 1;
            this.btnExcute.Text = "실행";
            this.btnExcute.UseVisualStyleBackColor = true;
            this.btnExcute.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancle.FlatAppearance.BorderSize = 0;
            this.btnCancle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancle.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnCancle.Location = new System.Drawing.Point(492, 404);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(80, 25);
            this.btnCancle.TabIndex = 1;
            this.btnCancle.Text = "닫기";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnAllCheck
            // 
            this.btnAllCheck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllCheck.FlatAppearance.BorderSize = 0;
            this.btnAllCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAllCheck.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnAllCheck.Location = new System.Drawing.Point(12, 404);
            this.btnAllCheck.Name = "btnAllCheck";
            this.btnAllCheck.Size = new System.Drawing.Size(80, 25);
            this.btnAllCheck.TabIndex = 1;
            this.btnAllCheck.Text = "전체 선택";
            this.btnAllCheck.UseVisualStyleBackColor = true;
            this.btnAllCheck.Click += new System.EventHandler(this.btnAllCheck_Click);
            // 
            // btnCheckCancle
            // 
            this.btnCheckCancle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckCancle.FlatAppearance.BorderSize = 0;
            this.btnCheckCancle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckCancle.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnCheckCancle.Location = new System.Drawing.Point(98, 404);
            this.btnCheckCancle.Name = "btnCheckCancle";
            this.btnCheckCancle.Size = new System.Drawing.Size(80, 25);
            this.btnCheckCancle.TabIndex = 1;
            this.btnCheckCancle.Text = "전체 취소";
            this.btnCheckCancle.UseVisualStyleBackColor = true;
            this.btnCheckCancle.Click += new System.EventHandler(this.btnCheckCancle_Click);
            // 
            // btnUnExeCheck
            // 
            this.btnUnExeCheck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnExeCheck.FlatAppearance.BorderSize = 0;
            this.btnUnExeCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnExeCheck.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnUnExeCheck.Location = new System.Drawing.Point(184, 404);
            this.btnUnExeCheck.Name = "btnUnExeCheck";
            this.btnUnExeCheck.Size = new System.Drawing.Size(80, 25);
            this.btnUnExeCheck.TabIndex = 1;
            this.btnUnExeCheck.Text = "미실행 선택";
            this.btnUnExeCheck.UseVisualStyleBackColor = true;
            this.btnUnExeCheck.Click += new System.EventHandler(this.btnUnExeCheck_Click);
            // 
            // btnDateLeft
            // 
            this.btnDateLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDateLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDateLeft.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnDateLeft.Location = new System.Drawing.Point(0, 0);
            this.btnDateLeft.Margin = new System.Windows.Forms.Padding(0);
            this.btnDateLeft.Name = "btnDateLeft";
            this.btnDateLeft.Size = new System.Drawing.Size(25, 25);
            this.btnDateLeft.TabIndex = 3;
            this.btnDateLeft.Text = "◀";
            this.btnDateLeft.UseVisualStyleBackColor = true;
            this.btnDateLeft.Click += new System.EventHandler(this.btnDateLeft_Click);
            // 
            // btnDateLight
            // 
            this.btnDateLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDateLight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDateLight.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnDateLight.Location = new System.Drawing.Point(155, 0);
            this.btnDateLight.Margin = new System.Windows.Forms.Padding(0);
            this.btnDateLight.Name = "btnDateLight";
            this.btnDateLight.Size = new System.Drawing.Size(25, 25);
            this.btnDateLight.TabIndex = 3;
            this.btnDateLight.Text = "▶";
            this.btnDateLight.UseVisualStyleBackColor = true;
            this.btnDateLight.Click += new System.EventHandler(this.btnDateRight_Click);
            // 
            // lblMonthDate
            // 
            this.lblMonthDate.AutoSize = true;
            this.lblMonthDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMonthDate.Font = new System.Drawing.Font("맑은 고딕", 12.5F);
            this.lblMonthDate.Location = new System.Drawing.Point(25, 0);
            this.lblMonthDate.Margin = new System.Windows.Forms.Padding(0);
            this.lblMonthDate.Name = "lblMonthDate";
            this.lblMonthDate.Size = new System.Drawing.Size(130, 25);
            this.lblMonthDate.TabIndex = 4;
            this.lblMonthDate.Text = "label1";
            this.lblMonthDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Controls.Add(this.btnDateLeft, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDateLight, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblMonthDate, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(360, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(180, 25);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // DateSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnUnExeCheck);
            this.Controls.Add(this.btnCheckCancle);
            this.Controls.Add(this.btnAllCheck);
            this.Controls.Add(this.btnExcute);
            this.Controls.Add(this.pnlDataGrid);
            this.Name = "DateSelectionForm";
            this.Text = "일결산 생성";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlDataGrid;
        private System.Windows.Forms.Button btnExcute;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnAllCheck;
        private System.Windows.Forms.Button btnCheckCancle;
        private System.Windows.Forms.Button btnUnExeCheck;
        private System.Windows.Forms.Button btnDateLeft;
        private System.Windows.Forms.Button btnDateLight;
        private System.Windows.Forms.Label lblMonthDate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}