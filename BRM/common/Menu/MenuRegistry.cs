using common.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace common.Menu
{
    public class MenuRegistry
    {
        public static string GetMenuDisplay(string key)
        {
            foreach (var category in GetSideMenuList())
            {
                var item = category.MenuItems.FirstOrDefault(m => m.Menukey == key);
                if (item != null)
                    return item.MenuDisplay;
            }
            return string.Empty;
        }
        public static List<MenuCategoryDto> GetSideMenuList()
        {
            return new List<MenuCategoryDto>
            {
                new MenuCategoryDto
                {
                    CategoryKey = "Basic",
                    CategoryDisplay = "기초관리",
                    MenuItems =  new List<MenuItemDto>
                    {
                        new MenuItemDto{Menukey = Menukeys.ProductList, MenuDisplay ="제품관리", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.ProductLog, MenuDisplay ="제품변경로그", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.CategoryBoard, MenuDisplay ="분류", OpenType = ViewOpenType.Popup},
                        new MenuItemDto{Menukey = Menukeys.EmployeeList, MenuDisplay ="직원관리", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.EmplyeeLog, MenuDisplay ="직원변경로그", OpenType = ViewOpenType.Panel },
                        new MenuItemDto{Menukey = Menukeys.AccessLog, MenuDisplay ="직원접속로그, OpenType = ViewOpenType.Panel"}
                    }
                },
                new MenuCategoryDto
                {
                    CategoryKey = "Supplier",
                    CategoryDisplay = "공급사/매입 관리",
                    MenuItems = new List<MenuItemDto>
                    {
                        new MenuItemDto{Menukey = Menukeys.SupplierList, MenuDisplay ="공급사 관리", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.SupplierLog, MenuDisplay ="공급사 변경 로그", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.PurchaseList, MenuDisplay ="매입", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.SupplierOrderList, MenuDisplay ="발주", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.PurchaseLog, MenuDisplay ="매입/발주 변경 로그", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.SupplierPayment, MenuDisplay ="공급사 결제", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.PaymentLog, MenuDisplay ="결제 변경 로그", OpenType = ViewOpenType.Panel}
                    }
                },
                new MenuCategoryDto
                {
                    CategoryKey = "Sales",
                    CategoryDisplay = "판매/주문 관리",
                    MenuItems = new List<MenuItemDto>
                    {
                        new MenuItemDto{Menukey = Menukeys.SaleReport, MenuDisplay ="판매현황", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.SaleOrderList, MenuDisplay ="판매 목록", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.CustomOrderList, MenuDisplay ="고객 주문서", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.DeliveryList, MenuDisplay ="배송 관리", OpenType = ViewOpenType.Panel}
                    }
                },
                new MenuCategoryDto
                {
                    CategoryKey = "Closing",
                    CategoryDisplay = "결산",
                    MenuItems = new List<MenuItemDto>
                    {
                        new MenuItemDto{Menukey = Menukeys.DailySummary, MenuDisplay ="일종합 내역", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.DailyByDay, MenuDisplay ="일별 내역", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.DailyByProduct, MenuDisplay ="일 제품별 내역", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.DailyBySupplier, MenuDisplay ="일 공급사별 내역", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.DailyByCategory, MenuDisplay ="일 분류별 내역", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.ExecutionDailyClosing, MenuDisplay ="일 결산 실행", OpenType = ViewOpenType.Popup},
                        new MenuItemDto{Menukey = Menukeys.MonthlySummary, MenuDisplay ="월종합 내역", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.MonthlyByDay, MenuDisplay ="월별 내역", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.MonthlyByProduct, MenuDisplay ="월 제품별 내역", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.MonthlyBySupplier, MenuDisplay ="월 공급사별 내역", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.MonthlyByCategory, MenuDisplay ="월 분류별 내역", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.ExecutionMonthlyClosing, MenuDisplay ="월 결산 실행", OpenType = ViewOpenType.Popup}
                    }
                },
                new MenuCategoryDto
                {
                    CategoryKey = "Member",
                    CategoryDisplay = "회원관리",
                    MenuItems = new List<MenuItemDto>
                    {
                        new MenuItemDto{Menukey = Menukeys.MemberList, MenuDisplay ="회원 조회", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.MemberReport, MenuDisplay ="회원별 매출", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.MemberLog, MenuDisplay ="회원 변경 로그", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.PointLog, MenuDisplay ="포인트 로그", OpenType = ViewOpenType.Panel},
                        new MenuItemDto{Menukey = Menukeys.Memberpayment, MenuDisplay ="회원 결제", OpenType = ViewOpenType.Panel}
                    }
                }
            };
        }

        public static List<MenuItemDto> GetPopupMenuList() => new List<MenuItemDto>()
        {
            new MenuItemDto { Menukey = Menukeys.SupplierSearch, MenuDisplay ="공급사검색", OpenType = ViewOpenType.Popup, CallbackParamCount = 1},
            new MenuItemDto { Menukey = Menukeys.CategoryBoard, MenuDisplay ="분류" , OpenType = ViewOpenType.Popup, CallbackParamCount = 3}
        };

        public static MenuItemDto GetDefinition(string key)
        {
            return GetSideMenuList()
                .SelectMany(c => c.MenuItems)
                .FirstOrDefault(m => m.Menukey == key);
        }
    }

    public static class Menukeys
    {
        public const string ProductList = "ProductList";
        public const string ProductDetail = "ProductDetail";
        public const string ProductLog = "ProductLog";
        public const string CategoryBoard = "CategoryBoard";
        public const string EmployeeList = "EmployeeList";
        public const string EmployeeDetail = "EmployeeDetail";
        public const string EmplyeeLog = "EmplyeeLog";
        public const string AccessLog = "AccessLog";
        public const string SupplierList = "SupplierList";
        public const string SupplierLog = "SupplierLog";
        public const string PurchaseList = "PurchaseList";
        public const string SupplierOrderList = "SupplierOrderList";
        public const string PurchaseLog = "PurchaseLog";
        public const string SupplierPayment = "SupplierPayment";
        public const string PaymentLog = "PaymentLog";
        public const string SaleReport = "SaleReport";
        public const string SaleOrderList = "SaleOrderList";
        public const string CustomOrderList = "CustomOrderList";
        public const string DeliveryList = "DeliveryList";
        public const string DailySummary = "DailySummary";
        public const string DailyByDay = "DailyByDay";
        public const string DailyByProduct = "DailyByProduct";
        public const string DailyBySupplier = "DailyBySupplier";
        public const string DailyByCategory = "DailyByCategory";
        public const string ExecutionDailyClosing = "ExecutionDailyClosing";
        public const string MonthlySummary = "MonthlySummary";
        public const string MonthlyByDay = "MonthlyByDay";
        public const string MonthlyByProduct = "MonthlyByProduct";
        public const string MonthlyBySupplier = "MonthlyBySupplier";
        public const string MonthlyByCategory = "MonthlyByCategory";
        public const string ExecutionMonthlyClosing = "ExecutionMonthlyClosing";
        public const string MemberList = "MemberList";
        public const string MemberReport = "MemberReport";
        public const string MemberLog = "MemberLog";
        public const string PointLog = "PointLog";
        public const string Memberpayment = "Memberpayment";
        public const string SupplierSearch = "SupplierSearch"; //  공급사 지정 검색창
    }

}
