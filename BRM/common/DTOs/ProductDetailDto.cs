using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.DTOs
{
    public class ProductDetailDto
    {
        public int Code { get; set; } // 제품 코드
        public string PdtNameKr { get; set; } //제품명(한글)
        public string PdtNameEn { get; set; } //제품명(영문)
        public string PdtNumber { get; set; } //제품번호
        public int CategoryTop { get; set; } //대분류
        public int CategoryMid { get; set; } //중분류
        public int CategoryBot { get; set; } //소분류
        public string PdtMemo { get; set; } //메모
        public int PdtStatus { get; set; } //상태
        public int PdtTax { get; set; } //과면세여부
        public int Pdtstock { get; set; } //재고
        public decimal PdtBprice { get; set; } //매입가
        public int PdtPriceKrw { get; set; } //판매가(\)
        public decimal PdtPriceUsd { get; set; } //판매가($)
        public decimal ProfitRate { get; set; } // 이익율
        public int PdtSupplier { get; set; } // 공급사
        public decimal Exchange { get; set; } // 환율
        public decimal PdtWidth { get; set; } // 제품 넓이
        public decimal Pdtlength { get; set; } // 제품 길이
        public decimal PdtHigth { get; set; } // 제품 높이
        public decimal PdtWeigth { get; set; } // 제품 중량
    }
}
