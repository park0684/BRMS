
namespace BRMS
{
    partial class LoginForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tBoxEmpName = new System.Windows.Forms.TextBox();
            this.tBoxEmpCode = new System.Windows.Forms.TextBox();
            this.tBoxPassword = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.bntCancle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label1.Location = new System.Drawing.Point(23, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "직원명";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label2.Location = new System.Drawing.Point(11, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "직원코드";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label3.Location = new System.Drawing.Point(11, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "비밀번호";
            // 
            // tBoxEmpName
            // 
            this.tBoxEmpName.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.tBoxEmpName.Location = new System.Drawing.Point(72, 31);
            this.tBoxEmpName.Name = "tBoxEmpName";
            this.tBoxEmpName.Size = new System.Drawing.Size(151, 23);
            this.tBoxEmpName.TabIndex = 0;
            // 
            // tBoxEmpCode
            // 
            this.tBoxEmpCode.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.tBoxEmpCode.Location = new System.Drawing.Point(72, 69);
            this.tBoxEmpCode.Name = "tBoxEmpCode";
            this.tBoxEmpCode.Size = new System.Drawing.Size(151, 23);
            this.tBoxEmpCode.TabIndex = 1;
            // 
            // tBoxPassword
            // 
            this.tBoxPassword.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.tBoxPassword.Location = new System.Drawing.Point(72, 107);
            this.tBoxPassword.Name = "tBoxPassword";
            this.tBoxPassword.Size = new System.Drawing.Size(151, 23);
            this.tBoxPassword.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(154)))), ((int)(((byte)(240)))));
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(95, 152);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 25);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.bntOk_Click);
            // 
            // bntCancle
            // 
            this.bntCancle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bntCancle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(154)))), ((int)(((byte)(240)))));
            this.bntCancle.FlatAppearance.BorderSize = 0;
            this.bntCancle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntCancle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.bntCancle.ForeColor = System.Drawing.Color.White;
            this.bntCancle.Location = new System.Drawing.Point(161, 152);
            this.bntCancle.Name = "bntCancle";
            this.bntCancle.Size = new System.Drawing.Size(60, 25);
            this.bntCancle.TabIndex = 13;
            this.bntCancle.Text = "취소";
            this.bntCancle.UseVisualStyleBackColor = false;
            this.bntCancle.Click += new System.EventHandler(this.bntCancle_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 189);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.bntCancle);
            this.Controls.Add(this.tBoxPassword);
            this.Controls.Add(this.tBoxEmpCode);
            this.Controls.Add(this.tBoxEmpName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "LoginForm";
            this.Text = "로그인";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tBoxEmpName;
        private System.Windows.Forms.TextBox tBoxEmpCode;
        private System.Windows.Forms.TextBox tBoxPassword;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button bntCancle;
    }
}