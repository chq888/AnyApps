using AnyApps.Core;
using AnyApps.Core.Repository.DataContext;
using AnyApps.Core.Repository.Ef;
using AnyApps.Core.Repository.Repositories;
using AnyApps.Core.Repository.UnitOfWork;
using AnyApps.DataModel;
using AnyApps.Entities;
using AnyApps.Repository;
using AnyApps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AnyApps.Controllers.API
{

    [RoutePrefix("api/test")]
    [Route("aaa/{action}")]
    public class TestApiController : ApiController
    {
        private readonly IProductService _customerService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        IDataContextAsync context;

        public TestApiController()
        {
            /*
            // Create new customer
            using (IDataContextAsync context = new AnyDbContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<Product> customerRepository = new Repository<Product>(context, unitOfWork);

                var customer = new Product
                {
                    ProductId = 2,
                    ProductName = Guid.NewGuid().ToString(),
                    ObjectState = ObjectState.Added
                };

                customerRepository.Insert(customer);
                unitOfWork.SaveChanges();
            }
            */


            context = new AnyDbContext();
            _unitOfWorkAsync = new UnitOfWork(context);
            var context11 = (AnyDbContext)context;
            /*
            var products = new List<Product>();
            for (var i = 0; i < 10; i++)
            {
                products.Add(new Product
                {
                    ProductId = i,
                    ProductName = $"Pro {i}",
                    ObjectState = ObjectState.Added
                });
            }

            IRepositoryAsync<Product> productRepository =  new Repository<Product>(context, _unitOfWorkAsync);
            productRepository.InsertGraphRange(products);
            _unitOfWorkAsync.SaveChanges();
                        products.Clear();
4
            */
            _customerService = new ProductService(_unitOfWorkAsync.RepositoryAsync<Product>());
        }


        // GET: api/TestApi
        [Route("doaaa")]
        public IEnumerable<ProductData> Get()
        {
            IEnumerable<ProductData> ss = _customerService.GetProducts();
            return ss;
        }

        [Route("{id}")]
        // GET: api/TestApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TestApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TestApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TestApi/5
        public void Delete(int id)
        {
        }
    }
}
