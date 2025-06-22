using Microsoft.EntityFrameworkCore;
using DocVault.API.Models;

namespace DocVault.API.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Document> Documents { get; set; }
    }

}