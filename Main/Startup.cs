using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Common;
using Microsoft.EntityFrameworkCore;
using Persistance.Common.Extensions;
using System.Reflection;
using Application.Users.Queries;
using Application.Interfaces.Persistance;
using Application.Users.Models;
using Persistance.Services.Users;
using Application.Lists.Models;
using Persistance.Services.Lists;
using Application.Lists.Queries;
using Microsoft.AspNetCore.Diagnostics;
using Application.Interfaces.Queries;
using Application.Items.Models;
using Persistance.Services.Items;
using Application.Items.Queries;
using Application.Users.Queries.Interfaces;
using Application.Users.Commands;
using Application.Interfaces.Persistance.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using AspNetCoreRateLimit;
using Application.Items.Queries.Interfaces;
using Application.Interfaces.Persistance.Items;
using Application.Items.Commands;
using Application.Interfaces.Persistance.Lists;
using Application.Lists.Commands;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Main
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
           
           AppDomain.CurrentDomain.Load("Service");
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            services.AddMvc(options =>
            {

                options.InputFormatters.Add(new XmlDataContractSerializerInputFormatter(options));
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                var outputFormatter = options.OutputFormatters.OfType<JsonOutputFormatter>().SingleOrDefault();

                if (outputFormatter != null)
                {
                    outputFormatter.SupportedMediaTypes.Add("application/vnd.marvin.hateoas+json");
                }

            })
                .AddApplicationPart(Assembly.Load("Service")).AddControllersAsServices()
                .AddJsonOptions(options => 
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddDbContextPool<ToDoListContext>(options => {

                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUserQuery, UserQuery>();
            services.AddScoped<IUserCommand, UserCommand>();
            services.AddScoped<IListQuery, ListQuery>();
            services.AddScoped<IListCommand, ListCommand>();
            services.AddScoped<IItemQuery, ItemQuery>();
            services.AddScoped<IItemsCommand, ItemsCommand>();
            services.AddScoped<IWriteUserRepository, UsersRepository>();
            services.AddScoped<IReadUserRepository, UsersRepository>();
            services.AddScoped<IReadListsRepository,ListRepository>();
            services.AddScoped<IWriteListsRepository, ListRepository>();
            services.AddScoped<IReadItemsRepository,ItemsRepository>();
            services.AddScoped<IWriteItemsRepository, ItemsRepository>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
             {
                 var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                 return new UrlHelper(actionContext);
             });

            services.AddHttpCacheHeaders(configureExpirationModelOptions =>
            {
                configureExpirationModelOptions.MaxAge = 600;

            }, configureValidationModelOptions => 
            {
                configureValidationModelOptions.AddMustRevalidate = true;
            }); 

            services.AddResponseCaching();

            services.AddMemoryCache();

            services.Configure<IpRateLimitOptions>(options => 
            {
                options.GeneralRules = new List<RateLimitRule>()
                {
                    new RateLimitRule()
                    {
                        Endpoint = "*",
                        Limit = 100,
                        Period = "1h"
                    },
                    new RateLimitRule()
                    {
                        Endpoint = "*",
                        Limit = 10,
                        Period = "10s"
                    }
                };
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
                
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ToDoListContext context, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
               
                 app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExceptionHandler(configure => configure.Run(async httpContext =>
                {
                  var feature = httpContext.Features.Get<IExceptionHandlerFeature>();
                    if(feature != null)
                    {
                        var logger = loggerFactory.CreateLogger("Exception Error");
                        logger.LogError(500, feature.Error, feature.Error.Message);
                        httpContext.Response.StatusCode = 500;
                        await httpContext.Response.WriteAsync(feature.Error.Message);
                    }
                })


               ) ;
            }

            context.SeedDatabase();
            app.UseIpRateLimiting();
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();
            
            app.UseMvc();
        }
    }
}
