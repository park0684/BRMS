using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using common.Interface;
using GridControl;
using common.Helpers;

namespace BRM.Views
{
    public partial class ProductDetailView : Form, IProductDetailView,IView
    {
        BrsDataGridView dgvPurList = new BrsDataGridView();
        BrsDataGridView dgvSaleLiet = new BrsDataGridView();
        BrsDataGridView dgvLogList = new BrsDataGridView();
        public ProductDetailView()
        {
            InitializeComponent();
            InitializeComboBox();
            InitializeTapcontrol();
            ViewEvent();
            this.Text = "상품등록정보";
            this.ControlBox = false;
            this.MaximizeBox = false;
        }
        // 필드
        public string PdtNameKr { get => txtProductName_kr.Text; set { txtProductName_kr.Text = value; } }
        public string PdtNameEn { get => txtProductName_en.Text; set => txtProductName_en.Text = value;  }
        public string PdtNumber { get => txtProductNumber.Text; set => txtProductNumber.Text =value ; }
        public string CategoryNameKr { set => lblCategoryKr.Text = value; }
        public string CategoryNameEn { set =>lblCategoryEn.Text = value; }
        public string PdtMemo { get => txtProductMemo.Text; set => txtProductMemo.Text = value; }
        public int PdtStatus 
        {
            get => (int) cmbProductStstus.SelectedValue;
            set => cmbProductStstus.SelectedValue = value;
        }
        public int PdtTax 
        { 
            get => (int)cmbTax.SelectedValue;
            set => cmbTax.SelectedValue = value; 
        }
        public decimal PdtBprice 
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(txtBprice.Text)) 
                    return 0m;
                decimal.TryParse(txtBprice.Text.Replace(",", ""), out var result);
                return result;
            }
            set => txtBprice.Text = value.ToString("#,###.#0"); 
        }
        public int PdtPriceKrw 
        { 
            get 
            {
                if (string.IsNullOrWhiteSpace(txtPriceKrw.Text))
                    return 0;
                int.TryParse(txtPriceKrw.Text.Replace(",", ""), out var result);  // 콤마 제거 (사용자가 직접 입력 시)
                return result;
            }
            set
            {
                txtPriceKrw.Text = value.ToString("#,##0");
            }
        }
        public decimal PdtPriceUsd
        {
            get
            {
                if (string.IsNullOrWhiteSpace(txtPriceUsd.Text))
                    return 0m;
                decimal.TryParse(txtPriceUsd.Text.Replace(",", ""), out var result);
                return result;
            }
            set => txtPriceUsd.Text = value.ToString("#,###.#0");
        }
        public decimal PdtProfitRate 
        {
            get
            {
                if (string.IsNullOrWhiteSpace(txtProfitRate.Text))
                    return 0;
                decimal.TryParse(txtProfitRate.Text.Replace(",", ""), out var result);
                return result;
            }
            set 
            {
                txtProfitRate.Text = value.ToString("##.#0");
            } 
        }
        public string PdtSupplier { set => lblSupplier.Text = value; }
        public DateTime PurchaseFromDate => dtpPurchaseDateTo.Value;

        public DateTime purchaseTodate => dtpPurchaseDateTo.Value;

        public DateTime SaleFromDate => dtpOrderFrom.Value;

        public DateTime SaleToDate => dtpOrderTo.Value;

        public DateTime LogFromDate => dtpLogkFrom.Value;

        public DateTime LogToDate => dtpLogTo.Value;

        public int stock 
        { 
            get 
            {
                if (string.IsNullOrWhiteSpace(lblStock.Text))
                    return 0;
                int.TryParse(lblStock.Text.Replace(",",""), out var result);
                return result;
            } 
            set 
            {
                lblStock.Text = value.ToString("#,##0");
            }
        }

        public decimal PdtWidth 
        {
            get { return ValueConverterHelper.ToDecimal(txtWidth.Text) ; }
            set { txtWidth.Text = value.ToString("#,###.#0"); }
        }
        public decimal Pdtlength 
        {
            get { return ValueConverterHelper.ToDecimal(txtLength.Text); }
            set { txtLength.Text = value.ToString("#,###.#0"); }
        }
        public decimal PdtHigth
        {
            get { return ValueConverterHelper.ToDecimal(txtHight.Text); }
            set { txtHight.Text = value.ToString("#,###.#0"); }
        }
        public decimal PdtWeigth 
        {
            get { return ValueConverterHelper.ToDecimal(txtProductWeight.Text); }
            set { txtProductWeight.Text = value.ToString("#,###.#0"); }
        }

        /*이벤트*/
        public event EventHandler CloseFormEvent;
        public event EventHandler CategorySelectEvent;
        public event EventHandler SupplierSelectEvent;
        public event EventHandler SaveEvent;
        public event EventHandler UsdPriceChangeEvent;
        public event EventHandler KrwPriceChangeEvent;
        public event EventHandler BpriceChangeEvent;
        public event EventHandler ProfitRateChageEvent;

        /*메서드*/
        public void EditLogDateBinding(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public void PurchaseDataBinding(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public void SaleDataBinding(DataTable dataTable)
        {
            throw new NotImplementedException();
        }
        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            ShowDialog();
        }
        public Control AsControl()
        {
            return this;
        }
        public void CloseForm()
        {
            this.Close();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void SetTabcontrol(bool isNew, List<Dictionary<string, string>> columns = null)
        {
            if(isNew)
            {
                tabCtrlProduct.TabPages.Remove(tabPage2);
                tabCtrlProduct.TabPages.Remove(tabPage3);
                tabCtrlProduct.TabPages.Remove(tabPage4);
            }
            else
            {
                foreach(var column in columns[0])
                {
                    dgvPurList.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = column.Key,
                        DataPropertyName = column.Key,
                        HeaderText = column.Value
                    });
                }
                foreach (var column in columns[1])
                {
                    dgvSaleLiet.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = column.Key,
                        DataPropertyName = column.Key,
                        HeaderText = column.Value
                    });
                }
                foreach (var column in columns[2])
                {
                    dgvLogList.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = column.Key,
                        DataPropertyName = column.Key,
                        HeaderText = column.Value
                    });
                }
                foreach(DataGridViewColumn column in dgvPurList.Columns)
                {
                    column.HeaderCell.Style.WrapMode = DataGridViewTriState.False;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
                
        }

        /// <summary>
        /// 뷰 이벤트 
        /// </summary>
        private void ViewEvent()
        {
            btnCancle.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);
            btnCategory.Click += (s, e) => CategorySelectEvent?.Invoke(this, EventArgs.Empty);
            btnSupplier.Click += (s, e) => SupplierSelectEvent?.Invoke(this, EventArgs.Empty);
            btnOk.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);

            txtBprice.Leave += (s, e) => BpriceChangeEvent?.Invoke(this, EventArgs.Empty);
            txtBprice.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BpriceChangeEvent?.Invoke(this, EventArgs.Empty); };
            
            txtPriceKrw.Leave += (s, e) => KrwPriceChangeEvent?.Invoke(this, EventArgs.Empty);
            txtPriceKrw.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) KrwPriceChangeEvent?.Invoke(this, EventArgs.Empty); };
            
            txtPriceUsd.Leave += (s, e) => UsdPriceChangeEvent?.Invoke(this, EventArgs.Empty);
            txtPriceUsd.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) UsdPriceChangeEvent?.Invoke(this, EventArgs.Empty); };
            
            txtProfitRate.Leave += (s, e) => ProfitRateChageEvent?.Invoke(this, EventArgs.Empty);
            txtProfitRate.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) ProfitRateChageEvent?.Invoke(this, EventArgs.Empty); };

        }
        /// <summary>
        /// 탭컨트로 초기 설정
        /// </summary>
        private void InitializeTapcontrol()
        {
            tabCtrlProduct.TabPages[0].Text = "제품정보";
            tabCtrlProduct.TabPages[1].Text = "매입내역";
            tabCtrlProduct.TabPages[2].Text = "판매내역";
            tabCtrlProduct.TabPages[3].Text = "변경로그";
            
            pnlPurOrder.Controls.Add(dgvPurList);
            dgvPurList.Dock = DockStyle.Fill;
            dgvPurList.ReadOnly = true;
            dgvPurList.AutoGenerateColumns = false;

            pnlSale.Controls.Add(dgvSaleLiet);
            dgvSaleLiet.Dock = DockStyle.Fill;
            dgvSaleLiet.ReadOnly = true;
            dgvSaleLiet.AutoGenerateColumns = false;

            pnlLog.Controls.Add(dgvLogList);
            dgvLogList.Dock = DockStyle.Fill;
            dgvLogList.ReadOnly = true;
            dgvLogList.AutoGenerateColumns = false;
        }

        /// <summary>
        /// 콤보박스 초기 설정
        /// </summary>
        private void InitializeComboBox()
        {
            // 제품 상태 콤보박스 설정
            cmbProductStstus.DataSource = StatusHelper.GetMap(StatusHelper.Keys.ProductStatus).Select(status => new KeyValuePair<int, string>(status.Key, status.Value)).ToList();
            cmbProductStstus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProductStstus.DisplayMember = "Value";
            cmbProductStstus.ValueMember = "Key";
            cmbProductStstus.SelectedValue = 1;

            // 과면세 여부 콤보박스 설정
            cmbTax.DataSource = StatusHelper.GetMap(StatusHelper.Keys.TaxStatus).Select(tax => new KeyValuePair<int, string>(tax.Key, tax.Value)).ToList();
            cmbTax.DisplayMember = "Value";
            cmbTax.ValueMember = "Key";
            cmbTax.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTax.SelectedValue = 1;
            
        }


    }
}
