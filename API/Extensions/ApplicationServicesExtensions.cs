using API.Errors;
using Domain.Interfaces;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors).ToArray()
                        .Select(x => x.ErrorMessage);

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://localhost:4200")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            return services;
        }
    }
}
