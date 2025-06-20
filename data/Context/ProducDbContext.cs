using Microsoft.EntityFrameworkCore;
using clasProduct;
using catogory;
namespace ProducDbContext

{
    public class ProducDbContext : DbContext
    {
        public ProducDbContext(DbContextOptions<ProducDbContext> options) : base(options) {}
        public DbSet<Product> Products { get; set; }
        public DbSet<Catogory> catogories { get; set; }
        private const string ConnectionString = @"Server=DESKTOP-SKSDB0L\DUNGNE;Database=ProducDb;User Id=sa;Password=123456;TrustServerCertificate=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString);

            
        }


    
    }
}