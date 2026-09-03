using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TitanFitenss.Domain.Interfaces;
using TitanFitenss.Infrastructure.Persistence;
using TitanFitenss.Infrastructure.Repositories;
namespace TitanFitenss.Infrastructure;
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TitanFitnessDbContext>(options=>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b=>b.MigrationsAssembly(typeof(TitanFitnessDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<ICheckInRepository, CheckInRepository>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();
        services.AddScoped<IClassSessionRepository, ClassSessionRepository>();

        return services;
    }
    }