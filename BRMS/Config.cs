using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRMS
{
    public partial class Config : Form
    {
        private Form activeForm;

        public Config()
        {
            InitializeComponent();
            MenuLayout();
            InitializeButton();
            cUIManager.ApplyPopupFormStyle(this);
        }
        private void InitializeButton()
        {
            ButtonDesignSet(btnApply);
            ButtonDesignSet(btnClose);
            ButtonDesignSet(btnOk);
            ButtonDesignSet(btnConfigLog);
        }
        private void ButtonDesignSet(Button button)
        {
            button.Size = new System.Drawing.Size(75, 25);
            button.BackColor = cUIManager.Color.Blue;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
        }
        private void MenuLayout()
        {
            Button btnConfigBasic = CreateMenuButton("기본설정", btnConfigBasic_Click);
            Button btnConfigPoint = CreateMenuButton("포인트 설정", btnConfigPoint_Click);


            pnlMenu.Controls.Add(btnConfigPoint);
            pnlMenu.Controls.Add(btnConfigBasic);
        }
        private Button CreateMenuButton(string btnText, EventHandler eventHandler)
        {
            Button btn = new Button
            {
                Text = btnText,
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                UseVisualStyleBackColor = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(120, 35),
                Font = new Font("맑은 고딕", 10F)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += eventHandler;
            return btn;
        }
        private void btn_Click<T>(object sender, EventArgs e) where T : Form, new()
        {
            if (activeForm is T)
            {
                return;
            }

            T formInstance;
            string formName = typeof(T).Name;

            if (pnlConfig.Controls.ContainsKey(formName))
            {
                formInstance = (T)pnlConfig.Controls[formName];
            }
            else
            {

                formInstance = new T();
                formInstance.TopLevel = false;
                formInstance.FormBorderStyle = FormBorderStyle.None;
                formInstance.Dock = DockStyle.Fill;
                pnlConfig.Tag = ((Button)sender).Name;
                formInstance.Name = formName;
            }
            openChildForm(formInstance, sender);
        }
        private void openChildForm(Form childForm, object bntsender)
        {
            if (activeForm != null)
            {
                activeForm.Hide();
            }

            //ActiveButton(bntsender);
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlConfig.Controls.Add(childForm);
            //panelViewer.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        /// <summary>
        /// 실행중인 폼 모두 run Query 메소드 실행
        /// </summary>
        private void ExecuteQueryInActiveForm()
        {
            foreach(Control control in pnlConfig.Controls)
            {
                if (control is Form activeForm)
                {
                    Type formType = activeForm.GetType();
                    MethodInfo methodInfo = formType.GetMethod("RunQuery");
                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(activeForm, null);
                    }
                }
            }

        }

        private void btnConfigBasic_Click(object sender, EventArgs e)
        {
            btn_Click<ConfigBasic>(sender, e);
        }
        private void btnConfigPoint_Click(object sender, EventArgs e)
        {
            btn_Click<ConfigPoint>(sender, e);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ExecuteQueryInActiveForm();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ExecuteQueryInActiveForm();
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfigLog_Click(object sender, EventArgs e)
        {
            ConfigLog configLog = new ConfigLog();
            configLog.StartPosition = FormStartPosition.CenterParent;
            configLog.ShowDialog();
        }
    }
}
