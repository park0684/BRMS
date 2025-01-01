using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace BRMS
{
    public partial class MainForm : Form
    {
        private Form activeForm;
        //int accessedEmp;
        //Dictionary<int, string> accessPermission = new Dictionary<int, string>();
        int accessedEmp = cUserSession.AccessedEmp;
        Dictionary<int, string> accessPermission = cUserSession.AccessPermission;

        public MainForm()
        {
            InitializeComponent();
            SideMenuDesign();
            TopMenuButtonSet();
            lblMenuTitle.Text = "";
        }
        /// <summary>
        /// 상단 조회, 인쇄, 엑셀 버튼 디자인 설정
        /// </summary>
        private void TopMenuButtonSet()
        {
            Image exportImage = Image.FromFile("img\\ExcelIcon_img.png");
            Image connectioConfig = Image.FromFile("img\\connect_img.png");
            Image option = Image.FromFile("img\\option_img.png");
            Image printer = Image.FromFile("img\\print_img.png");

            btnExportExcel.Image = exportImage;
            btnExportExcel.Image = new Bitmap(exportImage, new Size(25 , 25));
            btnExportExcel.Text = "";
            btnDatabaseConnect.Image = connectioConfig;
            btnDatabaseConnect.Image = new Bitmap(connectioConfig, new Size(25, 25));
            btnDatabaseConnect.ImageAlign = ContentAlignment.TopCenter;
            btnPrint.Image = printer;
            btnPrint.Image = new Bitmap(printer, new Size(25, 25));
            btnPrint.Text = "";

        }
       /// <summary>
       /// 사이드 메뉴 디자인 설정
       /// </summary>
        private void SideMenuDesign()
        {
            panelBasicMenu.Visible = false;
            panelSupplyMenu.Visible = false;
            panelSalseNenu.Visible = false;
            panelCustomerMenu.Visible = false;
        }
        /// <summary>
        /// 사이드 메뉴 보이기/숨기시 기능 설정
        /// </summary>
        private void SubMenuHide()
        {
            if (panelBasicMenu.Visible == true)
            {
                panelBasicMenu.Visible = false;
            }
            if(panelSupplyMenu.Visible == true)
            {
                panelSupplyMenu.Visible = false;
            }
            if (panelSalseNenu.Visible == true)
            {
                panelSalseNenu.Visible = false;
            }
            if (panelCustomerMenu.Visible == true)
            {
                panelCustomerMenu.Visible = false;
            }
        }

        private void SubMenuShow(Panel sideMenu)
        {
            if(sideMenu.Visible == false)
            {
                SubMenuHide();
            }
            sideMenu.Visible = true;
            int buttonCount = sideMenu.Controls.OfType<Button>().Count();
            int sizeConvert = buttonCount * 35;
            sideMenu.Size = new System.Drawing.Size(150, sizeConvert);
        }
        /// <summary>
        /// 조회 버튼 클릭
        /// panelViewer에 호출 된 클래스의 데이터를 조회하는 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntSearch_Click(object sender, EventArgs e)
        {
            Form currentForm = panelViewer.Controls.Count > 0 ? panelViewer.Controls[0] as Form : null;

            if (currentForm != null)
            {
                string className = currentForm.GetType().FullName;

                // className을 사용하여 해당 클래스의 runQuery 메소드 실행
                Type formType = Type.GetType(className);
                if (formType != null)
                {
                    MethodInfo methodInfo = formType.GetMethod("RunQuery");
                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(currentForm, null);
                    }
                }
            }
        }
        /// <summary>
        /// 사이드 메뉴 버튼 클릭 시 panelViewer에 지정된 폼을 표시
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click<T>(object sender, EventArgs e, int permission) where T : Form, new()
        {
            // 권한 체크
            if (permission != -1 && !cUserSession.HasPermission(permission))
            {
                MessageBox.Show("이 작업을 수행할 권한이 없습니다.", "권한 부족", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (activeForm is T)
            {
                return;
            }

            T formInstance;
            string formName = typeof(T).Name;

            if (panelViewer.Controls.ContainsKey(formName))
            {
                formInstance = (T)panelViewer.Controls[formName];
            }
            else
            {

                formInstance = new T();
                formInstance.TopLevel = false;
                formInstance.FormBorderStyle = FormBorderStyle.None;
                formInstance.Dock = DockStyle.Fill;
                panelViewer.Controls.Add(formInstance);
                panelViewer.Tag = ((Button)sender).Name;
                formInstance.Name = formName;
            }
            openChildForm(formInstance, sender);
            lblMenuTitle.Text = formInstance.Text;
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
            panelViewer.Controls.Add(childForm);
            //panelViewer.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            Form currentForm = panelViewer.Controls.Count > 0 ? panelViewer.Controls[0] as Form : null;

            if (currentForm != null)
            {
                string className = currentForm.GetType().FullName;

                // className을 사용하여 해당 클래스의 runQuery 메소드 실행
                Type formType = Type.GetType(className);
                if (formType != null)
                {
                    MethodInfo methodInfo = formType.GetMethod("ExportExcel");
                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(currentForm, null);
                    }
                }
            }
        }
        private void bntBasicMenu_Click(object sender, EventArgs e)
        {
            SubMenuShow(panelBasicMenu);
        }

        private void bntSupplierMenu_Click(object sender, EventArgs e)
        {
            SubMenuShow(panelSupplyMenu);
        }

        private void bntSalseMenu_Click(object sender, EventArgs e)
        {
            SubMenuShow(panelSalseNenu);
        }
        private void bntCoustomer_Click(object sender, EventArgs e)
        {
            SubMenuShow(panelCustomerMenu);
        }
        /// <summary>
        /// 분류 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntCategory_Click(object sender, EventArgs e)
        {
            CategoryBoard categoryBoard = new CategoryBoard();
            categoryBoard.StartPosition = FormStartPosition.CenterParent;
            categoryBoard.EditeMode();
            categoryBoard.ShowDialog();
        }
        /// <summary>
        /// 제품관리 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntProduct_Click(object sender, EventArgs e)
        {
            btn_Click<ProductList>(sender, e,101);
        }
        /// <summary>
        /// 공급사 관리 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntSupplier_Click(object sender, EventArgs e)
        {
            btn_Click<SupplierList>(sender, e,131);
        }
        /// <summary>
        /// 매입 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntPurchase_Click(object sender, EventArgs e)
        {
            btn_Click<PurchaseList>(sender, e,201);
        }
        /// <summary>
        /// 공급사 결제 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntSupplierPayment_Click(object sender, EventArgs e)
        {
            btn_Click<PaymentList>(sender, e,221);
        }
        /// <summary>
        /// 발주 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntSupplierOrder_Click(object sender, EventArgs e)
        {
            btn_Click<PurchaseOrderList>(sender, e,201);
        }
        /// <summary>
        /// 주문서 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntOrderList_Click(object sender, EventArgs e)
        {
            btn_Click<CustomerOrderList>(sender, e,301);
        }
        /// <summary>
        /// 회원목록 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntCustomerList_Click(object sender, EventArgs e)
        {
            btn_Click<CustomerList>(sender, e,401);
        }

        private void bntSalseList_Click(object sender, EventArgs e)
        {
            btn_Click<SalesList>(sender, e,301);
        }

        private void bntSalesReport_Click(object sender, EventArgs e)
        {
            btn_Click<SalesReport>(sender, e,301);
        }

        private void btnDatabaseConnect_Click(object sender, EventArgs e)
        {
            cDataBaseConfig dbconfig = new cDataBaseConfig();
            dbconfig.StartPosition = FormStartPosition.CenterParent;
            dbconfig.ShowDialog();

        }
        private void bntEmployee_Click(object sender, EventArgs e)
        {
            btn_Click<EmployeeList>(sender, e,501);
        }
        

        private void btnProductLog_Click(object sender, EventArgs e)
        {
            btn_Click<ProductLog>(sender, e, 101);
        }

        private void btnSupplierLog_Click(object sender, EventArgs e)
        {
            btn_Click<SupplierLog>(sender, e, 131);
        }

        private void btnPurchaseLog_Click(object sender, EventArgs e)
        {
            btn_Click<PurchaseLog>(sender, e, 201);
        }

        private void btnPaymentLog_Click(object sender, EventArgs e)
        {
            btn_Click<PaymentLog>(sender, e, 221);
        }

        private void btnCustomerLog_Click(object sender, EventArgs e)
        {
            btn_Click<CustomerLog>(sender, e, 301);
        }

        private void btnEmpLog_Click(object sender, EventArgs e)
        {
            btn_Click<Emplog>(sender, e, 401);
        }

        private void btnAccessLog_Click(object sender, EventArgs e)
        {
            btn_Click<EmpAccessLog>(sender, e, 401);
        }

        private void btnSettlement_Click(object sender, EventArgs e)
        {
            DateSelectionForm dateSelectionForm = new DateSelectionForm();
            dateSelectionForm.StartPosition = FormStartPosition.CenterParent;
            dateSelectionForm.ShowDialog();
        }

        private void btnDailyreportByDay_Click(object sender, EventArgs e)
        {
            btn_Click<DailyReportViewByDay>(sender, e, 351);
        }

        private void btnDailyReportPdt_Click(object sender, EventArgs e)
        {
            btn_Click<DailyReportViewByProduct>(sender, e, 351);
        }

        private void btnDailyReportCategory_Click(object sender, EventArgs e)
        {
            btn_Click<DailyReportViewByCategory>(sender, e, 351);
        }
    }
}
