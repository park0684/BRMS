using System.Collections.Generic;

namespace ClubManagement.Common.Hlepers
{
    public static class MemberHelper
    {
        public readonly static Dictionary<int, string> MemStatus = new Dictionary<int, string>()
        {
            {1,"유효" },
            {2,"탈퇴" },
            {3,"열외" }
        };
        //회원 직책
        public readonly static Dictionary<int, string> MemPosition = new Dictionary<int, string>()
        {
            {1,"회장" },
            {2,"운영진" },
            {3,"볼링장 직원" },
            {4,"정회원" },
            {5,"프로샵 직원" }
        };
        //성별
        public readonly static Dictionary<int, string> MemGender = new Dictionary<int, string>()
        {
            {0, "여" },
            {1, "남" }
        };
        //회비
        public readonly static Dictionary<int, string> DuesType = new Dictionary<int, string>()
        {
            {1,"회비" },
            {2,"지출" },
            {3,"면제" },
            {99, "기타" }
        };


        //코드 -> 문자열 변환
        public static string GetMemberStatus(int keyCode)
        {
            return CodeHelper.GetStatusString(MemStatus, keyCode);
        }
        public static string GetMemberPositon(int keyCode)
        {
            return CodeHelper.GetStatusString(MemPosition, keyCode);
        }
        public static string GetMemberGenger(int keyCode)
        {
            return CodeHelper.GetStatusString(MemGender, keyCode);
        }

        public static string GetDuesType(int keycode)
        {
            return CodeHelper.GetStatusString(DuesType, keycode);
        }


        //문자열 -> 코드 변환
        public static int GetMemberStatusCode(string valueString)
        {
            return CodeHelper.GetStatusCode(MemStatus, valueString);
        }
        public static int GetMemnerPositionCode(string valueString)
        {
            return CodeHelper.GetStatusCode(MemPosition, valueString);
        }
        public static int GetMemberGenderCode(string valueString)
        {
            return CodeHelper.GetStatusCode(MemGender, valueString);
        }
        public static int GetDuesTypeCode(string valueString)
        {
            return CodeHelper.GetStatusCode(DuesType, valueString);
        }
    }
}
