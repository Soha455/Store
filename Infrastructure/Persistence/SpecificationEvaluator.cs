﻿using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    static class SpecificationEvaluator
    {
        // Generate Query Method
        public static IQueryable<TEntity> GetQuery<TEntity, TKey>(
                IQueryable<TEntity> inputQuery ,
                ISpecifications<TEntity, TKey> spec 
                )
                where TEntity :BaseEntity<TKey>
        {
            var query = inputQuery;

            // Where 
            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            // Include
            query = spec.IncludeExpressions.Aggregate(query, (currentQuery ,IncludeExpression) => currentQuery.Include(IncludeExpression));

            return query;
        }
    }
}
