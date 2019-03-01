using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;


namespace Dapper.Repositories
{
    public abstract class Repository
    {
        private readonly string _connectionString;

        private readonly RdbmsType _rdbms;

        private IDbConnection _connection
        {
            get
            {
                switch (_rdbms)
                {
                    case RdbmsType.MySql:
                        return new MySqlConnection(_connectionString);
                    case RdbmsType.Oracle:
                        return new OracleConnection(_connectionString);
                    case RdbmsType.SqlServer:
                        return new SqlConnection(_connectionString);
                    default:
                        throw new ArgumentException("Invalid RDBMS.");
                }
            }
        }

        protected Repository(string connectionString, RdbmsType rdbms)
        {
            _connectionString = connectionString;
            _rdbms = rdbms;
        }

        protected void Execute(string sql, dynamic param = null)
        {
            using (IDbConnection connection = _connection)
            {
                connection.Open();

                connection.Execute(
                    sql,
                    param: (object)param,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        protected async Task ExecuteAsync(string sql, dynamic param = null)
        {
            using (IDbConnection connection = _connection)
            {
                connection.Open();

                await connection.ExecuteAsync(
                    sql,
                    param: (object)param,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        protected IEnumerable<T> ExecuteQuery<T>(string sql, dynamic param = null)
        {
            using (IDbConnection connection = _connection)
            {
                connection.Open();

                return connection.Query<T>(
                    sql,
                    param: (object)param,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        protected async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string sql, dynamic param = null)
        {
            using (IDbConnection connection = _connection)
            {
                connection.Open();

                return await connection.QueryAsync<T>(
                    sql,
                    param: (object)param,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}
