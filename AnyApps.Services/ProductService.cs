using AnyApps.Core.Repository.Repositories;
using AnyApps.Repository;
using AnyApps.Core.Repository;
using AnyApps.Core.Service;
using System.Collections.Generic;
using AnyApps.Entities;
using AnyApps.DataModel;
using AutoMapper;

namespace AnyApps.Services
{
    public interface IProductService : IService<Product>
    {
        IEnumerable<ProductData> GetProducts();
    }

    public class ProductService : Service<Product>, IProductService
    {
        private readonly IRepositoryAsync<Product> _repository;

        public ProductService(IRepositoryAsync<Product> repository) : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<ProductData> GetProducts()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductData>());
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<ProductData>>(_repository.GetProducts());
        }
    }
}