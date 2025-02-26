using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace common.Helpers
{
    public class LogHelper
    {
        public static Dictionary<string, (int Code, string Desc)> ProductLogMap = new Dictionary<string, (int Code, string Desc)>
        {
            ["PdtNameKr"] = (101, "제품명(한글)"),
            ["PdtNameEn"] = (102, "제품명(영문)"),
            ["PdtSupplier"] = (103, "공급사"),
            ["PdtCategory"] = (104, "분류"),
            ["PdtStatus"] = (105, "상태"),
            ["PdtBprice"] = (106, "매입단가"),
            ["PdtPriceKrw"] = (107, "판매단가(한화)"),
            ["PdtPriceUsd"] = (108, "판매단가(미화)"),
            ["PdtPdtWeigth"] = (109, "무게"),
            ["PdtPdtWidth"] = (110, "넓이"),
            ["PdtPdtlength"] = (111, "길이"),
            ["PdtPdtHigth"] = (112, "높이"),
            ["PdtTax"] = (113, "과면세"),
            ["PdtCatNameKr"] = (151, "분류명(한글)"),
            ["PdtCatNameEn"] = (152, "분류명(영문)")
        };

        public static Dictionary<string, (int Code, string Desc)> SupplierLogMap = new Dictionary<string, (int Code, string Desc)>
        {
            //공급사등록정보 200
            ["SupName"] = (201, "공급사명"),
            ["SupBzNo"] = (202, "사업자 번호"),
            ["SupBzType"] = (203, "업종"),
            ["SupIndustry"] = (204, "업태"),
            ["SupCeoName"] = (205, "대표자명"),
            ["SupTel"] = (206, "공급사 전화"),
            ["SupCel"] = (207, "담당자 휴대폰"),
            ["SupFax"] = (208, "공급사 팩스"),
            ["SupEMail"] = (209, "이메일"),
            ["SupUrl"] = (210, "홈페이지"),
            ["SupMemo"] = (211, "비고"),
            ["SupBank"] = (212, "은행"),
            ["SupAccount"] = (213, "계좌번호"),
            ["SupDepasitor"] = (214, "예금주"),
            ["SupStatus"] = (215, "상태"),
            ["SupAddress"] = (216, "주소"),
            ["SupPayType"] = (217, "결제유형"),
        };

        public static Dictionary<string, (int Code, string Desc)> PurchaseLogMap = new Dictionary<string, (int Code, string Desc)>
        {
            //매입내역 300
            ["PurDate"] = (301, "매입일"),
            ["PupplierCode"] = (302, "매입공급사"),
            ["PurchaseAmount"] = (303, "매입액"),
            ["PurPayment"] = (304, "매입결제액"),
            ["PurType"] = (305, "매입유형"),
            ["PurchaseNote"] = (306, "매입 비고"),
            ["PurchaseTaxable"] = (307, "매입 과세액"),
            ["PurchaseTaxfree"] = (308, "매입 면세액"),
            ["PurdQty"] = (312, "제품 매입량"),
            ["PurdBprice"] = (313, "제품 매입단가"),
            ["PurdAmount"] = (314, "제품 매입액"),
            ["PurdStatus"] = (315, "제품 매입상태"),
            ["PurdAdd"] = (316, "제품 추가"),
            //발주내역 400
            ["PordSup"] = (401, "발주 공급사"),
            ["PordArrivla"] = (402, "발주 입고예정일"),
            ["PordAmount"] = (403, "발주 금액"),
            ["PordNote"] = (404, "발주 메모"),
            ["PordStatus"] = (405, "발주 상태"),
            ["PorddPdt"] = (411, "발주 제품"),
            ["PorddQty"] = (412, "발주량"),
            ["PorddBprice"] = (413, "발주 매입단가"),
            ["PorddSprice"] = (414, "발주 판매단가"),
            ["PorddAmount"] = (415, "발주액"),
            ["PorddStatus"] = (416, "발주서 제품 상태"),
            ["PorddAdd"] = (417, "제품 추가"),
        };

        public static Dictionary<string, (int Code, string Desc)> SupplierPaymentLogMap = new Dictionary<string, (int Code, string Desc)>
        {
            ["PaySup"] = (501, "결제 공급사"),
            ["PayCash"] = (502, "현금 결제액"),
            ["PayTransfer"] = (503, "계좌이체액"),
            ["PayCredit"] = (504, "카드 결제액"),
            ["PayNote"] = (505, "어음 결제액"),
            ["PayDc"] = (506, "할인"),
            ["PayCoupone"] = (507, "쿠폰"),
            ["PaySupsiby"] = (508, "장려금"),
            ["PayEtc"] = (509, "기타 결제액"),
            ["PayBank"] = (510, "은행"),
            ["PayAccount"] = (511, "계좌번호"),
            ["PayDepasitor"] = (512, "예금주"),
            ["PayMemo"] = (513, "메모"),
            ["PayDate"] = (514, "결제일"),
            ["PayStatus"] = (515, "상태"),
        };

        public static Dictionary<string, (int Code, string Desc)> CustomerLogMap = new Dictionary<string, (int Code, string Desc)>
        {
            ["CustName"] = (701, "회원명"),
            ["CustTell"] = (702, "전화번호"),
            ["CustCell"] = (703, "휴대전화번호"),
            ["CustEmail"] = (704, "이메일"),
            ["CustAddr"] = (705, "주소"),
            ["CustCountry"] = (706, "국가"),
            ["CustStatus"] = (707, "상태"),
            ["CustMemo"] = (708, "메모"),
        };

        public static Dictionary<string, (int Code, string Desc)> EmployeeLogMap = new Dictionary<string, (int Code, string Desc)>
        {
            ["EmpLoging"] = (801, "로그인일시"),
            ["EmpName"] = (802, "직원명"),
            ["EmpCell"] = (803, "연락처"),
            ["EmpAddr"] = (804, "주소"),
            ["EmpEmail"] = (805, "이메일"),
            ["EmpLevel"] = (806, "직급"),
            ["EmpPemission"] = (807, "권한"),
            ["EmpPassword"] = (808, "비밀번호"),
            ["EmpStatus"] = (809, "상태"),
            ["EmpMemo"] = (810, "메모")
        };

        public static Dictionary<string, (int Code, string Desc)> EmpAcceseLogMap = new Dictionary<string, (int Code, string Desc)>
        {
            //직원접속로그(작업) 900
            ["PdtSearch"] = (901, "상품 조회"),
            ["PdtRegisted"] = (902, "상품 등록"),
            ["PdtPurSearch"] = (903, "상품 매입 조회"),
            ["PdtSaelSearch"] = (904, "상품 판매 조회"),
            ["PdtLogSearch"] = (905, "상품 변경내역 조회"),
            ["PdtStockSearch"] = (906, "상품 수불내역 조회"),
            ["PdtModify"] = (907, "상품 수정"),
            ["PurSearch"] = (908, "매입 조회"),
            ["PurModify"] = (909, "매입 수정"),
            ["PurOrderSearch"] = (910, "발주 조회"),
            ["PurOrderModify"] = (911, "발주 수정"),
            ["PaymentSearch"] = (912, "결제 조회"),
            ["PaymentModify"] = (913, "결제 수정"),
            ["CustOrderSearch"] = (914, "주문서 조회"),
            ["CustOrderModify"] = (915, "주문서 삭제"),
            ["SalesSearch"] = (916, "판매 내역 조회"),
            ["SalesReportSearch"] = (917, "판매 조회"),
            ["CustomerSearch"] = (918, "고객 조회"),
            ["CustomerRegisted"] = (919, "고객 등록"),
            ["CustomerModify"] = (920, "고객 수정"),
            ["CustomerOrderSearch"] = (921, "고객 주문 조회"),
            ["CustomerSaleSearch"] = (922, "고객 판매 조회"),
            ["CustomerLogSearch"] = (922, "고객 변경로그 조회"),
            ["SupplierSearch"] = (923, "공급사 조회"),
            ["EmployeeSearch"] = (940, "직원 조회"),
            ["EmployeeRegisted"] = (941, "직원 등록"),
            ["EmployeeModify"] = (942, "직원 수정"),
            ["EmpLogSearch"] = (951, "직원 로그 조회"),
            ["SupplierLogSearch"] = (952, "공급사 로그 조회"),
            ["PurchaseLogSearch"] = (953, "매입/발주 로그 조회"),
            ["PaymentLogSearch"] = (954, "결제 로그 조회"),
            ["PayDetailSearch"] = (924, "결제상세 조회"),
            ["PurRegisted"] = (925, "매입 등록"),
            ["PurOrderRegisted"] = (926, "발주 등록"),
            ["CategoryRegist"] = (927, "분류 등록"),
            ["CategroyModify"] = (928, "분류 수정"),
            ["DailyReportWrite"] = (929, "일결산 등록"),
            ["DailyReportSearch"] = (930, "일결산 조회"),
            ["Exchange"] = (931, "환율 변경"),
            ["RewadRate"] = (932, "포인트 적립율"),
            ["RewadOption"] = (933, "포인트 적립 설정"),
            ["PointUseOption"] = (934, "포인트 사용")
        };


        /// <summary>
        /// 직원접속 로그 기록을 위한 테이블 생성 메서드
        /// </summary>
        /// <param name="logType">로그 타입</param>
        /// <param name="empCode">직원 코드</param>
        /// <param name="logParam">조회/수정된 대상 코드</param>
        /// <returns></returns>
        public static DataTable CreateAccessLogTable(int logType, int empCode, int logParam)
        {
            var table = new DataTable();
            table.Columns.Add(Columns.LogType, typeof(int));
            table.Columns.Add(Columns.BeforeValue, typeof(string));
            table.Columns.Add(Columns.AfterValue, typeof(string));
            table.Columns.Add(Columns.EmpCode, typeof(int));
            table.Columns.Add(Columns.LogParam, typeof(int));

            table.Rows.Add(logType, "", "", empCode, logParam);
            return table;
        }

        /// <summary>
        /// 로그 기록을 위한 테이블 생성 메서드
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateLogTable()
        {
            var table = new DataTable();
            table.Columns.Add(Columns.LogType, typeof(int));
            table.Columns.Add(Columns.BeforeValue, typeof(string));
            table.Columns.Add(Columns.AfterValue, typeof(string));
            table.Columns.Add(Columns.EmpCode, typeof(int));
            table.Columns.Add(Columns.LogParam, typeof(int));
            
            return table;
        }

        /// <summary>
        /// DB 사용자 정의 테이블의 칼럼 정보입니다.
        /// </summary>
        public static class Columns
        {
            public const string LogType = "logType";//로그 코드
            public const string BeforeValue = "beforeValue"; // 변경전 값
            public const string AfterValue = "afterValue"; // 변경후 값
            public const string LogParam = "logParam"; // 적용 대상 파라미터
            public const string EmpCode = "empCode"; // 작업자 코드
        }

        /// <summary>
        /// 프로시저에 사용되는 파라미터 정보 입니다.
        /// </summary>
        public static class Parameters
        {
            /// <summary>
            /// 기록할 테이블 정보
            /// </summary>
            public const string Target = "@target";
            /// <summary>
            /// 사용자 정의 테이블에 적용할 파라미터 이름
            /// </summary>
            public const string Logs = "@logs";
        }

        /// <summary>
        /// 프로시저 @taget의 정의입니다.
        /// </summary>
        public static class Targets
        {
            /// <summary>
            /// 제품 변경 로그
            /// </summary>
            public const string ProductLog = "ProductLog";
            /// <summary>
            /// 공급사 변경 로그
            /// </summary>
            public const string SupplierLog = "SupplierLog";
            /// <summary>
            /// 공급사 결제변경 로그
            /// </summary>
            public const string PaymentLog = "PaymentLog";
            /// <summary>
            /// 고객 변경 로그
            /// </summary>
            public const string CustomerLog = "CustomerLog";
            /// <summary>
            /// 직원 접속 로그
            /// </summary>
            public const string AccessLog = "AccessLog";
            /// <summary>
            /// 직원 변경 로그
            /// </summary>
            public const string EmployeeLog = "EmployeeLog";
        }

        /// <summary>
        /// 로그를 저장하는 프로시저 사용시 사용자 정의 테이블 이름입니다
        /// </summary>
        public static class TypeNames
        {
            /// <summary>
            /// 로그를 기록하는 사용자 정의 테이블입니다.
            /// </summary>
            public const string LogInfo = "dbo.logInfo";
            /// <summary>
            /// 상품정보 사용자 정의 테이블입니다.
            /// </summary>
            public const string ProductInfo = "dbo.productInfo";
        }
    }


}
