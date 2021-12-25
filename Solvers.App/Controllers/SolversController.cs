using Contexts.Solvers;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Solvers.App.Actions;
using Solvers.App.Actions.Queries;
using Solvers.App.Models;

namespace Solvers.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolversController : Controller
    {
        /// <summary>
        /// List all available solvers.
        /// </summary>
        /// <response code="200">Solvers listed successfully.</response>
        /// <response code="500">Internal error.</response>
        [ProducesResponseType(typeof(IList<Solver>), 200)]
        [ProducesResponseType(500)]
        [HttpGet]
        public IList<Solver> Index([FromServices] ListSolvers action)
        {
            return action.FromController();
        }

        /// <summary>
        /// Get a single solver by id.
        /// </summary>
        /// <response code="200">Solver found successfully.</response>
        /// <response code="404">Solver was not found.</response>
        /// <response code="500">Internal error.</response>
        [ProducesResponseType(typeof(Solver), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("{id}")]
        public ActionResult<Solver> Get([FromServices] GetSolver action, long id)
        {
            return action.FromController(id);
        }

        /// <summary>
        /// Create a new solver.
        /// </summary>
        /// <response code="200">Solver created successfully.</response>
        /// <response code="403">Insufficient permissions.</response>
        /// <response code="500">Internal error.</response>
        [ProducesResponseType(typeof(Solver), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<ActionResult<Solver>> Create([FromServices] CreateSolver action, CreateSolverModel model)
        {
            return await action.FromController(Request, model);
        }

        /// <summary>
        /// Update an existing solver by id.
        /// </summary>
        /// <response code="200">Solver updated successfully.</response>
        /// <response code="403">Insufficient permissions.</response>
        /// <response code="404">Solver not found.</response>
        /// <response code="500">Internal error.</response>
        [ProducesResponseType(typeof(Solver), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpPut]
        public async Task<ActionResult<Solver>> Update([FromServices] UpdateSolver action, UpdateSolverModel model)
        {
            return await action.FromController(Request, model);
        }


        /// <summary>
        /// Delete an existing solver by id.
        /// </summary>
        /// <response code="200">Solver deleted successfully.</response>
        /// <response code="403">Insufficient permissions.</response>
        /// <response code="404">Solver not found.</response>
        /// <response code="500">Internal error.</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Delete([FromServices] DeleteSolver action, long id)
        {
            return await action.FromController(Request, id);
        }
    }
}
