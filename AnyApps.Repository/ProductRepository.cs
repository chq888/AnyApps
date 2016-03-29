using AnyApps.Core.Repository.Ef;
using AnyApps.Core.Repository.Repositories;
using AnyApps.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AnyApps.Repository
{
    // Exmaple: How to add custom methods to a repository.
    public static class ProductRepository
    {
        public static IEnumerable<Product> GetProducts(this IRepositoryAsync<Product> repository)
        {
            return repository
                .Queryable()
                .AsEnumerable();
        }

    }
}