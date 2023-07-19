using MagicVilla_WebApi.Models;
using System.Linq.Expressions;

namespace MagicVilla_WebApi.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa entity);
    }
}
