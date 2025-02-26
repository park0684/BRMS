using common.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.Reposities;
using common.Helpers;

namespace BRIC.Repositories
{
    public class ProductRepository :BaseReporsitory, IProductRepository
    {
        public int LoadExchangeRete()
        {
            string query = "SELECT cf_value FROM config WHERE cf_code = 1";
            return Convert.ToInt32(SqlScalarQuery(query));
        }

        public DataRow LoadProduct(int pdtCode)
        {
            string query ="SELECT RTRIM(pdt_name_kr) pdt_name_kr,RTRIM(pdt_name_en) pdt_name_en,RTRIM(pdt_number) pdt_number,pdt_tax,pdt_top,pdt_mid,pdt_bot,pdt_stock,pdt_status,pdt_bprice,pdt_sprice_krw,pdt_sprice_usd," +
                $"pdt_weight,pdt_width,pdt_length,pdt_height,pdt_idate,pdt_udate,pdt_sup, pdt_memo FROM product WHERE pdt_code = {pdtCode}";
            return SqlAdapterQuery(query).Rows[0];
        }

        public DataTable LoadProductList(ProductSearchDto product)
        {
            StringBuilder query = new StringBuilder();
            string select = "SELECT pdt_code AS pdtCode, pdt_status, RTRIM(pdt_number) AS pdtNumber, RTRIM(pdt_name_kr) AS pdtNamekr, RTRIM(pdt_name_en)  AS pdtNameEn,\n" +
                "pdt_bprice AS pdtBprice, pdt_sprice_krw AS pdtPriceKrw, pdt_sprice_usd AS pdtPriceUsd,\n" +
                "(SELECT cat_name_kr FROM category WHERE cat_top = pdt_top AND cat_mid = 0 AND cat_bot = 0) AS pdtTopName,\n" +
                "(SELECT cat_name_kr FROM category WHERE cat_top = pdt_top AND cat_mid = pdt_mid AND cat_bot = 0 ) AS pdtMidName,\n" +
                "(SELECT cat_name_kr FROM category WHERE cat_top = pdt_top AND cat_mid = pdt_mid AND cat_bot = pdt_bot) AS pdtBotName, pdt_idate pdtIdate, pdt_udate pdtUdate FROM product\n";

            List<string> whereCondition = new List<string>();
            List<string> orderCondition = new List<string>();
            orderCondition.Add("pdt_number");
            // 분류 정보 
            if (product.CategoryTop != 0)
            {
                whereCondition.Add($"pdt_top = {product.CategoryTop}");
                orderCondition.Add("pdt_top");
                if (product.CategoryMid != 0)
                {
                    whereCondition.Add($"pdt_mid = {product.CategoryMid}");
                    orderCondition.Add($"pdt_mid");
                }
                if (product.CategoryMid != 0 && product.CategoryBot != 0)
                {
                    whereCondition.Add($"pdt_bot = {product.CategoryBot}");
                    orderCondition.Add($"pdt_bot");
                }
            }
            // 등록일 또는 수정일 
            switch (product.DateType1)
            {
                case 1:
                    whereCondition.Add($"pdt_idate >= {product.Type1FromDate.ToString("yyyy-MM-dd")} AND pdt_idate < {product.Type1ToDate.AddDays(1).ToString("yyyy-MM-dd")}");
                    break;
                case 2:
                    whereCondition.Add($"pdt_udate >= {product.Type1FromDate.ToString("yyyy-MM-dd")} AND pdt_udate < {product.Type1ToDate.AddDays(1).ToString("yyyy-MM-dd")}");
                    break;
            }
            // 매입, 판매일
            switch (product.DateType2)
            {
                case 1:

                    whereCondition.Add($" pdt_code IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND sale_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND sale_code =  saled_code)");
                    break;
                case 2:
                    whereCondition.Add($" pdt_code IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND pur_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND pur_code = purd_code)");
                    break;
                case 3:
                    whereCondition.Add($" pdt_code IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND sale_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND sale_code =  saled_code) AND" +
                        $" pdt_code IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND pur_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND pur_code = purd_code)");
                    break;
                case 4:
                    whereCondition.Add($" pdt_code NOT IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND sale_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND sale_code =  saled_code)");
                    break;
                case 5:
                    whereCondition.Add($" pdt_code NOT IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND pur_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND pur_code = purd_code)");
                    break;
                case 6:
                    whereCondition.Add($" pdt_code NOT IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND sale_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND sale_code =  saled_code) AND" +
                        $" pdt_code NOT IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND pur_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND pur_code = purd_code)");
                    break;
                case 7:
                    whereCondition.Add($" pdt_code IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND sale_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND sale_code =  saled_code) AND" +
                        $" pdt_code NOT IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND pur_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND pur_code = purd_code)");
                    break;
                case 8:
                    whereCondition.Add($" pdt_code NOT IN (SELECT DISTINCT(saled_pdt) FROM sales,saledetail WHERE sale_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND sale_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND sale_code =  saled_code) AND" +
                        $" pdt_code IN (SELECT DISTINCT(purd_pdt) FROM purchase,purdetail WHERE pur_date >= '{product.Type2FromDate.ToString("yyyy-MM-dd")}' AND pur_date <='{product.Type2ToDate.ToString("yyyy-MM-dd")}' AND pur_code = purd_code)");
                    break;
            }
            //제품 상태
            if (product.Status != -1)
                whereCondition.Add($"pdt_status = {product.Status}");
            
            if (!string.IsNullOrEmpty(product.SeachWord))
            {
                string where = " WHERE " + string.Join(" AND ", whereCondition);
                string orderby = " ORDER BY " + string.Join(",", orderCondition);
                query.Append(select + where);
                query.Append($" AND pdt_name_kr LIKE {product.SeachWord} UNION");
                query.Append(select + where);
                query.Append($" AND pdt_name_en LIKE {product.SeachWord} UNION");
                query.Append(select + where);
                query.Append($" AND pdt_number LIKE {product.SeachWord} ");
                query.Append(orderby);
            }
            else
            {
                string where = whereCondition.Count > 0 ? " WHERE " + string.Join(" AND ", whereCondition) : "";
                string orderby = " ORDER BY " + string.Join(",", orderCondition);
                query.Append(select + where + orderby);
            }
            return SqlAdapterQuery(query.ToString());
        }

