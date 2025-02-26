
namespace BRM.Views
{
    partial class ProductListView
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCategory = new System.Windows.Forms.Label();
            this.panelDataGrid = new System.Windows.Forms.Panel();
            this.cBoxDateType1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cBoxDateType2 = new System.Windows.Forms.ComboBox();
            this.dtpType1From = new System.Windows.Forms.DateTimePicker();
            this.dtpType1To = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cBoxStatus = new System.Windows.Forms.ComboBox();
            this.txtSearchWord = new System.Windows.Forms.TextBox();
            this.dtpType2From = new System.Windows.Forms.DateTimePicker();
            this.dtpType2To = new System.Windows.Forms.DateTimePicker();
            this.groupBoxDatePikc = new System.Windows.Forms.GroupBox();
            this.btnCategory = new System.Windows.Forms.Button();
            this.btnProductAdd = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBoxDatePikc.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblCategory.Location = new System.Drawing.Point(69, 7);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(45, 19);
            this.lblCategory.TabIndex = 25;
            this.lblCategory.Text = "label1";
            // 
            // panelDataGrid
            // 
            this.panelDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDataGrid.Location = new System.Drawing.Point(18, 159);
            this.panelDataGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelDataGrid.Name = "panelDataGrid";
            this.panelDataGrid.Size = new System.Drawing.Size(826, 517);
            this.panelDataGrid.TabIndex = 23;
            // 
            // cBoxDateType1
            // 
            this.cBoxDateType1.FormattingEnabled = true;
            this.cBoxDateType1.Location = new System.Drawing.Point(6, 24);
            this.cBoxDateType1.Name = "cBoxDateType1";
            this.cBoxDateType1.Size = new System.Drawing.Size(119, 20);
            this.cBoxDateType1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(301, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "~";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(301, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "~";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cBoxDateType2
            // 
            this.cBoxDateType2.FormattingEnabled = true;
            this.cBoxDateType2.Location = new System.Drawing.Point(6, 51);
            this.cBoxDateType2.Name = "cBoxDateType2";
            this.cBoxDateType2.Size = new System.Drawing.Size(119, 20);
            this.cBoxDateType2.TabIndex = 12;
            // 
            // dtpType1From
            // 
            this.dtpType1From.CalendarFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpType1From.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.dtpType1From.Location = new System.Drawing.Point(131, 23);
            this.dtpType1From.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpType1From.Name = "dtpType1From";
            this.dtpType1From.Size = new System.Drawing.Size(164, 23);
            this.dtpType1From.TabIndex = 3;
            // 
            // dtpType1To
            // 
            this.dtpType1To.CalendarFont = new System.Drawing.Font("맑은 고딕", 9F);
            this.dtpType1To.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.dtpType1To.Location = new System.Drawing.Point(326, 23);
            this.dtpType1To.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpType1To.Name = "dtpType1To";
            this.dtpType1To.Size = new System.Drawing.Size(164, 23);
            this.dtpType1To.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cBoxStatus);
            this.groupBox1.Controls.Add(this.txtSearchWord);
            this.groupBox1.Location = new System.Drawing.Point(18, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 82);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblStatus.Location = new System.Drawing.Point(21, 27);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(31, 15);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "상태";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label1.Location = new System.Drawing.Point(9, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "검색어";
            // 
            // cBoxStatus
            // 
            this.cBoxStatus.FormattingEnabled = true;
            this.cBoxStatus.Location = new System.Drawing.Point(64, 21);
            this.cBoxStatus.Name = "cBoxStatus";
            this.cBoxStatus.Size = new System.Drawing.Size(77, 20);
            this.cBoxStatus.TabIndex = 9;
            // 
            // txtSearchWord
            // 
            this.txtSearchWord.Location = new System.Drawing.Point(64, 51);
            this.txtSearchWord.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearchWord.Name = "txtSearchWord";
            this.txtSearchWord.Size = new System.Drawing.Size(214, 21);
            this.txtSearchWord.TabIndex = 4;
            // 
            // dtpType2From
            // 
            this.dtpType2From.CalendarFont = new System.Drawing.Font("맑은 고딕", 9F);
            this.dtpType2From.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.dtpType2From.Location = new System.Drawing.Point(131, 50);
            this.dtpType2From.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpType2From.Name = "dtpType2From";
            this.dtpType2From.Size = new System.Drawing.Size(164, 23);
            this.dtpType2From.TabIndex = 5;
            // 
            // dtpType2To
            // 
            this.dtpType2To.CalendarFont = new System.Drawing.Font("맑은 고딕", 9F);
            this.dtpType2To.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.dtpType2To.Location = new System.Drawing.Point(326, 50);
            this.dtpType2To.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpType2To.Name = "dtpType2To";
            this.dtpType2To.Size = new System.Drawing.Size(164, 23);
            this.dtpType2To.TabIndex = 6;
            // 
            // groupBoxDatePikc
            // 
            this.groupBoxDatePikc.Controls.Add(this.cBoxDateType1);
            this.groupBoxDatePikc.Controls.Add(this.label2);
            this.groupBoxDatePikc.Controls.Add(this.label4);
            this.groupBoxDatePikc.Controls.Add(this.cBoxDateType2);
            this.groupBoxDatePikc.Controls.Add(this.dtpType1From);
            this.groupBoxDatePikc.Controls.Add(this.dtpType1To);
            this.groupBoxDatePikc.Controls.Add(this.dtpType2From);
            this.groupBoxDatePikc.Controls.Add(this.dtpType2To);
            this.groupBoxDatePikc.Location = new System.Drawing.Point(349, 37);
            this.groupBoxDatePikc.Name = "groupBoxDatePikc";
            this.groupBoxDatePikc.Size = new System.Drawing.Size(499, 82);
            this.groupBoxDatePikc.TabIndex = 26;
            this.groupBoxDatePikc.TabStop = false;
            this.groupBoxDatePikc.Text = "조회 기간";
            // 
            // btnCategory
            // 
            this.btnCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(95)))), ((int)(((byte)(184)))));
            this.btnCategory.FlatAppearance.BorderSize = 0;
            this.btnCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCategory.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnCategory.ForeColor = System.Drawing.Color.White;
            this.btnCategory.Location = new System.Drawing.Point(18, 4);
            this.btnCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCategory.Name = "btnCategory";
            this.btnCategory.Size = new System.Drawing.Size(45, 26);
            this.btnCategory.TabIndex = 24;
            this.btnCategory.Text = "분류";
            this.btnCategory.UseVisualStyleBackColor = false;
            // 
            // btnProductAdd
            // 
            this.btnProductAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(95)))), ((int)(((byte)(184)))));
            this.btnProductAdd.FlatAppearance.BorderSize = 0;
            this.btnProductAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductAdd.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnProductAdd.ForeColor = System.Drawing.Color.White;
            this.btnProductAdd.Location = new System.Drawing.Point(18, 126);
            this.btnProductAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnProductAdd.Name = "btnProductAdd";
            this.btnProductAdd.Size = new System.Drawing.Size(100, 25);
            this.btnProductAdd.TabIndex = 28;
            this.btnProductAdd.Text = "새제품 등록";
            this.btnProductAdd.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(95)))), ((int)(((byte)(184)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(214, 17);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(64, 25);
            this.btnSearch.TabIndex = 29;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // PdtListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.panelDataGrid);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxDatePikc);
            this.Controls.Add(this.btnCategory);
            this.Controls.Add(this.btnProductAdd);
            this.Name = "PdtListView";
            this.Size = new System.Drawing.Size(866, 688);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxDatePikc.ResumeLayout(false);
            this.groupBoxDatePikc.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Panel panelDataGrid;
        private System.Windows.Forms.ComboBox cBoxDateType1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cBoxDateType2;
        private System.Windows.Forms.DateTimePicker dtpType1From;
        private System.Windows.Forms.DateTimePicker dtpType1To;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cBoxStatus;
        private System.Windows.Forms.TextBox txtSearchWord;
        private System.Windows.Forms.DateTimePicker dtpType2From;
        private System.Windows.Forms.DateTimePicker dtpType2To;
        private System.Windows.Forms.GroupBox groupBoxDatePikc;
        private System.Windows.Forms.Button btnCategory;
        private System.Windows.Forms.Button btnProductAdd;
        private System.Windows.Forms.Button btnSearch;
    }
}
