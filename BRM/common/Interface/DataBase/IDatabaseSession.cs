using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace common.Interface
{
    public interface IDatabaseSession : IDisposable
    {
        SqlConnection Connection { get; }
        SqlTransaction Transaction { get; }

        void Begin();
        void Commin();
        void Rollback();
    }
}
