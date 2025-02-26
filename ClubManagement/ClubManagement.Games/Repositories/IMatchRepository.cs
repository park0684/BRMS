using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Games.Models;

namespace ClubManagement.Games.Repositories
{
    public interface IMatchRepository
    {
        DataTable LoadMatchList(MatchSearchModel model);
        DataTable LoadPlayerList(int code);
        DataTable LoadMember(MatchSearchModel model);
        DataTable LoadAttendPlayer(int mactchCode);
        void MatchPlayerUpdate(MatchModel model);
        DataRow LoadMatchInfo(int MatchCode);//모임상세내역 조회
        void UpdateMatch(MatchModel model); //모임내역 업데이트
        void InsertMatch(MatchModel model); //신규 모임 등록
    }
}
