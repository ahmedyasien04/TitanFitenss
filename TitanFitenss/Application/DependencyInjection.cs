using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TitanFitenss.Application.Common.Behaviors;
namespace TitanFitenss.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly=typeof(DependencyInjection).Assembly;

        services.AddMediatR(config=>config.RegisterServicesFromAssembly(applicationAssembly));

        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
