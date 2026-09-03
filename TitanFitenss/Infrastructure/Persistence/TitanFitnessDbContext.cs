using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TitanFitenss.Domain.BranchAggregate;
using TitanFitenss.Domain.MemberAggregate;
using TitanFitenss.Domain.PlanAggregate;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Domain.CheckInAggregate;
using TitanFitenss.Domain.TrainerAggregate;
using TitanFitenss.Domain.ClassSessionAggregate;
namespace TitanFitenss.Infrastructure.Persistence;
    public class TitanFitnessDbContext:DbContext
    {
      public TitanFitnessDbContext(DbContextOptions<TitanFitnessDbContext> options):base(options){}
      
      public DbSet<Branch> Branches=>Set<Branch>();
      public DbSet<Member> Members=>Set<Member>();
      public DbSet<Plan> Plans=>Set<Plan>();
      public DbSet<Membership> Memberships=>Set<Membership>();
      public DbSet<CheckIn> CheckIns=>Set<CheckIn>();
      public DbSet<Trainer> Trainers=>Set<Trainer>();
      public DbSet<ClassSession> ClassSessions=>Set<ClassSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    }
    