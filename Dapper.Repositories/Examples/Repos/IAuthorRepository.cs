using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper.Repositories.Examples.Models;

namespace Dapper.Repositories.Examples.Repos
{
    public interface IAuthorRepository
    {
        Task<Author> GetAuthorById(int id);
        Task<IEnumerable<Author>> GetAuthorsByLastName(string lastName);
        Task CreateAuthor(Author author);
    }
}
