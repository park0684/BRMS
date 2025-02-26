using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using common.DTOs;

namespace common.Model
{
    public class ProductDetailModel
    {
        public bool IsNew { get; set; } // 신규 등록 제품 여부
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
        public int PdtPriceKrw { get; private set; } //판매가(\)
        public decimal PdtPriceUsd { get; private set; } //판매가($)
        public decimal ProfitRate { get; private set; } // 이익율
        public int PdtSupplier { get; set; } // 공급사
        public decimal Exchange { get; private set; } // 환율
        public decimal PdtWidth { get; set; } // 제품 넓이
        public decimal Pdtlength { get; set; } // 제품 길이
        public decimal PdtHigth { get; set; } // 제품 높이
        public decimal PdtWeigth { get; set; } // 제품 중량

        public ProductDetailModel(decimal exchange)
        {
            Exchange = exchange;
        }
        public static ProductDetailModel FromDto(ProductDetailDto dto)
        {
            var p = new ProductDetailModel(dto.Exchange)
            {
                Code = dto.Code,
                PdtNameKr = dto.PdtNameKr,
                PdtNameEn = dto.PdtNameEn,
                PdtNumber = dto.PdtNumber,
                CategoryTop = dto.CategoryTop,
                CategoryMid = dto.CategoryMid,
                CategoryBot = dto.CategoryBot,
                PdtSupplier = dto.PdtSupplier,
                PdtMemo = dto.PdtMemo,
                PdtStatus = dto.PdtStatus,
                PdtTax = dto.PdtTax,
                PdtBprice = dto.PdtBprice,
                PdtWidth = dto.PdtWidth,
                Pdtlength = dto.Pdtlength,
                PdtHigth = dto.PdtHigth,
                PdtWeigth = dto.PdtWeigth
            };
            p.SetPricesFromDb(dto.PdtPriceKrw, dto.PdtPriceUsd, dto.ProfitRate);
            return p;
        }
        public ProductDetailDto ToDto()
        {
            return new ProductDetailDto
            {
                Code = this.Code,
                PdtNameKr = this.PdtNameKr,
                PdtNameEn = this.PdtNameEn,
                PdtNumber  = this.PdtNumber,
                CategoryTop = this.CategoryTop,
                CategoryMid  = this.CategoryMid,              
                CategoryBot= this.CategoryBot,
                PdtSupplier = this.PdtSupplier,
                PdtMemo =this.PdtMemo,
                PdtStatus =this.PdtStatus,
                PdtTax =this.PdtTax,
                Pdtstock =this.PdtStatus,
                PdtBprice = this.PdtBprice,
                PdtPriceKrw = this.PdtPriceKrw,
                PdtPriceUsd = this.PdtPriceUsd,
                ProfitRate  = this.ProfitRate,
                Exchange = this.Exchange,
                PdtWidth = this.PdtWidth,
                Pdtlength = this.Pdtlength,
                PdtHigth = this.PdtHigth,
                PdtWeigth = this.PdtWeigth
            };

        }
        private void SetPricesFromDb(int krw, decimal usd, decimal profit)
        {
            PdtPriceKrw = krw;
            PdtPriceUsd = usd;
            ProfitRate = profit;
        }

        public void UpdatePriceKrw(int krw)
        {
            PdtPriceKrw = krw;
            PdtPriceUsd = (decimal)krw / Exchange;
            UpdateProfitRate();
        }

        public void UpdatePriceUsd(decimal usd)
        {
            PdtPriceUsd = usd;
            PdtPriceKrw = Convert.ToInt32(usd * Exchange);
            UpdateProfitRate();
        }

        public void UpdateProfitRate(decimal profit)
        {
            if (profit >= 100)
                throw new Exception("이익율은 99.99 이하로만 입력 가능합니다");
            if (PdtBprice == 0)
                throw new Exception("매입가가 0원일 경우 이익율로 판매가 설정이 불가합니다");

            ProfitRate = profit;
            PdtPriceKrw = Convert.ToInt32(PdtBprice / (1 - (profit / 100)));
            PdtPriceUsd = (decimal)PdtPriceKrw / Exchange;
        }

        private void UpdateProfitRate()
        {
            if (PdtPriceKrw <= 0)
                ProfitRate = 0;
            else
                ProfitRate = ((PdtPriceKrw - PdtBprice) / (decimal)PdtPriceKrw) * 100;
        }
    }
}
