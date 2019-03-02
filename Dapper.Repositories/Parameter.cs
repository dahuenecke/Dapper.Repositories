using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Dapper.Repositories
{
    public class Parameter : IParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public DbType? DbType { get; set; }
        //public OracleDbType? OracleDbType { get; set; }
        public ParameterDirection? ParameterDirection { get; set; }
        public int? Size { get; set; }
    }
}
