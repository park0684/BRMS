﻿using System;
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
    public partial class PurchaseOrderList : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet DgrPurorderList = new cDataGridDefaultSet();
        bool supplierToggle = false;
        string supplierCode = "";
        int accessedEmp = cUserSession.AccessedEmp;
        public PurchaseOrderList()
        {
            InitializeComponent();
            panelDatagrid.Controls.Add(DgrPurorderList.Dgv);
            DgrPurorderList.Dgv.Dock = DockStyle.Fill;
            GridForm();
            LoadDefault();
            DgrPurorderList.CellDoubleClick += DgrPurorderList_CellDoubleClick;
            cBoxpurTypeInfo();
            ModifyPermission();
        }
        private void ModifyPermission()
        {
            if (!cUserSession.HasPermission(202))
            {
                bntOrderReg.Enabled = false;
            }
        }
        private void cBoxpurTypeInfo()
        {
            cBoxOrderType.Items.Add("전체");
            foreach (var status in cStatusCode.purchaseType)
            {
                cBoxOrderType.Items.Add(new KeyValuePair<int, string>(status.Key, status.Value));
            }
            cBoxOrderType.DisplayMember = "Value";
            cBoxOrderType.ValueMember = "key";
            cBoxOrderType.DropDownStyle = ComboBoxStyle.DropDownList;
            cBoxOrderType.SelectedIndex = 0;
        }

        private void GetSupplier(int supCode, string supName)
        {
            lblSupplierName.Text = supName;
            supplierCode = Convert.ToString(supCode);
            supplierToggle = true;
        }
        private void LoadDefault()
        {
            lblSupplierName.Text = "전체";
            dtpRegDateFrom.Format = DateTimePickerFormat.Short;
            dtpRegDateTo.Format = DateTimePickerFormat.Short;

        }
        private void GridForm()
        {
            DgrPurorderList.Dgv.Columns.Add("purOrderCode", "발주코드");
            DgrPurorderList.Dgv.Columns.Add("purOrderType", "유형");
            DgrPurorderList.Dgv.Columns.Add("purOrderSupplier", "매입처");
            //DgrPurorderList.Dgv.Columns.Add("purOrderSupcode", "공급사코드");
            DgrPurorderList.Dgv.Columns.Add("purOrderDate", "발주일");
            DgrPurorderList.Dgv.Columns.Add("purOrderArrivaldate", "입고예정일");
            DgrPurorderList.Dgv.Columns.Add("purOrderAmount", "발주액");
            DgrPurorderList.Dgv.Columns.Add("purOrderStatus", "상태");
            DgrPurorderList.Dgv.Columns.Add("purOrderNote", "비고");
            DgrPurorderList.Dgv.Columns.Add("purOrderIdate", "수정일");
            DgrPurorderList.Dgv.Columns.Add("purOrderUdate", "수정일");
            //포멧 설정
            DgrPurorderList.FormatAsStringLeft("purOrderSupplier");
            DgrPurorderList.FormatAsStringCenter("purOrderCode", "purOrderSupcode", "purType");
            DgrPurorderList.FormatAsDateTime("purOrderDate", "purOrderIdate", "purOrderUdate");
            DgrPurorderList.FormatAsDate("purOrderArrivaldate");
            DgrPurorderList.FormatAsInt("purOrderAmount");
            
            DgrPurorderList.Dgv.ReadOnly = true;
            DgrPurorderList.Dgv.Columns["purOrderCode"].Visible = false;
            DgrPurorderList.ApplyDefaultColumnSettings();

        }
        private void GridFill(DataTable dataTable)
        {
            int rowIndex = 0;
            DgrPurorderList.Dgv.Rows.Clear();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DgrPurorderList.Dgv.Rows.Add();
                DgrPurorderList.Dgv.Rows[rowIndex].Cells["No"].Value = DgrPurorderList.Dgv.RowCount;
                DgrPurorderList.Dgv.Rows[rowIndex].Cells["purOrderType"].Value = cStatusCode.GetPurchaseType(Convert.ToInt32(dataRow["pord_type"]));
                DgrPurorderList.Dgv.Rows[rowIndex].Cells["purOrderCode"].Value = dataRow["pord_code"];
                DgrPurorderList.Dgv.Rows[rowIndex].Cells["purOrderSupplier"].Value = $"{dataRow["sup_name"]}({dataRow["pord_sup"]})";
                //DgrPurorderList.Dgv.Rows[rowIndex].Cells["purOrderSupcode"].Value = dataRow["pord_sup"];
                DgrPurorderList.Dgv.Rows[rowIndex].Cells["purOrderDate"].Value = Convert.ToDateTime(dataRow["pord_date"]);
                DgrPurorderList.Dgv.Rows[rowIndex].Cells["purOrderArrivaldate"].Value = Convert.ToDateTime(dataRow["pord_arrivaldate"]);
                DgrPurorderList.Dgv.Rows[rowIndex].Cells["purOrderAmount"].Value = Convert.ToInt32(dataRow["pord_Amount"]);
                DgrPurorderList.Dgv.Rows[rowIndex].Cells["purOrderStatus"].Value = cStatusCode.GetPurchaseOrderStatus(Convert.ToInt32(dataRow["pord_status"]));
                DgrPurorderList.Dgv.Rows[rowIndex].Cells["purOrderNote"].Value = dataRow["pord_note"];
                DgrPurorderList.Dgv.Rows[rowIndex].Cells["purOrderIdate"].Value = Convert.ToDateTime(dataRow["pord_idate"]);
                DgrPurorderList.Dgv.Rows[rowIndex].Cells["purOrderUdate"].Value = Convert.ToDateTime(dataRow["pord_udate"]);
                rowIndex++;

            }
        }
        private void QuerySetting()
        {
            DataTable resultData = new DataTable();
            string query = string.Format("SELECT pord_code, sup_name, pord_sup, pord_date, pord_arrivaldate, pord_Amount, pord_type, pord_note, pord_idate ,pord_udate,pord_status FROM purorder,supplier " +
                "WHERE pord_sup =  sup_code AND pord_date >= '{0}' ANd pord_date < '{1}' ", dtpRegDateFrom.Value.ToString("yyyy-MM-dd"), dtpRegDateTo.Value.AddDays(1).ToString("yyyy-MM-dd"));
            if (supplierCode != "")
            {
                query = string.Format(query + " AND sup_code ={0}", supplierCode);
            }

            switch (cBoxOrderType.SelectedIndex)
            {
                case 1:
                    query = string.Format(query + " AND pord_type = 1");
                    break;
                case 2:
                    query = string.Format(query + " AND pord_type = 2");
                    break;

            }

            dbconn.SqlReaderQuery(query, resultData);
            GridFill(resultData);
            cLog.InsertEmpAccessLogNotConnect("@purOrderSearch", accessedEmp, 0);
        }

        public void RunQuery()
        {
            try
            {
                QuerySetting();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DgrPurorderList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectOrderCode = DgrPurorderList.ConvertToInt(DgrPurorderList.Dgv.CurrentRow.Cells["purOrderCode"].Value);
            PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();
            purchaseOrderDetail.StartPosition = FormStartPosition.CenterParent;
            purchaseOrderDetail.GetPurOrderCode(selectOrderCode);
            cLog.InsertEmpAccessLogNotConnect("@purOrderSearch", accessedEmp, selectOrderCode);
            purchaseOrderDetail.ShowDialog();
        }
        private void bntSupplier_Click(object sender, EventArgs e)
        {
            if (supplierToggle == true)
            {
                supplierToggle = false;
                lblSupplierName.Text = "전체";
                supplierCode = "";
            }
            else
            {
                SupplierSelectBox supplierSelectBox = new SupplierSelectBox();
                supplierSelectBox.SupplierSelected += (supCode, supName) => { GetSupplier(supCode, supName); };
                supplierSelectBox.StartPosition = FormStartPosition.CenterParent;
                supplierSelectBox.ShowDialog();
            }
        }

        private void bntOrderReg_Click(object sender, EventArgs e)
        {
            int supCode = 0;
            PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();
            purchaseOrderDetail.StartPosition = FormStartPosition.CenterParent;
            purchaseOrderDetail.callSup += (supplierCode) => { supCode = supplierCode; };
            purchaseOrderDetail.AddpurOrder();
            //purchaseOrderDetail의 Addpurchase 실행시 SupplierSelectBox클래스 호출하여 공급사를 선택.
            //이때 선택하지 않고 닫을 경우 purchaseOrderDetail 클래스의 supplierCode값은 기본값인 0이다.
            //이 변수를 이벤트로 받아 supCode에 기록, 만약 그 값이 0이라면 purchaseOrderDetail 호출 자체를 취소한다.
            if (supCode != 0)
            {
                purchaseOrderDetail.ShowDialog();
            };
        }
    }
}
