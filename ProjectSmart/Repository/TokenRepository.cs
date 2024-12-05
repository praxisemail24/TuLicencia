using SmartLicencia.Data;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class TokenRepository : AbstractConnection
    {
        public TokenRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public bool Create(Token token) {
            string sql = "INSERT INTO Tokens (username, token, expired_at, user_id, origin) VALUES (@userName, @token, @expiredAt, @userId, @origin)";
            return ExecQuery<SqlConnection, SqlCommand, bool>(sql, (cmd) => {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@userName", token.UserName));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@token", token.AccessToken));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@expiredAt", token.ExpiredAt));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@userId", token.UserId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@origin", token.UseOrigin));
                var rows = cmd.ExecuteNonQuery();
                return rows > 0;
            });
        }

        public int TotalRecords()
        {
            string sql = "SELECT COUNT(*) AS total FROM Tokens";
            return ExecQuery<SqlConnection, SqlCommand, int>(sql, (cmd) => {
                var total = Convert.ToInt32(cmd.ExecuteScalar());
                return total;
            });
        }

        public IEnumerable<Token> ListToken(string search, int start = 0, int perPage = 10)
        {
            search = "%" + search + "%";
            string sql = "SELECT * FROM Tokens WHERE username LIKE @search ORDER BY token_id DESC OFFSET @offset ROWS FETCH NEXT @items ROWS ONLY";
            return ExecQuery<SqlConnection, SqlCommand, IEnumerable<Token>>(sql, (cmd) => {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@search", search));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@offset", start));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@items", perPage));
                var reader = cmd.ExecuteReader();
                List<Token> tokens = new List<Token>();
                while (reader.Read())
                {
                    var tokenInstance = new Token();
                    tokenInstance.Id = reader.GetInt64(reader.GetOrdinal("token_id"));
                    tokenInstance.UserName = reader.GetString(reader.GetOrdinal("username"));
                    tokenInstance.AccessToken = reader.GetString(reader.GetOrdinal("token"));
                    tokenInstance.ExpiredAt = reader.GetDateTime(reader.GetOrdinal("expired_at"));
                    tokenInstance.UserId = reader.IsDBNull(reader.GetOrdinal("user_id")) ? null : reader.GetInt32(reader.GetOrdinal("user_id"));
                    tokenInstance.UseOrigin = reader.IsDBNull(reader.GetOrdinal("origin")) ? 0 : reader.GetInt32(reader.GetOrdinal("origin"));
                    tokenInstance.RevokedAt = reader.IsDBNull(reader.GetOrdinal("revoked_at")) ? null : reader.GetDateTime(reader.GetOrdinal("revoked_at"));
                    tokens.Add(tokenInstance);
                }
                reader.Close();
                return tokens;
            });
        }

        public Token? GetToken(string token) {
            string sql = "SELECT * FROM Tokens WHERE token = @token";
            return ExecQuery<SqlConnection, SqlCommand, Token?>(sql, (cmd) => {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@token", token));
                var reader = cmd.ExecuteReader();
                Token? tokenInstance = null;
                if (reader.Read())
                {
                    tokenInstance = new Token();
                    tokenInstance.Id = reader.GetInt64(reader.GetOrdinal("token_id"));
                    tokenInstance.UserName = reader.GetString(reader.GetOrdinal("username"));
                    tokenInstance.AccessToken = reader.GetString(reader.GetOrdinal("token"));
                    tokenInstance.ExpiredAt = reader.GetDateTime(reader.GetOrdinal("expired_at"));
                    tokenInstance.UserId = reader.IsDBNull(reader.GetOrdinal("user_id")) ? null : reader.GetInt32(reader.GetOrdinal("user_id"));
                    tokenInstance.UseOrigin = reader.IsDBNull(reader.GetOrdinal("origin")) ? 0 : reader.GetInt32(reader.GetOrdinal("origin"));
                    tokenInstance.RevokedAt = reader.IsDBNull(reader.GetOrdinal("revoked_at")) ? null : reader.GetDateTime(reader.GetOrdinal("revoked_at"));
                    return tokenInstance;
                }
                reader.Close();
                return tokenInstance;
            });
        }

        public bool RevokedToken(int id)
        {
            var sql = "UPDATE Tokens SET revoked_at = GETDATE() WHERE token_id = @id";
            return ExecQuery<SqlConnection, SqlCommand, bool>(sql, (cmd) => {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", id));
                var rows = cmd.ExecuteNonQuery();
                return rows > 0;
            });
        }

        public bool CheckIfRevoked(string token)
        {
            var sql = "SELECT COUNT(*) AS cant FROM Tokens WHERE token = @token AND revoked_at IS NOT NULL";
            return ExecQuery<SqlConnection, SqlCommand, bool>(sql, (cmd) => {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@token", token));
                var total = Convert.ToInt32(cmd.ExecuteScalar());
                return total == 0;
            });
        }
    }
}
