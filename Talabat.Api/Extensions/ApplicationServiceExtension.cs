using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Talabat.Api.Errors;
using Talabat.Core.Repositories;
using Talabat.Repository;

namespace Talabat.Api.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
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


        return services;
    }
}
