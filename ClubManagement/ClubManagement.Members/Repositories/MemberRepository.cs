using ClubManagement.Common.Repositories;
using ClubManagement.Members.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ClubManagement.Members.Repositories
{
    class MemberRepository : BaseRepository, IMemberRepository
    {
        public DataTable GetMemberList(MemberSearchModel model)
        {
            //회원검색 query는 회원, 정기전 참석, 비정기전 참석, 이벤트전 참석, 회비납부 현황을 각각 FROM에서 구한 후 LEFT OUTER JOIN으로 연결
            //

            StringBuilder query = new StringBuilder();

            query.Append("SELECT mem_code, mem_name ,mem_birth, mem_gender,mem_position, ISNULL(match_regular.att_count,0) reglar_count, ");
            query.Append(" ISNULL(match_irregular.att_count, 0) irregular_count, ISNULL(match_event.att_count, 0) event_count,(SELECT COUNT(match_code) FROM match WHERE match_type = 1 AND match_date >= mem_access) game_count ,mem_access, mem_secess, mem_status, ");
            query.Append("ISNULL((SELECT MAX(match_date) FROM attend, match WHERE att_code = match_code AND att_name = mem_name),NULL) match_last,");
            query.Append("CAST(DATEDIFF(MONTH,CASE WHEN mem_access < '2025-02-01' THEN '2025-02-01'  ELSE mem_access END ,GETDATE()) as INT) + 1 mem_dues,ISNULL(payment, 0) payment, mem_memo FROM ");
            //회원 검색 구간
            query.Append("(SELECT mem_code, mem_name ,mem_birth, mem_gender, mem_position, mem_access, mem_secess, mem_status,");
            query.Append(" CAST(DATEDIFF(MONTH,CASE WHEN mem_access < '2025-02-01' THEN '2025-02-01'  ELSE mem_access END,");
            query.Append($"'{DateTime.Now.ToString("d")}') as INT) + 1 [mem_duse], mem_memo FROM member \n");

            List<string> whereCondition = new List<string>();
            if (!string.IsNullOrEmpty(model.SearchWord))
                whereCondition.Add($"mem_name LIKE '%{model.SearchWord}%'");
            if (model.Status != 0)
                if (model.ExcludeMember)
                {
                    whereCondition.Add($"mem_status != {model.Status}");
                }
                else
                { whereCondition.Add($"mem_status = {model.Status}"); }
            if (model.AccessCheck)
                whereCondition.Add($"mem_access >  '{ model.AccFromDate.Value.ToString("yyyy-MM-dd")}' AND mem_access < '{model.AccToDate.Value.AddDays(1).ToString("yyyy-MM-dd")}'");
            if (model.SecessCheck)
                whereCondition.Add($"mem_secess >  '{ model.SecFromDate.Value.ToString("yyyy-MM-dd")}' AND mem_secess < '{model.SecToDate.Value.AddDays(1).ToString("yyyy-MM-dd")}'");
            if (whereCondition.Count > 0)
            {
                query.Append(" WHERE ");
                query.Append(string.Join(" AND ", whereCondition));
            }
            query.Append(") as member LEFT OUTER JOIN \n");

            string gameDate = $"match_date > '2023-12-31' AND match_date < '{DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")}' AND ";
            if (model.GameCheck)
            {
                gameDate = $" match_date > '{model.GameFromDate.Value.ToString("yyyy-MM-dd")}' AND match_date < '{model.GameTodate.Value.AddDays(1).ToString("d")}' AND ";
                query.Replace("WHERE game_type = 1 AND match_date >= mem_access) gmae_count", $"WHERE {gameDate} game_type = 1 ) game_count");
            }

            // 정기전 참석 현황 집계
            if (!model.ExcludeRegular)
            {
                query.Append(GetGameQuery(1, gameDate));
                query.Append("as match_regular ON mem_code = match_regular.att_memcode LEFT OUTER JOIN \n");
            }
            else
            {
                query.Replace(" ISNULL(match_regular.att_count,0) reglar_count", " 0 reglar_count");
            }
            // 비정기전 참석 현황 집계
            if (!model.ExcludeIrregular)
            {
                query.Append(GetGameQuery(2, gameDate));
                query.Append("as match_irregular ON mem_code = match_irregular.att_memcode LEFT OUTER JOIN \n");
            }
            else
            {
                query.Replace(" ISNULL(match_irregular.att_count, 0) irregular_count", " 0 irreglar_count");
            }
            // 이벤트전 참석 현황 집계
            if (!model.ExcludeEvent)
            {
                query.Append(GetGameQuery(3, gameDate));
                query.Append("as match_event ON mem_code = match_event.att_memcode LEFT OUTER JOIN \n");
            }
            else
            {
                query.Replace(" ISNULL(match_event.att_count, 0) event_count", " 0 event_count");
            }

            //회비 납부 현황 집계
            query.Append("(select du_memcode, sum(du_apply) as payment FROM dues WHERE du_status = 1 GROUP BY du_memcode) as dues ON mem_code = du_memcode");

            //정렬 
            query.Append(" ORDER BY mem_position, mem_status, mem_code");

            return SqlAdapterQuery(query.ToString());
        }

        public string GetGameQuery(int type, string gameDate)
        {
            string query = "(SELECT att_memcode, COUNT(att_name) att_count FROM attend, match WHERE ";
            if (!gameDate.Equals("N/A"))
            {
                query += gameDate;
            }
            query += $" match_type = {type} AND att_code = match_code GROUP BY att_memcode)";
            return query;

        }

        public DataRow LoadMemberInfo(int value)
        {
            string query = $"SELECT mem_name, mem_birth, mem_gender, mem_status, mem_position, mem_access, mem_secess, mem_memo FROM member WHERE mem_code = {value}";
            DataTable result = SqlAdapterQuery(query);

            return result.Rows[0];
        }

        public void InsertMember(MemberModel model)
        {
            string query = "SELECT max(mem_code)+1 FROM member";
            DataTable result = SqlAdapterQuery(query);
            DataRow row = result.Rows[0];
            var code = row[0];
            query = "INSERT INTO MEMBER (mem_code, mem_name, mem_status, mem_birth, mem_gender, mem_position, mem_memo, mem_access)" +
                $"VALUES (@code, @name, @status, @birth, @gender, @position, @memo, @access)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@code",SqlDbType.Int){Value = code },
                new SqlParameter("@name",SqlDbType.NVarChar){Value = model.MemberName},
                new SqlParameter("@status",SqlDbType.Int){Value=model.Status },
                new SqlParameter("@birth",SqlDbType.NVarChar){Value=model.Birth },
                new SqlParameter("@gender",SqlDbType.Int){Value = model.Gender},
                new SqlParameter("@position", SqlDbType.Int){ Value = model.Position },
                new SqlParameter("@memo", SqlDbType.NVarChar){ Value = model.Memo },
                new SqlParameter("@access", SqlDbType.Date){Value = model.AccessDate.Value.ToString("yyyy-MM-dd")}
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
                    throw new Exception("회원 등록 오류 : "+ex.Message);
                }
            }
        }

        public void UpdateMember(MemberModel model)
        {
            string query = $"UPDATE member SET mem_name = @name, mem_birth=@birth, mem_status = @status, mem_gender = @gender, mem_position = @position, mem_access = @access, mem_secess = @secess, mem_memo = @memo WHERE mem_code = @code";
            SqlParameter[] parameters =
            {
                new SqlParameter("@code",SqlDbType.Int){Value = model.Code },
                new SqlParameter("@name",SqlDbType.NVarChar){Value = model.MemberName},
                new SqlParameter("@status",SqlDbType.Int){Value=model.Status },
                new SqlParameter("@birth",SqlDbType.NVarChar){Value=model.Birth },
                new SqlParameter("@gender",SqlDbType.Int){Value = model.Gender},
                new SqlParameter("@position", SqlDbType.Int){ Value = model.Position },
                new SqlParameter("@memo", SqlDbType.NVarChar){ Value = model.Memo },
                new SqlParameter("@access", SqlDbType.Date){Value = model.AccessDate.Value.ToString("yyyy-MM-dd")},
                new SqlParameter("@secess", SqlDbType.Date){Value = DBNull.Value  } //2025-03-31 탈퇴 회원이 아님에도 탈퇴일이 기록되어 기본Null로 수정 
            };
            if (model.Status == 2) // 2025-03-31 --> 회원 변경시 탈퇴이라면 탈퇴일자 적용
                parameters[8].Value = model.SecessDate.Value.ToString("yyyy-MM-dd");


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
                    throw new Exception("회원 수정 오류 : " + ex.Message);
                }
            }
        }
        public DataTable LoadMember(MemberSearchModel model)
        {
            StringBuilder query = new StringBuilder();
            List<string> whereCondition = new List<string>();
            query.Append($"SELECT mem_code, mem_name FROM member ");
            if (!string.IsNullOrEmpty(model.SearchWord))
                whereCondition.Add($"mem_name LIKE '%{model.SearchWord}%'");
            if (!model.IsInclude)
                whereCondition.Add("mem_status != 2");
            if (whereCondition.Count > 0)
                query.Append(" WHERE ");
            query.Append(string.Join(" AND ", whereCondition));
            query.Append(" ORDER BY mem_name");

            return SqlAdapterQuery(query.ToString());
        }
    }
}
