using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.Views
{
    public interface IMemberDetailView
    {
        string MemberName { get; set; }
        string Birth { get; set; }
        string Memo { get; set; }
        int? Status { get; set; }
        int? Position { get; set; }
        int? Gender { get; set; }
        DateTime? AccessDate { get; set; }
        DateTime? SecessDate { get; set; }
        bool IsPro { get; set; }

        event EventHandler SaveEvent;
        event EventHandler CloseFormEvent;

        void CloseForm();
        void ShowMessage(string message);
        void ShowForm();
    }
}
