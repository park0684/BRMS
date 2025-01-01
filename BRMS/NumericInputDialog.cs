using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRMS
{
    public partial class NumericInputDialog : Form
    {
        public event Action<decimal> ValueSubmit;
        public NumericInputDialog()
        {
            InitializeComponent();
            ControlBox = false;
            tBoxNumber.KeyPress += tBoxNumber_KeyPress;
        }
        /// <summary>
        /// 클래스 호출 시 변수
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="decimalPoint"></param>
        public void GetValue(string name, decimal value, bool decimalPoint )
        {
            lblName.Text = name;
            if(decimalPoint == true)
            {
                tBoxNumber.Text = value.ToString("#,##0.00");
            }
            else
            {
                tBoxNumber.Text = value.ToString("#,##0");
            }
        }
        /// <summary>
        /// 숫자만 입력 가능하게 이벤트 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBoxNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;  // 숫자 외의 입력을 막음
            }
        }

        private void bntSave_Click(object sender, EventArgs e)
        {
            decimal value = Convert.ToDecimal(tBoxNumber.Text);
            ValueSubmit?.Invoke(value);
            Close();
        }

        private void bntCancle_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
