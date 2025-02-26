using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.Models;

namespace ClubManagement.Members.Repositories
{
    public interface IDuesRepsotiry
    {
        DataTable GetMemberList(DuesModel model);
        DataTable GetStateList(DuesModel model);
        DataRow GetBalance(DuesModel model);
        void InsertStatment(StatementModel model);
        void UpdateStatment(StatementModel model);
        void DeleteStatment(int statmentCode);
        DataRow LoadStatmet(int statmentcode);

    }
}
