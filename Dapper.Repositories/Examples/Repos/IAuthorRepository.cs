using System;
using System.Threading.Tasks;

namespace Dapper.Repositories.Examples.Repos
{
    public interface IAuthorRepository
    {
        Task<string> GetNameById(int id);
    }
}
