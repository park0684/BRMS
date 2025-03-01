﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRMS
{
    public partial class SalesRegist : Form
    {
        cDatabaseConnect dbconn = new cDatabaseConnect();
        cDataGridDefaultSet PdtList = new cDataGridDefaultSet();
        cDataGridDefaultSet SalesInfo = new cDataGridDefaultSet();
        int accessedEmp = cUserSession.AccessedEmp;
        DateTime saleDate = DateTime.Now;
        int empCode = 0;
        int bprice = 0;
        int cashKrw = 0;
        int accountKrw = 0;
        int cardKrw = 0;
        int pointKrw = 0;
        int amountKrw = 0;
        int discount = 0;
        int deliveryKrw = 0;
        decimal cashUsd = 0;
        decimal accountUsd = 0;
        decimal cardUsd = 0;
        decimal pointUsd = 0;
        decimal amountUsd = 0;
        decimal deliveryUsd = 0;
        int customerCode = 0;
        int custPoint = 0;
        int pointSeq = 0;
        string customerName = "";
        string DeliveryAddr = "";
        string DeliveryRecipient = "";
        string DeliveryInvoice = "";
        string DeliveryTel = "";
        int deliveryCountry = 0;
        int reward = 0; // 적립 포인트
        int tax = 0; // 과면세 여부
        int saleCode = 0; // 거래코드
        int origineCode = 0;//원거래 코드
        int exchange = 0; // 환율
        int saleType = 1;//판매유형
        int deliveryCode = 0;// 배달 코드
        int paymentCode = 0;//결제 코드
        int custOrderCode = 0;//주문서 코드
        decimal cashRewardRate = 0;
        decimal cardRewardRate = 0;
        decimal acchRewardRate = 0;
        decimal pointRewardRate = 0;
        bool DeliveryWhether = false; // 배달 등록 여부
        bool custormCheck = false; // 회원여부

        public SalesRegist()
        {
            InitializeComponent();
            InitializeControls();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            LablesUpdate();
            GetConfig();
            tBoxWord.KeyDown += tBoxWordh_KeyDownEnter;
            panelPdtList.Controls.Add(PdtList.Dgv);
            PdtList.Dgv.Dock = DockStyle.Fill;
            //GridFormInfo();
            GridFormPdtList();
            
        }
        /// <summary>
        /// 콤보박스 기본 설정
        /// </summary>
        private void InitializeControls()
        {
            cmBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var status in cStatusCode.SaleType)
            {
                cmBoxType.Items.Add(status.Value); // 상태 문자열 추가
            }
            cmBoxType.Items.Remove("반품(구매 취소)");
            cmBoxType.SelectedIndex = 1;

            bntReturn.Visible = false;
            bntAfterDelivery.Visible = false;
        }
        private void customerToggle(bool check)
        {
            if(check == true)
            {
                custormCheck = true;
                bntCustomer.Text = "회원취소";
            }
            else
            {
                custormCheck = false;
                bntCustomer.Text = "회원";
                customerCode = 0;
                customerName = "";
                custPoint = 0;
                lblCustName.Text = customerName;
                lblCustPoint.Text = custPoint.ToString("#,##0");
            }
        }
        private void LablesUpdate()
        {
            lblChas.Text = string.Format("{0}({1})", cashKrw.ToString("#,##0"), cashUsd.ToString("#,##0.00"));
            lblCard.Text = string.Format("{0}({1})", cardKrw.ToString("#,##0"), cardUsd.ToString("#,##0.00"));
            lblAccount.Text = string.Format("{0}({1})", accountKrw.ToString("#,##0"), accountUsd.ToString("#,##0.00"));
            lblPoint.Text = string.Format("{0}({1})", pointKrw.ToString("#,##0"), pointUsd.ToString("#,##0.00"));
            lblCustName.Text = customerName;
            lblCustPoint.Text = custPoint.ToString("#,##0");
        }
        public void GetSaleCode(int code)
        {
            LoadSalesInfo(code);
            FillGridPdtList(code);
        }
        private void LoadSalesInfo(int code)
        {
            DataTable saleData = new DataTable();
            DataTable payData = new DataTable();
            DataTable deliveryData = new DataTable();
            
            string query = $"SELECT sale_cust,sale_type, sale_bprice, sale_sprice_krw, sale_sprice_usd, sale_dc, sale_tax, sale_reward, sale_delivery, sale_delfee, sale_emp , sale_origine FROM sales WHERE sale_code = {code} ";
            dbconn.SqlReaderQuery(query, saleData);
            DataRow dataRow = saleData.Rows[0];

            saleCode = code;
            saleType= Convert.ToInt32(dataRow["sale_type"]);
            bprice = Convert.ToInt32(dataRow["sale_bprice"]);
            amountKrw = Convert.ToInt32(dataRow["sale_sprice_krw"]);
            amountUsd = Convert.ToDecimal(dataRow["sale_sprice_usd"]);
            discount = Convert.ToInt32(dataRow["sale_dc"]);
            tax = Convert.ToInt32(dataRow["sale_tax"]);
            reward = cDataHandler.ConvertToInt(dataRow["sale_reward"]);
            customerCode = Convert.ToInt32(dataRow["sale_cust"]);
            deliveryCode = Convert.ToInt32(dataRow["sale_delivery"]);
            deliveryKrw = Convert.ToInt32(dataRow["sale_delfee"]);
            empCode = Convert.ToInt32(dataRow["sale_emp"]);
            origineCode = Convert.ToInt32(dataRow["sale_origine"]);
            customerCode = cDataHandler.ConvertToInt(dataRow["sale_cust"]);
            custormCheck = customerCode != 0 ? true : false;
            LoadCustomerInfo();
            query = $"SELECT spay_cash_krw, spay_cash_use, spay_account_krw, spay_account_usd, spay_credit_krw, spay_credit_usd, spay_point_krw, spay_point_usd, spay_exchenge FROM salepay WHERE spay_salecode = {code}";
            dbconn.SqlReaderQuery(query, payData);
            
            dataRow.Delete();
            dataRow = payData.Rows[0];
            exchange = Convert.ToInt32(dataRow["spay_exchenge"]);
            cashKrw = Convert.ToInt32(dataRow["spay_cash_krw"]);
            accountKrw = Convert.ToInt32(dataRow["spay_account_krw"]);
            cardKrw = Convert.ToInt32(dataRow["spay_credit_krw"]);
            pointKrw = Convert.ToInt32(dataRow["spay_point_krw"]);

            cashUsd = Convert.ToDecimal(dataRow["spay_cash_use"]);
            accountUsd = Convert.ToDecimal(dataRow["spay_account_usd"]);
            cardUsd = Convert.ToDecimal(dataRow["spay_credit_usd"]);
            pointUsd = Convert.ToDecimal(dataRow["spay_point_usd"]);
            deliveryUsd = Math.Round(Convert.ToDecimal(deliveryKrw) / Convert.ToDecimal(exchange), 2);

            if(deliveryCode != 0)
            {
                query = $"SELECT del_code, del_cust,del_country,del_addr,del_recipient,del_tel,del_invoice,del_salecode FROM delivery WHERE del_code ={deliveryCode}";
                dbconn.SqlReaderQuery(query, deliveryData);
                dataRow.Delete();
                dataRow = deliveryData.Rows[0];
                deliveryCountry = Convert.ToInt32(dataRow["del_country"]);
                DeliveryAddr = dataRow["del_addr"].ToString().Trim();
                DeliveryRecipient = dataRow["del_recipient"].ToString().Trim();
                DeliveryTel = dataRow["del_tel"].ToString().Trim();
                DeliveryInvoice = dataRow["del_invoice"].ToString().Trim();
            }
            

            GridFormInfo();
            SalesGridFill();
        }
        private void GridFormPdtList()
        {
            PdtList.Dgv.Columns.Add("pdtCode", "제품코드");
            PdtList.Dgv.Columns.Add("pdtTax", "과면세");
            PdtList.Dgv.Columns.Add("pdtNameKr", "제품명");
            PdtList.Dgv.Columns.Add("pdtNameEn", "제품명(영문)");
            PdtList.Dgv.Columns.Add("pdtNumber", "제품번호");
            PdtList.Dgv.Columns.Add("pdtSpriceKrw", "판매단가(￦)");
            PdtList.Dgv.Columns.Add("pdtSpriceUsd", "판매단가(＄)");
            PdtList.Dgv.Columns.Add("pdtQty", "수량");
            PdtList.Dgv.Columns.Add("pdtDc", "할인");
            PdtList.Dgv.Columns.Add("pdtPoint", "포인트적립여부");;
            PdtList.Dgv.Columns.Add("pdtAmountKrw", "판매액(￦)");
            PdtList.Dgv.Columns.Add("pdtAmountUsd", "판매액(＄)");
            PdtList.Dgv.Columns.Add("pdtBprice", "원가");
            PdtList.Dgv.Columns.Add("pdtTaxCode", "과면세코드");

            PdtList.Dgv.Columns["pdtPoint"].Visible = false;
            PdtList.ApplyDefaultColumnSettings();
            PdtList.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            PdtList.Dgv.Columns["pdtCode"].Visible = false;
            PdtList.Dgv.Columns["pdtTaxCode"].Visible = false;
            PdtList.FormatAsInt("pdtSpriceKrw", "pdtDc", "pdtQty", "pdtAmountKrw");
            PdtList.FormatAsDecimal("pdtSpriceUsd", "pdtAmountUsd");
            PdtList.FormatAsStringCenter("pdtTax");
            PdtList.Dgv.ReadOnly = true;
            foreach (DataGridViewColumn column in PdtList.Dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }

        /// <summary>
        /// 판매정보 데이터 그리드 생성
        /// </summary>
        private void GridFormInfo()
        {
            TlpPreset.Visible = false;
            tBoxWord.Visible = false;
            lblSearch.Visible = false;
            cmBoxType.Visible = false;
            if(origineCode != 0)
            {
                bntReturn.Enabled = false;
                bntAfterDelivery.Enabled = false;

            }
            bntReturn.Visible = true;
            bntAfterDelivery.Visible = true;
            bntReturn.Location = new Point(815, 434);
            panelInfo.Controls.Clear();
            panelInfo.Controls.Add(SalesInfo.Dgv);
            SalesInfo.Dgv.ColumnHeadersVisible = false;
            SalesInfo.Dgv.Columns["no"].Visible = false;
            SalesInfo.Dgv.Columns.Add("Item", "항목");
            SalesInfo.Dgv.Columns.Add("Value", "값");
            SalesInfo.Dgv.Dock = DockStyle.Fill;
            string[] items = { "판매일", "담당자", "유형", "과세액", "면세액", "합계", "현금", "계좌이체", "카드", "포인트", "배송비", "회원명", "적립포인트", "거래번호" };
            int index = 0;
            SalesInfo.Dgv.Columns["Item"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            SalesInfo.Dgv.Columns["Value"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            SalesInfo.Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            SalesInfo.Dgv.Columns["Item"].Width = 70;
            SalesInfo.Dgv.Columns["Value"].Width = 160;
            SalesInfo.Dgv.ReadOnly = true;
            foreach (DataGridViewColumn column in SalesInfo.Dgv.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (string item in items)
            {
                SalesInfo.Dgv.Rows.Add();
                SalesInfo.Dgv.Rows[index].Cells["Item"].Value = item;
                index++;
            }

        }
        /// <summary>
        /// 판매제품리스트 그리드 번호 설정
        /// </summary>
        private void GridNumberSetting()
        {
            int number = 1;
            for (int index = 0; index < PdtList.Dgv.RowCount; index++)
            {
                PdtList.Dgv.Rows[index].Cells["No"].Value = number;
                number++;
            }
        }
        private void SalesGridFill()
        {
            if (saleCode != 0)
            {
                SalesInfo.Dgv.Rows[0].Cells["Value"].Value = saleDate.ToString("yyyy년MM월dd일 HH:mm");
            }
            SalesInfo.Dgv.Rows[1].Cells["Value"].Value = empCode;
            SalesInfo.Dgv.Rows[2].Cells["Value"].Value = cStatusCode.GetSaleType(saleType);
            SalesInfo.Dgv.Rows[3].Cells["Value"].Value = tax.ToString("#,###");
            SalesInfo.Dgv.Rows[4].Cells["Value"].Value = (amountKrw - tax).ToString("#,###");
            SalesInfo.Dgv.Rows[5].Cells["Value"].Value = $"{amountKrw.ToString("#,###")}({amountUsd.ToString("#,###.##")}$)";
            SalesInfo.Dgv.Rows[6].Cells["Value"].Value = $"{cashKrw.ToString("#,###")}({cashUsd.ToString("#,###.##")}$)";
            SalesInfo.Dgv.Rows[7].Cells["Value"].Value = $"{accountKrw.ToString("#,###")}({accountUsd.ToString("#,###.##")}$)";
            SalesInfo.Dgv.Rows[8].Cells["Value"].Value = $"{cardKrw.ToString("#,###")}({cardUsd.ToString("#,###.##")}$)";
            SalesInfo.Dgv.Rows[9].Cells["Value"].Value = $"{pointKrw.ToString("#,###")}({pointUsd.ToString("#,###.##")}$)";
            SalesInfo.Dgv.Rows[10].Cells["Value"].Value = $"{deliveryKrw.ToString("#,###")}({deliveryUsd.ToString("#,###.##")}$)";
            SalesInfo.Dgv.Rows[11].Cells["Value"].Value = customerName;
            SalesInfo.Dgv.Rows[12].Cells["Value"].Value = reward;
            SalesInfo.Dgv.Rows[13].Cells["Value"].Value = saleCode;

        }
        /// <summary>
        /// 판매 제품 목록
        /// 판매 제품으 데이터 그리드에 추가
        /// </summary>
        /// <param name="code"></param>
        private void FillGridPdtList(int code)
        {
            DataTable resutlTable = new DataTable();
            DataTable procuctTable = new DataTable();
            string query = $"SELECT saled_pdt, saled_bprice, saled_sprice_krw, saled_sprice_usd, saled_dc, saled_qty, saled_amount_krw, saled_amount_usd, saled_tax, saled_point FROM saledetail WHERE saled_code = {code} ";
            dbconn.SqlDataAdapterQuery(query, resutlTable);
            int rowIndex = 0;
            foreach(DataRow row in resutlTable.Rows )
            {
                procuctTable.Clear();
                int pdtCode = Convert.ToInt32(row["saled_pdt"]);
                query = $"SELECT pdt_number, pdt_name_kr, pdt_name_en FROM product WHERE pdt_code  = {pdtCode}";
                dbconn.SqlReaderQuery(query, procuctTable);
                DataRow productRow = procuctTable.Rows[0];

                string pdtNumber = productRow["pdt_number"]?.ToString().Trim() ?? "";
                string pdtNameKr = productRow["pdt_name_kr"]?.ToString().Trim() ?? "";
                string pdtNameEn = productRow["pdt_name_en"]?.ToString().Trim() ?? "";
                decimal pdtBprice = cDataHandler.ConvertToDecimal(row["saled_bprice"]);
                int pdtSpriceKrw = cDataHandler.ConvertToInt(row["saled_sprice_krw"]);
                decimal pdtSpriceUsd = cDataHandler.ConvertToDecimal(row["saled_sprice_usd"]);
                int pdtAmountKrw = cDataHandler.ConvertToInt(row["saled_amount_krw"]);
                decimal pdtAmountUsd = cDataHandler.ConvertToDecimal(row["saled_amount_usd"]);
                int taxCode = cDataHandler.ConvertToInt(row["saled_tax"]);
                string tax = cStatusCode.GetTaxStatus(taxCode);
                int pdtPoint = cDataHandler.ConvertToInt(row["saled_point"]);

                PdtList.Dgv.Rows.Add();
                PdtList.Dgv.Rows[rowIndex].Cells["pdtCode"].Value = pdtCode;
                PdtList.Dgv.Rows[rowIndex].Cells["pdtNameKr"].Value = pdtNameKr;
                PdtList.Dgv.Rows[rowIndex].Cells["pdtNameEn"].Value = pdtNameEn;
                PdtList.Dgv.Rows[rowIndex].Cells["pdtNumber"].Value = pdtNumber;
                PdtList.Dgv.Rows[rowIndex].Cells["pdtSpriceKrw"].Value = pdtSpriceKrw;
                PdtList.Dgv.Rows[rowIndex].Cells["pdtSpriceUsd"].Value = pdtSpriceUsd;
                PdtList.Dgv.Rows[rowIndex].Cells["pdtQty"].Value = row["saled_qty"];
                PdtList.Dgv.Rows[rowIndex].Cells["pdtDc"].Value = row["saled_dc"];
                PdtList.Dgv.Rows[rowIndex].Cells["pdtPoint"].Value = row["saled_point"];
                PdtList.Dgv.Rows[rowIndex].Cells["pdtAmountKrw"].Value = pdtAmountKrw;
                PdtList.Dgv.Rows[rowIndex].Cells["pdtAmountUsd"].Value = pdtAmountUsd;
                PdtList.Dgv.Rows[rowIndex].Cells["pdtBprice"].Value = pdtBprice;
                PdtList.Dgv.Rows[rowIndex].Cells["pdtTax"].Value = tax;
                rowIndex++;
            }
            GridNumberSetting();
        }
        /// <summary>
        /// 판매 금액 확율 설정
        /// </summary>
        /// <returns></returns>
        private void GetConfig()
        {
            string query = "SELECT cf_value FROM config WHERE cf_code =  1";
            object resultObj = new object();
            dbconn.sqlScalaQuery(query, out resultObj);
            exchange = Convert.ToInt32(resultObj);
            query = "SELECT cf_value FROM config WHERE cf_code = 3";
            dbconn.sqlScalaQuery(query, out resultObj);
            cashRewardRate = cDataHandler.ConvertToDecimal(resultObj) / 1000 ;
            query = "SELECT cf_value FROM config WHERE cf_code = 4";
            dbconn.sqlScalaQuery(query, out resultObj);
            cardRewardRate = cDataHandler.ConvertToDecimal(resultObj) / 1000;
            query = "SELECT cf_value FROM config WHERE cf_code = 5";
            dbconn.sqlScalaQuery(query, out resultObj);
            acchRewardRate = cDataHandler.ConvertToDecimal(resultObj) / 1000;
            query = "SELECT cf_value FROM config WHERE cf_code = 6";
            dbconn.sqlScalaQuery(query, out resultObj);
            pointRewardRate = cDataHandler.ConvertToDecimal(resultObj) / 1000;
        }
        private void UpdateSalesInfo()
        {
            int rowCount = PdtList.Dgv.RowCount;
            amountKrw = 0;
            tax = 0;
            for (int count = 0; count < rowCount; count++)
            {
                amountKrw += PdtList.ConvertToInt(PdtList.Dgv.Rows[count].Cells["pdtAmountKrw"].Value);

                if(PdtList.ConvertToInt(PdtList.Dgv.Rows[count].Cells["pdtTax"].Value) == 1)
                {
                    tax += PdtList.ConvertToInt(PdtList.Dgv.Rows[count].Cells["pdtAmountKrw"].Value);
                }   
            }
            //PdtList.FormatAsInteger("pdtAmountKrw");
            amountUsd = Math.Round(Convert.ToDecimal(amountKrw) / Convert.ToDecimal(exchange),2);
            //SalesGridFill();
            cashKrw = 0;
            cashUsd = 0;
            CashUpdate(amountKrw, amountUsd);
            LablesUpdate();
        }
        /// <summary>
        /// 현금 금액 수정
        /// </summary>
        /// <param name="krw"></param>
        /// <param name="usd"></param>
        private void CashUpdate(int krw, decimal usd)
        {
            cashKrw += krw;
            cashUsd += usd;
        }
        /// <summary>
        /// 카드 결제 금액 등록
        /// </summary>
        private void PaymentCardRegist()
        {
            SalePayment salePayment = new SalePayment();
            salePayment.StartPosition = FormStartPosition.CenterParent;
            int krw = 0;
            double usd = 0;
            if (cardKrw == 0)
            {
                krw = cashKrw;
                usd = Convert.ToDouble(cashUsd);
            }
            else
            {
                krw = cardKrw;
                usd = Convert.ToDouble(cardUsd);
            }
            salePayment.GetPaymntInfo("카 드", krw, usd, exchange);
            salePayment.ForwardAmount += (resultKrw, resultUsd, resutlType) =>
            {
                if (resutlType == true)
                {
                    CardUpdate(resultKrw, resultUsd);
                }
                else
                {
                    return;
                };
            };
            salePayment.ShowDialog();
        }
        /// <summary>
        /// 카드 금액  및 현금 지불 대상 금액 수정
        /// </summary>
        /// <param name="krw"></param>
        /// <param name="usd"></param>
        private void CardUpdate(int krw, decimal usd)
        {
            int beforCardKrw = cardKrw;
            decimal beforCardUsd = cardUsd;
            cardKrw = krw;
            cardUsd = usd;
            if(krw == 0)
            {
                CashUpdate(beforCardKrw, beforCardUsd);
            }
            else
            {
                CashUpdate(krw * -1, usd * -1);
            }
            LablesUpdate();
        }
        /// <summary>
        /// 포인트 결제 등록
        /// </summary>
        private void PayPointRegist()
        {
            SalePayment salePayment = new SalePayment();
            salePayment.StartPosition = FormStartPosition.CenterParent;
            int krw = 0;
            double usd = 0;
            if (cardKrw == 0)
            {
                krw = custPoint;
                usd = Convert.ToDouble(custPoint / exchange);
            }
            else
            {
                krw = pointKrw;
                usd = Convert.ToDouble(pointUsd);
            }
            salePayment.GetPaymntInfo("포인트", krw, usd, exchange);
            salePayment.ForwardAmount += (resultKrw, resultUsd, resutlType) =>
            {
                if (resutlType == true)
                {
                    PointUpdate(resultKrw, resultUsd);
                }
                else
                {
                    return;
                };
            };
            salePayment.ShowDialog();
        }
        /// <summary>
        /// 포인트 금액  및 현금 지불 대상 금액 수정
        /// </summary>
        /// <param name="krw"></param>
        /// <param name="usd"></param>
        private void PointUpdate(int krw, decimal usd)
        {
            int beforPointKrw = pointKrw;
            decimal beforPointUsd = pointUsd;
            pointKrw = krw;
            pointUsd = usd;
            if (krw == 0 && custPoint >= krw)
            {
                CashUpdate(beforPointKrw, beforPointUsd);
            }
            else if( custPoint < krw)
            {
                MessageBox.Show("보유 포인트 보다 많은 포인트를 입력하였습니다", "알림");
                return;
            }
            else
            {
                CashUpdate(krw * -1, usd * -1);
            }
            LablesUpdate();
        }
        /// <summary>
        /// 계좌이체 결제 등록
        /// </summary>
        private void PaymentAccountRegist()
        {
            SalePayment salePayment = new SalePayment();
            salePayment.StartPosition = FormStartPosition.CenterParent;
            int krw = 0;
            double usd = 0;
            if (accountKrw == 0)
            {
                krw = cashKrw;
                usd = Convert.ToDouble(cashUsd);
            }
            else
            {
                krw = accountKrw;
                usd = Convert.ToDouble(accountUsd);
            }
            salePayment.GetPaymntInfo("계좌이체", krw, usd, exchange);
            salePayment.ForwardAmount += (resultKrw, resultUsd, resutlType) =>
            {
                if (resutlType == true)
                {
                    AccountUpdate(resultKrw, resultUsd);
                }
                else
                {
                    return;
                };
            };
            salePayment.ShowDialog();
        }
        /// <summary>
        /// 계좌이체 금액 및 현금 지불 대상 금액 수정
        /// </summary>
        /// <param name="krw"></param>
        /// <param name="usd"></param>
        private void AccountUpdate(int krw, decimal usd)
        {
            int beforAccountKrw = accountKrw;
            decimal beforAccountUsd = accountUsd;
            accountKrw = krw;
            accountUsd = usd;
            if (krw == 0)
            {
                CashUpdate(beforAccountKrw, beforAccountUsd);
            }
            else
            {
                CashUpdate(krw * -1, usd * -1);
            }
            LablesUpdate();
        }
        /// <summary>
        /// 배달비 설정
        /// </summary>
        private void DeliveryFeeRegist()
        {
            SalePayment salePayment = new SalePayment();
            salePayment.StartPosition = FormStartPosition.CenterParent;
            int krw = 0;
            double usd = 0;
            if (deliveryKrw == 0)
            {
                krw = 0;
                usd = 0;
            }
            else
            {
                krw = deliveryKrw;
                usd = Convert.ToDouble(deliveryUsd);
            }
            salePayment.GetPaymntInfo("배송비", krw, usd, exchange);
            salePayment.ForwardAmount += (resultKrw, resultUsd, resutlType) =>
            {
                if (resutlType == true)
                {
                    DeliveryUpdate(resultKrw, resultUsd);
                }
                else
                {
                    return;
                };
            };
            salePayment.ShowDialog();
        }
        private void DeliveryUpdate(int krw, decimal usd)
        {
            int beforAccountKrw = deliveryKrw;
            decimal beforAccountUsd = deliveryUsd;
            deliveryKrw = krw;
            deliveryUsd = usd;
            if (krw == 0)
            {
                CashUpdate(beforAccountKrw * -1, beforAccountUsd * - 1);
            }
            else
            {
                CashUpdate(krw, usd);
            }
            LablesUpdate();
        }
        /// <summary>
        /// 제품 수량 수정 
        /// </summary>
        /// <param name="index"></param> 수정해야 할 제품의 인덱스를 선택
        /// <param name="isScanned"></param> 상품의 검색여부를 나타난대다. 제품번호 부분에 번호를 입력 하여 추가 하였을 경우 true, 제품 추가 버튼을 클릭하여 검색한 경우 fales
        private void QuantityUpdata(int index, bool isScanned)
        {
            decimal qty = PdtList.ConvertToDecimal(PdtList.Dgv.Rows[index].Cells["pdtQty"].Value);
            decimal spriceKrw = PdtList.ConvertToDecimal(PdtList.Dgv.Rows[index].Cells["pdtSpriceKrw"].Value);
            decimal amountKrw = 0;
            decimal amountUsd = 0;
            decimal DC = PdtList.ConvertToDecimal(PdtList.Dgv.Rows[index].Cells["pdtDC"].Value);
            if(isScanned == false)
            {
                NumericInputDialog numericInputDialog = new NumericInputDialog();
                numericInputDialog.StartPosition = FormStartPosition.CenterParent;
                numericInputDialog.GetValue("수량", qty, false);
                numericInputDialog.ValueSubmit += (decimal value) => { qty = Convert.ToInt32(value); };
                numericInputDialog.ShowDialog();
            }
            else
            {
                qty++;
            }
            amountKrw = qty * spriceKrw - DC;
            amountUsd = amountKrw / Convert.ToDecimal(exchange);
            PdtList.Dgv.Rows[index].Cells["pdtQty"].Value = qty;
            PdtList.Dgv.Rows[index].Cells["pdtAmountKrw"].Value = amountKrw;
            PdtList.Dgv.Rows[index].Cells["pdtAmountUsd"].Value = amountUsd;
        }

        /// <summary>
        /// 할인액 설정
        /// </summary>
        private void DiscountSet()
        {
            decimal DC = 0;
            decimal pdtSpriceKrw = PdtList.ConvertToDecimal(PdtList.Dgv.CurrentRow.Cells["pdtSpriceKrw"].Value);
            decimal pdtSpriceUsd = PdtList.ConvertToDecimal(PdtList.Dgv.CurrentRow.Cells["pdtSpriceUsd"].Value);
            decimal pdtAmountKrw = 0;
            decimal pdtAamountUsd = 0;
            decimal qty = PdtList.ConvertToDecimal(PdtList.Dgv.CurrentRow.Cells["pdtQty"].Value);
            string typeString = "할인율";
            bool typeBool = true;
            DialogResult typeResult = MessageBox.Show("할인 금액을 직접 입력 하시겠습니까?", "알림", MessageBoxButtons.YesNo);
            
            if(typeResult == DialogResult.Yes)
            {
                typeString = "할인액";
                typeBool = false;
                SalePayment salePayment = new SalePayment();
                salePayment.StartPosition = FormStartPosition.CenterParent;
                salePayment.GetPaymntInfo("할인액", 0, 0, exchange);
                salePayment.ForwardAmount += (resultKrw, resultUsd, resutlType) =>
                {
                    if (resutlType == true)
                    {
                        DC = resultKrw;
                    }
                    else
                    {
                        return;
                    };
                };
                salePayment.ShowDialog();
                
            }
            
            else
            {
                NumericInputDialog numericInputDialog = new NumericInputDialog();
                numericInputDialog.StartPosition = FormStartPosition.CenterParent;
                numericInputDialog.GetValue(typeString, DC, typeBool);
                numericInputDialog.ValueSubmit += (decimal value) => { DC = value; };
                numericInputDialog.ShowDialog();
                DC = pdtSpriceKrw * qty * (DC/100);
            }
            pdtAmountKrw = pdtSpriceKrw * qty - DC;
            pdtAamountUsd = pdtAmountKrw / exchange;
            PdtList.Dgv.CurrentRow.Cells["pdtDC"].Value = DC;
            PdtList.Dgv.CurrentRow.Cells["pdtAmountKrw"].Value = Convert.ToInt32(pdtAmountKrw);
            PdtList.Dgv.CurrentRow.Cells["pdtAmountUsd"].Value = Convert.ToDecimal(pdtAamountUsd);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBoxWordh_KeyDownEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SearchQuery();
                UpdateSalesInfo();
            }
        }
        private bool DuplicateCheck(int pdtCode)
        {
            for(int i = 0; i < PdtList.Dgv.RowCount; i++)
            {
                if(PdtList.ConvertToInt(PdtList.Dgv.Rows[i].Cells["pdtCode"].Value) == pdtCode )
                {
                    QuantityUpdata(i, true);
                    return true;  // 중복 발견, true 반환
                }
            }
            return false;  // 중복이 없으면 false 반환
        }
    
        private void SearchQuery()
        {
            //int resultCount = 0;
            string keyword = tBoxWord.Text;
            string query = string.Format("SELECT top 1 pdt_code FROM product WHERE pdt_number  = '{0}'", keyword);

            //DataTable dataTable = new DataTable();
            //dbconn.SqlReaderQuery(query, dataTable);
            //resultCount = dataTable.Rows.Count;

            Object resultObj = new object();
            dbconn.sqlScalaQuery(query, out resultObj);
            if(resultObj == null)
            {
                MessageBox.Show("일치하는 상품이 검색되지 않았습니다", "알림");
            }
            
            else
            {
                int pdtCode = Convert.ToInt32(resultObj);
                //DataRow row = dataTable.Rows[0];
                if (!DuplicateCheck(pdtCode))
                {
                    // 중복이 없을 경우에만 상품을 추가
                    AddProductToGrid(pdtCode);
                }
                //AddProductToGrid(Convert.ToInt32(resultObj));//AddProductToGrid(Convert.ToInt32(row[0]));
                tBoxWord.Clear();
            }
 
        }
        /// <summary>
        /// 회원 정보 불러오기
        /// </summary>
        private void LoadCustomerInfo()
        {
            if(custormCheck == false)
            {
                CustomerSearchBox customerSearchBox = new CustomerSearchBox();
                customerSearchBox.StartPosition = FormStartPosition.CenterParent;
                customerSearchBox.GetCustomerCode += (custCode) => { customerCode = (custCode); };
                customerSearchBox.ShowDialog();   
            }
            if (customerCode != 0)
            {
                string query = "SELECT cust_name, cust_point FROM customer WHERE cust_code = " + customerCode;
                DataTable resultTable = new DataTable();
                object resultObj = new object();
                dbconn.SqlReaderQuery(query, resultTable);
                DataRow row = resultTable.Rows[0];
                
                customerName = row["cust_name"].ToString();
                custPoint = !string.IsNullOrWhiteSpace(row["cust_point"]?.ToString()) ? Convert.ToInt32(row["cust_point"]) : 0;
                lblCustName.Text = customerName;
                lblCustPoint.Text = custPoint.ToString("#,#00");
                query = $"SELECT ISNULL(MAX(ph_seq),0) + 1 FROM pointhistory WHERE ph_cust = {customerCode}";
                dbconn.sqlScalaQuery(query, out resultObj);
                pointSeq = cDataHandler.ConvertToInt(resultObj);
                //SalesGridFill();
                customerToggle(true);
            }
        }
        /// <summary>
        /// 회원 지정 취소
        /// </summary>
        private void ClearCustomerInfo()
        {
            DialogResult result = MessageBox.Show("지정된 회원 정보를 비회원으로 변경 하시겠습니까?", "알림", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }
            customerToggle(false);
            
        }
        /// <summary>
        /// 제품추가 함수
        /// </summary>
        /// <param name="pdtCode"></param>
        private void AddProductToGrid(int pdtCode)
        {
            PdtList.Dgv.Rows.Add();
            int index = PdtList.Dgv.RowCount - 1;
            string query = "SELECT pdt_name_kr, pdt_name_en, pdt_number, pdt_sprice_krw, pdt_sprice_usd, pdt_bprice, pdt_tax, pdt_point FROM product WHERE pdt_code = " + pdtCode;
            DataTable dataTable = new DataTable();
            dbconn.SqlReaderQuery(query, dataTable);
            DataRow dataRow = dataTable.Rows[0];

            string pdtNumber = dataRow["pdt_number"]?.ToString().Trim() ?? "";
            string pdtNameKr = dataRow["pdt_name_kr"]?.ToString().Trim() ?? "";
            string pdtNameEn = dataRow["pdt_name_en"]?.ToString().Trim() ?? "";
            decimal pdtBprice = dataRow["pdt_bprice"] != DBNull.Value ? Convert.ToDecimal(dataRow["pdt_bprice"]) : 0;
            int pdtSpriceKrw = dataRow["pdt_sprice_krw"] != DBNull.Value ? Convert.ToInt32(dataRow["pdt_sprice_krw"]) : 0;
            decimal pdtSpriceUsd = dataRow["pdt_sprice_usd"] != DBNull.Value ? Convert.ToDecimal(dataRow["pdt_sprice_usd"]) : 0;
            int taxCode = dataRow["pdt_tax"] != DBNull.Value ? Convert.ToInt32(dataRow["pdt_tax"]) : 1;
            string tax = cStatusCode.GetTaxStatus(taxCode);
            int pdtPoint = cDataHandler.ConvertToInt(dataRow["pdt_point"]);

            PdtList.Dgv.Rows[index].Cells["No"].Value = index + 1;
            PdtList.Dgv.Rows[index].Cells["pdtCode"].Value = pdtCode;
            PdtList.Dgv.Rows[index].Cells["pdtNameKr"].Value = pdtNameKr;
            PdtList.Dgv.Rows[index].Cells["pdtNameEn"].Value = pdtNameEn;
            PdtList.Dgv.Rows[index].Cells["pdtNumber"].Value = pdtNumber;
            PdtList.Dgv.Rows[index].Cells["pdtSpriceKrw"].Value = pdtSpriceKrw;
            PdtList.Dgv.Rows[index].Cells["pdtSpriceUsd"].Value = pdtSpriceUsd;
            PdtList.Dgv.Rows[index].Cells["pdtQty"].Value = 1;
            PdtList.Dgv.Rows[index].Cells["pdtDc"].Value = 0;
            PdtList.Dgv.Rows[index].Cells["pdtPoint"].Value = pdtPoint;
            PdtList.Dgv.Rows[index].Cells["pdtAmountKrw"].Value = pdtSpriceKrw;
            PdtList.Dgv.Rows[index].Cells["pdtAmountUsd"].Value = pdtSpriceUsd;
            PdtList.Dgv.Rows[index].Cells["pdtBprice"].Value = pdtBprice;
            PdtList.Dgv.Rows[index].Cells["pdtTaxCode"].Value = taxCode;
            PdtList.Dgv.Rows[index].Cells["pdtTax"].Value = tax;

            int AddCashKrw = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtAmountKrw"].Value);
            decimal AddCashUsd= PdtList.ConvertToDecimal(PdtList.Dgv.Rows[index].Cells["pdtAmountUsd"].Value);
            UpdateSalesInfo();
            //CashUpdate(AddCashKrw, AddCashUsd);
            PdtList.Dgv.CurrentCell = PdtList.Dgv.Rows[index].Cells[0];
           
        }
        /// <summary>
        /// 주문서에서 판매전환시 상품정보와 회원 정보 받아오기
        /// </summary>
        /// <param name="orderTable"></param>
        /// <param name="custCode"></param>
        public void ForwardOrder(DataTable prodcutTable, DataTable salesInfoTable)
        {
            ConvertOrderToSale(prodcutTable);
            DataRow dataRow = salesInfoTable.Rows[0];
            customerCode = Convert.ToInt32(dataRow["custCode"]);
            custOrderCode = Convert.ToInt32(dataRow["custOrderCode"]);
            if (customerCode != 0)
            { 
                customerToggle(true);
                LoadCustomerInfo();
            }
            else
            {
                customerToggle(false);
            }
            DeliveryRecipient = customerName;
            DeliveryAddr = dataRow["address"].ToString();
            deliveryCountry = Convert.ToInt32(dataRow["countryCode"]);
            deliveryKrw = Convert.ToInt32(dataRow["delivyerfeeKrw"]);
            deliveryUsd = Convert.ToDecimal(dataRow["delivyerfeeUsd"]);
        }
        /// <summary>
        /// 주문서 내역 판매 제품으로 등록
        /// </summary>
        /// <param name="prodcutTable"></param>
        private void ConvertOrderToSale(DataTable prodcutTable)
        {
            int rowIndex = 0;
            foreach(DataRow row in prodcutTable.Rows)
            {
                int pdtCode = Convert.ToInt32(row["custOrddPdtCode"]);
                AddProductToGrid(pdtCode);

                int qty = Convert.ToInt32(row["custOrddOfferQty"]);
                int Amountkrw = Convert.ToInt32(row["custOrddOfferAmounteKrw"]);
                int pdtAmount = PdtList.ConvertToInt(PdtList.Dgv.Rows[rowIndex].Cells["pdtSpriceKrw"].Value) * qty;


                PdtList.Dgv.Rows[rowIndex].Cells["pdtQty"].Value = qty ;
                PdtList.Dgv.Rows[rowIndex].Cells["pdtAmountKrw"].Value = Amountkrw;
                PdtList.Dgv.Rows[rowIndex].Cells["pdtAmountUsd"].Value = row["custOrddOfferAmounteUsd"];
                PdtList.Dgv.Rows[rowIndex].Cells["pdtDc"].Value = pdtAmount - Amountkrw;
                rowIndex++;
            }
        }
        /// <summary>
        /// 그리드에서 선택된 제품 제거
        /// </summary>
        private void DeleteProductToGrid()
        {
            if (PdtList.Dgv.CurrentRow != null && !PdtList.Dgv.CurrentRow.IsNewRow)
            {
                DataGridViewRow row = PdtList.Dgv.CurrentRow;
                PdtList.Dgv.Rows.Remove(row);
                GridNumberSetting();
                UpdateSalesInfo();
                
            }
            else
            {
                MessageBox.Show("삭제할 제품을 선택 하세요", "알림");
            }
                
        }
        /// <summary>
        /// 판품 메소드
        /// 판매조회 후 활성화된 반품 버튼 클릭시 실행
        /// </summary>
        private void AdjustForReturn()
        {
            DialogResult result = MessageBox.Show("기존 배송비도 포함하여 환불 하시겠습니까?\n아니요를 누를 경우 청구할 배송비를 입력 하실 수 있습니다", "알림", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                deliveryKrw = deliveryKrw * -1;
            }
            else if(result == DialogResult.No)
            {
                DeliveryFeeRegist();
            }
            else
            {
                return;
            }
            
            saleType = 2;
            amountKrw = amountKrw * -1;
            amountUsd = amountUsd * -1;
            cashKrw = cashKrw * -1;
            cashUsd = cashUsd * -1;
            accountKrw = accountKrw * -1;
            accountUsd = accountUsd * -1;
            cardKrw = cardKrw * -1;
            cardUsd = cardUsd * -1;
            pointKrw = pointKrw * -1;
            pointUsd = pointUsd * -1;
            reward = reward * -1;
            origineCode = saleCode;
            discount = discount * -1;
            tax = tax * 1;

            for (int index = 0; index < PdtList.Dgv.RowCount; index++)
            {
                int qty = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtQty"].Value);
                int bprice = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtBprice"].Value); 
                int pdtspriceKrw = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtSpriceKrw"].Value); 
                decimal pdtspriceUsd = PdtList.ConvertToDecimal(PdtList.Dgv.Rows[index].Cells["pdtSpriceUsd"].Value);
                int pdtAmountKrw = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtAmountKrw"].Value); 
                decimal pdtAmountUsd = PdtList.ConvertToDecimal(PdtList.Dgv.Rows[index].Cells["pdtAmountUsd"].Value);
                int dicscount = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtQty"].Value); 
                int pdtPoint = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtPoint"].Value);

                PdtList.Dgv.Rows[index].Cells["pdtQty"].Value = qty * -1;
                PdtList.Dgv.Rows[index].Cells["pdtBprice"].Value = bprice * -1;
                PdtList.Dgv.Rows[index].Cells["pdtSpriceKrw"].Value = pdtspriceKrw * -1;
                PdtList.Dgv.Rows[index].Cells["pdtSpriceUsd"].Value = pdtspriceUsd * -1;
                PdtList.Dgv.Rows[index].Cells["pdtPoint"].Value = pdtPoint ;
                //제품 포인트 적립 여부 추가
                PdtList.Dgv.Rows[index].Cells["pdtAmountKrw"].Value = pdtAmountKrw * -1;
                PdtList.Dgv.Rows[index].Cells["pdtAmountUsd"].Value = pdtAmountUsd * -1;
                PdtList.Dgv.Rows[index].Cells["pdtQty"].Value = dicscount * -1;

            }
        }
        private void PointReward()
        {
            if(customerCode.Equals(0))
            {
                return;    
            }
            int dgvCounter = PdtList.Dgv.Rows.Count;
            int savingPoint = 0;
            for (int i = 0; i <= dgvCounter - 1; i++)
            {
                if(cDataHandler.ConvertToInt(PdtList.Dgv.Rows[i].Cells["pdtPoint"].Value) == 1)
                {
                    savingPoint += cDataHandler.ConvertToInt(PdtList.Dgv.Rows[i].Cells["pdtAmountKrw"].Value);
                }
            }
            decimal amount = Convert.ToDecimal(amountKrw);
            decimal cashRate = Convert.ToDecimal(cashKrw) / amount;
            decimal cardRate = Convert.ToDecimal(cardKrw) / amount;
            decimal accRate = Convert.ToDecimal(accountKrw) / amount;
            decimal pointRate = Convert.ToDecimal(pointKrw) / amount;
            decimal cashReward = Convert.ToDecimal(savingPoint) * cashRate * (cashRewardRate);
            decimal cardReward = Convert.ToDecimal(savingPoint) * cardRate * (cardRewardRate);
            decimal accReward = Convert.ToDecimal(savingPoint) * accRate * (acchRewardRate);
            decimal pointReward = Convert.ToDecimal(savingPoint) * pointRate * (pointRewardRate);
            reward = Convert.ToInt32(cashReward + cardReward + accReward + pointReward);
        }
        /// <summary>
        /// 판매등록
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void InsertSaleVoucher(SqlConnection connection, SqlTransaction transaction)
        {
            PointReward();
            string query = "SELECT ISNULL(MAX(sale_code),0) + 1 FROM sales";
            object resultObj = new object();
            dbconn.sqlScalaQuery(query, out resultObj);
            saleCode = Convert.ToInt32(resultObj);
            saleDate = DateTime.Now;
            query = "INSERT INTO sales (sale_code,  sale_date, sale_cust, sale_type, sale_bprice, sale_sprice_krw, sale_sprice_usd, sale_dc, sale_tax, sale_reward,sale_delivery,sale_delfee,sale_udate, sale_origine, sale_emp)" +
                            "\nValues(@saleCode, @date, @cust, @type, @bprice, @spriceKrw, @spcieUsd, @dc, @tax, @reward, 0, @fee , GETDATE(), @origine, @emp)";
            SqlParameter[] salesParameter =
            {
                new SqlParameter("@saleCode",SqlDbType.Int){Value = saleCode},
                new SqlParameter("@date",SqlDbType.DateTime){Value = saleDate},
                new SqlParameter("@cust",SqlDbType.Int){Value = customerCode},
                new SqlParameter("@type",SqlDbType.Int){Value = saleType},
                new SqlParameter("@bprice",SqlDbType.Int){Value = bprice},
                new SqlParameter("@spriceKrw",SqlDbType.Int){Value = amountKrw},
                new SqlParameter("@spcieUsd",SqlDbType.Float){Value = amountUsd},
                new SqlParameter("@dc",SqlDbType.Int){Value = discount},
                new SqlParameter("@tax",SqlDbType.Int){Value = tax},
                new SqlParameter("@reward",SqlDbType.Int){Value = reward},
                new SqlParameter("@fee",SqlDbType.Int){Value = deliveryKrw},
                new SqlParameter("@origine",SqlDbType.Int){Value = origineCode},
                new SqlParameter("@emp",SqlDbType.Int){Value = accessedEmp}

            };
            dbconn.ExecuteNonQuery(query, connection, transaction, salesParameter);
            if(custOrderCode != 0)
            {
                query = "UPDATE custorder SET cord_sales = @saleCode, cord_udate = GETDATE() ";
                SqlParameter updateParameter = new SqlParameter("@saleCode", SqlDbType.Int) { Value = saleCode };
                dbconn.ExecuteNonQuery(query, connection, transaction, updateParameter);
            }
        }
        private void SaleOrigneCodeUpdate(SqlConnection connection, SqlTransaction transaction)
        {
            string query = $"UPDATE sales SET sale_origine = @origineCode WHERE sale_code = @saleCode";
            SqlParameter[] parameters =
            {
                new SqlParameter("@saleCode",SqlDbType.Int){Value = origineCode},
                new SqlParameter("@origineCode",SqlDbType.Int){Value = saleCode}
            };
            dbconn.ExecuteNonQuery(query, connection, transaction, parameters);
        }
        /// <summary>
        /// 판매 결제 내역 등록
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void InsertSalepayent(SqlConnection connection, SqlTransaction transaction)
        {
            string query = "SELECT ISNULL(MAX(spay_code),0) + 1 FROM salepay";
            object resultObj = new object();
            dbconn.sqlScalaQuery(query, out resultObj);
            paymentCode = Convert.ToInt32(resultObj);
            query = "INSERT INTO salepay (spay_code, spay_cash_krw, spay_cash_use, spay_account_krw, spay_account_usd, spay_credit_krw, spay_credit_usd, spay_point_krw, spay_point_usd, spay_exchenge, spay_salecode, spay_idate, spay_udate) " +
                "\nVALUES(@payCode, @cashKrw, @cashUse, @accountKrw, @accountUsd, @creditKrw, @creditUsd, @pointKrw, @pointUsd, @exchenge, @saleCode, GETDATE(), GETDATE())";
            SqlParameter[] paymentParameter =
            {
                new SqlParameter("@payCode",SqlDbType.Int){Value = paymentCode},
                new SqlParameter("@cashKrw",SqlDbType.Int){Value = cashKrw},
                new SqlParameter("@cashUse",SqlDbType.Float){Value = cashUsd},
                new SqlParameter("@accountKrw",SqlDbType.Int){Value = accountKrw},
                new SqlParameter("@accountUsd",SqlDbType.Float){Value = accountUsd},
                new SqlParameter("@creditKrw",SqlDbType.Int){Value = cardKrw},
                new SqlParameter("@creditUsd",SqlDbType.Float){Value = cardUsd},
                new SqlParameter("@pointKrw",SqlDbType.Int){Value = pointKrw},
                new SqlParameter("@pointUsd",SqlDbType.Float){Value = pointUsd},
                new SqlParameter("@exchenge",SqlDbType.Int){Value = exchange},
                new SqlParameter("@saleCode",SqlDbType.Int){Value = saleCode}
            };
            dbconn.ExecuteNonQuery(query, connection, transaction, paymentParameter);
            if (custormCheck == true && !pointKrw.Equals(0))
            {
                
                int usedPoint = pointKrw * -1;
                //query = $"UPDATE customer SET cust_lastsaledate = GETDATE(), cust_point = cust_point + @usePoint WHERE cust_code = {customerCode};";
                //SqlParameter[] sqlParameters =
                //{
                //    new SqlParameter("@usePoint",SqlDbType.Int){Value = usedPoint}
                //};
                //dbconn.ExecuteNonQuery(query, connection, transaction, sqlParameters);
                UpdateCustPoint(connection, transaction, usedPoint);
                InsertCustPointHistry(connection, transaction,2, usedPoint);
                custPoint += usedPoint;
                pointSeq++;

            }
            if (custormCheck == true && !reward.Equals(0))
            {
                //query = $"UPDATE customer SET cust_lastsaledate = GETDATE(), cust_point = cust_point + @rewardPoint WHERE cust_code = {customerCode};";
                //SqlParameter[] custmerParameter =
                //{
                //    new SqlParameter("@rewardPoint",SqlDbType.Int){Value = reward}
                //};
                //dbconn.ExecuteNonQuery(query, connection, transaction, custmerParameter);
                UpdateCustPoint(connection, transaction, reward);
                InsertCustPointHistry(connection, transaction,1, reward);
            }
        }
        private void UpdateCustPoint(SqlConnection connection, SqlTransaction transaction,int updatePoint)
        {
            string query = $"UPDATE customer SET cust_point = cust_point + @rewardPoint WHERE cust_code = {customerCode};";
            SqlParameter[] custmerParameter =
            {
                    new SqlParameter("@rewardPoint",SqlDbType.Int){Value = updatePoint}
            };
            dbconn.ExecuteNonQuery(query, connection, transaction, custmerParameter);
        }
        private void InsertCustPointHistry(SqlConnection connection, SqlTransaction transaction, int type, int rewardPoint)
        {
            string query = $"SELECT cust_point FROM customer WHERE cust_code = {customerCode}";
            query = "INSERT INTO pointhistory(ph_type, ph_cust, ph_param, ph_point, ph_previous, ph_date, ph_seq)" +
                "VALUES(@type, @cust, @param, @point, @previous, GETDATE(),@seq)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@type",SqlDbType.Int){Value = type },
                new SqlParameter("@cust",SqlDbType.Int){Value = customerCode},
                new SqlParameter("@param",SqlDbType.Int){Value = saleCode},
                new SqlParameter("@point",SqlDbType.Int){Value = rewardPoint},
                new SqlParameter("@previous",SqlDbType.Int){Value = custPoint},
                new SqlParameter("@seq",SqlDbType.Int){Value = pointSeq}
            };
            dbconn.ExecuteNonQuery(query, connection, transaction, parameters);
        }
        /// <summary>
        /// 배송정보 DB에 기록
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private void InsertDelivery(SqlConnection connection, SqlTransaction transaction)
        {
            string query = "SELECT ISNULL(MAX(del_code),0) + 1 FROM delivery";
            object resultObj = new object();
            dbconn.sqlScalaQuery(query, out resultObj);
            deliveryCode = Convert.ToInt32(resultObj);
            query = "INSERT INTO delivery(del_code, del_cust,del_country,del_addr,del_recipient,del_tel,del_invoice,del_salecode,del_idate,del_udate) " +
                "\nVALUES(@delcode,  @cust, @country, @addr, @recipient, @tel, @invoice, @saleCode, GETDATE(), GETDATE())";
            SqlParameter[] deliveryParameter =
            {
                new SqlParameter("@delCode",SqlDbType.Int){Value = deliveryCode},
                new SqlParameter("@cust",SqlDbType.Int){Value = customerCode},
                new SqlParameter("@country",SqlDbType.Int){Value = deliveryCountry},
                new SqlParameter("@addr",SqlDbType.VarChar){Value = DeliveryAddr},
                new SqlParameter("@recipient",SqlDbType.VarChar){Value = DeliveryRecipient},
                new SqlParameter("@tel",SqlDbType.VarChar){Value = DeliveryTel},
                new SqlParameter("@invoice",SqlDbType.VarChar){Value = DeliveryInvoice},
                new SqlParameter("@saleCode",SqlDbType.Int){Value = saleCode}
            };
            dbconn.ExecuteNonQuery(query, connection, transaction, deliveryParameter);

            query = "UPDATE sales SET sale_delivery = @delCode WHERE sale_code = @code";
            SqlParameter[] updateParameter =
            {
                new SqlParameter("@delCode",SqlDbType.Int){Value = deliveryCode},
                new SqlParameter("@code",SqlDbType.Int){Value = saleCode}
            };

            dbconn.ExecuteNonQuery(query, connection, transaction, updateParameter);
        }
        
        private void UpdateDelivery(SqlConnection connection, SqlTransaction transaction)
        {
            
            string query = $"UPDATE delivery SET del_cust = @cust, del_country = @country, del_addr = @addr, del_recipient = @recipient, del_tel = @tel, del_invoice = @invoice, del_udate = GETDATE() WHERE del_code = @delCode";
            SqlParameter[] deliveryParameter =
            {
                new SqlParameter("@delCode",SqlDbType.Int){Value = deliveryCode},
                new SqlParameter("@cust",SqlDbType.Int){Value = customerCode},
                new SqlParameter("@country",SqlDbType.Int){Value = deliveryCountry},
                new SqlParameter("@addr",SqlDbType.VarChar){Value = DeliveryAddr},
                new SqlParameter("@recipient",SqlDbType.VarChar){Value = DeliveryRecipient},
                new SqlParameter("@tel",SqlDbType.VarChar){Value = DeliveryTel},
                new SqlParameter("@invoice",SqlDbType.VarChar){Value = DeliveryInvoice},
                
            };
            dbconn.ExecuteNonQuery(query, connection, transaction, deliveryParameter);

            query = "UPDATE sales SET sale_udate = GETDATE() WHERE sale_code = @code";
            SqlParameter[] updateParameter =
            {
                new SqlParameter("@code",SqlDbType.Int){Value = saleCode}
            };

            dbconn.ExecuteNonQuery(query, connection, transaction, updateParameter);
        }
        /// <summary>
        /// 판매 제품 내역 등록
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>        
        private void InsertSaleDetail(SqlConnection connection, SqlTransaction transaction)
        {
            string query;
            for(int index =0; index < PdtList.Dgv.RowCount; index++)
            {
                query = "INSERT INTO saledetail(saled_code, saled_pdt,saled_bprice, saled_sprice_krw, saled_sprice_usd, saled_dc, saled_qty, saled_amount_krw, saled_amount_usd, saled_tax, saled_point)" +
                "\nVALUES(@saleCode, @pdt, @bprice, @sprice_krw, @sprice_usd, @dc, @qty, @amount_krw, @amount_usd, @tax, @point)";
                var pdtCode = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtCode"].Value);
                var pdtBprice = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtBprice"].Value);
                var pdtSpriceKrw = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtSpriceKrw"].Value);
                var pdtSpriceUsd = PdtList.ConvertToDecimal(PdtList.Dgv.Rows[index].Cells["pdtSpriceUsd"].Value);
                var pdtDc = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtDc"].Value);
                var pdtQty = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtQty"].Value);
                var pdtAmountKrw = PdtList.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtAmountKrw"].Value);
                var pdtAmountUsd = PdtList.ConvertToDecimal(PdtList.Dgv.Rows[index].Cells["pdtAmountUsd"].Value);
                var pdtTax = cStatusCode.GetTaxStatusCode(PdtList.Dgv.Rows[index].Cells["pdtTax"].Value.ToString());
                var pdtPoint = cDataHandler.ConvertToInt(PdtList.Dgv.Rows[index].Cells["pdtPoint"].Value);
                SqlParameter[] parameters =
                {
                    new SqlParameter("@saleCode",SqlDbType.Int){Value = saleCode},
                    new SqlParameter("@pdt",SqlDbType.Int){Value = pdtCode},
                    new SqlParameter("@bprice",SqlDbType.Int){Value = pdtBprice},
                    new SqlParameter("@sprice_krw",SqlDbType.Int){Value = pdtSpriceKrw},
                    new SqlParameter("@sprice_usd",SqlDbType.Float){Value = pdtSpriceUsd},
                    new SqlParameter("@dc",SqlDbType.Int){Value = pdtDc},
                    new SqlParameter("@qty",SqlDbType.Int){Value = pdtQty},
                    new SqlParameter("@amount_krw",SqlDbType.Int){Value = pdtAmountKrw},
                    new SqlParameter("@amount_usd",SqlDbType.Float){Value = pdtAmountUsd},
                    new SqlParameter("@tax",SqlDbType.Int){Value = pdtTax},
                    new SqlParameter("@point",SqlDbType.Int){Value = pdtPoint}
                };
                dbconn.ExecuteNonQuery(query, connection, transaction, parameters);
                UpdateProductStock(connection, transaction, pdtCode, pdtQty);
            }
        }
        private void UpdateProductStock(SqlConnection connection, SqlTransaction transaction, int productCode, int qty)
        {
            //string query = string.Format("UPDATE product SET pdt_stock = {0} WHERE pdt_code =  {1}", qty, productCode);
            //dbconn.NonQuery(query, connection, transaction);

            string query = "UPDATE product SET pdt_stock = pdt_stock + @qty WHERE pdt_code = @pdtCode";
            SqlParameter[] parameters =
            {
                new SqlParameter("@qty",SqlDbType.Int){Value = qty * -1},
                new SqlParameter("@pdtCode",SqlDbType.Int){ Value = productCode }
            };
            dbconn.ExecuteNonQuery(query, connection, transaction, parameters);
        }
        /// <summary>
        /// 배송정보 입력
        /// </summary>
        private void DeliveryInfoRegist()
        {
            DeliveryRegist deliveryRegist = new DeliveryRegist();
            deliveryRegist.StartPosition = FormStartPosition.CenterParent;
            if(deliveryCode == 0)
            {
                deliveryRegist.NewDelivery(customerCode);
            }   
            else
            {
                deliveryRegist.GetDeliveryCode(deliveryCode);
            }
            
            deliveryRegist.ForwardDeliveryInfo += (address, recipient, tell, invoice, country, Whether) => { GetDeliveryInfo(address, recipient, tell, invoice, country, Whether); };
            deliveryRegist.ShowDialog();
            if (deliveryCode != 0 && DeliveryWhether == true)
            {
                using (SqlConnection connection = dbconn.Opensql())
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        UpdateDelivery(connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }
        private void GetDeliveryInfo(string address, string reciient, string tel, string invoice, int country, bool Whether )
        {
            DeliveryAddr = address;
            DeliveryRecipient = reciient;
            DeliveryTel = tel;
            DeliveryInvoice = invoice;
            deliveryCountry = country;
            DeliveryWhether = Whether;
        }
        private void bntAddProduct_Click(object sender, EventArgs e)
        {
            int procudtCode = 0;
            ProductSearchBox productSearchBox = new ProductSearchBox();
            productSearchBox.StartPosition = FormStartPosition.CenterParent;
            productSearchBox.ProductForword += (pdtCode) => { procudtCode=pdtCode;};

            productSearchBox.ShowDialog();
            if (procudtCode != 0)
            {
                AddProductToGrid(procudtCode);
                UpdateSalesInfo();
            }

        }

        private void bntProductDelete_Click(object sender, EventArgs e)
        {
            DeleteProductToGrid();
        }
        
        private void bntDC_Click(object sender, EventArgs e)
        {
            if (PdtList.Dgv.CurrentRow == null || PdtList.Dgv.CurrentRow.Cells["pdtDC"].Value == null)
            {
                return; // 값이 없으면 메서드 종료 (아무 작업도 하지 않음)
            }

            DiscountSet();
            UpdateSalesInfo();
            
        }

        private void bntCustomer_Click(object sender, EventArgs e)
        {
            if(customerCode == 0)
            {
                LoadCustomerInfo();
            }
            else
            {
                ClearCustomerInfo();
            }
                
        }

        private void bntClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bntQty_Click(object sender, EventArgs e)
        {
            // PdtList에서 현재 선택된 셀이나 행이 있는지 확인
            if (PdtList.Dgv.CurrentRow == null || PdtList.Dgv.CurrentRow.Cells["pdtQty"].Value == null)
            {
                return; // 값이 없으면 메서드 종료 (아무 작업도 하지 않음)
            }
            int rowIndex = PdtList.Dgv.CurrentRow.Index;
            QuantityUpdata(rowIndex,false);
            UpdateSalesInfo();
            
        }

        private void bntDelivery_Click(object sender, EventArgs e)
        {
            if(cashKrw == 0 && cardKrw != 0 || accountKrw != 0 || pointKrw != 0)
            {
                MessageBox.Show("결제를 완료 하시거나 지정된 결제유형을 취소한 후 다시 시도해주세요", "알림");
                return;
            }
            
            DeliveryFeeRegist();
        }

        private void bntSaleRegits_Click(object sender, EventArgs e)
        {
            if(PdtList.Dgv.RowCount == 0)
            {
                MessageBox.Show("판매 등록 할 상품이 없습니다", "알림");
                return;
            }
            else if (MessageBox.Show("저장 하시겠습니까?", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection connection = dbconn.Opensql())
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {

                        InsertSaleVoucher(connection, transaction);
                        InsertSalepayent(connection, transaction);
                        InsertSaleDetail(connection, transaction);
                        if(DeliveryWhether == true)
                        {
                            InsertDelivery(connection, transaction);
                        }
                        transaction.Commit();
                        Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }
        
        private void bntPaymentCard_Click(object sender, EventArgs e)
        {
            if(cashKrw == 0 && cardKrw ==0)
            {
                MessageBox.Show("결제 대상 금액이 없습니다", "알림");
                return;
            }
            else
            {
                PaymentCardRegist();
            }
            
        }

        private void bntPaymentAccount_Click(object sender, EventArgs e)
        {
            if (cashKrw == 0 && accountKrw == 0)
            {
                MessageBox.Show("결제 대상 금액이 없습니다", "알림");
                return;
            }
            else
            {
                PaymentAccountRegist();
            }
        }

        private void bntPaymentPoint_Click(object sender, EventArgs e)
        {
            if(cashKrw == 0 && pointKrw == 0)
            {
                MessageBox.Show("결제 대상 금액이 없습니다", "알림");
                return;
            }
            else if(customerCode == 0)
            {
                return;
            }
            else
            {
                PayPointRegist();
            }
            
        }
        
        private void bntDeliveryRegist_Click(object sender, EventArgs e)
        {
            
            if (customerCode == 0)
            {
                DialogResult result = MessageBox.Show("회원이 지정되지 않았습니다. 계속 하시겠습니까?", "알림", MessageBoxButtons.YesNo);
                if(result == DialogResult.No)
                {
                    return;
                }
                else
                {
                    DeliveryInfoRegist();
                }
            }
            else
            {
                DeliveryInfoRegist();
            }
        }

        private void bntReturn_Click(object sender, EventArgs e)
        {
            if (PdtList.Dgv.RowCount == 0)
            {
                MessageBox.Show("반품 등록 할 상품이 없습니다", "알림");
                return;
            }
            if(saleType != 1)
            {
                return;
            }
            if(origineCode != 0)
            {
                return;
            }
            else if (MessageBox.Show("반품 하시겠습니까?", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection connection = dbconn.Opensql())
                {
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        AdjustForReturn();
                        InsertSaleVoucher(connection, transaction);
                        SaleOrigneCodeUpdate(connection, transaction);
                        InsertSalepayent(connection, transaction);
                        InsertSaleDetail(connection, transaction);
                        if (DeliveryWhether == true)
                        {
                            InsertDelivery(connection, transaction);
                        }
                        transaction.Commit();
                        Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }

        private void bntAfterDelivery_Click(object sender, EventArgs e)
        {
            if (customerCode == 0)
            {
                DialogResult result = MessageBox.Show("회원이 지정되지 않았습니다. 계속 하시겠습니까?", "알림", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }
                else
                {
                    DeliveryInfoRegist();
                }
            }
            else
            {
                DeliveryInfoRegist();
            }
        }
    }
}
