using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.Views
{
    public interface IStatementDetailView
    {
        DateTime StatementDate { get; set; }
        int? StatementType { get; set; }
        int DueCount { get; set; }
        int? DueAmount { get; set; }
        int? Withdrawal { get; set; }
        int? Apply { get; set; }
        string MemberName { get; set; }
        string WithdrawalDetail { get; set; }
        string Memo { get; set; }
        bool IsWithdrawal { get; }

        event EventHandler SaveEvent;
        event EventHandler CloseEvent;
        event EventHandler SelectMemberEvent;
        event EventHandler DeleteEvent;
        event EventHandler TypeChaingedEvnet;
        void CloseForm();
        void ShowForm();
        void SetDeleteButtonVisivle();
        void TypeChaingedSet();
    }
}
