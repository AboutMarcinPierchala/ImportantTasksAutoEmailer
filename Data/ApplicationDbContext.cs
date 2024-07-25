using ImpListApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ImpListApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"DB_connectionstring");
    }
}
