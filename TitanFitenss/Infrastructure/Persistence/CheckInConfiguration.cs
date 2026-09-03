using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitenss.Domain.CheckInAggregate;
namespace TitanFitenss.Infrastructure.Persistence
{
    public class CheckInConfiguration:IEntityTypeConfiguration<CheckIn>
    {
        public void Configure(EntityTypeBuilder<CheckIn> b)
        {
            b.ToTable("CheckIns");
            b.HasKey(c=>c.CheckInId);
            b.Property(c=>c.MemberId).IsRequired();
            b.Property(c=>c.BranchId).IsRequired();
            b.Property(c=>c.CheckInDateTime).HasColumnType("datetime2").IsRequired();
            b.Property(c=>c.Result).HasConversion<int>().IsRequired();
            b.Property(c=>c.RefusalReason).HasMaxLength(100).IsRequired(false);
        }
    }
}