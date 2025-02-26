using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Menu
{
    public static class MenuPermissionCodes
    {
        private static Dictionary<string, int> _map = new Dictionary<string, int>()
        {
            {Menukeys.ProductList , 101 },
            {Menukeys.ProductDetail , 101 },
            {Menukeys.ProductLog , 101 },
            {Menukeys.CategoryBoard , 121 },
            {Menukeys.EmployeeList , 501 },
            {Menukeys.EmplyeeLog , 501 },
            {Menukeys.AccessLog , 501 },
            {Menukeys.SupplierList , 131 },
            {Menukeys.SupplierLog , 131 },
            {Menukeys.PurchaseList , 201 },
            {Menukeys.SupplierOrderList , 201 },
            {Menukeys.PurchaseLog , 201 },
            {Menukeys.SupplierPayment , 221 },
            {Menukeys.PaymentLog , 221 },
            {Menukeys.SaleReport , 311 },
            {Menukeys.SaleOrderList , 311 },
            {Menukeys.CustomOrderList , 311 },
            {Menukeys.DeliveryList , 311 },
            {Menukeys.DailySummary , 351 },
            {Menukeys.DailyByDay , 351 },
            {Menukeys.DailyByProduct , 351 },
            {Menukeys.DailyBySupplier , 351 },
            {Menukeys.DailyByCategory , 351 },
            {Menukeys.ExecutionDailyClosing , 352 },
            {Menukeys.MonthlySummary , 351 },
            {Menukeys.MonthlyByDay , 351 },
            {Menukeys.MonthlyByProduct , 351 },
            {Menukeys.MonthlyBySupplier , 351 },
            {Menukeys.MonthlyByCategory , 351 },
            {Menukeys.ExecutionMonthlyClosing , 352 },
            {Menukeys.MemberList , 401 },
            {Menukeys.MemberReport , 401  },
            {Menukeys.MemberLog , 401 },
            {Menukeys.PointLog , 401 },
            {Menukeys.Memberpayment , 401 },
            {Menukeys.SupplierSearch , 131 },


        };

        public static int GetCode(string key)
        {
            return _map.TryGetValue(key, out var code) ? code : -1;
        }
    }
}
