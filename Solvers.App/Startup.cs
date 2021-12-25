using Contexts.Solvers;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Solvers.App.Actions;
using Solvers.App.Contracts;
using Solvers.App.Serializers;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace Solvers.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {



            // Auto register actions
            Assembly.GetExecutingAssembly()
                .GetTypes()
                .ToList()
                .Where(t => t.GetInterface(nameof(IAction)) != null)
                .ToList()
                .ForEach(t => services.AddScoped(t));

            //services.AddScoped<CreateSolver>();
            services.AddRazorPages(); 
            services.AddControllersWithViews();

            //ConnectionStrings__MyConnection
            

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,
                    ValidateActor = false,

                    RequireSignedTokens = false,



                    SignatureValidator = delegate (string token, TokenValidationParameters parameters) {

                        var jwt = new JwtSecurityToken(token);

                        return jwt;
                    }
                };
            });

            if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Test")
            {
                services.AddDbContext<WriteDbContext>(options => options
                .UseMySql(Configuration.GetConnectionString("WriteDatabase"), new MySqlServerVersion(new Version(8, 0, 27)))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

                services.AddDbContext<ReadDbContext>(options => options
                .UseMySql(Configuration.GetConnectionString("ReadDatabase"), new MySqlServerVersion(new Version(8, 0, 27)))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
            }

            
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
              {
                {
                  new OpenApiSecurityScheme
                  {
                    Reference = new OpenApiReference
                      {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                      },
                      Scheme = "oauth2",
                      Name = "Bearer",
                      In = ParameterLocation.Header,

                    },
                    new List<string>()
                  }
                });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Solvers Service", Description = "Documentation for the solver service.", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }


            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Test")
            {

                using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

                using var context = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

                if (context.Database.IsMySql())
                {
                    context.Database.Migrate();
                }

            }
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("api", () => "Solvers API");
                endpoints.MapControllerRoute(
                    name: "solvers",
                    pattern: "api/{controller=Solvers}/{action=Index}/{id?}");
            });

            /*var subscriber = new AutoSubscriber(app.ApplicationServices.GetRequiredService<IBus>(), "solvers")
            {
                AutoSubscriberMessageDispatcher = new ScopeDispatcher(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>())
            };

            subscriber.Subscribe(Assembly.GetExecutingAssembly().GetTypes());
            subscriber.SubscribeAsync(Assembly.GetExecutingAssembly().GetTypes()); */

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Solvers API");
            });

        }
    }

    public class ScopeDispatcher : IAutoSubscriberMessageDispatcher
    {
        private readonly IServiceScopeFactory _factory;

        public ScopeDispatcher(IServiceScopeFactory factory)
        {
            _factory = factory;
        }
        void IAutoSubscriberMessageDispatcher.Dispatch<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
        {
            using var scope = _factory.CreateScope();

            var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();

            consumer.Consume(message, cancellationToken);
        }

        async Task IAutoSubscriberMessageDispatcher.DispatchAsync<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
        {
            using var scope = _factory.CreateAsyncScope();

            var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();

            await consumer.ConsumeAsync(message, cancellationToken);
        }
    }
}
