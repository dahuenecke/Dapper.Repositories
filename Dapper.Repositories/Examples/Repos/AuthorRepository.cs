using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Repositories.Examples.Models;

namespace Dapper.Repositories.Examples.Repos
{
    public class AuthorRepository : Repository, IAuthorRepository
    {
        //private static object _dynamicParameters;

        public AuthorRepository(string connectionString, RdbmsType rdbms) : base(connectionString, rdbms)
        {
        }

        public async Task<Author> GetAuthorById(int id)
        {
            IEnumerable<IParameter> parameters = new List<Parameter>()
                { 
                    new Parameter()
                    { 
                        Name = "@Id",
                        Value = id,
                        DbType = DbType.Int32,
                        ParameterDirection = ParameterDirection.Input 
                    } 
                };

            var dynamicParameters = CreateDynamicParameters(parameters);

            return (await ExecuteQueryAsync<Author>("usp_GetAuthorById", dynamicParameters))
                    .FirstOrDefault();
        }

        public async Task<IEnumerable<Author>> GetAuthorsByLastName(string lastName)
        {
            IEnumerable<IParameter> parameters = new List<Parameter>()
                {
                    new Parameter()
                    {
                        Name = "@LastName",
                        Value = lastName,
                        DbType = DbType.Int32,
                        ParameterDirection = ParameterDirection.Input
                    }
                };

            var dynamicParameters = CreateDynamicParameters(parameters);

            return await ExecuteQueryAsync<Author>("usp_GetAuthorsByLastName", dynamicParameters);
        }

        public async Task CreateAuthor(Author author)
        {
            var dynamicParameters = CreateDynamicParameters(author);

            await ExecuteAsync("usp_InsertAuthor", dynamicParameters);
        }
    }
}
