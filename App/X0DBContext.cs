using App.Entities;
using Microsoft.EntityFrameworkCore;

namespace App
{
    public class X0DBContext : DbContext
    {
        public X0DBContext(DbContextOptions<X0DBContext> options) : base(options)
        { }

        public DbSet<Game> Games { get; set; }
    }
}
