using AnyApps.Core.Repository.Ef;
using AnyApps.Entities.Mapping;
using System.Data.Entity;

namespace AnyApps.Entities
{
    public partial class AnyDbContext : DataContext
    {
        static AnyDbContext()
        {
            Database.SetInitializer<AnyDbContext>(null);
        }

        public AnyDbContext() : base("Name=AnyDbContext")
        {
        }        

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProductMap());
        }
    }
}
