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
    public partial class DeliveryRegist : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cStatusCode statusCode = new cStatusCode();
        int deliveryCode = 0;
        int customerCode = 0;
        int countryCode = 0;
        public event Action<string, string, string, string, int, bool> ForwardDeliveryInfo;


        public DeliveryRegist()
        {
            InitializeComponent();
            InitializeStatus();
        }
        /// <summary>
        /// 초기 상태값 설정
        /// </summary>
        private void InitializeStatus()
        {
            lblCustName.Text = "";
            lblCustMail.Text = "";

            cmBoxCountry.DropDownStyle = ComboBoxStyle.DropDownList;
            string query = "SELECT ctry_code, ctry_name FROM country";
            DataTable resultTable = new DataTable();
            dbconn.SqlDataAdapterQuery(query, resultTable);
            cmBoxCountry.Items.Add("-국가 선택-");
            foreach (DataRow row in resultTable.Rows)
            {
                string countryName = row["ctry_name"].ToString();
                cmBoxCountry.Items.Add(countryName);
            }
            cmBoxCountry.SelectedIndex = 0;
        }

        /// <summary>
        /// 신규 주문의 경우 배송지새로 등록
        /// 회원코드가 있을 경우 회원코드로 회원 검색 후 정보 받아오기
        /// </summary>
        /// <param name="custCode"></param>
        public void NewDelivery(int custCode)
        {
            GetCustomerInfo(custCode);
        }
        /// <summary>
        /// 기존 주문서일 경우 주문서에 등록된 배송 주소 정보 조회
        /// </summary>
        /// <param name="code"></param>
        public void GetDeliveryCode(int code)
        {
            LoadDeliveryInfo(code);
        }
        /// <summary>
        /// 회원코드 기준으로 0일 경우 비회원, 회원코드가 있을 경우 회원 검색 후 각 라벨과 텍스트 박스에 정보 등록
        /// </summary>
        /// <param name="custCode"></param>
        private void GetCustomerInfo(int custCode)
        {
            if(custCode == 0)
            {
                lblCustName.Text = "비회원";
                lblCustMail.Text = "";
                lblTel.Text = "";
            }
            else
            {
                string query = "SELECT cust_name, cust_country, cust_addr, cust_email, cust_tell, cust_cell FROM customer WHERE cust_code = " + custCode;
                DataTable reusltTable = new DataTable();
                dbconn.SqlReaderQuery(query, reusltTable);
                DataRow row = reusltTable.Rows[0];
                lblCustName.Text = row["cust_name"].ToString();
                lblCustMail.Text = row["cust_email"].ToString();
                lblTel.Text = row["cust_tell"].ToString() + " | " + row["cust_cell"].ToString();
                countryCode = Convert.ToInt32(row["cust_country"]);
                cmBoxCountry.SelectedIndex = countryCode;
                tBoxAddr.Text = row["cust_addr"].ToString();
            } 
        }
        /// <summary>
        /// 기존 주문서 경우 주문서와 연결된 배송지 정보 조회
        /// </summary>
        /// <param name="delCode"></param>
        private void LoadDeliveryInfo(int delCode)
        {
            string query = "SELECT del_cust,del_country, del_addr, del_tel, del_recipient, del_invoice, del_idate, del_udate FROM delivery WHERE del_code = " + delCode;
            DataTable resultTable = new DataTable();
            dbconn.SqlReaderQuery(query, resultTable);
            DataRow row = resultTable.Rows[0];
            customerCode = Convert.ToInt32(row["del_cust"]);
            GetCustomerInfo(customerCode);
            deliveryCode = delCode;
            countryCode = Convert.ToInt32(row["del_country"]);
            tBoxAddr.Text = row["del_addr"].ToString();
            tBoxRecipient.Text = row["del_recipient"].ToString();
            tBoxTel.Text = row["del_tel"].ToString();
            tBoxInvoice.Text = row["del_invoice"].ToString();
            cmBoxCountry.SelectedIndex = countryCode;
            lblInDate.Text = $"등록일 : {Convert.ToDateTime(row["del_idate"]).ToString("yyyy-MM-dd HH:mm")}";
            lblUpdate.Text = $"등록일 : {Convert.ToDateTime(row["del_udate"]).ToString("yyyy-MM-dd HH:mm")}";

        }

        private void bntSave_Click(object sender, EventArgs e)
        {
            string address = tBoxAddr.Text;
            string recipient = tBoxRecipient.Text;
            string tell = tBoxTel.Text;
            string invoice = tBoxInvoice.Text;
            int country = cmBoxCountry.SelectedIndex;
            if(country == 0)
            {
                MessageBox.Show("국가 지정되지 않았습니다", "알림");
                return;
            }
            ForwardDeliveryInfo?.Invoke(address, recipient, tell, invoice, country, true);
            Close();
        }

        private void bntCancle_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
