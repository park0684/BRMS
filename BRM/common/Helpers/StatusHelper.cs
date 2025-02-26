using System.Collections.Generic;
using System.Linq;


namespace common.Helpers
{
    public class StatusHelper
    {
        public static class Keys
        {
            public const string ProductStatus = "ProductStatus";
            public const string CustomerStatus = "CustomerStatus";
            public const string OrderStatus = "OrderStatus";
            public const string SupplierStatus = "SupplierStatus";
            public const string purchaseType = "purchaseType";
            public const string PurchaseOrderStatus = "PurchaseOrderStatus";
            public const string TaxStatus = "TaxStatus";
            public const string CustomerOrderStatus = "CustomerOrderStatus";
            public const string SalePaymentType = "SalePaymentType";
            public const string SaleType = "SaleType";
            public const string EmployeeStatus= "EmployeeStatus";
            public const string SupplierPayment = "SupplierPayment";
            public const string PointHistory = "PointHistory";
            public const string DeliveryStatus = "DeliveryStatus";
            public const string ProductLog = "ProductLog";
        }
        /// <summary>
        /// 상태 등록
        /// </summary>
        private static readonly Dictionary<string, Dictionary<int, string>> _statusMap =new Dictionary<string, Dictionary<int, string>>
        {
            [Keys.ProductStatus] = new Dictionary<int, string>
            {
                 { 1, "판매가능" },
                 { 2, "품절" },
                 { 3, "단종" },
                 { 4, "취급 외" }
            },
            [Keys.CustomerStatus] = new Dictionary<int, string>
            {
                 { 0, "무효" },
                 { 1, "유효" }
            },
            [Keys.OrderStatus] = new Dictionary<int, string>
            {
                 { 0, "취소" },
                 { 1, "주문" },
                 { 2, "판매" }
            },
            [Keys.SupplierStatus] = new Dictionary<int, string>
            {
                { 0, "무효" },
                { 1, "정상" }
            },
            [Keys.purchaseType] = new Dictionary<int, string>
            {
                { 1, "매입" },
                { 2, "반품" }
            },
            [Keys.PurchaseOrderStatus] = new Dictionary<int, string>
            {
                { 0, "발주취소" },
                { 1, "입고대기" },
                { 2, "입고완료" }
            },
            [Keys.TaxStatus] = new Dictionary<int, string>
            {
                { 0, "면세" },
                { 1, "과세" }
            },
            [Keys.CustomerOrderStatus] = new Dictionary<int, string>
            {
                { 0, "취소" },
                { 1, "주문" },
                { 2, "판매" },
                { 3, "제안 전달" }
            },
            [Keys.SalePaymentType] = new Dictionary<int, string>
            {
                { 0, "현금" },
                { 1, "계좌이체" },
                { 2, "PG" },
                { 3, "포인트" }
            },
            [Keys.SaleType] = new Dictionary<int, string>
            {
                { 0, "반품" },
                { 1, "판매" },
                { 2, "반품(구매 취소)" }
            },
            [Keys.EmployeeStatus] = new Dictionary<int, string>
            {
                {0, "퇴사" },
                {1, "근무" },
                {2, "휴직" }
            },
            [Keys.SupplierPayment] = new Dictionary<int, string>
            {
                {0,"현금" },
                {1,"계좌이체" },
                {2,"신용카드" },
                {3,"어음" },
            },
            [Keys.PointHistory] = new Dictionary<int, string>
            {
                {1, "구매 적립" },
                {2, "포이트 지불" },
                {3, "이벤트 적립" },
                {4, "기간종료 차감" },
                {5, "포인트 수정" }
            },
            [Keys.DeliveryStatus] = new Dictionary<int, string>
            {
                {0, "배송취소" },
                {1, "배송중" },
                {2, "배송완료" }
            },
            [Keys.ProductLog] = new Dictionary<int, string>
            {   
                {1, "제품명(한글)"},
                {2, "제품명(영문)"},
                {3, "공급사"},
                {4, "분류"},
                {5, "상태"},
                {6, "매입단가"},
                {7, "판매단가(한화)"},
                {8, "판매단가(미화)"},
                {9, "무게"},
                {10, "넓이"},
                {11, "길이"},
                {12, "높이"},
                {13, "과면세"},
                
                {51, "분류명(한글)"},
                {52, "분류명(영문)"},
            }
        };
        /// <summary>
        /// 코드로 상태값 반환
        /// </summary>
        /// <param name="category"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetText(string category, int code)
        {
            if (_statusMap.TryGetValue(category, out var dict) &&
                dict.TryGetValue(code, out var text))
            {
                return text;
            }
            return "알 수 없음";
        }
        /// <summary>
        /// 상태값으로 코드 반환
        /// </summary>
        /// <param name="category"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int? GetCode(string category, string text)
        {
            if (_statusMap.TryGetValue(category, out var dict))
            {
                var match = dict.FirstOrDefault(x => x.Value == text);
                return !match.Equals(default(KeyValuePair<int, string>)) ? match.Key : (int?)null;
            }
            return null;
        }
        /// <summary>
        /// 전체 상태목록 반환
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetMap(string category)
        {
            return _statusMap.TryGetValue(category, out var dict) ? dict : null;
        }
    }
}
