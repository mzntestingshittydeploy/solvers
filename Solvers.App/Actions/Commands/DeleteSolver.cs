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
    public class DeleteSolver : IAction
    {
        private readonly WriteDbContext _context;

        public DeleteSolver(WriteDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> FromController(HttpRequest request, long id)
        {
            var role = request.Headers["Role"].FirstOrDefault();

            if (role != "admin")
            {
                return new ForbidResult();
            }

            var result = _context.Solvers.FirstOrDefault(s => s.Id == id);

            if (result == null)
            {
                return new StatusCodeResult(404);
            }

            _context.Remove(result);

            await _context.SaveChangesAsync();

            return new StatusCodeResult(200);
        }
    }
}
