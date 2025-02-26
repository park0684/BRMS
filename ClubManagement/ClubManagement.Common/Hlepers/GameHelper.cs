using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Common.Hlepers
{
    public static class GameHelper
    {
        public readonly static Dictionary<int, string> GaemType = new Dictionary<int, string>()
        {
            {1,"정기전" },
            {2,"비정기전" },
            {3,"이벤트" }
        };
        public readonly static Dictionary<int, string> PlayerType = new Dictionary<int, string>()
        {
            {1, "회원" },
            {2, "게스트" }
        };

        public static string GetGameType(int keyCode)
        {
            return CodeHelper.GetStatusString(GaemType, keyCode);
        }
        public static string GetPlayerType(int keycode)
        {
            return CodeHelper.GetStatusString(PlayerType, keycode);
        }

        public static int GetGameTypeCode(string valueString)
        {
            return CodeHelper.GetStatusCode(GaemType, valueString);
        }

        public static int GetPlayerTypeCode(string valueString)
        {
            return CodeHelper.GetStatusCode(GaemType, valueString);
        }
    }
}
