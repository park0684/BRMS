using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.Models;

namespace ClubManagement.Members.Repositories
{
    public interface IMemberRepository
    {
        DataTable GetMemberList(MemberSearchModel model);
        DataRow LoadMemberInfo(int value);
        void InsertMember(MemberModel model);
        void UpdateMember(MemberModel model);
        DataTable LoadMember(MemberSearchModel model);
    }
}
