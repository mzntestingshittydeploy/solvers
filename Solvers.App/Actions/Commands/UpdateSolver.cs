using Contexts.Solvers;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.Consumer;
using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using Solvers.App.Actions.Queries;
using Solvers.App.Contracts;
using Solvers.App.Events;
using Solvers.App.Models;

namespace Solvers.App.Actions
{
    public class UpdateSolver : IAction
    {
        private readonly WriteDbContext _context;
        private readonly GetSolver _getSolver;

        public UpdateSolver(GetSolver getSolver, WriteDbContext context)
        {
            _context = context;
            _getSolver = getSolver;
        }

        public async Task<ActionResult<Solver>> FromController(HttpRequest request, UpdateSolverModel model)
        {

            var role = request.Headers["Role"].FirstOrDefault();

            if (role != "admin")
            {
                return new ForbidResult();
            }

            var solver = _getSolver.Handle(model.Id);

            if (solver == null)
            {
                return new StatusCodeResult(404);
            }

            return await Handle(solver);
        }

        public async Task<Solver> Handle(Solver solver)
        {
            _context.Update(solver);
            await _context.SaveChangesAsync();
            return solver;
        }
    }

    public class UpdateSolverModel
    {
        public UpdateSolverModel()
        {
            Name = "";
            Image = "";
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }

}
