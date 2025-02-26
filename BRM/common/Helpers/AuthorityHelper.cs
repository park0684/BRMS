using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.Model;


namespace common.Helpers
{
    public class AuthorityHelper
    {
        public static bool Has(int AuthorityCode)
        {
            var user = UserSession.CurrentUser;
            return user != null && user.PermissionList.Contains(AuthorityCode);
        }

        public static void CheckOrThrow(int AuthorityCode)
        {
            if (!Has(AuthorityCode))
            {
                string name;
                if (!AuthorityMap.EmployeeAuthority.TryGetValue(AuthorityCode, out name))
                    name = AuthorityCode.ToString();

                throw new UnauthorizedAccessException($"[{name}] 조회 권한이 없습니다");
            }
        }
    }

    public static class AuthorityMap
    {
        public static readonly Dictionary<int, string> EmployeeAuthority = new Dictionary<int, string>
        {
            {101, "제품 조회" },
            {102, "제품 등록/수정" },
            {103, "제품 인쇄" },
            {104, "제품 엑셀" },
            {121, "분류 조회" },
            {122, "분류 등록/수정" },
            {131, "공급사 조회" },
            {132, "공급사 등록/수정" },
            {133, "공급사 인쇄" },
            {134, "공급사 엑셀" },
            {201, "매입발주 조회" },
            {202, "매입발주 등록/수정" },
            {203, "매입발주 인쇄" },
            {204, "매입발주 엑셀" },
            {221, "결제 조회" },
            {222, "결제 등록/수정" },
            {223, "결제 인쇄" },
            {224, "결제 엑셀" },
            {301, "주문서 조회" },
            {302, "주문서 등록/수정" },
            {303, "주문서 인쇄" },
            {304, "주문서 엑셀" },
            {311, "판매 조회" },
            {313, "판매내역 인쇄" },
            {314, "판매내역 엑셀" },
            {351, "일결산 조회" },
            {352, "일결산 등록/수정" },
            {353, "일결산 인쇄" },
            {354, "일결산 엑셀" },
            {401, "회원 조회" },
            {402, "회원 등록/수정" },
            {403, "회원 인쇄" },
            {404, "회원 엑셀" },
            {501, "직원 조회" },
            {502, "직원 등록/수정" },
            {503, "직원 인쇄" },
            {504, "직원 엑셀" },
        };
    };
}