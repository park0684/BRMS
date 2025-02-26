using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ClubManagement.Common.Repositories;
using ClubManagement.Members.Models;

namespace ClubManagement.Members.Repositories
{
    class DuesRepsotiry : BaseRepository, IDuesRepsotiry
    {
        public DataRow GetBalance(DuesModel model)
        {
            string query = $"SELECT ISNULL(SUM(CASE WHEN du_type = 1 THEN ISNULL(du_pay, 0) ELSE 0 END),0) AS deus, ISNULL(SUM(CASE WHEN du_type IN ( 2,4) THEN ISNULL(du_pay, 0) ELSE 0 END),0) AS payment FROM dues WHERE du_date < '{model.FromDate.ToString("yyyy-MM-dd")}' AND du_status = 1";

            return SqlAdapterQuery(query).Rows[0];
        }

        public DataTable GetMemberList(DuesModel model)
        {
            StringBuilder query = new StringBuilder();
            List<string> whereCondition = new List<string>();
            query.Append("SELECT mem_code, mem_name, mem_birth, SUM(ISNULL(payment,0)) payment, SUM(ISNULL(free,0)) free, DATEDIFF(m,CASE WHEN mem_access < '2025-02-01' THEN '2025-02-01' ELSE mem_access END, GETDATE())+1 totaldues  FROM member LEFT OUTER JOIN ");
            query.Append("(SELECT du_memcode , CASE WHEN du_type = 1 THEN SUM(du_apply) END payment, CASE WHEN du_type = 3 THEN SUM(du_apply) END free FROM dues WHERE du_type IN ( 1, 3) AND du_status = 1 GROUP BY du_memcode, du_type) as dues ON  mem_code = du_memcode");
            if (!string.IsNullOrEmpty(model.SearchWord))
            {
                whereCondition.Add($"mem_name LIKE '%{model.SearchWord}%'");
            }
            if (!model.SecessInclude)
            {
                whereCondition.Add($"mem_status != 2");
            }
            if (whereCondition.Count > 0)
            {
                query.Append(" Where ");
                query.Append(string.Join(" and ", whereCondition));
            }
            query.Append(" GROUP BY mem_code, mem_name, mem_birth, mem_access");
            return SqlAdapterQuery(query.ToString());
        }

        public DataTable GetStateList(DuesModel model)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT du_code, du_date, du_type, du_detail , du_apply, CASE WHEN du_type IN (1,3) THEN du_pay ELSE 0 END as deposit, CASE WHEN du_type IN (2,4) THEN du_pay ELSE 0 END whthdrawal FROM dues");
            query.Append($" WHERE du_date >= '{model.FromDate.ToString("yyyy-MM-dd")}' AND du_date < '{model.ToDate.AddDays(1).ToString("yyyy-MM-dd")}' AND du_status = 1");
            if (model.MemberCode != 0)
                query.Append($" AND du_memcode = {model.MemberCode}");
            query.Append("ORDER BY du_date, du_code");
            return SqlAdapterQuery(query.ToString());
        }

        public void DeleteStatment(int statmentCode)
        {
            string query = "UPDATE dues SET du_status = 0 WHERE du_code = @code";
            SqlParameter[] parameters = { new SqlParameter("@code", SqlDbType.Int) { Value = statmentCode } };
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNonQuery(query, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public void InsertStatment(StatementModel model)
        {
            string query = "SELECT ISNULL(MAX(du_code),0) +1 FROM dues";
            int code = Convert.ToInt32(ScalaQuery(query));
            query = "INSERT INTO dues(du_code, du_date, du_apply, du_type, du_pay, du_memcode, du_detail, du_memo, du_idate, du_udate, du_status) VALUES(@code, @date, @apply, @type, @amount, @memcode, @detail, @memo, GETDATE(), GETDATE(), 1)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@code",SqlDbType.Int){Value = code },
                new SqlParameter("@date",SqlDbType.Date){Value = model.StatementDate},
                new SqlParameter("@apply",SqlDbType.Int){Value = model.Apply},
                new SqlParameter("@type",SqlDbType.Int){Value = model.StatementType},
                new SqlParameter("@amount",SqlDbType.Int){Value = model.StatementAmount},
                new SqlParameter("@memcode",SqlDbType.Int){Value = model.MemberCode?? 0},
                new SqlParameter("@detail",SqlDbType.VarChar){Value = model.StatementDetail},
                new SqlParameter("@memo",SqlDbType.VarChar){Value = model.Memo}
            };
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNonQuery(query, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public DataRow LoadStatmet(int statmentcode)
        {
            string query = $"SELECT du_date, du_apply, du_pay, du_memcode, du_type,  du_detail, du_memo FROM dues WHERE du_code = {statmentcode} ";

            return SqlAdapterQuery(query).Rows[0];
        }

        public void UpdateStatment(StatementModel model)
        {
            string query = $"UPDATE dues SET du_date  = @date, du_apply = @apply, du_type  = @type, du_pay  = @amount, du_memcode = @memcode, du_detail = @detail, du_memo = @memo, du_udate = GETDATE() , du_status = 1 WHERE du_code ={model.StatementCode}";
            SqlParameter[] parameters =
            {
                new SqlParameter("@date",SqlDbType.Date){Value = model.StatementDate.ToString("yyyy-MM-dd")},
                new SqlParameter("@type", SqlDbType.Int) { Value = model.StatementType },
                new SqlParameter("@apply",SqlDbType.Int){ Value = model.Apply},
                new SqlParameter("@amount", SqlDbType.Int) { Value = model.StatementAmount },
                new SqlParameter("@memcode", SqlDbType.Int) { Value = model.MemberCode },
                new SqlParameter("@detail", SqlDbType.VarChar) { Value = model.StatementDetail },
                new SqlParameter("@memo", SqlDbType.VarChar) { Value = model.Memo }
            };
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNonQuery(query, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
