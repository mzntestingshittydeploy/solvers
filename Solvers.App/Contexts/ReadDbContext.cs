
using Microsoft.EntityFrameworkCore;
using Solvers.App.Models;


namespace Contexts.Solvers
{
    public class ReadDbContext : DbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
        {
        }

        public DbSet<Solver> Solvers { get; set; }



    }
}
