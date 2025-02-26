using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BRIC.Repositories;
using common.DTOs;
using common.Helpers;
using common.Interface;

namespace BRIC.Services
{
    public class LogService
    {
        ILogRepository _repository;
        ILookupRepository _lokupRepository;
        IDatabaseSession _dbsession;
        int _empCode;

        public LogService(int empCode)
        {
            _repository = new LogRepository();
            _lokupRepository = new LookupRepository();
            _dbsession = new DatabaseSession();
            _empCode = empCode;
        }

        /// <summary>
        /// 제품 및 분류 변경 로그 조회
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public DataTable LoadProductLog(LogSearchDto search)
        {
            var result = _repository.LoadProductLog(search);
            result.Columns.Add("logPdtName");
            result.Columns.Add("logPdtNumber");
            result.Columns.Add("logBefore");
            result.Columns.Add("logAfter");
            //result.Columns.Add("logEmpName");
            result.Columns.Add("logType");
            result.Columns.Add("No");
            //result.Columns["pdtlog_before"].ColumnName = "logBefore";
            //result.Columns["pdtlog_after"].ColumnName = "logAfter";
            result.Columns["pdtlog_param"].ColumnName = "logParam";
            result.Columns["emp_name"].ColumnName = "logEmp";
            result.Columns["pdtlog_date"].ColumnName = "logDate";
            
            //제품 로그 딕셔너리 생성
            var items = LogHelper.ProductLogMap;
            Dictionary<int, string> logMap = new Dictionary<int, string>();
            foreach(var item in items)
            {
                logMap.Add(item.Value.Code, item.Value.Desc);
            }

            int number = 1;
            foreach (DataRow row in result.Rows)
            {
                int paramCode = Convert.ToInt32(row["logParam"]);
                DataRow info;
                if (Convert.ToInt32(row["pdtlog_type"]) < 150)
                {
                    info = _repository.LoadProductINfo(paramCode);
                    row["logPdtNumber"] = info["pdt_number"];
                    row["logPdtName"] = info["pdt_name_Kr"];
                }
                else
                {
                    info = _repository.LoadCategorInfo(paramCode);
                    row["logPdtNumber"] = info["cat_full"];
                    row["logPdtName"] = info["cat_name"];
                }
                //row["logEmpName"] = _repository.LoadEmployee(Convert.ToInt32(row["logEmp"])); => 작업자를 로그 조회 후 반영이 아닌 조회시 가져오도록 수정
                
                // 작업 유형
                logMap.TryGetValue((int)row["pdtlog_type"], out string typeDesc);
                row["logType"] = typeDesc;

                // 작업 유형에 따른 변경 전, 후 칼럼의 입력할 데이터 가공
                switch (row["pdtlog_type"])
                {
                    case 103: // 공급사변경
                        row["logBefore"] = _lokupRepository.SupplierName(Convert.ToInt32(row["pdtlog_before"]));
                        row["logAfter"] = _lokupRepository.SupplierName(Convert.ToInt32(row["pdtlog_after"]));
                        break;
                    case 104: // 분류변경
                        string[] before = row["pdtlog_before"].ToString().Split('_');
                        string[] after = row["pdtlog_after"].ToString().Split('_');
                        int beforeTop = int.Parse(before[0]);
                        int beforeMid = int.Parse(before[1]);
                        int beforeBot = int.Parse(before[2]); 
                        int afterTop = int.Parse(before[0]);
                        int afterMid = int.Parse(before[1]); 
                        int afterBot = int.Parse(before[2]);
                        List<string> beforeName = new List<string>
                        {
                            _lokupRepository.CategoryNameKr(beforeTop, 0, 0),
                            _lokupRepository.CategoryNameKr(beforeTop, beforeMid, 0),
                            _lokupRepository.CategoryNameKr(beforeTop, beforeMid, beforeBot),
                        };
                        List<string> afterName = new List<string>
                        {
                            _lokupRepository.CategoryNameKr(afterTop, 0, 0),
                            _lokupRepository.CategoryNameKr(afterTop, afterMid, 0),
                            _lokupRepository.CategoryNameKr(afterTop, afterMid, afterBot),
                        };

                        row["logBefore"] = string.Join(" ▶ ", beforeName);
                        row["logAfter"] = string.Join(" ▶ ", afterName);
                        break;
                    case 105: //상태변경
                        row["logBefore"] = StatusHelper.GetText(StatusHelper.Keys.ProductStatus, Convert.ToInt32(row["pdtlog_before"]));
                        row["logAfter"] = StatusHelper.GetText(StatusHelper.Keys.ProductStatus, Convert.ToInt32(row["pdtlog_after"]));
                        break;
                    case 133: // 과면세 변경
                        row["logBefore"] = StatusHelper.GetText(StatusHelper.Keys.TaxStatus, Convert.ToInt32(row["pdtlog_before"]));
                        row["logAfter"] = StatusHelper.GetText(StatusHelper.Keys.TaxStatus, Convert.ToInt32(row["pdtlog_after"]));
                        break;
                    default: // 그 외 작업유형은 기존 값 수정 없이 그대로 반영
                        row["logBefore"] = row["pdtlog_before"];
                        row["logAfter"] = row["pdtlog_after"];
                        break;

                }
                row["No"] = number;
                number++; 
            }
            return result;
        }

        /// <summary>
        /// 공급사 변경 로그 조회
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public DataTable LodaSupplierLog(LogSearchDto search)
        {
            var result = _repository.LoadSupplierLog(search);
            _dbsession.Begin();
            InsertAccLog(952);
            return result;
        }

        /// <summary>
        /// 매입/발주 변경로그 조회
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public DataTable LoadPurchaseLog(LogSearchDto search)
        {
            var result = _repository.LoadPurchaseLog(search);

            return result;
        }

        /// <summary>
        /// 공급사 결제 변경 로그 조회
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public DataTable LoadSupPaymentLog(LogSearchDto search)
        {
            var result = _repository.LoadSupPaymentLog(search);

            return result;
        }

        /// <summary>
        /// 고객 변경 로그 조회
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public DataTable LoadCustomerLog(LogSearchDto search)
        {
            var result = _repository.LoadCustomerLog(search);

            return result;
        }

        /// <summary>
        /// 직원 변경 로그 조회
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public DataTable LoadEmployeeLog(LogSearchDto search)
        {
            var result = _repository.LoadEmployeeLog(search);

            return result;
        }

        /// <summary>
        /// 직원 접속 로그 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public DataTable LoadEmpAccessLog(LogSearchDto search)
        {
            var result = _repository.LoadEmpAccessLog(search);

            return result;
        }

        /// <summary>
        /// 로그 조회 내용 직원접속로그에 기록
        /// </summary>
        /// <param name="typeCode">조회내역</param>
        private void InsertAccLog(int typeCode)
        {
            _dbsession.Begin();
            try
            {
                _repository.InsertAccessLog(0, _empCode, typeCode, _dbsession.Connection, _dbsession.Transaction);
                _dbsession.Commin();
            }
            catch(Exception ex)
            {
                _dbsession.Rollback();
                throw new Exception(ex.Message);
            }
            
        }
    }
}
