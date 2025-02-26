using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.DTOs;
using BRIC.Repositories;
using System.Data;
using common.Helpers;
using System.Reflection;
using common.Interface;

namespace BRIC.Services
{
    public class ProductService
    {
        IProductRepository _repository;
        ILogRepository _logRepositry;
        ILookupRepository _lookupRepository;
        IDatabaseSession _dbsession;
        int _empCode;
        public ProductService(IProductRepository repository, int empCode)
        {
            _repository = repository;
            _logRepositry = new LogRepository();
            _dbsession = new DatabaseSession();
            _lookupRepository = new LookupRepository();
            _empCode = empCode;
        }
        /// <summary>
        /// 제품 리스트 조회
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public DataTable LoadProductList(ProductSearchDto product)
        {
            var result = _repository.LoadProductList(product);
            result.Columns.Add("pdtStatus");
            result.Columns.Add("No");
            int no = 1;
            foreach(DataRow row in result.Rows)
            {
                row["No"] = no;
                no++;
                int code = Convert.ToInt32(row["pdt_status"]);
                row["pdtStatus"] = StatusHelper.GetText(StatusHelper.Keys.ProductStatus, code);

            }
            return  result;
        }

        public int LoadExchange()
        {
            return _repository.LoadExchangeRete();
        }

        /// <summary>
        /// 제품 상세 정보 조회
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ProductDetailDto LoadProductInfo(int code)
        {
            var result = _repository.LoadProduct(code);
            ProductDetailDto product = new ProductDetailDto
            {
                Code = code,
                PdtNameKr = result["pdt_name_kr"].ToString().Trim(),
                PdtNameEn = result["pdt_name_en"].ToString().Trim(),
                PdtNumber = result["pdt_number"].ToString().Trim(),
                CategoryTop = ValueConverterHelper.ToInt(result["pdt_top"]),
                CategoryMid = ValueConverterHelper.ToInt(result["pdt_mid"]),
                CategoryBot = ValueConverterHelper.ToInt(result["pdt_bot"]),
                PdtBprice = ValueConverterHelper.ToInt(result["pdt_bprice"]),
                PdtPriceKrw = ValueConverterHelper.ToInt(result["pdt_sprice_krw"]),
                PdtPriceUsd = ValueConverterHelper.ToDecimal(result["pdt_sprice_usd"]),
                PdtStatus = ValueConverterHelper.ToInt(result["pdt_status"]),
                PdtTax = ValueConverterHelper.ToInt(result["pdt_tax"]),
                PdtSupplier  = ValueConverterHelper.ToInt(result["pdt_sup"]),
                PdtWidth = ValueConverterHelper.ToDecimal(result["pdt_width"]),
                Pdtlength = ValueConverterHelper.ToDecimal(result["pdt_length"]),
                PdtHigth = ValueConverterHelper.ToDecimal(result["pdt_height"]),
                PdtWeigth = ValueConverterHelper.ToDecimal(result["pdt_weight"]),
                Pdtstock = ValueConverterHelper.ToInt(result["pdt_stock"]),
                PdtMemo = result["pdt_memo"].ToString().Trim()
            };
            _dbsession.Begin();
            try
            {
                _logRepositry.InsertAccessLog(code, _empCode, 901, _dbsession.Connection, _dbsession.Transaction);
                _dbsession.Commin();
            }
            catch(Exception ex)
            {
                _dbsession.Rollback();
                throw new Exception(ex.Message);
            }
            return product;
            
        }

        /// <summary>
        /// 분류명 조회
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public (string Kr, string En) GetCategoryName(int topCode, int midcode, int botCode)
        {
            var krName = new List<string>
            {
                _lookupRepository.CategoryNameKr(topCode, 0, 0),
                _lookupRepository.CategoryNameKr(topCode, midcode, 0),
                _lookupRepository.CategoryNameKr(topCode, midcode, botCode)
            };
            var enName = new List<string>
            {
                _lookupRepository.CategoryNameEn(topCode, 0, 0),
                _lookupRepository.CategoryNameEn(topCode, midcode, 0),
                _lookupRepository.CategoryNameEn(topCode, midcode, botCode)
            };

            string kr = string.Join(" ▶", krName);
            string en = string.Join(" ▶", enName);

            return (kr, en);
        }
        /// <summary>
        /// 공급사 이름 조회
        /// </summary>
        /// <param name="supCode"></param>
        /// <returns></returns>
        public string GetSupplierName(int supCode)
        {
            return _lookupRepository.SupplierName(supCode);
        }
        
        /// <summary>
        /// 새로운 제품 등록
        /// </summary>
        /// <param name="product"></param>
        public void InsertProduct(ProductDetailDto product)
        {
            _dbsession.Begin();
            try
            {
                _repository.InsertProduct(product, _dbsession.Connection, _dbsession.Transaction);
                _logRepositry.InsertAccessLog(product.Code, _empCode, 902, _dbsession.Connection, _dbsession.Transaction);
            }
            catch(Exception ex)
            {
                _dbsession.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbsession.Dispose();
            }
        }

        /// <summary>
        /// 제품 수정 등록
        /// </summary>
        /// <param name="product"></param>
        /// <param name="log"></param>
        public void UpdateProdcut(ProductDetailDto product, List<LogDto> log)
        {
            _dbsession.Begin();
            try
            {
                _repository.UpdateProduct(product, _dbsession.Connection, _dbsession.Transaction);
                _logRepositry.InsertLog(log, LogHelper.Targets.ProductLog, _dbsession.Connection, _dbsession.Transaction);
                _logRepositry.InsertAccessLog(product.Code, _empCode, 907, _dbsession.Connection, _dbsession.Transaction);
                _dbsession.Commin();
            }
            catch(Exception ex)
            {
                _dbsession.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbsession.Dispose();
            }
            
        }

        /// <summary>
        /// 제품 수정내역 항목 조회 및 리스트 생성
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oringinProduct">원본 제품Dtop</param>
        /// <param name="editProduct">수정 제품Dto</param>
        /// <returns></returns>
        public List<LogDto> CompareModel<T>(ProductDetailDto oringinProduct, ProductDetailDto editProduct)
        {
            List<LogDto> changes = new List<LogDto>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var originVal = property.GetValue(oringinProduct).ToString();
                var editVal = property.GetValue(editProduct).ToString();
                if (originVal == editVal)
                    continue;
                if (LogHelper.ProductLogMap.TryGetValue(property.Name, out var loginfo))
                {
                    LogDto log = new LogDto
                    {
                        logType = loginfo.Code,
                        OriginalValue = originVal,
                        EditValue = editVal
                    };
                    changes.Add(log);
                }
                else
                    continue;
            }
            return changes;
        }
    }
}
