
using Contexts.Solvers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using Solvers.App;
using Solvers.App.Actions;
using Solvers.App.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Solvers.Test.Feature
{
    public class CreateSolverTest : BaseFeatureTest
    {

        [Test]
        public async Task AdminCanCreateSolvers()
        {
            var model = new CreateSolverModel()
            {
                Name = "Test.name",
                Image = "Test.image"
            };

            var myContent = JsonConvert.SerializeObject(model);

            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            byteContent.Headers.Add("Role", "admin");

            var response = await Client.PostAsync("/api/Solvers", byteContent);

            var json = await response.Content.ReadAsStringAsync();

            var solver = JsonConvert.DeserializeObject<Solver>(json);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(solver);
            Assert.NotNull(solver.Id);
            Assert.AreEqual(model.Name, solver.Name);
            Assert.AreEqual(model.Image, solver.Image);
        }

        [Test]
        public async Task UserCannotCreateSolver()
        {
            var model = new CreateSolverModel()
            {
                Name = "Test2.name",
                Image = "Test2.image"
            };

            var myContent = JsonConvert.SerializeObject(model);

            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            byteContent.Headers.Add("Role", "user");

            var response = await Client.PostAsync("/api/Solvers", byteContent);

            var json = await response.Content.ReadAsStringAsync();

            var solver = JsonConvert.DeserializeObject<Solver>(json);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.IsNull(solver);
        }
    }
}
