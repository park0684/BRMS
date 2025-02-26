using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ClubManagement.Members.Views
{
    public interface IDuesManageView
    {
        string SearchWord { get; set; }
        DateTime FromDate { get; set; }
        DateTime ToDate { get; set; }
        bool SecessInclude { get; set; }
        int? GetMemberCode { get; }
        int? GetStateMentCode { get; }

        event EventHandler MemberSearchEvent;
        event EventHandler StatementSearchEvent;
        event EventHandler StatementAddEvent;
        event EventHandler StatementEditEvent;

        void SetMemberListBinding(DataTable members);
        void SetStateListBinding(DataTable States);
    }
}

