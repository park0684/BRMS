using System;

namespace common.DTOs
{
    public class ProductSearchDto
    {
        public string SeachWord { get; set; }
        public int CategoryTop { get; set; }
        public int CategoryMid { get; set; }
        public int CategoryBot { get; set; }
        public DateTime Type1FromDate { get; set; }
        public DateTime Type1ToDate { get; set; }
        public DateTime Type2FromDate { get; set; }
        public DateTime Type2ToDate { get; set; }
        public int Status { get; set; }
        public int DateType1 { get; set; }
        public int DateType2 { get; set; }
    }
}