        public DataTable LoadCategory(int catTop, int catMid, bool invalidity)
        {
            StringBuilder query =  new StringBuilder();
            List<string> whereCondition = new List<string>();
            if (catTop == 0)
            {
                query.Append("SELECT cat_top as catCode");
                whereCondition.Add("cat_mid = 0");
            }
            else if (catTop != 0 && catMid == 0)
            {
                query.Append($"SELECT cat_mid as catCode");
                whereCondition.Add($"cat_top = {catTop} AND cat_mid != 0 AND cat_bot = 0");
            }
            else
            {
                query.Append($"SELECT cat_bot as catCode");
                whereCondition.Add($"cat_top = {catTop} AND cat_mid = {catMid} AND cat_bot != 0");
            }
            query.Append(", cat_name_kr as catNameKr, cat_name_en as catNameEn FROM category");
            if (invalidity == false)
                whereCondition.Add(" cat_status = 1 ");

            if (whereCondition.Count > 0)
                query.Append(" WHERE " + string.Join("AND", whereCondition));
            query.Append(" ORDER BY cat_top, cat_mid, cat_bot");


            return SqlAdapterQuery(query.ToString());
        }

        public void InsertProduct(ProductDetailDto product, SqlConnection connection, SqlTransaction transaction )
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@pdtNumber",SqlDbType.VarChar){Value = product.PdtNumber},
                new SqlParameter("@pdtNameKr",SqlDbType.VarChar){Value = product.PdtNameKr},
                new SqlParameter("@pdtNameEn",SqlDbType.VarChar){Value = product.PdtNameEn},
                new SqlParameter("@pdtSup",SqlDbType.Int){Value = product.PdtSupplier},
                new SqlParameter("@pdtTop",SqlDbType.Int){Value = product.CategoryTop},
                new SqlParameter("@pdtMid",SqlDbType.Int){Value = product.CategoryMid},
                new SqlParameter("@pdtBot",SqlDbType.Int){Value = product.CategoryBot},
                new SqlParameter("@pdtStatus",SqlDbType.Int){Value = product.PdtStatus},
                new SqlParameter("@pdtBprice",SqlDbType.Float){Value =  product.PdtBprice},
                new SqlParameter("@pdtPriceKrw",SqlDbType.Int){Value =  product.PdtPriceKrw},
                new SqlParameter("@pdtPriceUsd",SqlDbType.Float){Value = product.PdtPriceUsd},
                new SqlParameter("@pdtWeight",SqlDbType.Float){Value = product.PdtWeigth},
                new SqlParameter("@pdtWidth",SqlDbType.Float){Value =  product.PdtWidth},
                new SqlParameter("@pdtLength",SqlDbType.Float){Value =  product.Pdtlength},
                new SqlParameter("@pdtHeight",SqlDbType.Float){Value =  product.PdtHigth},
                new SqlParameter("@pdtTax",SqlDbType.Int){Value = product.PdtTax},
            };
            SqlExecuteNonQuery(StoredProcedures.InsertProduct, connection, transaction, parameters);
        }

        public void UpdateProduct(ProductDetailDto product, SqlConnection connection, SqlTransaction transaction)
        {
            SqlParameter[] parameters =
           {
                new SqlParameter("pdtCode",SqlDbType.Int){Value = product.Code},
                new SqlParameter("@pdtNumber",SqlDbType.VarChar){Value = product.PdtNumber},
                new SqlParameter("@pdtNameKr",SqlDbType.VarChar){Value = product.PdtNameKr},
                new SqlParameter("@pdtNameEn",SqlDbType.VarChar){Value = product.PdtNameEn},
                new SqlParameter("@pdtSup",SqlDbType.Int){Value = product.PdtSupplier},
                new SqlParameter("@pdtTop",SqlDbType.Int){Value = product.CategoryTop},
                new SqlParameter("@pdtMid",SqlDbType.Int){Value = product.CategoryMid},
                new SqlParameter("@pdtBot",SqlDbType.Int){Value = product.CategoryBot},
                new SqlParameter("@pdtStatus",SqlDbType.Int){Value = product.PdtStatus},
                new SqlParameter("@pdtBprice",SqlDbType.Float){Value =  product.PdtBprice},
                new SqlParameter("@pdtPriceKrw",SqlDbType.Int){Value =  product.PdtPriceKrw},
                new SqlParameter("@pdtPriceUsd",SqlDbType.Float){Value = product.PdtPriceUsd},
                new SqlParameter("@pdtWeight",SqlDbType.Float){Value = product.PdtWeigth},
                new SqlParameter("@pdtWidth",SqlDbType.Float){Value =  product.PdtWidth},
                new SqlParameter("@pdtLength",SqlDbType.Float){Value =  product.Pdtlength},
                new SqlParameter("@pdtHeight",SqlDbType.Float){Value =  product.PdtHigth},
                new SqlParameter("@pdtTax",SqlDbType.Int){Value = product.PdtTax},
            };
            SqlExecuteNonQuery(StoredProcedures.UpdateProduct, connection, transaction, parameters);
        }
    }
}
