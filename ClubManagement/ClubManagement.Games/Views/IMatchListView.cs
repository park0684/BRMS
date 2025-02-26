using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ClubManagement.Games.Views
{
    public interface IMatchListView
    {
        DateTime MatchFromDate { get; set; }
        DateTime MatchToDate { get; set; }
        int? MatchType { get; set; }
        bool ExcludeType { get; set; }
        int? GetMatchCode { get; }
        string MatchTile { get; }

        //이벤트
        event EventHandler SearchMatchEvent;
        event EventHandler AddMatchEvent;
        event EventHandler EditMatchEvent;
        event EventHandler SearchPlayerEvent;
        event EventHandler EditPlayerEvent;

        //메소드
        void SetGameListBinding(DataTable source);
        void SetPlayerListBinding(DataTable source);
        void ShowMessage(string message);
    }
}
