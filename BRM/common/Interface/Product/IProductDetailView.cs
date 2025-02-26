using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace common.Interface
{
    public interface IProductDetailView
    {
        string PdtNameKr { get; set; } //제품명(한글)
        string PdtNameEn { get; set; } //제품명(영문)
        string PdtNumber { get; set; } //제품번호
        string CategoryNameKr { set; } //분류명(한글)
        string CategoryNameEn { set; } //분류명(영문)
        string PdtMemo { get; set; } //메모
        int PdtStatus { get; set; } //상태
        int PdtTax { get; set; } //과면세여부
        decimal PdtBprice { get; set; } //매입가
        int PdtPriceKrw { get; set; } //판매가(\)
        decimal PdtPriceUsd { get; set; } //판매가($)
        decimal PdtProfitRate { get; set; } // 이익율
        string PdtSupplier { set; } // 공급사
        decimal PdtWidth { get; set; } // 제품 넓이
        decimal Pdtlength { get; set; } // 제품 길이
        decimal PdtHigth { get; set; } // 제품 높이
        decimal PdtWeigth { get; set; } // 제품 중량
        DateTime PurchaseFromDate { get; } // 매입 조회 시작일
        DateTime purchaseTodate { get; } // 매입 조회 종료일
        DateTime SaleFromDate { get; } // 판매 조회 시작일
        DateTime SaleToDate { get; } // 판매 조회 종료일
        DateTime LogFromDate { get; } // 로그 조회 시작일
        DateTime LogToDate { get; } // 로그 조회 종료일
        int stock { get; set; } // 현재고

        event EventHandler CloseFormEvent;
        event EventHandler CategorySelectEvent;
        event EventHandler SupplierSelectEvent;
        event EventHandler SaveEvent;
        event EventHandler UsdPriceChangeEvent;
        event EventHandler KrwPriceChangeEvent;
        event EventHandler BpriceChangeEvent;
        event EventHandler ProfitRateChageEvent;

        void PurchaseDataBinding(DataTable dataTable);
        void SaleDataBinding(DataTable dataTable);
        void EditLogDateBinding(DataTable dataTable);
        void ShowForm();
        void CloseForm();
        void ShowMessage(string message);
        void SetTabcontrol(bool isNew, List<Dictionary<string, string>> columns = null);
    }
}
