using Contexts.Solvers;
using Microsoft.AspNetCore.Mvc;
using Solvers.App.Contracts;
using Solvers.App.Models;

namespace Solvers.App.Actions.Queries
{
    public class ListSolvers : IAction
    {
        private readonly ReadDbContext _context;
        public ListSolvers(ReadDbContext context)
        {
            _context = context;
        }
        public IList<Solver> FromController()
        {
            return _context.Solvers.ToList();
        }
    }
}
