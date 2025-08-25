using CurtainStoreManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CurtainStoreManagement.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        public DbSet<Curtain> curtains { get; set; }

    }
}
