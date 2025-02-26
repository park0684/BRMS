using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ClubManagement.Games.Models;
using ClubManagement.Common.Repositories;

namespace ClubManagement.Games.Repositories
{
    class MatchRepository : BaseRepository, IMatchRepository
    {
        public DataTable LoadMatchList(MatchSearchModel model)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT Match_code, Match_date, Match_type, Match_title, Match_host, ");
            query.Append("COUNT(att_code) as totalplayer, COUNT(CASE WHEN att_memtype = 1 THEN 1 END) as member, COUNT( CASE WHEN att_memtype = 2 THEN 1 END) as guest ");
            query.Append("FROM Match LEFT OUTER JOIN attend ON Match_code =att_code WHERE");
            query.Append($" Match_date >= '{model.FromDate.ToString("yyyy-MM-dd")}' AND Match_date < '{model.ToDate.AddDays(1).ToString("yyyy-MM-dd")}'");

            if (model.MatchType != 0)
            {
                if (model.ExcludeType)
                {
                    query.Append($" AND Match_type != {model.MatchType}");
                }
                else
                {
                    query.Append($" AND Match_type = {model.MatchType}");
                }
            }
            query.Append(" GROUP BY Match_code, Match_date, Match_type, Match_title, Match_host ORDER BY Match_date, Match_code");
            try
            {
                return SqlAdapterQuery(query.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable LoadPlayerList(int code)
        {
            string query = ($"SELECT att_name,  ISNULL(mem_birth,'') mem_birth, ISNULL(mem_gender,'') mem_gender, att_memtype FROM attend LEFT JOIN member ON att_memcode = mem_code  WHERE att_code = {code}");
            try
            {
                return SqlAdapterQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable LoadMember(MatchSearchModel model)
        {
            StringBuilder query = new StringBuilder();
            List<string> whereCondition = new List<string>();
            query.Append("SELECT mem_code, mem_name FROM member ");
            if (!string.IsNullOrEmpty(model.SearchWord))
            {
                whereCondition.Add($"mem_name LIKE '%{model.SearchWord}%'");
            }
            if (!model.IncludeSecessMember)
            {
                whereCondition.Add($"mem_status != 2");
            }

            if (whereCondition.Count > 0)
            {
                query.Append(" WHERE ");
                query.Append(string.Join(" AND ", whereCondition));
            }
            query.Append(" ORDER BY mem_name ");

            return SqlAdapterQuery(query.ToString());
        }

        public DataTable LoadAttendPlayer(int mactchCode)
        {
            string query = $"SELECT att_name, ISNULL(att_memcode,0) att_memcode FROM attend WHERE att_code = {mactchCode} ";
            return SqlAdapterQuery(query);
        }
        public void MatchPlayerUpdate(MatchModel model)
        {
            int matchCode = (int)model.MatchCode;
            string detetequery = "DELETE FROM attend WHERE att_code = @code";
            SqlParameter[] deleteParam = { new SqlParameter("@code", SqlDbType.Int) { Value = matchCode } };
            string insertQuery = "INSERT INTO attend (att_code, att_name, att_memcode, att_memtype, att_gender, att_pro, att_handi) VALUES (@code, @name, @memCode, @memType, @gender, @isPro, @handi)";

            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNonQuery(detetequery, connection, transaction, deleteParam);
                    foreach (var player in model.PlayerList)
                    {
                        SqlParameter[] insertParams =
                        {
                            new SqlParameter("@code", SqlDbType.Int) { Value = matchCode },
                            new SqlParameter("@name", SqlDbType.NVarChar) { Value = player.PalyerName },
                            new SqlParameter("@memCode", SqlDbType.Int) { Value = player.MemberCode },
                            new SqlParameter("@memType", SqlDbType.Int) { Value = player.MemberCode == 0 ? 2 : 1 },
                            new SqlParameter("@gender", SqlDbType.Int) { Value = player.Gender == true ? 0 :1 },
                            new SqlParameter("@isPro", SqlDbType.Int) { Value = player.IsPro == true? 1:0 },
                            new SqlParameter("@handi", SqlDbType.Int) { Value = player.Handycap }
                        };
                        ExecuteNonQuery(insertQuery, connection, transaction, insertParams);

                    }
                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }

        }
        public DataRow LoadMatchInfo(int matchCode)
        {
            string query = $"SELECT game_title, game_host, game_date, game_type, game_memo FROM game WHERE game_code = {matchCode}";
            DataTable result = SqlAdapterQuery(query);
            return result.Rows[0];
        }
        public void UpdateMatch(MatchModel model)
        {
            string query = "UPDATE game SET game_title = @title, game_host = @code, game_date = @date, game_type = @type, game_memo = @memo WHERE game_code = @code ";
            SqlParameter[] parameters =
            {
                new SqlParameter ("@code",SqlDbType.Int){Value = model.MatchCode},
                new SqlParameter ("@title",SqlDbType.NVarChar){Value = model.MatchTitle},
                new SqlParameter ("@host",SqlDbType.NVarChar){Value = model.MatchHost},
                new SqlParameter ("@date",SqlDbType.Date){Value = model.MatchDate.Value.ToString("yyyy-MM-dd")},
                new SqlParameter ("@type",SqlDbType.Int){Value = model.MatchType},
                new SqlParameter ("@memo",SqlDbType.NVarChar){Value = model.MatchMemo},
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
        public void InsertMatch(MatchModel model)
        {
            string query = "SELECT ISNULL(MAX(game_code),0) + 1 FROM game";
            int code = (int)ScalaQuery(query);

            query = "INSERT INTO game(game_code, game_title, game_host, game_date, game_type, game_memo) VALUES(@code, @title, @host, @date, @type, @memo)";
            SqlParameter[] parameters =
            {
                new SqlParameter ("@code",SqlDbType.Int){Value = code},
                new SqlParameter ("@title",SqlDbType.NVarChar){Value = model.MatchTitle},
                new SqlParameter ("@host",SqlDbType.NVarChar){Value = model.MatchHost},
                new SqlParameter ("@date",SqlDbType.Date){Value = model.MatchDate.Value.ToString("yyyy-MM-dd")},
                new SqlParameter ("@type",SqlDbType.Int){Value = model.MatchType},
                new SqlParameter ("@memo",SqlDbType.NVarChar){Value = model.MatchMemo},
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
