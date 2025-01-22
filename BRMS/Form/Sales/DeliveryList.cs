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
    public partial class DeliveryList : Form
    {
        int accessedEmp = cUserSession.AccessedEmp;
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet dgvDeliveryList = new cDataGridDefaultSet();
        static Dictionary<string, (int typeCode, string typeString)> parameter = new Dictionary<string, (int, string)>();
        int customerCode = 0;
        bool custCheck = false;
        int transitCount = 0;
        int completedCount = 0;
        int cancleCount = 0;

        public DeliveryList()
        {
            InitializeComponent();
            InitialzeComboBox();
            UpdateLabel();
            GridForm();
            dgvDeliveryList.CellDoubleClick += dgvDeliveryList_CellDoubleClick;
            tBoxSearch.Enabled = false;
        }
        /// <summary>
        /// 상태 선태 콤보박스 설정
        /// </summary>
        private void InitialzeComboBox()
        {
            cmBoxStatus.Items.Add("전체");
            foreach (var entry in cStatusCode.DeliveryStatus)
            {
                cmBoxStatus.Items.Add(new KeyValuePair<int, string>(entry.Key, entry.Value));
            }

            cmBoxStatus.DisplayMember = "Value"; // 사용자에게 보여질 값
            cmBoxStatus.ValueMember = "Key";    // 내부적으로 사용할 값
            cmBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmBoxStatus.SelectedIndex = 0;
        }
        private void UpdateLabel()
        {
            lblDeliveryTotal.Text = (transitCount + completedCount + cancleCount).ToString("#,##0");
            lblDeliveryTransit.Text = transitCount.ToString();
            lblDeliveryCompleted.Text = completedCount.ToString();
            lblDeliveryCancle.Text = cancleCount.ToString();
        }
        /// <summary>
        /// 데이터 그리드 설정
        /// </summary>
        private void GridForm()
        {
            pnlDataGrid.Controls.Add(dgvDeliveryList.Dgv);
            dgvDeliveryList.Dgv.Dock = DockStyle.Fill;

            dgvDeliveryList.Dgv.Columns.Add("delCode", "배달번호");
            dgvDeliveryList.Dgv.Columns.Add("delStatus", "상태");
            dgvDeliveryList.Dgv.Columns.Add("delCust", "배달회원");
            dgvDeliveryList.Dgv.Columns.Add("delCustName", "회원명");
            dgvDeliveryList.Dgv.Columns.Add("delRecipient", "수령자");
            dgvDeliveryList.Dgv.Columns.Add("delCountry", "국가");
            dgvDeliveryList.Dgv.Columns.Add("delAddr", "주소");
            dgvDeliveryList.Dgv.Columns.Add("delDate", "접수일");

            dgvDeliveryList.ApplyDefaultColumnSettings();
            dgvDeliveryList.FormatAsDateTime("delDate");
            dgvDeliveryList.FormatAsStringCenter("delCode", "delStatus", "delCountry");
            dgvDeliveryList.FormatAsStringLeft("delCustName", "delRecipient", "delCountry", "delAddr");
        }
        private void FillGrid(DataTable dataTable)
        {
            dgvDeliveryList.Dgv.Rows.Clear();
            foreach(DataRow row in dataTable.Rows)
            {
                var newRow = dgvDeliveryList.Dgv.Rows.Add();
                var rowIndex = dgvDeliveryList.Dgv.Rows[newRow];

            }
            
        }

        /// <summary>
        /// 셀 더블 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDeliveryList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadDeiveryDetailForm();
        }
        /// <summary>
        /// 배송 상세 정보 활성
        /// </summary>
        private void LoadDeiveryDetailForm()
        {

        }
        /// <summary>
        /// 조회 쿼리 설정
        /// </summary>
        private void QuerySetting()
        {

        }

    }
}
