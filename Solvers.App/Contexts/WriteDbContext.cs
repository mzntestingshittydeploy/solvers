
using Microsoft.EntityFrameworkCore;
using Solvers.App.Models;


namespace Contexts.Solvers
{
    public class WriteDbContext : DbContext
    {
        public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options)
        {
        }

        public DbSet<Solver> Solvers { get; set; }



    }
}
