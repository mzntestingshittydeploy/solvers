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
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Solvers.Test
{
    public abstract class BaseFeatureTest : BaseTest
    {
        protected WebApplicationFactory<Program> App;

        protected HttpClient Client;

        protected BaseFeatureTest()
        {
            App = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {

                    builder.ConfigureServices(services =>
                    {
                        services.AddDbContext<WriteDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestingDb");
                        });

                        services.AddDbContext<ReadDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestingDb");
                        });
                    });
                });

            Client = App.CreateClient();
        }
    }
}
