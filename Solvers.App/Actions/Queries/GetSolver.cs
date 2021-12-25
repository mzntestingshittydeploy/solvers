using Contexts.Solvers;
using Microsoft.AspNetCore.Mvc;
using Solvers.App.Contracts;
using Solvers.App.Models;

namespace Solvers.App.Actions.Queries
{
    public class GetSolver : IAction
    {
        private readonly ReadDbContext _context;
        public GetSolver(ReadDbContext context)
        {
            _context = context;
        }

        public ActionResult<Solver> FromController(long id)
        {
            var result = Handle(id);

            if (result == null)
            {
                return new StatusCodeResult(404);
            }

            return result;
        }

        public Solver? Handle(long id)
        {
            return _context.Solvers.FirstOrDefault(s => s.Id == id);
        }
    }
}
