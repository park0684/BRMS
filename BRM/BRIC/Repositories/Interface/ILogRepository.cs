using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using common.DTOs;
using System.Data.SqlClient;

namespace BRIC.Repositories
{
    public interface ILogRepository
    {
        /// <summary>
        /// 제품 변경 로그 조회
        /// </summary>
        /// <param name="search">검색 조건</param>
        /// <returns></returns>
        
        DataTable LoadProductLog(LogSearchDto search); // 로그 조회
        /// <summary>
        /// 공급사 변경 로그 조회
        /// </summary>
        /// <param name="search">검색 조건</param>
        /// <returns></returns>
        DataTable LoadSupplierLog(LogSearchDto search); // 로그 조회

        /// <summary>
        /// 매입/발주 변경 로그 조회
        /// </summary>
        /// <param name="search">검색 조건</param>
        /// <returns></returns>
        DataTable LoadPurchaseLog(LogSearchDto search);

        /// <summary>
        /// 공급사 결제 변경 로그 조회
        /// </summary>
        /// <param name="search">검색 조건</param>
        /// <returns></returns>
        DataTable LoadSupPaymentLog(LogSearchDto search);

        /// <summary>
        /// 고객 변경 로그 조회
        /// </summary>
        /// <param name="search">검색 조건</param>
        /// <returns></returns>
        DataTable LoadCustomerLog(LogSearchDto search);

        /// <summary>
        /// 직원 변경 로그 조회
        /// </summary>
        /// <param name="search">검색 조건</param>
        /// <returns></returns>
        DataTable LoadEmployeeLog(LogSearchDto search);

        /// <summary>
        /// 직원 접속 로그 조회
        /// </summary>
        /// <param name="search">검색 조건</param>
        /// <returns></returns>
        DataTable LoadEmpAccessLog(LogSearchDto search); // 로그 조회

        /// <summary>
        /// 로그에서 표시 할 작업자명 호출
        /// </summary>
        /// <param name="empCode">직원코드</param>
        /// <returns></returns>
        string LoadEmployee(int empCode);

        /// <summary>
        /// 로그에서 표시할 공급사 이름
        /// </summary>
        /// <param name="supCode"></param>
        /// <returns></returns>
        string LoadSupplierName(int supCode);

        /// <summary>
        /// 제품 변경로그에 표시 할 제품정보 호출
        /// </summary>
        /// <param name="pdtCode"></param>
        /// <returns></returns>
        DataRow LoadProductINfo(int pdtCode); // 제품 호출
        DataRow LoadCategorInfo(int catCode);

        /// <summary>
        /// 직원접속 로그 기록 메서드.
        /// </summary>
        /// <param name="parameterCode">작업 대상 코드</param>
        /// <param name="empCode">접속 직원 코드</param>
        /// <param name="TypeCode">로그 코드</param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        void InsertAccessLog(int parameterCode, int empCode, int TypeCode, SqlConnection connection, SqlTransaction transaction);

        /// <summary>
        /// 로그 기록 메서드.
        /// proceduerTaget은 LogHelper.Target에서 지정
        /// </summary>
        /// <param name="logs">기록할 변경 내역 목록</param>
        /// <param name="proceduerTaget">기록 할 로그 대상</param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        void InsertLog(List<LogDto> logs, string proceduerTaget, SqlConnection connection, SqlTransaction transaction);
    }
}
