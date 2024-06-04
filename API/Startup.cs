using API.Extensions;
using API.Helpers;
using API.Middleware;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfiles));
        services.AddDbContext<StoreDataContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
        services.AddDbContext<AppIdentityDbContext>(options =>
           options.UseSqlServer(_configuration.GetConnectionString("IdentityConnection")));

        services.AddSingleton<IConnectionMultiplexer>(x =>
        {
            var configuration = ConfigurationOptions.Parse(_configuration.GetConnectionString("Redis"), true);
            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddControllers();
        services.AddApplicationServices();
        services.AddIdentityServices(_configuration);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerDocumentation();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseStatusCodePagesWithReExecute("/errors/{0}");
        app.UseHttpsRedirection();
        app.UseCors("AllowSpecificOrigin");
        app.UseRouting();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        if (env.IsDevelopment())
        {
            app.UseSwaggerDocumentation();
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
