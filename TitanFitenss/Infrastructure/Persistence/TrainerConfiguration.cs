using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TitanFitenss.Domain.TrainerAggregate;
namespace TitanFitenss.Infrastructure.Persistence
{
    public class TrainerConfiguration:IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> b)
        {
            b.ToTable("Trainers");
            b.HasKey(t=>t.TrainerId);
            b.Property(t=>t.TrainerName).HasMaxLength(100).IsRequired();
            b.Property(t=>t.Email).HasMaxLength(100).IsRequired();
            b.Property(t=>t.Phone).HasMaxLength(20).IsRequired();
            b.Property(t=>t.IsActive).IsRequired();
        }
    }
}