using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Behaviors;
using Application.InternalServices;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }
            ,AssemblyReference.Assembly);
        
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);
        
        services.Scan(scan => scan
            .FromAssemblies(AssemblyReference.Assembly)

            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()

            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()

            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.AddScoped<IBoardsStatusesService, BoardsStatusesService>();
        
        return services;
    }
}