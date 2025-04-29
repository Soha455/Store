using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        // Method to generate any repository
        IGenericRepository<TEntity,TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }
}
