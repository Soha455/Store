using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Contracts
{
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Expression<Func<TEntity, bool>>? Criteria { get; set; }                            // Where
        List<Expression<Func<TEntity, object>>> IncludeExpressions { get; set; }           // Include

        Expression<Func<TEntity, Object>>? OrderBy { get; set; }                           // Order By
        Expression<Func<TEntity, Object>>? OrderByDescending { get; set; }                 // Order By Descending


        // Pagination 
        int Skip { get; set; }
        int Take { get; set; }
        public bool IsPagination { get; set; }



    }
}
