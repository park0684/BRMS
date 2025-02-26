using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Model
{
    /// <summary>
    /// 제품 검색을 위한 모델
    /// </summary>
    public class ProductSearchModel
    {
        public string SeachWord { get; set; }
        public int CategoryTop { get; set; }
        public int CategoryMid { get; set; }
        public int CategoryBot { get; set; }
        public int DateType1 { get; set; }
        public int DataType2 { get; set; }
        public DateTime Type1FromDate { get; set; }
        public DateTime Type1ToDate { get; set; }
        public DateTime Type2FromDate { get; set; }
        public DateTime Type2ToDate { get; set; }
        public int Status { get; set; }
        public string CategoryTopName { get; set; }
        public string CategoryMidName { get; set; }
        public string CategoryBotName { get; set; }
        
        /// <summary>
        /// 제품의 분류 번호로 분류명 생성
        /// </summary>
        public string CategoryName 
        { 
            get
            {
                if (CategoryTop == 0) return "전체";
                if (CategoryMid == 0) return CategoryTopName;
                if (CategoryBot == 0) return $"{CategoryTopName} ▶ {CategoryMidName}";
                return $"{CategoryTopName} ▶ {CategoryMidName} ▶ {CategoryBotName}";
            }
        }
    }
}
