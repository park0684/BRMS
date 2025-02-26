using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIC.Repositories
{
    public interface ILookupRepository
    {
        /// <summary>
        /// 직원명 조회
        /// </summary>
        /// <param name="empCode"></param>
        /// <returns></returns>
        string EmployeeName(int empCode);

        /// <summary>
        /// 공급사명 조회
        /// </summary>
        /// <param name="supCode"></param>
        /// <returns></returns>
        string SupplierName(int supCode);

        /// <summary>
        /// 제품명(한글) 조회
        /// </summary>
        /// <param name="pdtCode"></param>
        /// <returns></returns>
        string ProductName(int pdtCode);

        /// <summary>
        /// 지정된 분류코드 분류명 조회.
        /// </summary>
        /// <param name="catCode"></param>
        /// <returns></returns>
        string CategoryName(int catCode);

        /// <summary>
        /// 분류번호로 분류명(한글) 조회
        /// </summary>
        /// <param name="top"></param>
        /// <param name="mid"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        string CategoryNameKr(int top, int mid, int bot);

        /// <summary>
        /// 분류번호로 분류명(영문) 조회
        /// </summary>
        /// <param name="top"></param>
        /// <param name="mid"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        string CategoryNameEn(int top, int mid, int bot);

        /// <summary>
        /// 고객명 조회
        /// </summary>
        /// <param name="memCode"></param>
        /// <returns></returns>
        string CustomerName(int memCode);
    }
}
