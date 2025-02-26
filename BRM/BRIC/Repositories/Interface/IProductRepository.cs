using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using common.DTOs;

namespace BRIC.Repositories
{
    public interface IProductRepository
    {
        DataTable LoadProductList(ProductSearchDto product); // 제품 리스트 조회
        int LoadExchangeRete(); // 환율 호출
        DataRow LoadProduct(int code); // 제품 조회
        DataTable LoadCategory(int catTop, int catMid, bool invalidity); // 분류 정보 호출
        void InsertProduct(ProductDetailDto product, SqlConnection connection, SqlTransaction transaction); // 신규 제품 등록
        void UpdateProduct(ProductDetailDto product, SqlConnection connection, SqlTransaction transaction); // 제품 정보 수정

        
    }
}
