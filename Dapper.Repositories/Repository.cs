using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
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

        protected object CreateDynamicParameters<T>(T obj)
        {
            IEnumerable<IParameter> parameters = ToParameters(obj);

            return CreateDynamicParameters(parameters);
        }

        protected object CreateDynamicParameters(IEnumerable<IParameter> parameters)
        {
            switch (_rdbms)
            {
                case RdbmsType.Oracle:
                    return CreateOracleParameters(parameters);
                default:
                    return CreateParameters(parameters);
            }
        }

        private static DynamicParameters CreateParameters(IEnumerable<IParameter> parameters)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();

            foreach (IParameter parameter in parameters)
            {
                dynamicParameters.Add(parameter.Name, parameter.Value, dbType: parameter.DbType, direction: parameter.ParameterDirection, size: parameter.Size);
            }

            return dynamicParameters;
        }

        private static OracleDynamicParameters CreateOracleParameters(IEnumerable<IParameter> parameters)
        {
            OracleDynamicParameters oracleDynamicParameters = new OracleDynamicParameters();

            foreach (IParameter parameter in parameters)
            {
                oracleDynamicParameters.Add(parameter.Name, parameter.Value, dbType: parameter.DbType, direction: parameter.ParameterDirection, size: parameter.Size);
            }

            return oracleDynamicParameters;
        }

        private IEnumerable<IParameter> ToParameters<T>(T obj)
        {
            List<Parameter> parameters = new List<Parameter>();

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                Parameter parameter = new Parameter()
                {
                    Name = $"@{property.Name}",
                    Value = property.GetValue(obj),
                    DbType = ToDbType(property.GetType()),
                    ParameterDirection = ParameterDirection.Input
                };

                parameters.Add(parameter);
            }
            return parameters;
        }

        private DbType ToDbType(Type type)
        {
            // create typemap
            return TypeMapping.TypeMap[type];

        }
    }
}
