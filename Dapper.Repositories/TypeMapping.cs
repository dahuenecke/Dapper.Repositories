using System;
using System.Collections.Generic;
using System.Data;

namespace Dapper.Repositories
{
    public static class TypeMapping
    {

        public static Dictionary<Type, DbType> TypeMap { get; set; }
    }
}
