using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TitanFitenss.Domain.PlanAggregate;
namespace TitanFitenss.Infrastructure.Persistence;
    public class PlanConfiguration:IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> b)
    {
        b.ToTable("Plans");
        b.HasKey(p=>p.PlanId);
        b.Property(p=>p.PlanName).HasMaxLength(50).IsRequired();
        b.Property(p=>p.Price).HasPrecision(5,2).IsRequired();
        b.Property(p=>p.DurationInMonths).IsRequired();
        b.Property(p=>p.MaxFreezeDays).IsRequired();
        b.Property(p=>p.MaxNumberOfFreezes).IsRequired();
        b.Property(p=>p.GuestPassQuota).IsRequired();
        b.Property(p=>p.AccessScope).HasConversion<int>().IsRequired();
        b.Property(p=>p.IsPublished).IsRequired();  
    }
    }