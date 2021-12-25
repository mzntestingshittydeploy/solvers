using Contexts.Solvers;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.Consumer;
using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using Solvers.App.Contracts;
using Solvers.App.Events;
using Solvers.App.Models;

namespace Solvers.App.Actions
{
    public class CreateSolver : IAction
    {
        private WriteDbContext _context;

        public CreateSolver(WriteDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult<Solver>> FromController(HttpRequest request, CreateSolverModel model)
        {

            var role = request.Headers["Role"].FirstOrDefault();

            if (role != "admin")
            {
                return new ForbidResult();
            }

            var solver = new Solver
            {
                Name = model.Name,
                Image = model.Image,
            };

            await _context.AddAsync(solver);

            await _context.SaveChangesAsync();

            return solver;
        }
    }

    public class CreateSolverModel
    {
        public CreateSolverModel()
        {
            Name = "";
            Image = "";
        }

        public string Name { get; set; }
        public string Image { get; set; }
    }

}
