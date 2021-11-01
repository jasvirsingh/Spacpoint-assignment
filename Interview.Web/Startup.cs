using Interview.Web.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sparcpoint.Data.Access.EF;
using Sparcpoint.Data.Access.Interfaces;
using Sparcpoint.Data.Access.Repo;
using Sparcpoint.Model;
using Sparcpoint.Model.Services;
using Sparcpoint.Model.Services.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Interview.Web
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddLogging();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();

            // controllers
            services.AddTransient<ProductController>();
            services.AddTransient<CategoryController>();


            // Services
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();


            // Repositories
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();

            // add db context
            var builderAction = new Action<IServiceProvider, DbContextOptionsBuilder>((sp, options) =>
            {
                var config = sp.GetRequiredService<IOptionsMonitor<SparcpointDbConfig>>();
                options.UseSqlServer(config.CurrentValue.DbConnectionString, o => o.EnableRetryOnFailure());
                options.EnableSensitiveDataLogging(false);
            });

            //services.AddDbContext<SparcpointDbContext>(builderAction, ServiceLifetime.Scoped);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseExceptionHandler((builder) =>
            {
                builder.Run(async (context) =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandlerFeature?.Error != null)
                    {
                        await OnException(context: context, ex: exceptionHandlerFeature.Error).ConfigureAwait(false);
                    }
                });
            });

        }

        /// <summary>Called when [exception].</summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private Task OnException(HttpContext context, Exception ex)
        {
            try
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var logger = context.RequestServices.GetService<ILogger<Startup>>();

                if (ex is OperationCanceledException)
                {
                    // Issuing non-standard status code when the client 
                    // has disconnected (This is based off of nginx)
                    context.Response.StatusCode = 499;
                    context.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Client Closed Request";
                    logger.LogWarning(ex, "The client disconnected");
                }
                else
                {
                    logger.LogError(ex, "An unhandled exception occured.");
                }
            }
            catch (Exception) { }

            return Task.CompletedTask;
        }
    }
}
