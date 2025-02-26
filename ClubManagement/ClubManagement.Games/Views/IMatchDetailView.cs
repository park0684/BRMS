using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.Views
{
    public interface IMatchDetailView
    {
        string GameTitle { get; set; }
        string GameHost { get; set; }
        string GameMemo { get; set; }
        DateTime GameDate { get; set; }
        int GameType { get; set; }

        //
        event EventHandler SaveEvent;
        event EventHandler CloseEvenvt;

        void CloseForm();
        void ShowForm();
    }
}
