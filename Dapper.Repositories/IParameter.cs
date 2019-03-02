using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Dapper.Repositories
{
    public interface IParameter
    {
        string Name { get; set; }
        object Value { get; set; }
        DbType? DbType { get; set; }
        //OracleDbType? OracleDbType { get; set; }
        ParameterDirection? ParameterDirection { get; set; }
        int? Size { get; set; }
    }
}
