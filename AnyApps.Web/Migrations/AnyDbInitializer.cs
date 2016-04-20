using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using BeYourMarket.Core.Migrations;
using System.Globalization;
using AnyApps.DataModel;
using AnyApps.Entities;
using AnyApps.Core.Repository.UnitOfWork;
using AnyApps.Services;
using AnyApps.Core.Repository.DataContext;
using AnyApps.Core.Repository.Ef;
using AnyApps.Core.Repository.Repositories;
using AnyApps.Core.Repository.Infrastructure;
using System.Data.Entity.Infrastructure;
using Microsoft.Practices.Unity;

namespace AnyApps.Web.Migrations
{
    public class AnyDbInitializer : CreateAndMigrateDatabaseInitializer<AnyDbContext, ConfigurationInstall<AnyDbContext>>
    {
        private InstallModel _installModel;
        private IProductService _productService;
        private IUnitOfWorkAsync _unitOfWorkAsync;
        private IUnityContainer _container;

        #region Constructor
        // pass user model, and database info
        public AnyDbInitializer(InstallModel installModel)
            : base()
        {
            _installModel = installModel;
            InitializeDatabase(new AnyApps.Entities.AnyDbContext());

            /*
            // Create new customer
            using (IDataContextAsync context = new AnyDbContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<Product> customerRepository = new Repository<Product>(context, unitOfWork);

                var p = new Product
                {
                    ProductId = 1,
                    ProductName = "CBRE"
                };

                customerRepository.Insert(p);
                unitOfWork.SaveChanges();
            }
            */
        }
        #endregion

        #region Methods

        protected override async void Seed(AnyDbContext context)
        {
            _container = App_Start.UnityConfig.GetConfiguredContainer();
            _unitOfWorkAsync = _container.Resolve<IUnitOfWorkAsync>();
            _productService = _container.Resolve<IProductService>();

            InstallSettings(context);
            InstallProducts();

            if (_installModel.InstallSampleData)
            {

            }
        }

        private async void InstallProducts()
        {
            var p = new Product
            {
                ProductId = 1,
                ProductName = "CBRE"
            };

            _productService.Insert(p);

            try
            {
                await _unitOfWorkAsync.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {

                if (_productService.Query(a => a.ProductId == p.ProductId).Select().Any())
                {
                    // return Conflict();
                }

                throw;
            }
        }
        private void InstallSettings(AnyDbContext context)
        {
            /*			context.Products.Add(new 
            {
                Name = "Massage",
                Description = "Massage",
                Parent = 0,
                Enabled = true,
                Ordering = 0,
                ObjectState = Core.Repository.Infrastructure.ObjectState.Added
            });


            if (_installModel.InstallSampleData)
            {
                context.Settings.Add(new Model.Models.Setting()
                {
                    ID = 1,
                    Name = "BeYourMarket",
                    Description = "Find the beauty and spa service providers in your neighborhood!",
                    Slogan = "BeYourMarket - Spa Demo",
                    SearchPlaceHolder = "Search your Spa...",
                    EmailContact = "hello@beyourmarket.com",
                    Version = "1.0",
                    Currency = "DKK",
                    TransactionFeePercent = 1,
                    TransactionMinimumSize = 10,
                    TransactionMinimumFee = 10,
                    EmailConfirmedRequired = false,
                    Theme = "Default",
                    DateFormat = DateTimeFormatInfo.CurrentInfo.ShortDatePattern,
                    TimeFormat = DateTimeFormatInfo.CurrentInfo.ShortTimePattern,
                    ListingReviewEnabled = true,
                    ListingReviewMaxPerDay = 5,
                    Created = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added
                });

                // Copy files
                var files = new string[] { "cover.jpg", "favicon.jpg", "logo.png" };
                foreach (var file in files)
                {
                    var pathFrom = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/images/sample/community"), file);
                    var pathTo = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/images/community"), file);
                    File.Copy(pathFrom, pathTo, true);
                }
            }
            else
            {
                context.Settings.Add(new Model.Models.Setting()
                {
                    ID = 1,
                    Name = "BeYourMarket",
                    Description = "Create your own peer to peer market place in 5 minutes!",
                    Slogan = "Slogan...",
                    SearchPlaceHolder = "Search...",
                    EmailContact = "hello@beyourmarket.com",
                    Version = "1.0",
                    Currency = "DKK",
                    TransactionFeePercent = 1,
                    TransactionMinimumSize = 10,
                    TransactionMinimumFee = 10,
                    EmailConfirmedRequired = false,
                    Theme = "Default",
                    DateFormat = DateTimeFormatInfo.CurrentInfo.ShortDatePattern,
                    TimeFormat = DateTimeFormatInfo.CurrentInfo.ShortTimePattern,
                    ListingReviewEnabled = true,
                    ListingReviewMaxPerDay = 5,
                    Created = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added
                });
            }
            */
        }
        #endregion
    }
}