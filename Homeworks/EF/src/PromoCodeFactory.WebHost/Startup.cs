using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.DBContext;
using PromoCodeFactory.DataAccess.Repositories;

// ReSharper disable All

namespace PromoCodeFactory.WebHost;

public class Startup
{
    private IConfiguration Configuration{ get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(
            options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("SQLiteConnectionString"));
            });


        services.AddScoped<IEfDbInit, EfDbInit>();
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));


        services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
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
            app.UseHsts();
        }

        app.UseOpenApi();
        app.UseSwaggerUi(x =>
        {
            x.DocExpansion = "list";


        });

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
