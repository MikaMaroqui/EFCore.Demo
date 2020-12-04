using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.Demo.Db;
using EFCore.Demo.Options;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Demo
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
            services.AddControllers();

            services.Configure<ConnectionStrings>(Configuration.GetSection(nameof(ConnectionStrings)));

            // Inject Db elements
            // services.AddDbContext<DemoContext>(
            //     options => options.UseSqlServer(Configuration["Database:ConnectionString"]),
            //     ServiceLifetime.Transient
            // );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DemoContext context)
        {
            // This allows us to migrate the database on app launch
            context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
