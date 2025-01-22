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
        //메뉴 등록될 패널 전역 변수 선언
        Panel pnlSalesMenu = new Panel();
        Panel pnlCustomerMenu = new Panel();
        Panel pnlsupplierMenu = new Panel();
        Panel pnlBasicMenu = new Panel();
        //사이드 메뉴 버튼 전역 변수 선언
        Button btnBasicMenu = new Button();
        Button btnSupplierMenu = new Button();
        Button btnSalesMenu = new Button();
        Button btnCustomerMenu = new Button();
        public MainForm()
        {
            InitializeComponent();
            MenuLayout();
            SideMenuDesign();
            TopMenuButtonSet();
            lblMenuTitle.Text = "";
            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;
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
            btnConfig.Image = option;
            btnConfig.Image = new Bitmap(option, new Size(25, 25));
            btnConfig.ImageAlign = ContentAlignment.TopCenter;

        }
        private void MenuLayout()
        {
            /*사이드 메뉴 패널 객체 등록
             해당 부분은 사이드 메뉴 표시에 순서 영향이 있음*/
            //회원
            panelSideMenu.Controls.Add(pnlCustomerMenu);
            panelSideMenu.Controls.Add(btnCustomerMenu);
            //판매관리
            panelSideMenu.Controls.Add(pnlSalesMenu);
            panelSideMenu.Controls.Add(btnSalesMenu);
            //공급사/매입관리
            panelSideMenu.Controls.Add(pnlsupplierMenu);
            panelSideMenu.Controls.Add(btnSupplierMenu);
            //기초관리
            panelSideMenu.Controls.Add(pnlBasicMenu);
            panelSideMenu.Controls.Add(btnBasicMenu);
            panelSideMenu.Controls.Add(panelMenuTitle);

            //실행 메뉴 버튼 생성,이벤트 지정

            Button btnSupplierLog = CreateMenuButton("공급사 변경 로그", btnSupplierLog_Click);
            Button btnProductLog = CreateMenuButton("제품변경 로그", btnProductLog_Click);
            Button btnEmployee = CreateMenuButton("직원관리", btnEmployee_Click);
            Button btnSalesReport = CreateMenuButton("판매현황", btnSalesReport_Click);
            Button btnSalesList = CreateMenuButton("판매내역", btnSalesList_Click);
            Button btnCustomerList = CreateMenuButton("회원목록", btnCustomerList_Click);
            Button btnOrderList = CreateMenuButton("주문서", btnOrderList_Click);
            Button btnSupplierOrder = CreateMenuButton("발주", btnSupplierOrder_Click);
            Button btnPurchase = CreateMenuButton("매입", btnPurchase_Click);
            Button btnSupplier = CreateMenuButton("공급사 관리", btnSupplier_Click);
            Button btnCategory = CreateMenuButton("분류", btnCategory_Click);
            Button btnProduct = CreateMenuButton("제품관리", btnProduct_Click);
            Button btnPurchaseLog = CreateMenuButton("매입변경로그", btnPurchaseLog_Click);
            Button btnPaymentLog = CreateMenuButton("결제변경로그", btnPaymentLog_Click);
            Button btnCustomerLog = CreateMenuButton("회원변경 로그", btnCustomerLog_Click);
            Button btnEmpLog = CreateMenuButton("직원변경 로그", btnEmpLog_Click);
            Button btnAccessLog = CreateMenuButton("직원접속 로그", btnAccessLog_Click);
            Button btnSettlement = CreateMenuButton("일결산 실행", btnSettlement_Click);
            Button btnDailyreportByDay = CreateMenuButton("일결산 일별 조회", btnDailyreportByDay_Click);
            Button btnDailyReportPdt = CreateMenuButton("일결산 제품별 조회", btnDailyReportPdt_Click);
            Button btnDailyReportCategory = CreateMenuButton("일결산 분류별 조회", btnDailyReportCategory_Click);
            Button btnSupplierPayment = CreateMenuButton("공급사 결제", btnSupplierPayment_Click);
            Button btnPointHistory = CreateMenuButton("포인트 내역", btnPointHistroy_Click);
            Button btnDeliveryList = CreateMenuButton("배송", btnDeliveryList_Click);

            //사이드 버튼 설정
            SideButtonSetting(btnCustomerMenu, "회원관리", btnCustomerMenu_Click);
            SideButtonSetting(btnSalesMenu, "판매관리", btnSalesMenu_Click);
            SideButtonSetting(btnSupplierMenu, "공급사/매입관리", btnSupplierMenu_Click);
            SideButtonSetting(btnBasicMenu, "기초관리", btnBasicMenu_Click);

            //기초관리 패널 설정
            SidePanalSetting(pnlBasicMenu);// 패널 설정
            pnlBasicMenu.Controls.Add(btnAccessLog);
            pnlBasicMenu.Controls.Add(btnEmpLog);
            pnlBasicMenu.Controls.Add(btnEmployee);
            pnlBasicMenu.Controls.Add(btnCategory);
            pnlBasicMenu.Controls.Add(btnProductLog);
            pnlBasicMenu.Controls.Add(btnProduct);

            //공급사/매입관리 패널 설정
            SidePanalSetting(pnlsupplierMenu);
            pnlsupplierMenu.Controls.Add(btnPaymentLog);
            pnlsupplierMenu.Controls.Add(btnSupplierPayment);
            pnlsupplierMenu.Controls.Add(btnPurchaseLog);
            pnlsupplierMenu.Controls.Add(btnSupplierOrder);
            pnlsupplierMenu.Controls.Add(btnPurchase);
            pnlsupplierMenu.Controls.Add(btnSupplierLog);
            pnlsupplierMenu.Controls.Add(btnSupplier);

            //판매관리 패널 설정
            SidePanalSetting(pnlSalesMenu);
            pnlSalesMenu.Controls.Add(btnDailyReportCategory);
            pnlSalesMenu.Controls.Add(btnDailyReportPdt);
            pnlSalesMenu.Controls.Add(btnDailyreportByDay);
            pnlSalesMenu.Controls.Add(btnSettlement);
            pnlSalesMenu.Controls.Add(btnDeliveryList);
            pnlSalesMenu.Controls.Add(btnOrderList);
            pnlSalesMenu.Controls.Add(btnSalesList);
            pnlSalesMenu.Controls.Add(btnSalesReport);

            //회원관리 패널 설정
            SidePanalSetting(pnlCustomerMenu);
            pnlCustomerMenu.Controls.Add(btnPointHistory);
            pnlCustomerMenu.Controls.Add(btnCustomerLog);
            pnlCustomerMenu.Controls.Add(btnCustomerList);
        }
        /// <summary>
        /// 메뉴버튼 생성 메소드
        /// </summary>
        /// <param name="btnText"></param>
        /// <param name="eventHandler"></param>해당 버튼 클릭시 실행될 이벤트 지정
        /// <returns></returns>
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
                Size = new Size(150, 35),
                Font = new Font("맑은 고딕", 10F)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += eventHandler;
            return btn;
        }

        /// <summary>
        /// 사이드 버튼 설정 메소드
        /// </summary>
        /// <param name="button"></param> 전역에서 생성된 변수 지정
        /// <param name="btnString"></param> 표시될 메뉴명
        /// <param name="eventHandler"></param> 클릭시 실해될 이벤트 지정
        private void SideButtonSetting(Button button, string btnString, EventHandler eventHandler)
        {
            button.BackColor = Color.FromArgb(39, 39, 58);
            button.Text = btnString;
            button.Dock = DockStyle.Top;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.ForeColor = Color.White;
            button.Size = new Size(150, 40);
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Font = new Font("맑은 고딕", 10F);
            button.Click += eventHandler;
        }
        /// <summary>
        /// 사이드 메뉴 패널 설정 메소드
        /// </summary>
        /// <param name="pnl"></param>
        private void SidePanalSetting(Panel pnl)
        {
            pnl.BackColor = Color.FromArgb(45, 58, 88);
            pnl.Dock = DockStyle.Top;
        }
        /// <summary>
        /// 사이드 메뉴 디자인 설정
        /// </summary>
        private void SideMenuDesign()
        {
            pnlBasicMenu.Visible = false;
            pnlsupplierMenu.Visible = false;
            pnlSalesMenu.Visible = false;
            pnlCustomerMenu.Visible = false;
        }
        /// <summary>
        /// 사이드 메뉴 보이기/숨기시 기능 설정
        /// </summary>
        private void SubMenuHide()
        {
            if (pnlBasicMenu.Visible == true)
            {
                pnlBasicMenu.Visible = false;
            }
            if(pnlsupplierMenu.Visible == true)
            {
                pnlsupplierMenu.Visible = false;
            }
            if (pnlSalesMenu.Visible == true)
            {
                pnlSalesMenu.Visible = false;
            }
            if (pnlCustomerMenu.Visible == true)
            {
                pnlCustomerMenu.Visible = false;
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
            Search();
        }
        private void Search()
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
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5)
            {
                this.ActiveControl = null;
                Search();
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
        private void btnBasicMenu_Click(object sender, EventArgs e)
        {
            SubMenuShow(pnlBasicMenu);
        }

        private void btnSupplierMenu_Click(object sender, EventArgs e)
        {
            SubMenuShow(pnlsupplierMenu);
        }

        private void btnSalesMenu_Click(object sender, EventArgs e)
        {
            SubMenuShow(pnlSalesMenu);
        }
        private void btnCustomerMenu_Click(object sender, EventArgs e)
        {
            SubMenuShow(pnlCustomerMenu);
        }
        /// <summary>
        /// 분류 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCategory_Click(object sender, EventArgs e)
        {
            CategoryBoard categoryBoard = new CategoryBoard();
            categoryBoard.WorkType = 0;
            categoryBoard.StartPosition = FormStartPosition.CenterParent;
            categoryBoard.EditeMode();
            categoryBoard.ShowDialog();
        }
        /// <summary>
        /// 제품관리 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProduct_Click(object sender, EventArgs e)
        {
            btn_Click<ProductList>(sender, e,101);
        }
        /// <summary>
        /// 공급사 관리 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupplier_Click(object sender, EventArgs e)
        {
            btn_Click<SupplierList>(sender, e,131);
        }
        /// <summary>
        /// 매입 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPurchase_Click(object sender, EventArgs e)
        {
            btn_Click<PurchaseList>(sender, e,201);
        }
        /// <summary>
        /// 공급사 결제 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupplierPayment_Click(object sender, EventArgs e)
        {
            btn_Click<PaymentList>(sender, e,221);
        }
        /// <summary>
        /// 발주 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupplierOrder_Click(object sender, EventArgs e)
        {
            btn_Click<PurchaseOrderList>(sender, e,201);
        }
        /// <summary>
        /// 주문서 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrderList_Click(object sender, EventArgs e)
        {
            btn_Click<CustomerOrderList>(sender, e,301);
        }
        /// <summary>
        /// 회원목록 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCustomerList_Click(object sender, EventArgs e)
        {
            btn_Click<CustomerList>(sender, e,401);
        }

        private void btnSalesList_Click(object sender, EventArgs e)
        {
            btn_Click<SalesList>(sender, e,301);
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            btn_Click<SalesReport>(sender, e,301);
        }

        private void btnDatabaseConnect_Click(object sender, EventArgs e)
        {
            cDataBaseConfig dbconfig = new cDataBaseConfig();
            dbconfig.StartPosition = FormStartPosition.CenterParent;
            dbconfig.ShowDialog();

        }
        private void btnEmployee_Click(object sender, EventArgs e)
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
        private void btnPointHistroy_Click(object sender, EventArgs e)
        {
            btn_Click<PointHistory>(sender, e, 401);
        }
        private void btnDeliveryList_Click(object sender, EventArgs e)
        {
            btn_Click<DeliveryList>(sender, e, 301);
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            Config config = new Config();
            config.StartPosition = FormStartPosition.CenterParent;
            config.ShowDialog();
        }
    }
}
