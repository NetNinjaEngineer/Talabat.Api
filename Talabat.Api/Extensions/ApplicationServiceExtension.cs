using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Talabat.Api.Errors;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.Api.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = (actionContext) =>
            {
                var errors = actionContext.ModelState.Where(x => x.Value!.Errors.Count() > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage);

                var apiValidationErrorResponse = new ApiValidationErrorResponse
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(apiValidationErrorResponse);
            };
        });

        services.AddScoped<IBasketRepository, BasketRepository>();


        return services;
    }
}
